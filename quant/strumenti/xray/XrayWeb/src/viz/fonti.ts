// Lo stile di una serie secondo la sua fonte: il colore e il tratto della fonte. Se nello stesso
// grafico ci sono due serie della stessa fonte (VXN da NDX e vol di QQQ, entrambe Cboe), la seconda
// porta anche i simboli sui punti: il colore resta quello della fonte, la forma le distingue.
// Un grafico mostra o le fonti o il segno, mai tutti e due.

import type { LineSeriesOption } from 'echarts';
import { C, FONT } from '../tema/colori';
import { fonte, type Tratto } from '../tema/fonti';
import type { Voce } from '../componenti/Legenda';

export interface StileSerie {
  colore: string;
  tratto: Tratto;
  simbolo: 'none' | 'triangle' | 'rect';
}

export function stiliPerFonte(fonti: readonly string[]): StileSerie[] {
  const viste = new Map<string, number>();
  return fonti.map((id) => {
    const f = fonte(id);
    const k = viste.get(id) ?? 0;
    viste.set(id, k + 1);
    return { colore: f.colore, tratto: f.tratto, simbolo: k === 0 ? 'none' : k === 1 ? 'triangle' : 'rect' };
  });
}

/**
 * Una linea nel colore e nel tratto della sua fonte, col nome scritto in fondo (etichetta diretta).
 * `etichettaFinale` va spento sotto i 600 px (etichetteFinali di grafici.ts): i nomi stanno nella legenda.
 * `spostamento` (px, da distanzia) tiene i nomi in fondo lontani fra loro.
 */
export function lineaFonte(
  nome: string,
  dati: [number, number | null][],
  stile: StileSerie,
  etichettaFinale = true,
  spostamento = 0,
): LineSeriesOption {
  return {
    type: 'line',
    name: nome,
    data: dati,
    showSymbol: stile.simbolo !== 'none',
    symbol: stile.simbolo === 'none' ? 'circle' : stile.simbolo,
    symbolSize: 7,
    connectNulls: false,
    lineStyle: { color: stile.colore, width: 2, type: stile.tratto },
    itemStyle: { color: stile.colore, borderColor: C.superficie, borderWidth: 1 },
    emphasis: { focus: 'series' },
    endLabel: etichettaFinale
      ? { show: true, formatter: nome, color: C.testo2, fontSize: 11, fontFamily: FONT, distance: 6, offset: [0, spostamento] }
      : { show: false },
  };
}

/** La voce di legenda di una serie: tratto, icona della fonte, nome. */
export function voceFonte(nome: string, idFonte: string, stile: StileSerie, vuota = false): Voce {
  return { colore: stile.colore, testo: nome, tratto: stile.tratto, fonte: idFonte, vuota, forma: 'linea' };
}
