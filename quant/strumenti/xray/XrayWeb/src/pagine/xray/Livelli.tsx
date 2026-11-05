// Livelli nel tempo (5 novembre 2026): le pagine Livelli e Nel tempo unite in una (C.6 del progetto).
// In cima la risposta in quattro tessere (regime, flip piu' vicino, freno e acceleratore piu' forti);
// sotto il grafico grande: il prezzo della giornata con le tracce dei flip e i picchi minuto per minuto,
// e, se si sceglie, uno sfondo (la mappa di gamma osservata, charm o GEX per strike, attenuata); a
// destra la scala dei livelli di adesso. La prossima seduta sta in Previsione.

import { useCallback, useMemo, useState } from 'react';
import type { LineSeriesOption, ScatterSeriesOption } from 'echarts';
import { useMediaQuery } from '@mantine/hooks';
import { useMisura } from '../../api/domande';
import type { Picco, RispostaAdesso, RispostaIntervalli, RispostaIntervalliStrike, RispostaLivelli, Scadenza, Traccia } from '../../api/tipi';
import { distanza, doppiaOra, num, oraDoppiaCorta, oraNY, segno } from '../../formato';
import { indicePrima, useFiltri, type Filtri } from '../../stato/contesto';
import { useScelte } from '../../stato/scelte';
import { C } from '../../tema/colori';
import { Grafico } from '../../componenti/Grafico';
import { Legenda, type Voce } from '../../componenti/Legenda';
import { Scheda } from '../../componenti/Scheda';
import { Tessera } from '../../componenti/Tessera';
import { ScalaPrezzi } from '../../componenti/ScalaPrezzi';
import { Esito, Vuoto } from '../../componenti/Esito';
import { Divisore, Segmenti, Tendina } from '../../componenti/Comandi';
import { Termine } from '../../componenti/Termine';
import type { Opzioni } from '../../viz/echarts';
import {
  assePrezzi,
  asseTempo,
  cartellino,
  coloreDi,
  escape,
  etichetteFinali,
  griglia,
  intervalloPrezzi,
  lineaPrezzo,
  passoDi,
  rigaTip,
  scala98,
  segnoFlusso,
  segnoGamma,
  testaTip,
  zoomTempo,
  type ScalaSegno,
} from '../../viz/grafici';
import { disegnaMappa, serieMappa, valoreAl, type DatiMappa } from '../../viz/mappa';
import './xray2.css';

type Sfondo = 'nessuno' | 'gamma' | 'charm' | 'gex';

interface Scelte {
  sfondo: Sfondo;
  scadenza: Scadenza;
  passo: string;
  ampiezza: string;
}

/** I colori dello sfondo: la gamma e la GEX col segno della gamma, la charm col flusso (blu e arancio). */
const SCALA: Record<Exclude<Sfondo, 'nessuno'>, ScalaSegno> = { gamma: segnoGamma, gex: segnoGamma, charm: segnoFlusso };

const PAROLE: Record<Exclude<Sfondo, 'nessuno'>, { pos: string; neg: string }> = {
  gamma: { pos: 'freno', neg: 'acceleratore' },
  gex: { pos: 'freno', neg: 'acceleratore' },
  charm: { pos: 'comprano col tempo', neg: 'vendono col tempo' },
};

/** Lo sfondo all'opacita' 0,55: si legge, ma prezzo e flip restano sopra. */
const OPACITA_SFONDO = 0.55;

const regimeParola = (r: string | null | undefined) =>
  r === 'freno' ? 'Freno' : r === 'acceleratore' ? 'Acceleratore' : r === 'neutro' ? 'Neutro' : '–';
const tonoDi = (r: string | null | undefined) => (r === 'freno' || r === 'acceleratore' ? r : null);

/** Il picco piu' forte di una lista (il peso piu' grande in valore assoluto). */
function piuForte(lista: readonly Picco[]): Picco | null {
  let m: Picco | null = null;
  for (const p of lista) if (!m || Math.abs(p.peso ?? 0) > Math.abs(m.peso ?? 0)) m = p;
  return m;
}

/** Il punto di una traccia all'istante t (l'ultimo <= t, se non e' piu' lontano di `tolleranza`). */
function puntoAl<T extends [number, number, ...unknown[]]>(punti: readonly T[], t: number, tolleranza: number): T | null {
  let lo = 0;
  let hi = punti.length - 1;
  let r = -1;
  while (lo <= hi) {
    const m = (lo + hi) >> 1;
    if ((punti[m]?.[0] ?? Infinity) <= t) {
      r = m;
      lo = m + 1;
    } else hi = m - 1;
  }
  const p = r >= 0 ? punti[r] : undefined;
  return p && t - p[0] <= tolleranza ? p : null;
}

/** I valori della legenda a un istante della giornata (sotto il cursore). */
function valoriAl(d: RispostaLivelli, t: number) {
  const tolleranza = Math.max(1, d.passo) * 60_000 * 1.5;
  const i = indicePrima(d.tempi, t);
  const prezzo = d.riferimento[i] ?? null;
  let flip: number | null = null;
  for (const z of d.zeri) {
    const p = puntoAl(z, t, tolleranza);
    if (p && (flip == null || prezzo == null || Math.abs(p[1] - prezzo) < Math.abs(flip - prezzo))) flip = p[1];
  }
  let freno: [number, number] | null = null;
  let acc: [number, number] | null = null;
  for (const k of d.picchi) {
    const p = puntoAl(k.punti, t, tolleranza);
    if (!p) continue;
    const peso = Math.abs(p[2] ?? 0);
    if (k.tipo === 'positivo' && (!freno || peso > freno[1])) freno = [p[1], peso];
    if (k.tipo === 'negativo' && (!acc || peso > acc[1])) acc = [p[1], peso];
  }
  return { t: d.tempi[i] ?? t, prezzo, flip, freno: freno?.[0] ?? null, acc: acc?.[0] ?? null };
}

export function Livelli() {
  const f = useFiltri();
  const telefono = useMediaQuery('(max-width: 767px)') ?? false;
  const [s, setS] = useScelte<Scelte>('livelli', { sfondo: 'nessuno', scadenza: '0dte', passo: '5', ampiezza: '1.5' });
  const ql = useMisura<RispostaLivelli>('livelli', { giorno: f.giorno, passo: 1 }, f.vivo, f.pronto);
  const qa = useMisura<RispostaAdesso>('adesso', f.parametri, f.vivo, f.pronto);
  const conMappa = s.sfondo === 'gamma' || s.sfondo === 'charm';
  const qp = useMisura<RispostaIntervalli>(
    'intervalli',
    { giorno: f.giorno, greca: s.sfondo === 'charm' ? 'charm' : 'gamma', passo: s.passo, ampiezza: s.ampiezza },
    f.vivo,
    f.pronto && conMappa,
  );
  const qs = useMisura<RispostaIntervalliStrike>(
    'intervalli-strike',
    { giorno: f.giorno, metrica: 'gex', scadenza: s.scadenza, passo: s.passo, ampiezza: s.ampiezza },
    f.vivo,
    f.pronto && s.sfondo === 'gex',
  );
  const sfondo = s.sfondo === 'nessuno' ? null : s.sfondo === 'gex' ? (qs.data ?? null) : (qp.data ?? null);

  // il cursore: l'istante sotto il mouse, per i valori della legenda
  const [cursore, setCursore] = useState<number | null>(null);

  const a = qa.data;
  const P = f.p;
  const rif = a?.riferimento.prezzo ?? null;
  const freno = a ? piuForte(a.picchiPositivi) : null;
  const acc = a ? piuForte(a.picchiNegativi) : null;
  const fv = a?.flipVicino ?? null;
  const conDistanza = (x: number | null | undefined) =>
    x == null ? '–' : (
      <>
        {num(P(x), 0)}
        {rif != null && <span className="valore-dist"> {distanza(x - rif, 0)}</span>}
      </>
    );

  const sotto = ql.data && cursore != null ? valoriAl(ql.data, cursore) : null;
  const leg = {
    prezzo: sotto ? sotto.prezzo : rif,
    flip: sotto ? sotto.flip : (fv?.prezzo ?? null),
    freno: sotto ? sotto.freno : (freno?.strike ?? null),
    acc: sotto ? sotto.acc : (acc?.strike ?? null),
  };
  const valore = (x: number | null, cifre = 0) => (x == null ? null : num(P(x), cifre));
  const voci: Voce[] = [
    { colore: C.prezzo, testo: `prezzo ${f.unita}`, valore: valore(leg.prezzo, 2) },
    { colore: C.prezzo, testo: 'fuori cassa: ES − base', tratto: 'dashed' },
    { colore: C.flip, testo: 'flip', tratto: 'dashed', valore: valore(leg.flip) },
    { colore: C.freno, testo: 'freno più forte', forma: 'punto', valore: valore(leg.freno) },
    { colore: C.acceleratore, testo: 'acceleratore più forte', forma: 'punto', valore: valore(leg.acc) },
  ];
  const massimo = sfondo ? scala98(sfondo.valori) : 0;
  const parole = s.sfondo === 'nessuno' ? null : PAROLE[s.sfondo];

  const comandi = (
    <>
      <Segmenti<Sfondo>
        etichetta="Sfondo"
        valore={s.sfondo}
        cambia={(v) => setS((x) => ({ ...x, sfondo: v }))}
        opzioni={[
          { valore: 'gamma', testo: 'Gamma osservata' },
          { valore: 'charm', testo: 'Charm' },
          { valore: 'gex', testo: 'GEX per strike' },
          { valore: 'nessuno', testo: 'Nessuno' },
        ]}
      />
      {s.sfondo === 'gex' && (
        <Tendina<Scadenza>
          etichetta="Scadenza"
          valore={s.scadenza}
          cambia={(v) => setS((x) => ({ ...x, scadenza: v }))}
          opzioni={[
            { valore: '0dte', testo: '0DTE' },
            { valore: 'next_expiry', testo: 'Prossima scadenza' },
            { valore: 'opex', testo: 'Opex mensile' },
          ]}
        />
      )}
      {s.sfondo !== 'nessuno' && (
        <>
          <Divisore />
          <Segmenti<string>
            etichetta="Intervallo"
            valore={s.passo}
            cambia={(v) => setS((x) => ({ ...x, passo: v }))}
            opzioni={[
              { valore: '1', testo: '1 min' },
              { valore: '5', testo: '5 min' },
              { valore: '15', testo: '15 min' },
            ]}
          />
          <Segmenti<string>
            etichetta="Ampiezza"
            valore={s.ampiezza}
            cambia={(v) => setS((x) => ({ ...x, ampiezza: v }))}
            opzioni={[
              { valore: '0.75', testo: '±0,75%' },
              { valore: '1.5', testo: '±1,5%' },
              { valore: '3', testo: '±3%' },
            ]}
          />
        </>
      )}
    </>
  );

  const scala = (
    <Scheda
      titolo="La scala dei livelli"
      fonti={['xray']}
      larghezza={4}
      className="livelli-scala"
      breve="I livelli di adesso dal più alto al più basso, con il prezzo al suo posto e la distanza."
      semplice="Ogni riga è un livello di OptionsDepth all’istante scelto: i flip, il freno e l’acceleratore più forti, i muri di ogni scadenza, e i bordi del valore del profilo di ATAS. Le righe vicine meno di 6 punti sono fuse in una zona. Sono zone da guardare, non livelli provati."
    >
      <Esito q={qa} rotta="adesso">
        {(d) => {
          const righe = d.scala.filter((r) => r.quando === 'adesso');
          return righe.length ? (
            <ScalaPrezzi righe={righe} prezzo={d.riferimento.prezzo} unita={f.unita} converti={P} compatta={telefono} />
          ) : (
            <Vuoto>Nessun livello all’istante scelto.</Vuoto>
          );
        }}
      </Esito>
    </Scheda>
  );

  return (
    <>
      <div className="griglia tessere">
        <Tessera
          etichetta="Regime"
          fonte="xray"
          tag="non provato"
          tono={tonoDi(a?.gamma.regime)}
          valore={regimeParola(a?.gamma.regime)}
          dettaglio={
            a ? `${a.gamma.da != null ? `da ${oraDoppiaCorta(a.gamma.da)} · ` : ''}gamma al prezzo ${segno(a.gamma.valore, 1)}` : undefined
          }
          larga
        />
        <Tessera
          etichetta="Flip più vicino"
          fonte="xray"
          valore={conDistanza(fv?.prezzo)}
          dettaglio={fv ? `sopra: ${fv.sopra ?? '–'} · sotto: ${fv.sotto ?? '–'}` : 'nessun flip all’istante scelto'}
        />
        <Tessera
          etichetta="Freno più forte"
          fonte="xray"
          tono="freno"
          valore={conDistanza(freno?.strike)}
          dettaglio={freno ? `peso ${num(freno.peso, 0)} · il picco di gamma positiva` : 'nessun picco'}
        />
        <Tessera
          etichetta="Acceleratore più forte"
          fonte="xray"
          tono="acceleratore"
          valore={conDistanza(acc?.strike)}
          dettaglio={acc ? `peso ${num(acc.peso, 0)} · il picco di gamma negativa` : 'nessun picco'}
        />
      </div>
      <div className="griglia">
        {telefono && scala}
        <Scheda
          titolo="Il prezzo e i livelli della giornata"
          fonti={['xray']}
          larghezza={8}
          comandi={comandi}
          legenda={
            <Legenda
              voci={voci}
              scala={sfondo && parole && s.sfondo !== 'nessuno' ? { massimo, segno: SCALA[s.sfondo], positivo: parole.pos, negativo: parole.neg, unita: 'unità di OptionsDepth, intensità relativa' } : undefined}
            >
              {sotto && <span className="nota">alle {oraDoppiaCorta(sotto.t)}</span>}
            </Legenda>
          }
          breve={
            <>
              Il prezzo con i <Termine k="flip">flip</Termine> e i picchi di <Termine k="freno">freno</Termine> e{' '}
              <Termine k="acceleratore">acceleratore</Termine>, minuto per minuto: dove il regime cambia rispetto al prezzo.
            </>
          }
          semplice="La linea bianca è il prezzo (tratteggiata fuori dalla cassa, dove è ES meno la base). Le linee grigie tratteggiate sono i flip: oltre quei prezzi i market maker passano da frenare ad accelerare, o viceversa. I punti verdi sono il freno più forte, i rossi l’acceleratore più forte: più grande è il punto, più pesa. Lo sfondo, se lo scegli, mostra a ogni prezzo e a ogni minuto dove le coperture frenano o accelerano. Sono zone da guardare, non livelli provati."
          sotto="Livelli dal file dei livelli di OptionsDepth (un passo al minuto); ogni traccia di flip è un flip solo, interrotta quando nasce o muore. Lo sfondo è la mappa per intervalli, scala tagliata al 98° percentile."
        >
          <Esito q={ql} rotta="livelli">
            {(d) => <GraficoLivelli d={d} f={f} sfondo={sfondo} scelte={s} massimo={massimo} onCursore={setCursore} />}
          </Esito>
        </Scheda>
        {!telefono && scala}
      </div>
    </>
  );
}

interface PropsGrafico {
  d: RispostaLivelli;
  f: Filtri;
  sfondo: RispostaIntervalli | RispostaIntervalliStrike | null;
  scelte: Scelte;
  massimo: number;
  onCursore: (t: number | null) => void;
}

function GraficoLivelli({ d, f, sfondo, scelte, massimo, onCursore }: PropsGrafico) {
  const P = f.p;
  const ts = f.ts;

  // lo sfondo (l'immagine) si rifa' solo se cambiano i suoi dati
  const mappa = useMemo(() => {
    if (!sfondo || !sfondo.tempi.length) return null;
    const prezzi = 'strike' in sfondo ? sfondo.strike : sfondo.prezzi;
    if (!prezzi.length) return null;
    const dati: DatiMappa = {
      tempi: sfondo.tempi,
      prezzi,
      valori: sfondo.valori,
      durata: Number(scelte.passo) * 60_000,
      passoPrezzo: passoDi(prezzi),
      massimo,
      scala: scelte.sfondo === 'nessuno' ? segnoGamma : SCALA[scelte.sfondo],
      // l'ultima colonna finisce con l'ultimo minuto dei livelli, non a fine intervallo
      fine: (d.tempi.at(-1) ?? 0) + 60_000,
    };
    return { dati, immagine: disegnaMappa(dati) };
  }, [sfondo, scelte.passo, scelte.sfondo, massimo, d.tempi]);

  const opzioni = useCallback(
    (larghezza: number): Opzioni | null => {
      const t = d.tempi;
      if (!t.length) return null;
      const prezzi = d.riferimento.map((r) => (r == null ? null : P(r)));
      const pesoMax = Math.max(1, ...d.picchi.flatMap((p) => p.punti.map((q) => Math.abs(q[2] ?? 0))));
      const ultimo = t.at(-1) ?? 0;
      const tolleranza = Math.max(1, d.passo) * 60_000 * 1.5;
      const fine = etichetteFinali(larghezza);

      const flip = d.zeri.map((linea: Traccia, i): LineSeriesOption => {
        // il cartellino solo per i flip vivi alla fine, e solo se c'e' spazio a destra (sotto i 600 px i valori stanno nella legenda)
        const vivaAllaFine = fine && (linea.at(-1)?.[0] ?? 0) >= ultimo - tolleranza;
        return {
          type: 'line',
          name: 'flip',
          id: `flip${i}`,
          data: linea.map(([x, p]) => [x, P(p)]),
          showSymbol: false,
          silent: true,
          lineStyle: { color: C.flip, width: 1.5, type: 'dashed', shadowColor: 'rgba(0,0,0,.85)', shadowBlur: 3 },
          itemStyle: { color: C.flip },
          z: 6,
          endLabel: vivaAllaFine
            ? { show: true, ...cartellino(C.flip), formatter: (p: { value?: unknown }) => `FLIP ${num(Array.isArray(p.value) ? Number(p.value[1]) : NaN, 0)}` }
            : { show: false },
          labelLayout: { moveOverlap: 'shiftY' },
        };
      });
      const picchi = d.picchi.map(
        (p): ScatterSeriesOption => ({
          type: 'scatter',
          name: p.tipo === 'positivo' ? 'freno più forte' : 'acceleratore più forte',
          data: p.punti.map(([x, y, w]) => [x, P(y), w ?? 0]),
          symbolSize: (v: number[]) => 4 + 6 * Math.sqrt(Math.abs(v[2] ?? 0) / pesoMax),
          itemStyle: { color: p.tipo === 'positivo' ? C.freno : C.acceleratore, opacity: 0.85, borderColor: C.superficie, borderWidth: 0.5 },
          silent: true,
          z: 4,
        }),
      );

      // l'asse dei prezzi: la finestra dello sfondo, o tutto cio' che si disegna, arrotondato al passo degli strike
      let basso: number;
      let alto: number;
      if (mappa) {
        basso = P(mappa.immagine.pBasso);
        alto = P(mappa.immagine.pAlto);
      } else {
        const ys = [
          ...prezzi,
          ...d.zeri.flatMap((l) => l.map((q) => P(q[1]))),
          ...d.picchi.flatMap((p) => p.punti.map((q) => P(q[1]))),
        ].filter((y): y is number => y != null && Number.isFinite(y));
        const lo = Math.min(...ys);
        const hi = Math.max(...ys);
        const margine = Math.max(5, (hi - lo) * 0.04);
        basso = lo - margine;
        alto = hi + margine;
      }
      // gli estremi ai multipli dell'intervallo delle tacche: le etichette cadono su numeri tondi
      const passoAsse = intervalloPrezzi(basso, alto, 5);
      basso = Math.floor(basso / passoAsse) * passoAsse;
      alto = Math.ceil(alto / passoAsse) * passoAsse;
      const t0 = Math.min(t[0] ?? 0, mappa?.immagine.t0 ?? Infinity);
      const t1 = Math.max(ultimo, mappa?.immagine.t1 ?? -Infinity);

      const istante: LineSeriesOption[] = ts
        ? [
            {
              type: 'line',
              name: 'istante',
              data: [],
              markLine: {
                symbol: 'none',
                silent: true,
                animation: false,
                data: [{ xAxis: ts }],
                lineStyle: { color: C.testo2, width: 1.5, type: 'solid' },
                label: { show: true, ...cartellino(C.testo2), position: 'end', distance: 2, formatter: oraNY(ts) },
              },
            },
          ]
        : [];

      const serie: Opzioni['series'] = [];
      if (mappa) {
        const base = serieMappa(mappa.immagine, P, 'sfondo');
        const disegna = base.renderItem as (p: unknown, api: unknown) => { style?: Record<string, unknown> } | null;
        serie.push({
          ...base,
          renderItem: ((p: unknown, api: unknown) => {
            const r = disegna(p, api);
            if (r?.style) r.style = { ...r.style, opacity: OPACITA_SFONDO };
            return r;
          }) as typeof base.renderItem,
        });
      }
      const prezzo = lineaPrezzo(t, prezzi, d.vivo, `prezzo ${f.unita}`).map((x) => (fine ? x : { ...x, endLabel: { show: false } }));
      serie.push(...picchi, ...flip, ...prezzo, ...istante);

      return {
        animation: false,
        // con l'istante scelto, in alto lo spazio per il suo cartellino
        grid: griglia(larghezza, { destra: 'cartellino', sopra: ts ? 24 : 10, sotto: 34 }),
        xAxis: asseTempo(t0, t1),
        yAxis: assePrezzi(basso, alto, { passo: 5 }),
        dataZoom: zoomTempo(),
        tooltip: { show: false },
        series: serie,
      };
    },
    [d, P, f.unita, ts, mappa],
  );

  // il tooltip dalla posizione: l'ora, il prezzo, il flip piu' vicino e, con lo sfondo, il valore della cella
  const puntatore = useMemo(
    () => (x: number, y: number) => {
      const v = valoriAl(d, x);
      let html = testaTip(doppiaOra(v.t));
      const i = indicePrima(d.tempi, x);
      if (v.prezzo != null) html += rigaTip(C.prezzo, d.vivo[i] ? `prezzo ${f.unita} (spot vivo)` : `prezzo ${f.unita} (ES − base)`, num(P(v.prezzo), 2));
      if (v.flip != null) html += rigaTip(C.flip, 'flip', num(P(v.flip), 0));
      if (mappa && scelte.sfondo !== 'nessuno') {
        const c = valoreAl(mappa.dati, x, y, P);
        const parole = PAROLE[scelte.sfondo];
        if (c && c.valore != null)
          html +=
            rigaTip(coloreDi(c.valore, mappa.dati.massimo, mappa.dati.scala), `${c.valore >= 0 ? parole.pos : parole.neg} a ${num(P(c.prezzo), 0)}`, num(c.valore, Math.abs(c.valore) >= 10 ? 0 : 2)) +
            `<div style="color:${C.spento}">${escape('unità di OptionsDepth')}</div>`;
      }
      return html;
    },
    [d, f.unita, P, mappa, scelte.sfondo],
  );

  const cursore = useCallback((t: number | null) => onCursore(t == null ? null : (d.tempi[indicePrima(d.tempi, t)] ?? null)), [d.tempi, onCursore]);

  if (!d.tempi.length) return <Vuoto>Nessun dato in questa giornata.</Vuoto>;
  return <Grafico opzioni={opzioni} altezza="alto" etichetta="Il prezzo e i livelli della giornata" puntatore={puntatore} onCursore={cursore} />;
}
