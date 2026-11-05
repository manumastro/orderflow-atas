// I grafici della volatilita': le serie nel tempo, la struttura a termine, il sorriso (C.10, D.4). Ogni
// curva ha il colore e il tratto della sua fonte, mai i colori del segno. La legenda mostra solo le serie
// con dati, CON il valore (l'ultimo, o quello sotto il cursore); le vuote stanno in una riga "senza dati
// oggi". I nomi in fondo alle linee si distanziano (distanzia) e sotto i 600 px si spengono.

import { useCallback, useMemo, useState } from 'react';
import type { LineSeriesOption, MarkLineComponentOption, ScatterSeriesOption } from 'echarts';
import type { CurvaSorriso, CurvaTermine, Indice, RispostaVolTermine, SerieVol } from '../../api/tipi';
import { doppiaOra, giornoCorto, num } from '../../formato';
import { C } from '../../tema/colori';
import { Grafico, altezzaPx } from '../../componenti/Grafico';
import { Legenda, type Voce } from '../../componenti/Legenda';
import { Vuoto } from '../../componenti/Esito';
import type { Opzioni } from '../../viz/echarts';
import {
  asseTempo,
  asseValori,
  coloreTip,
  croce,
  distanzia,
  etichetteFinali,
  griglia,
  lineaVerticale,
  puntiTip,
  rigaTip,
  testaTip,
  tooltip,
  valoriDi,
} from '../../viz/grafici';
import { lineaFonte, stiliPerFonte, voceFonte, type StileSerie } from '../../viz/fonti';

const SOPRA = 26;
const SOTTO_TEMPO = 30;
const SOTTO_ASSE = 44;

/** L'altezza utile della griglia, per distanziare i nomi in fondo alle linee. */
function altezzaGriglia(sotto: number): number {
  const finestra = typeof window === 'undefined' ? { larghezza: 1600, altezza: 900 } : { larghezza: window.innerWidth, altezza: window.innerHeight };
  return Math.max(80, altezzaPx('medio', finestra) - SOPRA - sotto);
}

/** I limiti dell'asse della volatilita': almeno mezzo punto sopra e sotto i dati, interi. */
export function limitiVol(valori: readonly number[]): [number, number] {
  if (!valori.length) return [0, 1];
  return [Math.floor(Math.min(...valori) - 0.5), Math.ceil(Math.max(...valori) + 0.5)];
}

const asseVol = (lo: number, hi: number) =>
  asseValori({
    min: lo,
    max: hi,
    minInterval: 0.5,
    name: 'vol %',
    nameLocation: 'end',
    nameGap: 8,
    axisLabel: { color: C.spento, fontSize: 11, formatter: (v: number) => num(v, Number.isInteger(v) ? 0 : 1), showMinLabel: false, showMaxLabel: false },
  });

/** Il massimo tondo dell'asse dei giorni (logaritmico): 10, 20, 50, 100, 200, 500... */
export function massimoTondo(v: number): number {
  for (const k of [10, 20, 50, 100, 200, 500, 1000, 2000, 5000]) if (v <= k) return k;
  return 10_000;
}

const scriviVol = (v: number | null | undefined) => (v == null ? null : `${num(v, 1)}%`);

/** Le serie vuote, in una riga sotto la legenda. */
function SenzaDati({ nomi }: { nomi: string[] }) {
  if (!nomi.length) return null;
  return <p className="nota senza-dati">senza dati oggi: {nomi.join(', ')}</p>;
}

// ------------------------------------------------------------------ nel tempo

/** L'ultimo valore fino al tempo t (o l'ultimo in assoluto). */
export function valoreAl(punti: readonly [number, number | null][], t: number | null): number | null {
  for (let i = punti.length - 1; i >= 0; i--) {
    const p = punti[i]!;
    if (p[1] != null && Number.isFinite(p[1]) && (t == null || p[0] <= t)) return p[1];
  }
  return null;
}

/** Le serie di un indice nel tempo, sovrapposte. */
export function VolNelTempo({ serie, indice }: { serie: SerieVol[]; indice: Indice }) {
  const [cursore, setCursore] = useState<number | null>(null);
  const base = useMemo(() => {
    const mie = serie.filter((s) => s.indice === indice);
    const piene = mie.filter((s) => s.punti.some((p) => p[1] != null));
    const vuote = mie.filter((s) => !piene.includes(s)).map((s) => s.nome);
    if (!piene.length) return { mie, piene, vuote, stili: [] as StileSerie[], tempi: [0, 1] as [number, number], lim: [0, 1] as [number, number] };
    const stili = stiliPerFonte(piene.map((s) => s.fonte));
    const tempi = piene.flatMap((s) => s.punti.map((p) => p[0]));
    const valori = piene.flatMap((s) => s.punti.map((p) => p[1]).filter((v): v is number => v != null));
    return { mie, piene, vuote, stili, tempi: [Math.min(...tempi), Math.max(...tempi)] as [number, number], lim: limitiVol(valori) };
  }, [serie, indice]);

  const opzioni = useCallback(
    (larghezza: number): Opzioni | null => {
      const { piene, stili, tempi, lim } = base;
      if (!piene.length) return null;
      const fine = etichetteFinali(larghezza);
      const sposta = distanzia(
        piene.map((s) => valoreAl(s.punti, null) ?? lim[0]),
        lim,
        altezzaGriglia(SOTTO_TEMPO),
      );
      return {
        animation: false,
        grid: griglia(larghezza, { destra: fine ? 'etichette' : 'niente', sopra: SOPRA, sotto: 8 }),
        xAxis: asseTempo(tempi[0], tempi[1], fine ? {} : { name: '' }),
        yAxis: asseVol(lim[0], lim[1]),
        tooltip: tooltip({
          trigger: 'axis',
          axisPointer: croce('xy', { x: (v) => doppiaOra(v), y: (v) => `${num(v, 1)}%` }),
          formatter: (p: unknown) => {
            const lista = puntiTip(p);
            const t = valoriDi(lista[0] ?? { value: [0], name: '', dataIndex: 0 })[0] ?? 0;
            return (
              testaTip(doppiaOra(t)) +
              lista
                .filter((q) => Array.isArray(q.value) && q.value[1] != null)
                .map((q) => rigaTip(coloreTip(q), q.seriesName ?? '', `${num(valoriDi(q)[1], 1)}%`))
                .join('')
            );
          },
        }),
        series: piene.map((s, i) => lineaFonte(s.nome, s.punti, stili[i]!, fine, sposta[i] ?? 0)),
      };
    },
    [base],
  );

  if (!base.mie.length) return <Vuoto>Nessuna serie per {indice}.</Vuoto>;
  if (!base.piene.length) return <Vuoto>Le serie di {indice} ci sono, ma oggi non hanno ancora dati.</Vuoto>;
  const voci: Voce[] = base.piene.map((s, i) => ({ ...voceFonte(s.nome, s.fonte, base.stili[i]!), valore: scriviVol(valoreAl(s.punti, cursore)) }));
  return (
    <>
      <Legenda voci={voci} />
      <SenzaDati nomi={base.vuote} />
      <Grafico opzioni={opzioni} altezza="medio" etichetta={`Volatilità di ${indice} nel tempo, per fonte`} onCursore={setCursore} />
    </>
  );
}

// ------------------------------------------------------------------ la struttura a termine

type PuntoTermine = CurvaTermine['punti'][number];

/** Il punto piu' vicino a x su una scala logaritmica (i giorni). */
function vicinoLog<T extends { giorni: number }>(punti: readonly T[], x: number): T | null {
  let migliore: T | null = null;
  let d = Infinity;
  for (const p of punti) {
    const e = Math.abs(Math.log(p.giorni) - Math.log(x));
    if (e < d) {
      d = e;
      migliore = p;
    }
  }
  return migliore;
}

/**
 * I giorni alla data di un evento, sulla stessa scala delle scadenze: dalla scadenza della stessa data, o
 * da una qualunque spostata di tanti giorni di calendario. Le date sono "AAAA-MM-GG": nessun fuso entra.
 */
export function giorniAlla(giorno: string, punti: readonly PuntoTermine[]): number | null {
  const stessa = punti.find((p) => p.scadenza === giorno);
  if (stessa) return stessa.giorni;
  const rif = punti.find((p) => /^\d{4}-\d{2}-\d{2}$/.test(p.scadenza));
  if (!rif) return null;
  return rif.giorni + (Date.parse(giorno) - Date.parse(rif.scadenza)) / 86_400_000;
}

/** Il nome breve di un evento: senza le parentesi, al massimo 30 caratteri. */
const nomeBreve = (s: string) => {
  const corto = s.replace(/\s*\(.*$/, '').trim();
  return corto.length > 30 ? `${corto.slice(0, 29)}…` : corto;
};

/** La struttura a termine: la volatilita' al prezzo per giorni alla scadenza (asse logaritmico). */
export function StrutturaTermine({ curve, indice, eventi = [] }: { curve: CurvaTermine[]; indice: Indice; eventi?: RispostaVolTermine['eventi'] }) {
  const [cursore, setCursore] = useState<number | null>(null);
  const base = useMemo(() => {
    const mie = curve.filter((c) => c.indice === indice);
    const nomeDi = (c: CurvaTermine) => c.nome ?? c.id;
    const valide = (c: CurvaTermine) => c.punti.filter((p) => p.giorni > 0 && p.vol != null);
    const piene = mie.filter((c) => valide(c).some((p) => !p.sospetto));
    const vuote = mie.filter((c) => !piene.includes(c)).map(nomeDi);
    const stili = stiliPerFonte(piene.map((c) => c.fonte));
    const tutti = piene.flatMap(valide);
    const buoni = tutti.filter((p) => !p.sospetto).map((p) => p.vol as number);
    const giorni = tutti.map((p) => p.giorni);
    const lim = limitiVol(buoni);
    const gmin = giorni.length ? Math.max(0.05, Math.min(...giorni) * 0.8) : 0.1;
    const gmax = massimoTondo(giorni.length ? Math.max(...giorni) * 1.1 : 10);
    const tuttiPunti = mie.flatMap((c) => c.punti);
    const ev = (eventi ?? [])
      .map((e) => ({ ...e, giorni: giorniAlla(e.giorno, tuttiPunti) }))
      .filter((e): e is { giorno: string; nome: string; giorni: number } => e.giorni != null && e.giorni <= gmax)
      .map((e) => ({ ...e, giorni: Math.max(e.giorni, gmin * 1.02) }));
    const sospetti = piene.flatMap((c) => valide(c).filter((p) => p.sospetto).map((p) => ({ c, p })));
    return { mie, piene, vuote, stili, lim, gmin, gmax, ev, sospetti, nomeDi, valide };
  }, [curve, indice, eventi]);

  const opzioni = useCallback(
    (larghezza: number): Opzioni | null => {
      const { piene, stili, lim, gmin, gmax, ev, sospetti, nomeDi, valide } = base;
      if (!piene.length) return null;
      const fine = etichetteFinali(larghezza);
      const ultimi = piene.map((c) => valide(c).filter((p) => !p.sospetto).at(-1)?.vol ?? lim[0]);
      const sposta = distanzia(ultimi, lim, altezzaGriglia(SOTTO_ASSE));
      const linee: LineSeriesOption[] = piene.map((c, i) => ({
        ...lineaFonte(
          nomeDi(c),
          valide(c)
            .filter((p) => !p.sospetto)
            .map((p): [number, number | null] => [p.giorni, p.vol]),
          stili[i]!,
          fine,
          sposta[i] ?? 0,
        ),
        showSymbol: true,
        symbolSize: 5,
      }));
      const vuoti: ScatterSeriesOption = {
        type: 'scatter',
        name: 'dato sospetto',
        data: sospetti.map(({ c, p }) => {
          const fuori = (p.vol as number) > lim[1];
          return {
            value: [p.giorni, Math.min(p.vol as number, lim[1])],
            itemStyle: { color: C.superficie, borderColor: stili[piene.indexOf(c)]?.colore ?? C.spento, borderWidth: 1.5 },
            label: fuori ? { show: true, position: 'bottom', formatter: `${num(p.vol, 0)}% ↑`, color: C.testo2, fontSize: 10 } : { show: false },
          };
        }),
        symbol: 'circle',
        symbolSize: 9,
        z: 4,
      };
      const marcheEventi: MarkLineComponentOption = {
        symbol: 'none',
        silent: true,
        animation: false,
        data: ev.map((e) => ({
          xAxis: e.giorni,
          name: `evento ${giornoCorto(e.giorno)} ${e.nome}`.toLowerCase(),
          label: { show: true, position: 'insideEndTop', formatter: `${giornoCorto(e.giorno)} ${nomeBreve(e.nome)}`, color: C.testo2, fontSize: 10 },
        })),
        lineStyle: { color: C.decoro, width: 1, type: 'dashed' },
      };
      return {
        animation: false,
        grid: griglia(larghezza, { destra: fine ? 'etichette' : 'niente', sopra: SOPRA, sotto: 30 }),
        xAxis: asseValori({
          type: 'log',
          logBase: 10,
          min: gmin,
          max: gmax,
          name: 'giorni alla scadenza',
          nameLocation: 'middle',
          nameGap: 24,
          axisLabel: { color: C.spento, fontSize: 11, formatter: (v: number) => num(v, v < 1 ? 1 : 0), showMinLabel: false, showMaxLabel: false, hideOverlap: true },
        }),
        yAxis: asseVol(lim[0], lim[1]),
        tooltip: tooltip({
          trigger: 'axis',
          axisPointer: croce('x', { x: (v) => `${num(v, v < 10 ? 1 : 0)} g` }),
          formatter: (p: unknown) => {
            const lista = puntiTip(p).filter((q) => q.seriesName !== 'dato sospetto');
            const g = valoriDi(lista[0] ?? { value: [0], name: '', dataIndex: 0 })[0] ?? 0;
            return (
              testaTip(`circa ${num(g, g < 10 ? 1 : 0)} giorni alla scadenza`) +
              lista
                .map((q) => {
                  const curva = piene.find((c) => nomeDi(c) === q.seriesName);
                  const punto = curva ? valide(curva).filter((x) => !x.sospetto)[q.dataIndex] : null;
                  return rigaTip(coloreTip(q), `${q.seriesName ?? ''}${punto ? ` · ${giornoCorto(punto.scadenza)}` : ''}`, `${num(valoriDi(q)[1], 1)}%`);
                })
                .join('')
            );
          },
        }),
        series: [...linee, ...(sospetti.length ? [vuoti] : []), ...(ev.length ? [{ type: 'line' as const, name: 'eventi', data: [], markLine: marcheEventi }] : [])],
      };
    },
    [base],
  );

  if (!base.piene.length) return <Vuoto>Nessuna scadenza per {indice}.</Vuoto>;
  const x = cursore != null && cursore > 0 ? cursore : 30;
  const voci: Voce[] = base.piene.map((c, i) => {
    const p = vicinoLog(base.valide(c).filter((q) => !q.sospetto), x);
    return { ...voceFonte(base.nomeDi(c), c.fonte, base.stili[i]!), valore: p ? `a ${num(p.giorni, p.giorni < 10 ? 1 : 0)} g: ${num(p.vol, 1)}%` : null };
  });
  return (
    <>
      <Legenda voci={voci} />
      <SenzaDati nomi={base.vuote} />
      <Grafico opzioni={opzioni} altezza="medio" etichetta={`Struttura a termine di ${indice}, per fonte`} onCursore={setCursore} />
      {base.sospetti.length > 0 && (
        <p className="nota">
          ○ punto vuoto = dato sospetto (vol più del doppio della mediana della curva), da scartare:{' '}
          {base.sospetti.map(({ c, p }) => `${base.nomeDi(c)} ${giornoCorto(p.scadenza)} ${num(p.vol, 1)}%`).join(', ')}.
        </p>
      )}
    </>
  );
}

// ------------------------------------------------------------------ il sorriso

/** Il sorriso: la volatilita' per strike, in moneyness (strike / forward − 1), per fonte. */
export function Sorriso({ curve, indice }: { curve: CurvaSorriso[]; indice: Indice }) {
  const [cursore, setCursore] = useState<number | null>(null);
  const base = useMemo(() => {
    const nomeDi = (c: CurvaSorriso) => c.nome ?? c.id;
    const piene = curve.filter((c) => c.punti.some((p) => p.vol != null));
    const vuote = curve.filter((c) => !piene.includes(c)).map(nomeDi);
    const stili = stiliPerFonte(piene.map((c) => c.fonte));
    const lim = limitiVol(piene.flatMap((c) => c.punti.map((p) => p.vol).filter((v): v is number => v != null)));
    return { piene, vuote, stili, lim, nomeDi };
  }, [curve]);

  const opzioni = useCallback(
    (larghezza: number): Opzioni | null => {
      const { piene, stili, lim, nomeDi } = base;
      if (!piene.length) return null;
      const fine = etichetteFinali(larghezza);
      const ultimi = piene.map((c) => [...c.punti].reverse().find((p) => p.vol != null)?.vol ?? lim[0]);
      const sposta = distanzia(ultimi, lim, altezzaGriglia(SOTTO_ASSE));
      return {
        animation: false,
        grid: griglia(larghezza, { destra: fine ? 'etichette' : 'niente', sopra: SOPRA, sotto: 30 }),
        xAxis: asseValori({
          scale: true,
          name: 'strike / forward − 1 (%)',
          nameLocation: 'middle',
          nameGap: 24,
          axisLabel: { color: C.spento, fontSize: 11, formatter: (v: number) => `${v > 0 ? '+' : ''}${num(v, 0)}%`, hideOverlap: true },
        }),
        yAxis: asseVol(lim[0], lim[1]),
        tooltip: tooltip({
          trigger: 'axis',
          axisPointer: croce('x', { x: (v) => `${v > 0 ? '+' : ''}${num(v, 1)}%` }),
          formatter: (p: unknown) => {
            const lista = puntiTip(p);
            return (
              testaTip('vol per strike') +
              lista
                .map((q) => {
                  const curva = piene.find((c) => nomeDi(c) === q.seriesName);
                  const punto = curva?.punti[q.dataIndex];
                  return rigaTip(coloreTip(q), `${q.seriesName ?? ''}${punto ? ` · strike ${num(punto.strike, 0)} (${num(punto.moneyness, 1)}%)` : ''}`, `${num(valoriDi(q)[1], 1)}%`);
                })
                .join('')
            );
          },
        }),
        series: [
          ...piene.map((c, i) => lineaFonte(nomeDi(c), c.punti.map((p): [number, number | null] => [p.moneyness, p.vol]), stili[i]!, fine, sposta[i] ?? 0)),
          {
            type: 'line',
            name: 'al forward',
            data: [],
            markLine: { ...lineaVerticale(0, C.spento), data: [{ xAxis: 0, name: 'al forward 0%' }], label: { show: true, position: 'insideEndTop', formatter: 'al forward', color: C.testo2, fontSize: 11 } },
          },
        ],
      };
    },
    [base],
  );

  if (!base.piene.length) return <Vuoto>Nessun sorriso per {indice}.</Vuoto>;
  const x = cursore ?? 0;
  const voci: Voce[] = base.piene.map((c, i) => {
    let p: CurvaSorriso['punti'][number] | null = null;
    for (const q of c.punti) if (q.vol != null && (p == null || Math.abs(q.moneyness - x) < Math.abs(p.moneyness - x))) p = q;
    const dettagli = [c.giorni != null ? `${num(c.giorni, 0)} g` : null, c.forward != null ? `forward ${num(c.forward, 1)}` : null].filter(Boolean).join(', ');
    return {
      ...voceFonte(`${base.nomeDi(c)}${dettagli ? ` · ${dettagli}` : ''}`, c.fonte, base.stili[i]!),
      valore: p ? `${cursore == null ? 'al forward' : `${p.moneyness > 0 ? '+' : ''}${num(p.moneyness, 1)}%`}: ${num(p.vol, 1)}%` : null,
    };
  });
  return (
    <>
      <Legenda voci={voci} />
      <SenzaDati nomi={base.vuote} />
      <Grafico opzioni={opzioni} altezza="medio" etichetta={`Sorriso di volatilità di ${indice}, per fonte`} onCursore={setCursore} />
    </>
  );
}
