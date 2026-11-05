// La GEX viva di NQ nel tempo (C.11): sopra il prezzo del future, lo zero gamma (tutte le scadenze e 0DTE)
// e i muri, minuto per minuto; sotto, in una fascia sottile, la GEX totale e quella 0DTE a barre attorno
// allo zero, nel segno (verde freno, rosso acceleratore: segno supposto, convenzione A). Una sola fonte,
// la catena CME, quindi i colori dicono il ruolo e non la fonte. Il nome in fondo a ogni linea porta il
// valore ("zero gamma 28.831"); sotto i 600 px i nomi vanno nella legenda.

import { useCallback, useMemo, useState } from 'react';
import type { BarSeriesOption, LineSeriesOption } from 'echarts';
import type { PuntoGexTempo, RispostaGexTempo } from '../../api/tipi';
import { auto, doppiaOra, num, oraDoppiaCorta } from '../../formato';
import { C } from '../../tema/colori';
import { Grafico, altezzaPx, type Altezza } from '../../componenti/Grafico';
import { Legenda, type Voce } from '../../componenti/Legenda';
import { Vuoto } from '../../componenti/Esito';
import type { Opzioni } from '../../viz/echarts';
import { asseTempo, asseValori, cartellino, coloreSegno, croce, distanzia, etichetteFinali, parolaSegno, puntiTip, rigaTip, testaTip, tooltip, valoriDi } from '../../viz/grafici';

type Campo = 'zero' | 'zero0' | 'callWall' | 'putWall';

interface Linea {
  campo: Campo;
  nome: string;
  colore: string;
  tratto: 'solid' | 'dashed' | 'dotted';
  gradini?: boolean;
}

/** I livelli, nell'ordine della legenda (il prezzo va prima, a parte). */
const LINEE: readonly Linea[] = [
  { campo: 'zero', nome: 'zero gamma', colore: C.flip, tratto: 'dashed' },
  { campo: 'zero0', nome: 'zero 0DTE', colore: C.flip, tratto: 'dotted' },
  { campo: 'callWall', nome: 'call wall', colore: C.freno, tratto: 'solid', gradini: true },
  { campo: 'putWall', nome: 'put wall', colore: C.acceleratore, tratto: 'solid', gradini: true },
];

export const ALTEZZA_GEX_TEMPO: Altezza = { min: 320, max: 480, schermo: 0.5 };
const FASCIA = 56;
const SOPRA = 16;
const SOTTO = 26;
const FRA = 22;

/**
 * La scala dei prezzi: il prezzo e gli zeri, piu' i muri entro l'1,5% dall'ultimo prezzo, con un margine
 * dello 0,3%, a multipli di 50. Un muro lontano esce dal grafico invece di schiacciarlo (il 2/11/2026 un
 * ruolo passo' a uno strike quasi alla pari, 31.000 contro 29.000).
 */
export function scalaNq(p: readonly PuntoGexTempo[]): { min: number; max: number } {
  const ultimo = p.at(-1)?.future ?? null;
  const vicini = (v: number | null) => v != null && ultimo != null && Math.abs(v - ultimo) <= ultimo * 0.015;
  const v = p
    .flatMap((x) => [x.future, x.zero, x.zero0, ...[x.callWall, x.putWall].filter(vicini)])
    .filter((x): x is number => x != null);
  if (!v.length) return { min: 0, max: 1 };
  const lo = Math.min(...v);
  const hi = Math.max(...v);
  const margine = ((lo + hi) / 2) * 0.003;
  return { min: Math.floor((lo - margine) / 50) * 50, max: Math.ceil((hi + margine) / 50) * 50 };
}

/** Un limite tondo per l'asse della GEX (verso l'esterno): 50, 100, 250, 500... */
export function tondo(v: number): number {
  if (v === 0) return 0;
  const a = Math.abs(v);
  const passo = [50, 100, 250, 500, 1000, 2500, 5000].find((k) => a <= k * 4) ?? 10_000;
  return Math.sign(v) * Math.ceil(a / passo) * passo;
}

/** Il punto all'istante t (l'ultimo con ts <= t), o l'ultimo. */
function puntoAl(p: readonly PuntoGexTempo[], t: number | null): PuntoGexTempo | undefined {
  if (t == null) return p.at(-1);
  let migliore: PuntoGexTempo | undefined;
  for (const x of p) if (x.ts <= t) migliore = x;
  return migliore ?? p[0];
}

export function GexNelTempo({ d }: { d: RispostaGexTempo }) {
  const [cursore, setCursore] = useState<number | null>(null);
  const p = d.punti;
  const sc = useMemo(() => scalaNq(p), [p]);

  const opzioni = useCallback(
    (larghezza: number): Opzioni | null => {
      if (p.length < 2) return null;
      const fine = etichetteFinali(larghezza);
      const destra = fine ? 132 : 16;
      const ultimo = p.at(-1)!;
      const dentro = (v: number | null | undefined): v is number => v != null && v >= sc.min && v <= sc.max;
      const finestra = typeof window === 'undefined' ? { larghezza: 1600, altezza: 900 } : { larghezza: window.innerWidth, altezza: window.innerHeight };
      const alta = altezzaPx(ALTEZZA_GEX_TEMPO, finestra) - SOPRA - SOTTO - FASCIA - FRA;
      const visibili = LINEE.filter((l) => dentro(ultimo[l.campo]));
      const sposta = distanzia([ultimo.future ?? sc.min, ...visibili.map((l) => ultimo[l.campo] as number)], [sc.min, sc.max], alta);
      const tempi = [p[0]!.ts, ultimo.ts] as const;

      const prezzo: LineSeriesOption = {
        type: 'line',
        name: 'prezzo NQ',
        data: p.map((x) => [x.ts, x.future]),
        showSymbol: false,
        connectNulls: false,
        z: 10,
        lineStyle: { color: C.prezzo, width: 2, shadowColor: 'rgba(0,0,0,.85)', shadowBlur: 3 },
        itemStyle: { color: C.prezzo },
        emphasis: { disabled: true },
        endLabel: { show: ultimo.future != null, ...cartellino(C.prezzo), formatter: num(ultimo.future, 2), offset: [0, sposta[0] ?? 0] },
      };
      const livelli: LineSeriesOption[] = LINEE.map((l) => {
        const k = visibili.indexOf(l);
        return {
          type: 'line',
          name: l.nome,
          data: p.map((x) => [x.ts, x[l.campo] ?? null]),
          step: l.gradini ? 'end' : undefined,
          showSymbol: false,
          connectNulls: false,
          clip: true,
          lineStyle: { color: l.colore, width: 1.5, type: l.tratto },
          itemStyle: { color: l.colore },
          emphasis: { focus: 'series' },
          endLabel: {
            show: fine && k >= 0,
            formatter: `${l.nome} ${num(ultimo[l.campo], 0)}`,
            color: C.testo2,
            fontSize: 11,
            distance: 6,
            offset: [0, sposta[k + 1] ?? 0],
          },
        };
      });
      const barre = (campo: 'totale' | 'totale0', nome: string, tenue: boolean): BarSeriesOption => ({
        type: 'bar',
        name: nome,
        xAxisIndex: 1,
        yAxisIndex: 1,
        data: p.map((x) => ({
          value: [x.ts, x[campo]],
          itemStyle: {
            color: coloreSegno(x[campo] ?? 0),
            opacity: tenue ? 0.45 : 1,
            ...(tenue ? { borderColor: coloreSegno(x[campo] ?? 0), borderWidth: 1 } : {}),
          },
        })),
        barMaxWidth: 6,
        barGap: '0%',
      });
      const gex = p.flatMap((x) => [x.totale, x.totale0]).filter((v): v is number => v != null);
      const asseNq = { min: sc.min, max: sc.max, axisLabel: { color: C.spento, fontSize: 11, formatter: (v: number) => num(v, 0), showMinLabel: false, showMaxLabel: false } };
      return {
        animation: false,
        grid: [
          { left: 64, right: destra, top: SOPRA, bottom: SOTTO + FASCIA + FRA },
          { left: 64, right: destra, height: FASCIA, bottom: SOTTO },
        ],
        xAxis: [
          asseTempo(tempi[0], tempi[1], { gridIndex: 0, axisLabel: { show: false }, name: '' }),
          asseTempo(tempi[0], tempi[1], { gridIndex: 1, name: '' }),
        ],
        yAxis: [
          asseValori({ gridIndex: 0, ...asseNq }),
          asseValori({
            gridIndex: 1,
            min: tondo(Math.min(0, ...gex)),
            max: tondo(Math.max(0, ...gex)),
            splitNumber: 2,
            name: 'GEX',
            nameLocation: 'end',
            nameGap: 4,
            axisLabel: { color: C.spento, fontSize: 10, formatter: (v: number) => num(v, 0), showMinLabel: true, showMaxLabel: true },
          }),
        ],
        axisPointer: { link: [{ xAxisIndex: 'all' }] },
        tooltip: tooltip({
          trigger: 'axis',
          axisPointer: croce('x', { x: (v) => doppiaOra(v) }),
          formatter: (q: unknown) => {
            const punti = puntiTip(q);
            const t = valoriDi(punti[0] ?? ({ value: [0], name: '', dataIndex: 0 } as never))[0] ?? 0;
            const x = p.find((y) => y.ts === t);
            if (!x) return '';
            return (
              testaTip(doppiaOra(t)) +
              rigaTip(C.prezzo, 'prezzo NQ', num(x.future, 2)) +
              LINEE.map((l) => rigaTip(l.colore, l.nome, num(x[l.campo], 0))).join('') +
              rigaTip(coloreSegno(x.totale ?? 0), `GEX totale (${parolaSegno(x.totale ?? 0)})`, auto(x.totale)) +
              rigaTip(coloreSegno(x.totale0 ?? 0), `GEX 0DTE (${parolaSegno(x.totale0 ?? 0)})`, auto(x.totale0))
            );
          },
        }),
        series: [prezzo, ...livelli, barre('totale', 'GEX totale', false), barre('totale0', 'GEX 0DTE', true)],
      };
    },
    [p, sc],
  );

  if (p.length < 2) return <Vuoto>Ancora pochi minuti di GEX viva in questa giornata: il server ne scrive uno al minuto, a CME aperto.</Vuoto>;
  const x = puntoAl(p, cursore)!;
  const voci: Voce[] = [
    { colore: C.prezzo, testo: 'prezzo NQ', valore: num(x.future, 2) },
    ...LINEE.map((l) => ({ colore: l.colore, testo: l.nome, tratto: l.tratto, valore: x[l.campo] == null ? null : num(x[l.campo], 0) })),
    { colore: coloreSegno(x.totale ?? 0), testo: `GEX totale, ${parolaSegno(x.totale ?? 0)}`, forma: 'rett' as const, valore: auto(x.totale) },
    { colore: coloreSegno(x.totale0 ?? 0), testo: `GEX 0DTE, ${parolaSegno(x.totale0 ?? 0)}`, forma: 'rett' as const, opacita: 0.6, valore: auto(x.totale0) },
  ];
  return (
    <>
      <Legenda voci={voci}>
        <span className="spento">{cursore == null ? `ultimo minuto ${oraDoppiaCorta(p.at(-1)!.ts)}` : oraDoppiaCorta(x.ts)} · ora di New York · GEX in M$ per 1%</span>
      </Legenda>
      <Grafico opzioni={opzioni} altezza={ALTEZZA_GEX_TEMPO} etichetta="GEX viva di NQ nel tempo: prezzo, zero gamma, muri e GEX totale" onCursore={setCursore} />
    </>
  );
}
