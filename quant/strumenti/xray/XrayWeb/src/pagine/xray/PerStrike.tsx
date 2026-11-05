// Per strike: l'esposizione di una scadenza, strike per strike, attorno al prezzo (C.4). In cima quattro
// tessere (la netta, entro l'1%, i due muri o i due estremi del flusso), poi il profilo grande con prezzo,
// flip e muri, e accanto gli strike vicini al prezzo, sopra e sotto. Il colore dice solo il segno: freno e
// acceleratore per la GEX, comprato e venduto per DEX e VEX.

import { useCallback, useMemo, useState, type ReactNode } from 'react';
import type { UseQueryResult } from '@tanstack/react-query';
import { useMisura } from '../../api/domande';
import type { Gruppo, RispostaStrike } from '../../api/tipi';
import { distanza, doppiaOra, nome, num, sig } from '../../formato';
import { useFiltri, type Filtri } from '../../stato/contesto';
import { useScelte } from '../../stato/scelte';
import { C } from '../../tema/colori';
import { Grafico, altezzaPx } from '../../componenti/Grafico';
import { Legenda, type Voce } from '../../componenti/Legenda';
import { Scheda } from '../../componenti/Scheda';
import { Tessera, type Tono } from '../../componenti/Tessera';
import { Esito, Vuoto } from '../../componenti/Esito';
import { Divisore, Segmenti, Tendina, type Opzione } from '../../componenti/Comandi';
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
  type ScalaSegno,
} from '../../viz/grafici';
import './xray1.css';

type MetricaS = 'gex' | 'dex' | 'vex';
type ScadenzaS = '0dte' | 'next_expiry' | 'opex' | 'all';
type Vista = 'netto' | 'callput';

interface Scelte {
  metrica: MetricaS;
  gruppo: Gruppo;
  scadenza: ScadenzaS;
  vista: Vista;
  /** l'ampiezza scelta dall'utente per ogni scadenza; se manca, quella di default della scadenza */
  ampiezze: Partial<Record<ScadenzaS, string>>;
}

/** L'ampiezza di default: piu' la scadenza e' lontana, piu' la finestra si allarga. */
export const AMPIEZZA_DEFAULT: Record<ScadenzaS, string> = { '0dte': '1', next_expiry: '2', opex: '3', all: '2' };

const GRUPPI: Opzione<Gruppo>[] = [
  { valore: 'nonprocust', testo: 'Retail' },
  { valore: 'procust', testo: 'Clienti professionali' },
  { valore: 'firm', testo: 'Firm' },
  { valore: 'broker', testo: 'Broker-dealer' },
];

const AMPIEZZE: Opzione<string>[] = ['1', '2', '3', '5'].map((a) => ({ valore: a, testo: `±${a}%` }));

const FONTE = ['xray'] as const;

const scalaDi = (m: MetricaS): ScalaSegno => (m === 'gex' ? segnoGamma : segnoFlusso);
const tonoDi = (v: number | null | undefined, m: MetricaS): Tono | null =>
  v == null || !Number.isFinite(v) ? null : m === 'gex' ? (v >= 0 ? 'freno' : 'acceleratore') : v >= 0 ? 'compra' : 'vende';

export function PerStrike() {
  const f = useFiltri();
  const [s, setS] = useScelte<Scelte>('strike', { metrica: 'gex', gruppo: 'nonprocust', scadenza: '0dte', vista: 'netto', ampiezze: {} });
  const ampiezza = s.ampiezze[s.scadenza] ?? AMPIEZZA_DEFAULT[s.scadenza];
  const q = useMisura<RispostaStrike>(
    'strike',
    { ...f.parametri, metrica: s.metrica, gruppo: s.metrica === 'dex' ? s.gruppo : 'mm', scadenza: s.scadenza, ampiezza },
    f.vivo,
    f.pronto,
  );

  const scegliMetrica = (m: MetricaS) =>
    setS((x) => ({
      ...x,
      metrica: m,
      scadenza: m === 'dex' ? (x.scadenza === '0dte' || x.scadenza === 'all' ? x.scadenza : '0dte') : x.scadenza === 'all' ? '0dte' : x.scadenza,
    }));

  const scadenze: Opzione<ScadenzaS>[] =
    s.metrica === 'dex'
      ? [
          { valore: '0dte', testo: '0DTE' },
          { valore: 'all', testo: 'Tutte' },
        ]
      : [
          { valore: '0dte', testo: '0DTE' },
          { valore: 'next_expiry', testo: 'Prossima' },
          { valore: 'opex', testo: 'Opex' },
        ];

  const comandi = (
    <>
      <span className="comandi-tag">
        <Segmenti<MetricaS>
          etichetta="Misura"
          valore={s.metrica}
          cambia={scegliMetrica}
          opzioni={[
            { valore: 'gex', testo: 'GEX' },
            { valore: 'dex', testo: 'DEX' },
            { valore: 'vex', testo: 'VEX' },
          ]}
        />
        {s.metrica === 'vex' && <span className="tag">sperimentale</span>}
      </span>
      <Divisore />
      <Segmenti<ScadenzaS> etichetta="Scadenza" valore={s.scadenza} cambia={(v) => setS((x) => ({ ...x, scadenza: v }))} opzioni={scadenze} />
      {s.metrica === 'dex' && <Tendina<Gruppo> etichetta="Gruppo" valore={s.gruppo} cambia={(g) => setS((x) => ({ ...x, gruppo: g }))} opzioni={GRUPPI} />}
      <Divisore />
      <Segmenti<Vista>
        etichetta="Vista"
        valore={s.vista}
        cambia={(v) => setS((x) => ({ ...x, vista: v }))}
        opzioni={[
          { valore: 'netto', testo: 'Netto' },
          { valore: 'callput', testo: 'Call e put' },
        ]}
      />
      <Divisore />
      <Segmenti<string>
        etichetta="Ampiezza"
        valore={ampiezza}
        cambia={(v) => setS((x) => ({ ...x, ampiezze: { ...x.ampiezze, [x.scadenza]: v } }))}
        opzioni={AMPIEZZE}
      />
    </>
  );

  return (
    <>
      <Esito q={q} rotta="strike" altezza={72}>
        {(d) => <Tessere d={d} m={s.metrica} f={f} />}
      </Esito>
      <div className="griglia">
        <Profilo q={q} s={s} f={f} comandi={comandi} />
        <Scheda
          titolo="Strike vicini al prezzo"
          fonti={FONTE}
          larghezza={4}
          breve="Gli strike subito sopra e subito sotto il prezzo, dal più vicino; in grassetto i tre più pesanti."
          semplice="La stessa misura del grafico, in tabella: prima gli strike sopra il prezzo, poi quelli sotto, ciascuno dal più vicino al più lontano. La barra va a destra se il valore è positivo, a sinistra se è negativo."
          sotto="Distanza in punti dallo strike al prezzo; i tre più pesanti per valore assoluto del netto sono in grassetto. Call e put nella vista «Call e put», a 2 cifre significative."
        >
          <Esito q={q} rotta="strike" altezza={320}>
            {(d) => <StrikeVicini d={d} s={s} f={f} />}
          </Esito>
        </Scheda>
      </div>
    </>
  );
}

// ------------------------------------------------------------------ le tessere

function Tessere({ d, m, f }: { d: RispostaStrike; m: MetricaS; f: Filtri }) {
  if (!d.presente) return null;
  const P = f.p;
  const r = d.riferimento.prezzo;
  const gex = m === 'gex';
  const tag = m === 'vex' ? 'sperimentale' : null;
  const muro = (x: { strike: number; valore: number | null } | null) =>
    x == null ? (
      'nessuno'
    ) : (
      <>
        {num(P(x.strike), 0)}
        <span className="tessera-dist">{distanza(x.strike - r, 0)}</span>
      </>
    );
  const dettaglioMuro = (x: { strike: number; valore: number | null } | null, parola: string) =>
    x == null ? 'nessuno entro il 5% dal prezzo' : `${nome(m)} ${sig(x.valore, 2)} · ${parola} entro il 5%`;
  return (
    <div className="griglia tessere x1">
      <Tessera
        etichetta="Netta della scadenza"
        larghezza={3}
        fonte="xray"
        tag={tag}
        tono={tonoDi(d.totale.netto, m)}
        valore={sig(d.totale.netto, 2)}
        dettaglio={`call ${sig(d.totale.call, 2)} · put ${sig(d.totale.put, 2)} · ${doppiaOra(d.istante)}`}
      />
      <Tessera
        etichetta="Entro l’1%"
        larghezza={3}
        fonte="xray"
        tag={tag}
        tono={tonoDi(d.entroUnoPercento, m)}
        valore={sig(d.entroUnoPercento, 2)}
        dettaglio="solo gli strike a meno dell’1% dal prezzo"
      />
      <Tessera
        etichetta={gex ? 'Muro freno' : 'Più comprato'}
        larghezza={3}
        fonte="xray"
        tag={gex ? 'non provato' : tag}
        tono={gex ? 'freno' : 'compra'}
        valore={muro(d.muri.positivo)}
        dettaglio={dettaglioMuro(d.muri.positivo, gex ? 'il freno più forte' : 'il più comprato')}
      />
      <Tessera
        etichetta={gex ? 'Muro acceleratore' : 'Più venduto'}
        larghezza={3}
        fonte="xray"
        tag={gex ? 'non provato' : tag}
        tono={gex ? 'acceleratore' : 'vende'}
        valore={muro(d.muri.negativo)}
        dettaglio={dettaglioMuro(d.muri.negativo, gex ? 'l’acceleratore più forte' : 'il più venduto')}
      />
    </div>
  );
}

// ------------------------------------------------------------------ il profilo

const SPIEGA: Record<MetricaS, string> = {
  gex: 'quanta gamma hanno i market maker su ogni strike. Verde: lì frenano il prezzo; rosso: lì lo accelerano. Una barra lunga vicina al prezzo è uno strike che pesa; dice quanto, non dove va il prezzo.',
  vex: 'quanto cambia la copertura dei market maker se cambia la volatilità implicita (la vanna). Blu: comprerebbero; arancio: venderebbero. Misura sperimentale: non la leggiamo ancora.',
  dex: 'il delta che il gruppo ha scambiato su ogni strike, sommato dall’inizio della giornata. Blu: il gruppo ha comprato; arancio: ha venduto. È un flusso, non una posizione.',
};

const TERMINE: Record<MetricaS, 'GEX' | 'DEX' | 'VEX'> = { gex: 'GEX', dex: 'DEX', vex: 'VEX' };

function margineCartellini(testi: readonly string[]): number {
  const lungo = Math.max(0, ...testi.map((t) => t.length));
  return Math.min(250, Math.max(64, Math.round(lungo * 6.6 + 16)));
}

/** Le tacche dell'asse degli strike: un multiplo del passo, almeno 16 px fra due etichette. */
function intervalloStrike(passo: number, n: number, altezza: number): number {
  const k = Math.max(1, Math.ceil((n * 16) / Math.max(200, altezza)));
  const tondi = [1, 2, 4, 5, 10, 20, 40, 50, 100];
  return passo * (tondi.find((t) => t >= k) ?? k);
}

function Profilo({ q, s, f, comandi }: { q: UseQueryResult<RispostaStrike>; s: Scelte; f: Filtri; comandi: ReactNode }) {
  const [cursore, setCursore] = useState<number | null>(null);
  const d = q.data?.presente ? q.data : null;
  const P = f.p;
  const scala = scalaDi(s.metrica);
  const gex = s.metrica === 'gex';

  const calcolo = useMemo(() => {
    if (!d || !d.strike.length) return null;
    const passo = passoDi(d.strike);
    const r = d.riferimento.prezzo;
    const netto = s.vista === 'netto';
    const massimo = massimoAssoluto(netto ? d.netto : [...d.call, ...d.put]) || 1;
    // lo spazio oltre la barra piu' lunga per la sua etichetta diretta
    const lim = massimo * (netto ? 1.3 : 1.1);
    const lo = Math.min(...d.strike);
    const hi = Math.max(...d.strike);
    const asse: [number, number] = [P(lo) - passo, P(hi) + passo];
    const dentro = (y: number) => y >= asse[0] && y <= asse[1];
    const linee: LineaOrizzontale[] = [
      { y: P(r), colore: C.prezzo, spessore: 2, etichetta: `prezzo ${num(P(r), 2)}`, testo: `${f.unita} ${num(P(r), 2)}`, tipo: 'prezzo' },
    ];
    if (gex) for (const z of d.flip) linee.push({ y: P(z), colore: C.flip, spessore: 1.5, tratto: 'dashed', etichetta: `flip ${num(P(z), 0)}`, testo: `FLIP ${num(P(z), 0)}`, tipo: 'flip' });
    const pos = d.muri.positivo;
    const neg = d.muri.negativo;
    const parole = gex ? ['muro freno', 'muro acceleratore'] : ['più comprato', 'più venduto'];
    if (pos) linee.push({ y: P(pos.strike), colore: scala.pos, tratto: 'dotted', etichetta: `${parole[0]} ${num(P(pos.strike), 0)}`, testo: `${parole[0]!.toUpperCase()} ${num(P(pos.strike), 0)}`, tipo: 'livello' });
    if (neg) linee.push({ y: P(neg.strike), colore: scala.neg, tratto: 'dotted', etichetta: `${parole[1]} ${num(P(neg.strike), 0)}`, testo: `${parole[1]!.toUpperCase()} ${num(P(neg.strike), 0)}`, tipo: 'livello' });
    // le tre barre piu' lunghe, con l'etichetta diretta
    const pesanti = d.strike
      .map((k, i) => ({ k, v: d.netto[i] ?? null }))
      .filter((x): x is { k: number; v: number } => x.v != null)
      .sort((a, b) => Math.abs(b.v) - Math.abs(a.v))
      .slice(0, 3);
    return { passo, r, netto, massimo, lim, asse, visibili: linee.filter((l) => l.tipo === 'prezzo' || dentro(l.y)), pesanti };
  }, [d, s.vista, P, f.unita, gex, scala]);

  const puntatore = useMemo(() => {
    if (!d || !calcolo) return undefined;
    const r = calcolo.r;
    return puntatoreStrike(d.strike.map(P), d.netto, (i) => {
      setCursore(i);
      const k = d.strike[i] ?? r;
      const v = d.netto[i] ?? null;
      return (
        testaTip(`strike ${num(P(k), 0)} ${f.unita} · ${distanza(k - r, 0)} dal prezzo`) +
        rigaTip(v == null ? null : coloreSegno(v, scala), v == null ? 'netto' : `netto · ${parolaSegno(v, scala)}`, sig(v, 2)) +
        rigaTip(null, 'call', sig(d.call[i], 2)) +
        rigaTip(null, 'put', sig(d.put[i], 2))
      );
    });
  }, [d, calcolo, P, f.unita, scala]);

  const opzioni = useCallback(
    (larghezza: number): Opzioni | null => {
      if (!d || !calcolo) return null;
      const { passo, r, netto, lim, asse, visibili, pesanti } = calcolo;
      const stretto = larghezza > 0 && larghezza < 600;
      const alto = altezzaPx('alto', { larghezza: window.innerWidth, altezza: window.innerHeight });
      const ml = lineeOrizzontali(visibili, { asse, altezzaPx: alto - 44, prezzo: P(r), fondi: 6, cifre: 0 });
      const dati = (ml.data ?? []) as { label?: Record<string, unknown> }[];
      const destra = stretto ? 12 : margineCartellini(dati.map((x) => String(x.label?.formatter ?? '')));
      if (stretto) for (const x of dati) if (x.label) x.label = { ...x.label, position: 'insideEndTop' };
      const barre = netto
        ? [{ ...serieBarre({ prezzi: d.strike, valori: d.netto, spessore: passo, scala, nome: 'netto', converti: P }), markLine: ml }]
        : [
            { ...serieBarre({ prezzi: d.strike.map((x) => x + passo * 0.2), valori: d.call, spessore: passo * 0.5, scala, nome: 'call', converti: P }), markLine: ml },
            serieBarre({ prezzi: d.strike.map((x) => x - passo * 0.2), valori: d.put, spessore: passo * 0.5, scala, nome: 'put', converti: P, opacita: () => 0.45, bordo: true }),
          ];
      const n = d.strike.length;
      return {
        animation: false,
        grid: { ...griglia(larghezza, { destra: 'cartellino', sotto: 4 }), right: destra },
        xAxis: asseValori({
          min: -lim,
          max: lim,
          axisLabel: { color: C.spento, fontSize: 11, formatter: (v: number) => sig(v, 2), showMinLabel: false, showMaxLabel: false, hideOverlap: true },
        }),
        yAxis: assePrezzi(asse[0], asse[1], { interval: intervalloStrike(passo, n, alto) }),
        tooltip: tooltip({ trigger: 'axis', axisPointer: croce('y', { y: (v) => num(v, 0) }), showContent: false }),
        series: [
          ...barre,
          serieLinee(asseZero('x'), 'zero'),
          // le etichette dirette sulle tre barre piu' lunghe: "7.764 · +14,9"
          {
            type: 'scatter',
            name: 'più pesanti',
            symbolSize: 0,
            silent: true,
            z: 5,
            // sotto i 600 px niente etichette dirette: coprirebbero i cartellini (i numeri stanno nella tabella)
            data: netto && !stretto
              ? pesanti.map((x) => ({
                  value: [x.v, P(x.k)],
                  label: {
                    show: true,
                    position: x.v >= 0 ? 'right' : 'left',
                    formatter: `${num(P(x.k), 0)} · ${sig(x.v, 2)}`,
                    color: C.testo,
                    fontSize: 11,
                    fontWeight: 600,
                    distance: 4,
                    backgroundColor: C.superficie,
                    padding: [1, 3],
                  },
                }))
              : [],
          },
        ],
      };
    },
    [d, calcolo, P, scala],
  );

  const voci: Voce[] = [];
  if (calcolo && d) {
    const i = cursore;
    const v = i != null ? (d.netto[i] ?? null) : null;
    voci.push({ colore: C.prezzo, testo: `prezzo ${f.unita}`, valore: num(P(calcolo.r), 2) });
    if (i != null) voci.push({ colore: v == null ? C.neutro : coloreSegno(v, scala), forma: 'rett', testo: `strike ${num(P(d.strike[i] ?? 0), 0)}`, valore: sig(v, 2) });
    if (gex) {
      const fl = [...d.flip].sort((a, b) => Math.abs(a - calcolo.r) - Math.abs(b - calcolo.r))[0];
      voci.push({ colore: C.flip, tratto: 'dashed', testo: 'flip', valore: fl == null ? null : `${num(P(fl), 0)} ${distanza(fl - calcolo.r, 0)}` });
    }
    voci.push({ colore: scala.pos, forma: 'rett', testo: scala.parolaPos });
    voci.push({ colore: scala.neg, forma: 'rett', testo: scala.parolaNeg });
    if (!calcolo.netto) voci.push({ colore: C.testo2, forma: 'rett', testo: 'call (piena) · put (tenue)' });
  }

  return (
    <Scheda
      titolo="Esposizione per strike"
      fonti={FONTE}
      larghezza={8}
      comandi={comandi}
      legenda={voci.length ? <Legenda voci={voci} /> : undefined}
      breve={
        gex
          ? 'Su ogni strike, se i market maker frenano o accelerano il prezzo.'
          : s.metrica === 'dex'
            ? 'Su ogni strike, quanto delta il gruppo ha comprato o venduto oggi.'
            : 'Su ogni strike, cosa farebbero i market maker se la volatilità implicita cambiasse.'
      }
      semplice={
        <p>
          <Termine k={TERMINE[s.metrica]}>{TERMINE[s.metrica]}</Termine>: {SPIEGA[s.metrica]}
        </p>
      }
      sotto={
        d
          ? `${s.metrica === 'dex' ? `${nome(s.gruppo)} · ` : ''}${nome(s.scadenza)}${d.dte > 0 ? ` (${d.dte} giorni)` : ''} · dato delle ${doppiaOra(d.istante)}${s.metrica === 'dex' ? ', sommato dall’inizio della giornata' : ''}. Unità di OptionsDepth; muri = il netto più positivo e più negativo entro il 5% dal prezzo.`
          : undefined
      }
    >
      <Esito q={q} rotta="strike" altezza={420}>
        {(x) =>
          x.presente && x.strike.length ? (
            <div onMouseLeave={() => setCursore(null)}>
              <Grafico opzioni={opzioni} altezza="alto" etichetta="Esposizione per strike" puntatore={puntatore} />
            </div>
          ) : (
            <Vuoto>
              Nessun dato per {x.famiglia} a quest’ora: l’indicatore non era aperto su ES, o (sugli 0DTE prima della cassa) non ci sono
              ancora scambi.
            </Vuoto>
          )
        }
      </Esito>
    </Scheda>
  );
}

// ------------------------------------------------------------------ gli strike vicini al prezzo

const PER_BLOCCO = 15;

function StrikeVicini({ d, s, f }: { d: RispostaStrike; s: Scelte; f: Filtri }) {
  if (!d.presente || !d.strike.length) return <Vuoto>Nessun dato.</Vuoto>;
  const P = f.p;
  const r = d.riferimento.prezzo;
  const scala = scalaDi(s.metrica);
  const callput = s.vista === 'callput';
  const righe = d.strike.map((k, i) => ({ k, n: d.netto[i] ?? null, c: d.call[i] ?? null, p: d.put[i] ?? null }));
  const pesanti = new Set(
    [...righe]
      .filter((x) => x.n != null)
      .sort((a, b) => Math.abs(b.n ?? 0) - Math.abs(a.n ?? 0))
      .slice(0, 3)
      .map((x) => x.k),
  );
  const massimo = massimoAssoluto(righe.map((x) => x.n));
  const sopra = righe.filter((x) => x.k > r).sort((a, b) => a.k - b.k).slice(0, PER_BLOCCO);
  const sotto = righe.filter((x) => x.k <= r).sort((a, b) => b.k - a.k).slice(0, PER_BLOCCO);
  const colonne = callput ? 5 : 3;
  const riga = (x: (typeof righe)[number]) => (
    <tr key={x.k}>
      <td className={pesanti.has(x.k) ? 'forte' : undefined}>{num(P(x.k), 0)}</td>
      <td>{distanza(x.k - r, 0)}</td>
      <td>
        <BarraInline valore={x.n} massimo={massimo} scala={scala} />
      </td>
      {callput && <td>{sig(x.c, 2)}</td>}
      {callput && <td>{sig(x.p, 2)}</td>}
    </tr>
  );
  return (
    <div className="tabella-scorre strike-vicini">
      <table>
        <thead>
          <tr>
            <th>Strike {f.unita}</th>
            <th>Dist.</th>
            <th>Netto</th>
            {callput && <th>Call</th>}
            {callput && <th>Put</th>}
          </tr>
        </thead>
        <tbody>
          <tr className="blocco">
            <td colSpan={colonne}>Sopra, dal più vicino</td>
          </tr>
          {sopra.length ? sopra.map(riga) : (
            <tr>
              <td colSpan={colonne} className="vuota">
                nessuno strike sopra nella finestra
              </td>
            </tr>
          )}
          <tr className="riga-prezzo">
            <td>{num(P(r), 2)}</td>
            <td colSpan={colonne - 1} className="testo">
              prezzo {f.unita}
            </td>
          </tr>
          <tr className="blocco">
            <td colSpan={colonne}>Sotto, dal più vicino</td>
          </tr>
          {sotto.length ? sotto.map(riga) : (
            <tr>
              <td colSpan={colonne} className="vuota">
                nessuno strike sotto nella finestra
              </td>
            </tr>
          )}
        </tbody>
      </table>
    </div>
  );
}
