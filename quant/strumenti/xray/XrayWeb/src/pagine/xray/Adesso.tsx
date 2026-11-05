// Adesso: le quattro domande in un colpo d'occhio (C.3). In cima sei tessere (prezzo, regime, flip, meteo,
// muri sul valore, pressione), poi il profilo freno/acceleratore grande accanto alla scala dei prezzi, poi
// chi ha comprato delta oggi. Le frasi lunghe stanno nella "i" di ogni riquadro, non a vista.
//
// Il server calcola, la pagina disegna: il regime sintetico ("segnali opposti"), il meteo e i muri sul
// valore vengono da /api/quattro-domande (prezzi in ES), la scala con le zone fuse da /api/adesso (in SPX).

import { useCallback, useMemo, useState, type ReactNode } from 'react';
import { useMediaQuery } from '@mantine/hooks';
import { IconSunrise } from '@tabler/icons-react';
import type { UseQueryResult } from '@tanstack/react-query';
import { useMisura, useQuattroDomande } from '../../api/domande';
import type { DexGruppo, Esposizione, RispostaAdesso, RispostaProfilo, RispostaQuattroDomande, Scadenza } from '../../api/tipi';
import { distanza, giornoNY, nome, num, segno, sig } from '../../formato';
import { useFiltri, type Filtri } from '../../stato/contesto';
import { useScelte } from '../../stato/scelte';
import { C } from '../../tema/colori';
import { Grafico, altezzaPx } from '../../componenti/Grafico';
import { Legenda } from '../../componenti/Legenda';
import { Scheda } from '../../componenti/Scheda';
import { Tessera, type Tono } from '../../componenti/Tessera';
import { Esito, Vuoto } from '../../componenti/Esito';
import { Segmenti } from '../../componenti/Comandi';
import { ScalaPrezzi } from '../../componenti/ScalaPrezzi';
import { BarraInline } from '../../componenti/BarraInline';
import { Termine } from '../../componenti/Termine';
import type { Opzioni } from '../../viz/echarts';
import {
  asseValori,
  assePrezzi,
  asseZero,
  coloreSegno,
  croce,
  griglia,
  lineeOrizzontali,
  massimoAssoluto,
  parolaSegno,
  passoDi,
  puntatoreStrike,
  rigaTip,
  segnoFlusso,
  segnoGamma,
  serieBarre,
  serieLinee,
  testaTip,
  tooltip,
  type LineaOrizzontale,
} from '../../viz/grafici';
import './xray1.css';

const FONTE = ['xray'] as const;

type Verso = 'freno' | 'acceleratore';
const ABBREVIA: Record<Verso, string> = { freno: 'freno', acceleratore: 'acc.' };

/** Una data AAAA-MM-GG come "gio 05/11" (la data e' gia' quella della seduta di New York). */
export function giornoSeduta(g: string | null | undefined): string {
  if (!g) return '–';
  const ms = Date.parse(`${g}T17:00:00Z`);
  return Number.isFinite(ms) ? giornoNY(ms) : g;
}

/** Il nome breve di una scadenza: "0DTE", "3 g", "opex 15 g". */
export function scadenzaBreve(s: Scadenza, dte: number | null | undefined): string {
  if (s === '0dte') return '0DTE';
  const g = dte != null && dte > 0 ? `${dte} g` : '';
  return s === 'opex' ? `opex ${g}`.trim() : g || 'pross.';
}

/** Un pallino del segno con la parola: "● 0DTE freno". Il segno si legge anche senza il colore. */
function Pastiglia({ testo, verso, valore }: { testo: string; verso: Verso | null | undefined; valore?: string }) {
  return (
    <span className="segno-pastiglia" data-verso={verso ?? undefined}>
      <span
        className={verso ? 'segno-punto' : 'segno-punto senza'}
        style={verso ? { background: verso === 'freno' ? C.freno : C.acceleratore } : undefined}
        aria-hidden
      />
      {testo}
      {valore != null && <span className="num"> {valore}</span>}
      <b>{verso ? ABBREVIA[verso] : '–'}</b>
    </span>
  );
}

const versoDi = (v: number | null | undefined): Verso | null => (v == null || !Number.isFinite(v) ? null : v >= 0 ? 'freno' : 'acceleratore');

export function Adesso() {
  const f = useFiltri();
  const [s, setS] = useScelte<{ ampiezza: string }>('adesso', { ampiezza: '2.5' });
  const qa = useMisura<RispostaAdesso>('adesso', f.parametri, f.vivo, f.pronto);
  const qp = useMisura<RispostaProfilo>('profilo', { ...f.parametri, greca: 'gamma', ampiezza: s.ampiezza }, f.vivo, f.pronto);
  const qd = useQuattroDomande('ES', f.parametri, f.vivo, f.pronto);
  const telefono = useMediaQuery('(max-width: 767px)') ?? false;
  const a = qa.data;

  // i prezzi delle quattro domande sono in ES: si portano in SPX con la loro base, poi nell'unita' scelta
  const baseEs = qd.data?.baseEsSpx ?? f.base?.valore ?? 0;
  const P = f.p;
  const daEs = useCallback((x: number) => P(x - baseEs), [P, baseEs]);

  const preCassa = a != null && !a.riferimento.vivo && f.cassa != null && f.istante != null && f.istante < f.cassa.apertura;
  const sedutaPrevista = a?.outlook?.seduta ?? null;

  return (
    <>
      {preCassa && (
        <div className="avviso-cassa" role="note">
          <IconSunrise size={16} aria-hidden />
          <span>
            Pre-cassa: X-Ray vale per i livelli previsti per la seduta di {giornoSeduta(sedutaPrevista ?? a?.giorno)}
          </span>
        </div>
      )}
      <Esito q={qa} rotta="adesso" altezza={72}>
        {(d) => <Tessere a={d} qd={qd} f={f} daEs={daEs} />}
      </Esito>
      <div className="griglia">
        <Profilo qp={qp} a={a} qd={qd.data} f={f} daEs={daEs} ampiezza={s.ampiezza} cambiaAmpiezza={(x) => setS({ ampiezza: x })} />
        <Scheda
          titolo="La scala dei prezzi"
          fonti={FONTE}
          larghezza={4}
          className={`adesso-scala${preCassa && sedutaPrevista === a?.giorno ? ' scala-pre-cassa' : ''}`}
          breve="I livelli dall'alto in basso, con la distanza dal prezzo; in grigio quelli previsti per un'altra seduta."
          semplice={
            <p>
              I prezzi dove OptionsDepth vede un <Termine k="flip">flip</Termine>, il <Termine k="freno">freno</Termine> o l’
              <Termine k="acceleratore">acceleratore</Termine> più forti e i <Termine k="muro">muri</Termine> di ogni scadenza, più i
              bordi del valore del chart di ATAS (<Termine k="VAL">VAL</Termine>, <Termine k="VAH">VAH</Termine>,{' '}
              <Termine k="POC">POC</Termine>). I livelli a meno di 6 punti si fondono in una zona. Sono zone da guardare, non
              supporti o resistenze provati.
            </p>
          }
          sotto="Options Key Levels (observed e outlook) e GEX per strike dei market maker; le zone e l'ordine li calcola il server (Scala.cs). Sotto, la netta di ogni gruppo di scadenze, in unità di OptionsDepth."
        >
          <Esito q={qa} rotta="adesso" altezza={320}>
            {(d) =>
              d.scala.length ? (
                <>
                  <ScalaPrezzi righe={d.scala} prezzo={d.riferimento.prezzo} unita={f.unita} converti={P} compatta={telefono} />
                  <StrisciaScadenze gex={d.gex} />
                </>
              ) : (
                <Vuoto>Nessun livello: Options Key Levels non è aperto su ES.</Vuoto>
              )
            }
          </Esito>
        </Scheda>
        <Scheda
          titolo="Chi ha comprato delta oggi"
          fonti={FONTE}
          larghezza={12}
          breve="Il delta che ogni gruppo ha comprato o venduto da inizio giornata, sulle opzioni di oggi e su tutte."
          legenda={
            <Legenda
              voci={[
                { colore: C.compra, forma: 'rett', testo: 'comprato' },
                { colore: C.vende, forma: 'rett', testo: 'venduto' },
              ]}
            >
              <span className="nota">flusso sommato da inizio giornata, non posizione</span>
            </Legenda>
          }
          semplice={
            <p>
              Il <Termine k="DEX">DEX</Termine>: quanto rialzo (comprato) o ribasso (venduto) ogni gruppo si è caricato oggi con le
              opzioni su SPX, sugli <Termine k="0DTE">0DTE</Termine> e su tutte le scadenze, e quanto negli ultimi 30 minuti. È un
              flusso, non dice da solo dove va il prezzo.
            </p>
          }
          sotto="DEX per gruppo di OptionsDepth, minuto per minuto, sommato dall'inizio della giornata; unità di OptionsDepth."
        >
          <Esito q={qa} rotta="adesso" altezza={140}>
            {(d) => <TabellaDelta dex={d.dex} />}
          </Esito>
        </Scheda>
      </div>
    </>
  );
}

// ------------------------------------------------------------------ le tessere

/** Una riga di dettaglio che puo' finire in ellissi: il testo intero nel title. */
const conTitolo = (t: string) => <span title={t}>{t}</span>;

function Tessere({ a, qd, f, daEs }: { a: RispostaAdesso; qd: UseQueryResult<RispostaQuattroDomande>; f: Filtri; daEs: (x: number) => number }) {
  const r = a.riferimento;
  const P = f.p;
  const u = f.unita;
  const d = qd.data;
  const reg = d?.regime ?? null;

  // il regime: la lettura del server; se le quattro domande mancano, il segno della gamma al prezzo
  const ripiego = a.gamma.regime === 'freno' || a.gamma.regime === 'acceleratore' ? a.gamma.regime : null;
  const lettura = reg ? reg.lettura : ripiego;
  const valoreRegime = lettura == null ? '–' : lettura === 'segnali opposti' ? 'Segnali opposti' : lettura === 'freno' ? 'Freno' : 'Acceleratore';
  const tonoRegime: Tono | null = lettura === 'freno' || lettura === 'acceleratore' ? lettura : null;

  const fv = a.flipVicino;
  const m = d?.meteo ?? null;
  const mu = d?.muri ?? null;
  const vicino = mu?.vicini[0] ?? null;
  const pr = a.pressione;

  const versoMuro = (muro: string): Verso | null => (muro.startsWith('freno') ? 'freno' : muro.startsWith('acc') ? 'acceleratore' : null);
  const nomeMuro = (muro: string) => (muro.startsWith('freno') ? 'freno' : muro.startsWith('acc') ? 'acc.' : muro);

  return (
    <div className="griglia tessere x1">
      <Tessera
        etichetta="Prezzo"
        larghezza={2}
        fonte="xray"
        valore={num(P(r.prezzo), 2)}
        dettaglio={r.vivo ? `${u} · spot vivo` : r.fonte === 'ES meno la base' ? `${u} · cassa chiusa, ES − base` : `${u} · ${r.fonte}`}
      />
      <Tessera
        etichetta="Regime"
        larghezza={2}
        larga
        fonte="xray"
        tag="non provato"
        tono={tonoRegime}
        valore={valoreRegime}
        dettaglio={
          reg ? (
            <span className="segno-pastiglie">
              <Pastiglia testo="al prezzo" verso={reg.alPrezzo} />
              <Pastiglia testo="0DTE" verso={reg.zeroDte} />
              <Pastiglia testo="tutte" verso={reg.tutte} />
            </span>
          ) : a.gamma.valore == null ? (
            'profilo osservato assente'
          ) : (
            `gamma al prezzo ${segno(a.gamma.valore, 1)}`
          )
        }
      />
      <Tessera
        etichetta="Flip più vicino"
        larghezza={2}
        fonte="xray"
        valore={
          fv == null ? (
            'nessuno'
          ) : (
            <>
              {num(P(fv.prezzo), 0)}
              <span className="tessera-dist">{distanza(fv.distanza, 0)}</span>
            </>
          )
        }
        dettaglio={fv == null ? 'nessun flip tracciato adesso' : `sopra: ${fv.sopra ?? '–'} · sotto: ${fv.sotto ?? '–'}`}
      />
      <Tessera
        etichetta="Meteo"
        larghezza={2}
        fonte={m?.fonteLivello ?? 'yahoo'}
        tag={m ? null : 'solo dal vivo'}
        valore={m?.movimentoOggi != null ? `±${num(m.movimentoOggi, 0)}` : '–'}
        dettaglio={conTitolo(
          m
            ? [
                m.fasciaPrezzi ? `${num(daEs(m.fasciaPrezzi[0]), 0)}–${num(daEs(m.fasciaPrezzi[1]), 0)}` : null,
                m.livello != null ? `${m.nomeIndice} ${num(m.livello, 1)}${m.fascia ? ` ${m.fascia}` : ''}` : null,
                m.implicitaTesto,
              ]
                .filter(Boolean)
                .join(' · ')
            : f.passato
              ? 'il meteo c’è solo dal vivo'
              : (d?.errori[0] ?? 'nessun dato'),
        )}
      />
      <Tessera
        etichetta="Muri sul valore"
        larghezza={2}
        fonte="xray"
        tag="non provato"
        tono={vicino ? versoMuro(vicino.muro) : null}
        valore={vicino ? `${nomeMuro(vicino.muro)} ${num(daEs(vicino.prezzo), 0)}` : mu ? `nessuno entro ${num(mu.sogliaPunti, 0)}` : '–'}
        dettaglio={conTitolo(
          vicino
            ? `a ${num(vicino.distanza, 0)} dal ${vicino.bordo} ${num(daEs(vicino.prezzoBordo), 0)}${(mu?.vicini.length ?? 0) > 1 ? ` · +${(mu?.vicini.length ?? 1) - 1} altro` : ''}`
            : mu
              ? (mu.nota ?? 'punti dai bordi del valore (VAL, VAH, POC)')
              : 'muri e bordi del valore non disponibili',
        )}
      />
      <Tessera
        etichetta="Pressione 0DTE"
        larghezza={2}
        fonte="xray"
        valore={pr ? segno(pr.delta, 0) : '–'}
        dettaglio={pr ? `30 min fa ${segno(pr.trentaMinutiPrima, 0)} · ${pr.verso ?? 'verso non dichiarato'}` : 'nessun dato'}
      />
    </div>
  );
}

// ------------------------------------------------------------------ il profilo freno/acceleratore

const AMPIEZZE = [
  { valore: '1', testo: '±1%' },
  { valore: '2.5', testo: '±2,5%' },
  { valore: '5', testo: '±5%' },
];

/** La larghezza a destra per i cartellini: quanto serve al piu' lungo (11 px, 600). */
function margineCartellini(testi: readonly string[]): number {
  const lungo = Math.max(0, ...testi.map((t) => t.length));
  return Math.min(250, Math.max(64, Math.round(lungo * 6.6 + 16)));
}

/** I cartellini come markLine; sotto i 600 px dentro la griglia, sopra fuori, sul bordo destro. */
function cartellini(linee: LineaOrizzontale[], asse: [number, number], altezza: number, prezzo: number, stretto: boolean) {
  const ml = lineeOrizzontali(linee, { asse, altezzaPx: altezza, prezzo, fondi: 6, cifre: 0 });
  const dati = (ml.data ?? []) as { label?: Record<string, unknown> }[];
  const testi = dati.map((x) => String(x.label?.formatter ?? ''));
  if (stretto) {
    for (const x of dati) if (x.label) x.label = { ...x.label, position: 'insideEndTop' };
  }
  return { ml, destra: stretto ? 12 : margineCartellini(testi) };
}

interface PropsProfilo {
  qp: UseQueryResult<RispostaProfilo>;
  a: RispostaAdesso | undefined;
  qd: RispostaQuattroDomande | undefined;
  f: Filtri;
  daEs: (x: number) => number;
  ampiezza: string;
  cambiaAmpiezza: (a: string) => void;
}

function Profilo({ qp, a, qd, f, daEs, ampiezza, cambiaAmpiezza }: PropsProfilo) {
  const [cursore, setCursore] = useState<number | null>(null);
  const prof = qp.data?.presente ? qp.data : null;
  const P = f.p;

  const calcolo = useMemo(() => {
    if (!prof || !prof.prezzi.length) return null;
    const prezzi = prof.prezzi;
    const valori = prof.valori;
    const massimo = massimoAssoluto(valori) || 1;
    const lim = massimo * 1.1;
    const passo = passoDi(prezzi);
    const r = prof.riferimento.prezzo;
    const asse: [number, number] = [P(prezzi[0] ?? r) - passo, P(prezzi.at(-1) ?? r) + passo];
    const dentro = (y: number) => y >= asse[0] && y <= asse[1];

    const linee: LineaOrizzontale[] = [
      { y: P(r), colore: C.prezzo, spessore: 2, etichetta: `prezzo ${num(P(r), 2)}`, testo: `${f.unita} ${num(P(r), 2)}`, tipo: 'prezzo' },
    ];
    for (const z of prof.zeri) linee.push({ y: P(z), colore: C.flip, spessore: 1.5, tratto: 'dashed', etichetta: `flip ${num(P(z), 0)}`, testo: `FLIP ${num(P(z), 0)}`, tipo: 'flip' });
    const pp = a?.picchiPositivi[0];
    const pn = a?.picchiNegativi[0];
    if (pp) linee.push({ y: P(pp.strike), colore: C.freno, etichetta: `freno più forte ${num(P(pp.strike), 0)}`, testo: `FRENO PIÙ FORTE ${num(P(pp.strike), 0)}`, tipo: 'livello' });
    if (pn) linee.push({ y: P(pn.strike), colore: C.acceleratore, etichetta: `acceleratore più forte ${num(P(pn.strike), 0)}`, testo: `ACC. PIÙ FORTE ${num(P(pn.strike), 0)}`, tipo: 'livello' });
    const z = a?.gex.find((x) => x.scadenza === '0dte');
    if (z?.presente && z.muroFreno) {
      const y = P(z.muroFreno.strike);
      linee.push({ y, colore: C.freno, tratto: 'dotted', etichetta: `muro freno 0dte ${num(y, 0)}`, testo: `MURO FRENO 0DTE ${num(y, 0)}`, tipo: 'livello' });
    }
    if (z?.presente && z.muroAcceleratore) {
      const y = P(z.muroAcceleratore.strike);
      linee.push({ y, colore: C.acceleratore, tratto: 'dotted', etichetta: `muro acceleratore 0dte ${num(y, 0)}`, testo: `MURO ACC. 0DTE ${num(y, 0)}`, tipo: 'livello' });
    }
    for (const b of qd?.muri?.bordi ?? []) {
      if (b.tipo !== 'VAL' && b.tipo !== 'VAH' && b.tipo !== 'POC') continue;
      const y = daEs(b.prezzo);
      linee.push({ y, colore: C.bordoValore, tratto: 'dotted', etichetta: `${b.nome.toLowerCase()} ${num(y, 0)}`, testo: `${b.tipo} ${num(y, 0)}`, tipo: 'bordo' });
    }
    const visibili = linee.filter((l) => l.tipo === 'prezzo' || dentro(l.y));
    const fuori = linee.filter((l) => l.tipo !== 'prezzo' && !dentro(l.y));
    return { prezzi, valori, massimo, lim, passo, r, asse, visibili, fuori };
  }, [prof, a, qd, P, f.unita, daEs]);

  const puntatore = useMemo(() => {
    if (!calcolo) return undefined;
    const { prezzi, valori, r } = calcolo;
    const conv = prezzi.map(P);
    return puntatoreStrike(conv, valori, (i) => {
      const v = valori[i];
      const k = prezzi[i] ?? r;
      if (v == null) return null;
      return testaTip(`${f.unita} ${num(P(k), 0)} · ${distanza(k - r, 0)} dal prezzo`) + rigaTip(coloreSegno(v), `gamma · ${parolaSegno(v)}`, segno(v, 1));
    });
  }, [calcolo, P, f.unita]);

  const opzioni = useCallback(
    (larghezza: number): Opzioni | null => {
      if (!calcolo) return null;
      const { prezzi, valori, lim, passo, r, asse, visibili } = calcolo;
      const stretto = larghezza > 0 && larghezza < 600;
      const alto = altezzaPx('alto', { larghezza: window.innerWidth, altezza: window.innerHeight });
      const { ml, destra } = cartellini(visibili, asse, alto - 44, P(r), stretto);
      return {
        animation: false,
        grid: { ...griglia(larghezza, { destra: 'cartellino', sotto: 4 }), right: destra },
        xAxis: asseValori({
          min: -lim,
          max: lim,
          axisLabel: { color: C.spento, fontSize: 11, formatter: (v: number) => sig(v, 2), showMinLabel: false, showMaxLabel: false, hideOverlap: true },
        }),
        yAxis: assePrezzi(asse[0], asse[1], { passo: 5 }),
        tooltip: tooltip({ trigger: 'axis', axisPointer: croce('y', { y: (v) => num(v, 0) }), showContent: false }),
        series: [
          { ...serieBarre({ prezzi, valori, spessore: passo, scala: segnoGamma, nome: 'gamma', converti: P }), markLine: ml },
          serieLinee(asseZero('x'), 'zero'),
        ],
      };
    },
    [calcolo, P],
  );

  // la legenda: il valore al prezzo, o al prezzo sotto il cursore (la riga piu' vicina entro un passo)
  const sotto = useMemo(() => {
    if (!calcolo || cursore == null) return -1;
    let migliore = -1;
    let d = Infinity;
    calcolo.prezzi.forEach((k, i) => {
      const e = Math.abs(P(k) - cursore);
      if (e < d) {
        d = e;
        migliore = i;
      }
    });
    return d <= calcolo.passo * 0.6 ? migliore : -1;
  }, [calcolo, cursore, P]);
  const gammaQui = calcolo && sotto >= 0 ? (calcolo.valori[sotto] ?? null) : (a?.gamma.valore ?? prof?.alPrezzo ?? null);
  const prezzoQui = calcolo && sotto >= 0 ? (calcolo.prezzi[sotto] ?? null) : null;
  const fv = a?.flipVicino;
  const legenda = (
    <Legenda
      voci={[
        { colore: C.prezzo, testo: `prezzo ${f.unita}`, valore: a ? num(P(a.riferimento.prezzo), 2) : null },
        {
          colore: gammaQui == null ? C.neutro : coloreSegno(gammaQui),
          forma: 'rett',
          testo: prezzoQui == null ? 'gamma al prezzo' : `gamma a ${num(P(prezzoQui), 0)}`,
          valore: gammaQui == null ? null : segno(gammaQui, 1),
        },
        { colore: C.flip, tratto: 'dashed', testo: 'flip', valore: fv ? `${num(P(fv.prezzo), 0)} ${distanza(fv.distanza, 0)}` : null },
        { colore: C.freno, forma: 'rett', testo: 'freno' },
        { colore: C.acceleratore, forma: 'rett', testo: 'acceleratore' },
        { colore: C.bordoValore, tratto: 'dotted', testo: 'VAL · VAH · POC' },
      ]}
    />
  );

  return (
    <Scheda
      titolo="Freno e acceleratore, prezzo per prezzo"
      fonti={FONTE}
      larghezza={8}
      legenda={legenda}
      comandi={<Segmenti<string> etichetta="Ampiezza" valore={ampiezza} cambia={cambiaAmpiezza} opzioni={AMPIEZZE} />}
      breve="A ogni prezzo, se le coperture dei market maker frenano o accelerano il movimento."
      semplice={
        <p>
          A ogni prezzo, cosa fanno i market maker quando si coprono. Verde, <Termine k="freno">freno</Termine>: comprano se il prezzo
          scende e vendono se sale. Rosso, <Termine k="acceleratore">acceleratore</Termine>: vanno nella stessa direzione del prezzo. La
          linea bianca è il prezzo, le grigie tratteggiate sono i <Termine k="flip">flip</Termine>. Dice quanto si può allungare un
          movimento, non dove va; sui nostri dati (42 sedute di ES) il regime non ha allargato la giornata: non provato.
        </p>
      }
      sotto="Gamma osservata dei market maker (Strike Profile di OptionsDepth), in unità di OptionsDepth; linee: flip, picchi, muri dello 0DTE e bordi del valore della cassa (chart M30 di ATAS)."
    >
      <Esito q={qp} rotta="profilo" altezza={420}>
        {() =>
          calcolo ? (
            <div onMouseLeave={() => setCursore(null)}>
              <Grafico opzioni={opzioni} altezza="alto" etichetta="La gamma dei market maker prezzo per prezzo" puntatore={puntatore} onCursore={setCursore} cursoreSu="y" />
              {calcolo.fuori.length > 0 && (
                <p className="nota">
                  Fuori dal grafico: {calcolo.fuori.map((l) => `${l.etichetta} (${distanza(l.y - P(calcolo.r), 0)})`).join(' · ')}
                </p>
              )}
            </div>
          ) : (
            <Vuoto>Il profilo osservato non c’è: lo Strike Profile (gamma) non è aperto su ES.</Vuoto>
          )
        }
      </Esito>
    </Scheda>
  );
}

// ------------------------------------------------------------------ la scala: il regime per scadenza

function StrisciaScadenze({ gex }: { gex: Esposizione[] }) {
  return (
    <div className="striscia-scadenze" aria-label="Regime per scadenza">
      <span className="striscia-titolo">Regime per scadenza</span>
      {gex.map((x) =>
        x.presente ? (
          <Pastiglia key={x.scadenza} testo={scadenzaBreve(x.scadenza, x.dte)} valore={sig(x.netto, 2)} verso={versoDi(x.netto)} />
        ) : (
          <span key={x.scadenza} className="segno-pastiglia">
            {scadenzaBreve(x.scadenza, null)} –
          </span>
        ),
      )}
    </div>
  );
}

// ------------------------------------------------------------------ il delta per gruppo

const ORDINE_GRUPPI: DexGruppo['gruppo'][] = ['nonprocust', 'procust', 'firm', 'broker'];

function TabellaDelta({ dex }: { dex: DexGruppo[] }) {
  const righe = [...dex].sort((x, y) => ORDINE_GRUPPI.indexOf(x.gruppo) - ORDINE_GRUPPI.indexOf(y.gruppo));
  if (!righe.length) return <Vuoto>Nessun dato del delta per gruppo a quest’ora.</Vuoto>;
  const max0 = massimoAssoluto(righe.map((d) => d.zeroDte));
  const maxT = massimoAssoluto(righe.map((d) => d.tutte));
  const trenta = (v: number | null): ReactNode => (v == null ? '–' : `${v >= 0 ? '▲' : '▼'} ${sig(v, 2)}`);
  return (
    <div className="tabella-scorre tabella-flusso">
      <table>
        <thead>
          <tr>
            <th>Gruppo</th>
            <th>0DTE, da inizio giornata</th>
            <th data-priorita="2">0DTE, ultimi 30 min</th>
            <th>Tutte le scadenze</th>
            <th data-priorita="2">Tutte, ultimi 30 min</th>
          </tr>
        </thead>
        <tbody>
          {righe.map((d) => (
            <tr key={d.gruppo}>
              <td className="nome-gruppo">{nome(d.gruppo)}</td>
              <td>
                <BarraInline valore={d.zeroDte} massimo={max0} scala={segnoFlusso} />
              </td>
              <td className="trenta" data-priorita="2">
                {trenta(d.zeroDteTrentaMinuti)}
              </td>
              <td>
                <BarraInline valore={d.tutte} massimo={maxT} scala={segnoFlusso} />
              </td>
              <td className="trenta" data-priorita="2">
                {trenta(d.tutteTrentaMinuti)}
              </td>
            </tr>
          ))}
        </tbody>
      </table>
    </div>
  );
}
