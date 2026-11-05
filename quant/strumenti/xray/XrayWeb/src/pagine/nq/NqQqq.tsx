// NQ · QQQ, la vista Exposure (C.11): in cima sei tessere (regime, zero gamma, zero 0DTE, muri, movimento
// atteso), al centro il profilo GEX per strike nel segno (verde freno, rosso acceleratore; la fonte la
// dice il badge, e "segno supposto" resta sempre in vista), accanto la scala dei livelli, sotto la GEX
// viva nel tempo. Il regime viene da /api/quattro-domande?radice=NQ: la pagina e il giro dicono la
// stessa frase. Tutti i prezzi sono di NQ; QQQ e NDX sono portati su NQ dal server.

import { useCallback, useMemo } from 'react';
import { Link } from 'react-router';
import { useMediaQuery } from '@mantine/hooks';
import { useGexTempo, useNqGex, useQuattroDomande } from '../../api/domande';
import type { GexFonte, PrezzoQqq, RispostaNqGex, RispostaQuattroDomande } from '../../api/tipi';
import { distanza, doppiaOra, etaLunga, num, segno, sig } from '../../formato';
import { Tessera } from '../../componenti/Tessera';
import { useFiltri } from '../../stato/contesto';
import { useScelte } from '../../stato/scelte';
import { useStatoFonte } from '../../stato/fonti';
import { C } from '../../tema/colori';
import { Grafico, type Altezza } from '../../componenti/Grafico';
import { Legenda, type Voce } from '../../componenti/Legenda';
import { FonteBadge } from '../../componenti/FonteBadge';
import { Scheda } from '../../componenti/Scheda';
import { ScalaPrezzi } from '../../componenti/ScalaPrezzi';
import { Termine } from '../../componenti/Termine';
import { Esito, Vuoto } from '../../componenti/Esito';
import { Divisore, Segmenti } from '../../componenti/Comandi';
import type { Opzioni } from '../../viz/echarts';
import {
  asseValori,
  assePrezzi,
  asseZero,
  coloreSegno,
  croce,
  lineeOrizzontali,
  parolaSegno,
  passoDi,
  puntatoreStrike,
  rigaTip,
  serieBarre,
  serieLinee,
  testaTip,
  tooltip,
  type LineaOrizzontale,
} from '../../viz/grafici';
import { GexNelTempo } from './GexNelTempo';
import './nq.css';

type Ambito = 'tutte' | '0dte';
type Finestra = 'vicino' | 'tutto';
/** quale profilo si vede grande: l'id della fonte (gex-nq, gex-qqq, gex-ndx) o tutti affiancati */
type Vista = string;
const TUTTI = 'tutti';

/** I muri e lo zero vicini fra loro meno di questi punti di NQ diventano un cartellino solo (E.3). */
const FONDI_NQ = 25;

const SESSIONE: Record<PrezzoQqq['sessione'], string> = {
  pre: 'pre-mercato',
  cassa: 'in cassa',
  dopo: 'dopo-mercato',
  chiuso: 'mercato chiuso',
};

const ALTEZZA_PICCOLO: Altezza = { min: 200, max: 260, schermo: 0.28 };

const RISERVA_NQ =
  'Il segno dei market maker su NQ è supposto (convenzione A: lunghi le call, corti le put), non misurato. Il regime dice quanto si può allungare un movimento, non dove va il prezzo; nessun livello di questa pagina è provato come supporto o resistenza.';

export function NqQqq() {
  const f = useFiltri();
  const [s, setS] = useScelte<{ ambito: Ambito; vista: Vista; finestra: Finestra }>('nq', { ambito: 'tutte', vista: 'gex-nq', finestra: 'vicino' });
  const q = useNqGex(s.ambito);
  const qd = useQuattroDomande('NQ', {}, true);
  const qt = useGexTempo('NQ', f.giorno);
  const telefono = useMediaQuery('(max-width: 767px)') ?? false;

  const fonti = q.data?.fonti ?? [];
  const ids = fonti.map((x) => x.id);
  const vista = s.vista === TUTTI || ids.includes(s.vista) ? s.vista : (ids[0] ?? TUTTI);

  const comandi = (
    <>
      <Segmenti<Ambito>
        etichetta="Scadenze"
        valore={s.ambito}
        cambia={(a) => setS((p) => ({ ...p, ambito: a }))}
        opzioni={[
          { valore: 'tutte', testo: 'Tutte le scadenze' },
          { valore: '0dte', testo: 'Solo oggi (0DTE)' },
        ]}
      />
      <Divisore />
      <Segmenti<Vista>
        etichetta="Profilo"
        valore={vista}
        cambia={(v) => setS((p) => ({ ...p, vista: v }))}
        opzioni={[...fonti.map((x) => ({ valore: x.id, testo: x.nome.replace(/ \(.*$/, '') })), { valore: TUTTI, testo: 'Tutti affiancati' }]}
      />
      <Divisore />
      <Segmenti<Finestra>
        etichetta="Finestra"
        valore={s.finestra}
        cambia={(v) => setS((p) => ({ ...p, finestra: v }))}
        opzioni={[
          { valore: 'vicino', testo: 'Vicino al prezzo' },
          { valore: 'tutto', testo: 'Tutto' },
        ]}
      />
    </>
  );

  return (
    <>
      <div className="griglia tessere">
        <Tessere d={qd.data} nq={q.data} />
      </div>
      <div className="griglia nq">
        <Scheda
          titolo="Profilo GEX per strike, in prezzi di NQ"
          fonti={['catena', 'cboe']}
          larghezza={8}
          comandi={comandi}
          riserva="segno supposto"
          riservaIntera={qd.data?.regime?.provato ? `${RISERVA_NQ} Il regime: ${qd.data.regime.provato}.` : RISERVA_NQ}
          breve="Le barre a destra frenano il prezzo, quelle a sinistra lo accelerano; lo zero gamma è dove si passa dall’uno all’altro."
          semplice={
            <p>
              La <Termine k="GEX">GEX</Termine> dice quanto i market maker devono comprare o vendere quando il prezzo si muove dell’1%. Verde
              = <Termine k="freno">freno</Termine>, rosso = <Termine k="acceleratore">acceleratore</Termine>, nella{' '}
              <Termine k="convenzione A">convenzione A</Termine>. Scegli la fonte: il suo profilo si vede grande, gli altri restano piccoli
              accanto. Ogni profilo ha la sua scala in orizzontale: si confrontano forma e livelli, non i numeri.
            </p>
          }
          sotto="Milioni di $ per 1% di movimento. “Vicino al prezzo” = il prezzo ± 2 movimenti attesi della giornata, dato dal server. QQQ e NDX sono portati su NQ con la conversione scritta nella “i” dei livelli."
          legenda={<LegendaProfilo d={q.data} vista={vista} />}
        >
          <Esito q={q} rotta="nq/gex" altezza={480}>
            {(d) => <Profili d={d} vista={vista} finestra={s.finestra} cambia={(v) => setS((p) => ({ ...p, vista: v }))} />}
          </Esito>
        </Scheda>
        <Scheda
          titolo="I livelli"
          fonti={['catena', 'cboe']}
          larghezza={4}
          breve="Tutti i livelli in ordine di prezzo, con la distanza dal prezzo e quanto è vecchia la fonte."
          semplice={
            <p>
              Zero gamma, <Termine k="call wall">call wall</Termine> e <Termine k="put wall">put wall</Termine> di tutte le fonti, più i
              bordi del valore del chart (<Termine k="VAL">VAL</Termine>, <Termine k="VAH">VAH</Termine>, <Termine k="POC">POC</Termine>,{' '}
              <Termine k="ping-pong">ping-pong</Termine>). I livelli entro 25 punti diventano una zona.
            </p>
          }
          sotto={q.data ? <Conversioni d={q.data} /> : undefined}
          legenda={
            <Legenda
              voci={[
                { colore: C.freno, testo: 'call wall', forma: 'punto' },
                { colore: C.acceleratore, testo: 'put wall', forma: 'punto' },
                { colore: C.flip, testo: 'zero gamma', forma: 'punto' },
                { colore: C.bordoValore, testo: 'bordo del valore', tratto: 'dotted' },
              ]}
            />
          }
        >
          <Esito q={q} rotta="nq/gex" altezza={360}>
            {(d) =>
              d.scala?.length && d.prezzoNQ != null ? (
                <>
                  <ScalaPrezzi righe={d.scala} prezzo={d.prezzoNQ} unita="NQ" mostraQuando={false} mostraFonte compatta={telefono} />
                  <ViciniAlBordo d={qd.data} />
                </>
              ) : (
                <Vuoto>La scala dei livelli non c’è ancora.</Vuoto>
              )
            }
          </Esito>
        </Scheda>
        <Scheda
          titolo="La GEX viva di NQ nel tempo"
          fonti={['catena']}
          larghezza={12}
          riserva="segno supposto"
          riservaIntera={RISERVA_NQ}
          breve="Minuto per minuto, dove sta il prezzo rispetto allo zero gamma e ai muri; sotto, quando la GEX cambia segno."
          semplice="Sopra lo zero gamma i market maker (se la convenzione A è giusta) frenano, sotto accelerano. Lo 0DTE si sposta di più, perché le sue quotazioni cambiano di continuo. Le barre in basso sono la GEX totale (piene) e quella 0DTE (tenui)."
          sotto="Lo 0DTE si rilegge ogni minuto, le altre scadenze a rotazione; l'open interest è quello di ieri. Due strike quasi alla pari possono scambiarsi il ruolo di muro. Un muro lontano esce dal grafico invece di schiacciarlo."
        >
          <Esito q={qt} rotta="gex/tempo" altezza={360}>
            {(d) => <GexNelTempo d={d} />}
          </Esito>
        </Scheda>
      </div>
    </>
  );
}

// ------------------------------------------------------------------ le tessere

const LETTURA: Record<'freno' | 'acceleratore' | 'segnali opposti', string> = {
  freno: 'Freno',
  acceleratore: 'Acceleratore',
  'segnali opposti': 'Segnali opposti',
};

/** Il livello con la distanza dal prezzo: "28.851 ↑520". */
function ConDistanza({ v, prezzo }: { v: number | null | undefined; prezzo: number | null | undefined }) {
  if (v == null) return <>–</>;
  return (
    <>
      {num(v, 0)}
      {prezzo != null && <span className="valore-dist"> {distanza(v - prezzo, 0)}</span>}
    </>
  );
}

/** Di quanto si e' mosso un livello in un'ora: "▲ +16 in un’ora". */
function inUnOra(adesso: number | null | undefined, prima: number | null | undefined): string {
  if (adesso == null || prima == null) return 'un’ora fa: –';
  const d = adesso - prima;
  if (Math.abs(d) < 0.5) return 'fermo da un’ora';
  return `${d > 0 ? '▲' : '▼'} ${segno(d, 0)} in un’ora`;
}

function Tessere({ d, nq }: { d: RispostaQuattroDomande | undefined; nq: RispostaNqGex | undefined }) {
  const r = d?.regime ?? null;
  const prezzo = d?.prezzo ?? nq?.prezzoNQ ?? null;
  const muro = (nome: string) => d?.muri?.muri.find((m) => m.muro === nome)?.prezzo ?? null;
  const u = nq?.unOraFa ?? null;
  const m = d?.meteo ?? null;
  const lettura = r?.lettura ?? null;
  const tutte = r?.valori.tutte ?? null;
  const zero0 = r?.valori.zeroDte ?? null;
  const concordi = tutte != null && zero0 != null ? (Math.sign(tutte) === Math.sign(zero0) ? 'concordi' : 'opposti') : null;
  const cw0 = muro('call wall 0DTE');
  const pw0 = muro('put wall 0DTE');
  return (
    <>
      <Tessera
        etichetta="Regime NQ"
        larghezza={2}
        larga
        fonte="catena"
        tag="supposto"
        tono={lettura === 'freno' || lettura === 'acceleratore' ? lettura : null}
        valore={lettura ? LETTURA[lettura] : '–'}
        dettaglio={tutte != null || zero0 != null ? `tutte ${segno(tutte, 0)} · 0DTE ${segno(zero0, 0)}${concordi ? `: ${concordi}` : ''}` : 'il regime non è arrivato'}
      />
      <Tessera
        etichetta="Zero gamma"
        larghezza={2}
        fonte="catena"
        valore={<ConDistanza v={r?.zero?.tutte} prezzo={prezzo} />}
        dettaglio={inUnOra(r?.zero?.tutte, u?.zero)}
      />
      <Tessera
        etichetta="Zero 0DTE"
        larghezza={2}
        fonte="catena"
        valore={<ConDistanza v={r?.zero?.zeroDte} prezzo={prezzo} />}
        dettaglio={inUnOra(r?.zero?.zeroDte, u?.zero0)}
      />
      <Tessera
        etichetta="Call wall"
        larghezza={2}
        fonte="catena"
        tono="freno"
        valore={<ConDistanza v={muro('call wall')} prezzo={prezzo} />}
        dettaglio={cw0 != null ? `0DTE ${num(cw0, 0)}${prezzo != null ? ` ${distanza(cw0 - prezzo, 0)}` : ''}` : '0DTE –'}
      />
      <Tessera
        etichetta="Put wall"
        larghezza={2}
        fonte="catena"
        tono="acceleratore"
        valore={<ConDistanza v={muro('put wall')} prezzo={prezzo} />}
        dettaglio={pw0 != null ? `0DTE ${num(pw0, 0)}${prezzo != null ? ` ${distanza(pw0 - prezzo, 0)}` : ''}` : '0DTE –'}
      />
      <Tessera
        etichetta="Movimento atteso oggi"
        larghezza={2}
        fonte={m?.fonteLivello ?? 'catena'}
        valore={m?.movimentoOggi != null ? `±${num(m.movimentoOggi, 0)}` : '–'}
        dettaglio={m?.fasciaPrezzi ? `${num(m.fasciaPrezzi[0], 0)}–${num(m.fasciaPrezzi[1], 0)} NQ` : 'fino alla chiusura delle 16:00'}
      />
    </>
  );
}

// ------------------------------------------------------------------ la legenda del profilo

function LegendaProfilo({ d, vista }: { d: RispostaNqGex | undefined; vista: Vista }) {
  const x = d?.fonti.find((y) => y.id === vista) ?? d?.fonti[0];
  const y = d?.qqq;
  const voci: Voce[] = [
    { colore: C.freno, testo: 'freno', forma: 'rett' },
    { colore: C.acceleratore, testo: 'acceleratore', forma: 'rett' },
    { colore: C.prezzo, testo: 'prezzo NQ', valore: d?.prezzoNQ == null ? null : num(d.prezzoNQ, 2) },
    { colore: C.flip, testo: 'zero gamma', tratto: 'dashed', valore: x?.zeroGamma == null ? null : num(x.zeroGamma, 0) },
    { colore: C.freno, testo: 'call wall', valore: x?.callWall == null ? null : num(x.callWall, 0) },
    { colore: C.acceleratore, testo: 'put wall', valore: x?.putWall == null ? null : num(x.putWall, 0) },
  ];
  return (
    <Legenda voci={voci}>
      {y && (
        <span className="chiave qqq-riga" title={`QQQ da Yahoo, ${doppiaOra(y.quando)}${y.prezzoCboe != null ? `; il file del Cboe ha ${num(y.prezzoCboe, 2)}` : ''}`}>
          QQQ <strong>{num(y.prezzo, 2)}</strong> ({y.variazione == null ? '' : `${segno(y.variazione, 2)}%, `}
          {SESSIONE[y.sessione]}) <FonteBadge id="yahoo" soloIcona /> · NQ/QQQ <strong>{num(y.rapportoNQ, 2)}</strong>
        </span>
      )}
      <Link className="rimando" to="/volatilita" state={{ scheda: 'Il meteo · Nasdaq 100' }}>
        la paura: Volatilità › Il meteo · Nasdaq 100
      </Link>
    </Legenda>
  );
}

// ------------------------------------------------------------------ i profili

/** La finestra dei prezzi: il prezzo ± 2 movimenti (dal server), o tutto il profilo. */
function useFinestra(d: RispostaNqGex, finestra: Finestra): [number, number] {
  return useMemo(() => {
    // a multipli di 50, cosi' le tacche dell'asse cadono sugli strike
    if (finestra === 'vicino' && d.finestra) return [Math.floor(d.finestra[0] / 50) * 50, Math.ceil(d.finestra[1] / 50) * 50];
    const prezzi = d.fonti.flatMap((x) => x.strike.map((s) => s.prezzo));
    if (d.prezzoNQ != null) prezzi.push(d.prezzoNQ);
    for (const x of d.fonti) for (const l of [x.zeroGamma, x.callWall, x.putWall]) if (l != null) prezzi.push(l);
    if (!prezzi.length) return [0, 1];
    const margine = (Math.max(...prezzi) - Math.min(...prezzi)) * 0.03 || 50;
    return [Math.floor((Math.min(...prezzi) - margine) / 50) * 50, Math.ceil((Math.max(...prezzi) + margine) / 50) * 50];
  }, [d, finestra]);
}

function Profili({ d, vista, finestra, cambia }: { d: RispostaNqGex; vista: Vista; finestra: Finestra; cambia: (v: Vista) => void }) {
  const asse = useFinestra(d, finestra);
  const grande = d.fonti.find((x) => x.id === vista);
  if (vista === TUTTI || !grande) {
    if (!d.fonti.some((x) => x.strike.length)) return <Vuoto>Nessuna fonte ha ancora un profilo per strike.</Vuoto>;
    return (
      <div className="affiancati">
        {d.fonti.map((x) => (
          <div key={x.id} className="affiancato">
            <TitoloProfilo x={x} />
            {x.strike.length ? <Profilo x={x} prezzo={d.prezzoNQ} asse={asse} altezza="medio" /> : <Vuoto>Nessun dato da questa fonte.</Vuoto>}
          </div>
        ))}
      </div>
    );
  }
  const altri = d.fonti.filter((x) => x.id !== grande.id);
  return (
    <div className="profilo-scelto">
      <div className="affiancato grande">
        <TitoloProfilo x={grande} />
        {grande.strike.length ? <Profilo x={grande} prezzo={d.prezzoNQ} asse={asse} altezza="alto" /> : <Vuoto>Nessun dato da questa fonte.</Vuoto>}
        {grande.nota && <p className="nota">{grande.nota}</p>}
      </div>
      <div className="profili-piccoli">
        {altri.map((x) => (
          <Piccolo key={x.id} x={x} d={d} asse={asse} cambia={cambia} />
        ))}
      </div>
    </div>
  );
}

/** Il titolo di un profilo: il badge, il nome e l'eta' sempre in vista; "vecchia" se la fonte lo e'. */
function TitoloProfilo({ x }: { x: GexFonte }) {
  const stato = useStatoFonte(x.fonte);
  return (
    <h3>
      <FonteBadge id={x.fonte} /> {x.nome}
      {x.secondiFa != null && <span className="profilo-eta"> · {etaLunga(x.secondiFa)}</span>}
      {stato?.vecchia && <span className="profilo-vecchia"> · {stato.stato === 'vecchio' ? 'vecchia' : stato.testo}</span>}
    </h3>
  );
}

function Piccolo({ x, d, asse, cambia }: { x: GexFonte; d: RispostaNqGex; asse: [number, number]; cambia: (v: Vista) => void }) {
  const stato = useStatoFonte(x.fonte);
  return (
    <button
      type="button"
      className="affiancato piccolo"
      data-vecchia={stato?.vecchia ? 'si' : undefined}
      onClick={() => cambia(x.id)}
      aria-label={`Apri il profilo di ${x.nome}`}
    >
      <TitoloProfilo x={x} />
      {x.strike.length ? <Profilo x={x} prezzo={d.prezzoNQ} asse={asse} altezza={ALTEZZA_PICCOLO} piccolo /> : <Vuoto>Nessun dato.</Vuoto>}
    </button>
  );
}

function Profilo({
  x,
  prezzo,
  asse: [lo, hi],
  altezza,
  piccolo = false,
}: {
  x: GexFonte;
  prezzo: number | null;
  asse: [number, number];
  altezza: Altezza;
  piccolo?: boolean;
}) {
  const dentro = useMemo(() => x.strike.filter((s) => s.prezzo >= lo && s.prezzo <= hi), [x, lo, hi]);
  const prezzi = useMemo(() => dentro.map((s) => s.prezzo), [dentro]);
  const valori = useMemo(() => dentro.map((s) => s.gex), [dentro]);
  const puntatore = useMemo(
    () =>
      piccolo
        ? undefined
        : puntatoreStrike(prezzi, valori, (i) => {
            const p = prezzi[i];
            const v = valori[i];
            if (p == null || v == null) return null;
            return (
              testaTip(`NQ ${num(p, 0)}${prezzo != null ? ` · ${distanza(p - prezzo, 0)} dal prezzo` : ''}`) +
              rigaTip(coloreSegno(v), `${parolaSegno(v)} · M$ per 1%`, sig(v)) +
              rigaTip(null, 'fonte', x.nome)
            );
          }),
    [prezzi, valori, prezzo, piccolo, x.nome],
  );

  const opzioni = useCallback(
    (larghezza: number): Opzioni | null => {
      if (!dentro.length) return null;
      const passo = passoDi(x.strike.map((s) => s.prezzo));
      const massimo = Math.max(1e-9, ...valori.map((v) => Math.abs(v ?? 0)));
      // un po' di spazio in piu' per le etichette dirette sulle barre piu' lunghe
      const xmax = massimo * (piccolo ? 1.1 : 1.3);
      const linee: LineaOrizzontale[] = [];
      if (prezzo != null) linee.push({ y: prezzo, colore: C.prezzo, spessore: 2, tipo: 'prezzo', etichetta: `prezzo nq ${num(prezzo, 0)}`, testo: `NQ ${num(prezzo, 2)}` });
      if (x.zeroGamma != null) linee.push({ y: x.zeroGamma, colore: C.flip, tratto: 'dashed', spessore: 1.5, tipo: 'flip', etichetta: `zero gamma ${num(x.zeroGamma, 0)}`, testo: `ZERO ${num(x.zeroGamma, 0)}` });
      if (x.callWall != null) linee.push({ y: x.callWall, colore: C.freno, spessore: 1.5, tipo: 'livello', etichetta: `call wall ${num(x.callWall, 0)}`, testo: `CALL W. ${num(x.callWall, 0)}` });
      if (x.putWall != null) linee.push({ y: x.putWall, colore: C.acceleratore, spessore: 1.5, tipo: 'livello', etichetta: `put wall ${num(x.putWall, 0)}`, testo: `PUT W. ${num(x.putWall, 0)}` });
      const visibili = linee.filter((l) => l.y >= lo && l.y <= hi);
      const fuori = !piccolo && larghezza >= 600;
      const finestra = typeof window === 'undefined' ? { larghezza: 1600, altezza: 900 } : { larghezza: window.innerWidth, altezza: window.innerHeight };
      const alt = Math.max(120, (altezza === 'alto' ? Math.min(640, Math.max(360, finestra.altezza - 330)) : 300) - 40);
      const markLine = piccolo
        ? {
            symbol: 'none' as const,
            silent: true,
            animation: false,
            data: visibili
              .filter((l) => l.tipo !== 'livello')
              .map((l) => ({ yAxis: l.y, name: l.etichetta, lineStyle: { color: l.colore, width: l.spessore ?? 1, type: l.tratto ?? 'solid' }, label: { show: false } })),
          }
        : lineeOrizzontali(visibili, { ...(fuori ? { asse: [lo, hi] as [number, number], altezzaPx: alt } : {}), prezzo: prezzo ?? undefined, fondi: FONDI_NQ });
      // le etichette dirette sulle tre barre piu' lunghe
      const lunghe = dentro
        .map((s, i) => ({ s, i }))
        .filter(({ s }) => s.gex != null)
        .sort((a, b) => Math.abs(b.s.gex ?? 0) - Math.abs(a.s.gex ?? 0))
        .slice(0, piccolo ? 0 : 3);
      return {
        animation: false,
        grid: { left: 52, right: fuori ? 132 : piccolo ? 6 : 12, top: 8, bottom: 26 },
        xAxis: asseValori({
          min: -xmax,
          max: xmax,
          name: piccolo ? '' : 'M$ per 1%',
          nameLocation: 'end',
          nameGap: -40,
          nameTextStyle: { color: C.spento, fontSize: 10, verticalAlign: 'top', padding: [14, 0, 0, 0] },
          axisLabel: { color: C.spento, fontSize: 10, formatter: (v: number) => sig(v, 2), hideOverlap: true, showMinLabel: false, showMaxLabel: false },
        }),
        yAxis: assePrezzi(lo, hi, { passo: 50 }),
        tooltip: piccolo ? { show: false } : tooltip({ trigger: 'axis', axisPointer: croce('y', { y: (v) => num(v, 0) }), showContent: false }),
        series: [
          { ...serieBarre({ prezzi, valori, spessore: passo, nome: x.nome }), clip: true, markLine },
          serieLinee(asseZero('x'), 'zero'),
          {
            type: 'scatter',
            name: 'barre più lunghe',
            silent: true,
            symbolSize: 0,
            data: lunghe.map(({ s }) => ({
              value: [s.gex, s.prezzo],
              label: { show: true, position: (s.gex ?? 0) >= 0 ? 'right' : 'left', formatter: sig(s.gex), color: C.testo2, fontSize: 10, distance: 4 },
            })),
          },
        ],
      };
    },
    [dentro, prezzi, valori, x, prezzo, lo, hi, piccolo, altezza],
  );

  if (!dentro.length) return <Vuoto>Nessuno strike di questa fonte nella finestra: scegli “Tutto”.</Vuoto>;
  return <Grafico opzioni={opzioni} altezza={altezza} etichetta={`GEX di ${x.nome} per strike in prezzi di NQ`} puntatore={puntatore} />;
}

// ------------------------------------------------------------------ i livelli

/** "vicino al bordo": un muro entro 25 punti da un bordo del valore (la domanda 3), dal server. */
function ViciniAlBordo({ d }: { d: RispostaQuattroDomande | undefined }) {
  const m = d?.muri;
  if (!m) return null;
  if (!m.bordi.length) return <p className="nota">{m.nota ?? 'I bordi del valore non sono arrivati dal chart di ATAS.'}</p>;
  if (!m.vicini.length) return <p className="nota">Nessun muro entro {num(m.sogliaPunti, 0)} punti da un bordo del valore.</p>;
  return (
    <p className="nota vicino-bordo">
      Vicino al bordo:{' '}
      {m.vicini.map((v) => `${v.muro} ${num(v.prezzo, 0)} a ${num(Math.abs(v.distanza), 0)} punti da ${v.bordo} ${num(v.prezzoBordo, 0)}`).join('; ')}.
    </p>
  );
}

/** Come ogni fonte e' portata in prezzi di NQ (nella "i" dei livelli). */
function Conversioni({ d }: { d: RispostaNqGex }) {
  return (
    <>
      Come si porta in NQ:{' '}
      {d.fonti.map((x, i) => (
        <span key={x.id}>
          {i > 0 ? '; ' : ''}
          {x.nome}:{' '}
          {x.conversione == null
            ? 'nessuna: è già NQ'
            : `${x.conversione.da} ${x.conversione.tipo === 'rapporto' ? '×' : '+'} ${num(x.conversione.valore, 2)}${x.conversione.nota ? ` (${x.conversione.nota})` : ''}`}
        </span>
      ))}
      .
    </>
  );
}
