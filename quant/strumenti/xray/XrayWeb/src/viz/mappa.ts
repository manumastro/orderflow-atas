// La mappa di calore (tempo x prezzo) disegnata come UN'immagine: un canvas fuori schermo con un
// pixel per cella, ingrandito senza sfumare, messo sotto gli assi di ECharts da una serie custom con
// un solo elemento. Prima ogni cella era un rettangolo di ECharts: 24.000 celle bloccavano la pagina
// per circa 3 secondi a ogni disegno (2 novembre 2026). Il tooltip si calcola dalla posizione
// (valoreAl), non dai rettangoli.

import type {
  CustomSeriesOption,
  CustomSeriesRenderItemAPI,
  CustomSeriesRenderItemReturn,
} from 'echarts';
import { rgbDi, segnoGamma, type ScalaSegno } from './grafici';

export interface DatiMappa {
  /** l'inizio di ogni colonna, ms */
  tempi: readonly number[];
  /** i prezzi delle righe (in SPX) */
  prezzi: readonly number[];
  /** valori[colonna][riga] */
  valori: readonly (readonly (number | null)[])[];
  /** la durata di una colonna, ms */
  durata: number;
  passoPrezzo: number;
  /** la scala: +-massimo e' il colore pieno */
  massimo: number;
  /** i colori del segno: la gamma (default) o il flusso (la charm: comprano/vendono col tempo) */
  scala?: ScalaSegno;
  /**
   * la fine vera dei dati, ms: l'ultima colonna dal vivo e' spesso a meta' (alle 10:32 la colonna delle
   * 10:30 di 5 minuti ha 2 minuti) e non deve andare oltre il prezzo. Assente = colonne tutte intere.
   */
  fine?: number;
}

/** Il lato massimo dell'immagine ingrandita, in pixel. */
const LATO_MASSIMO = 4096;

export interface Immagine {
  canvas: HTMLCanvasElement | null;
  /** gli estremi in coordinate dei dati: tempo da t0 a t1, prezzo (SPX) da p0 (in alto) a p1 */
  t0: number;
  t1: number;
  pAlto: number;
  pBasso: number;
}

/** Disegna la mappa su un canvas: una cella = un pixel, poi ingrandita senza sfumare i bordi. */
export function disegnaMappa(m: DatiMappa): Immagine {
  const t0 = m.tempi[0] ?? 0;
  const ultima = m.tempi.at(-1) ?? 0;
  const t1 = ultima + m.durata;
  const pMin = Math.min(...m.prezzi);
  const pMax = Math.max(...m.prezzi);
  const mezzo = m.passoPrezzo / 2;
  const immagine: Immagine = { canvas: null, t0, t1, pAlto: pMax + mezzo, pBasso: pMin - mezzo };
  const colonne = Math.max(1, Math.round((t1 - t0) / m.durata));
  const righe = Math.max(1, Math.round((pMax - pMin) / m.passoPrezzo) + 1);
  if (typeof document === 'undefined') return immagine;
  const piccolo = document.createElement('canvas');
  piccolo.width = colonne;
  piccolo.height = righe;
  const cp = piccolo.getContext('2d');
  if (!cp) return immagine;
  const img = cp.createImageData(colonne, righe);
  const px = img.data;
  m.tempi.forEach((t, i) => {
    const x = Math.round((t - t0) / m.durata);
    if (x < 0 || x >= colonne) return;
    const riga = m.valori[i] ?? [];
    m.prezzi.forEach((p, j) => {
      const c = rgbDi(riga[j], m.massimo, m.scala ?? segnoGamma);
      if (!c) return;
      const y = Math.round((pMax - p) / m.passoPrezzo);
      if (y < 0 || y >= righe) return;
      const k = (y * colonne + x) * 4;
      px[k] = c[0];
      px[k + 1] = c[1];
      px[k + 2] = c[2];
      px[k + 3] = 255;
    });
  });
  cp.putImageData(img, 0, 0);
  // ingrandita a mano, senza sfumare: ECharts poi la stira al rettangolo degli assi
  const s = Math.max(1, Math.min(Math.floor(LATO_MASSIMO / colonne), Math.floor(LATO_MASSIMO / righe), Math.ceil(2400 / Math.max(colonne, righe))));
  // l'ultima colonna, se e' a meta', si taglia: in pixel interi, e t1 segue il taglio
  const frazione = m.fine != null && m.fine > ultima ? Math.min(1, (m.fine - ultima) / m.durata) : 1;
  const ultimaPx = Math.max(1, Math.round(s * frazione));
  const grande = document.createElement('canvas');
  grande.width = (colonne - 1) * s + ultimaPx;
  immagine.t1 = t1 - m.durata + (m.durata * ultimaPx) / s;
  grande.height = righe * s;
  const cg = grande.getContext('2d');
  if (!cg) return immagine;
  cg.imageSmoothingEnabled = false;
  cg.drawImage(piccolo, 0, 0, colonne * s, grande.height);
  immagine.canvas = grande;
  return immagine;
}

/** La serie che mette l'immagine sotto gli assi, negli stessi tempi e prezzi delle linee. */
export function serieMappa(im: Immagine, converti: (p: number) => number = (p) => p, nome = 'mappa'): CustomSeriesOption {
  const alto = converti(im.pAlto);
  const basso = converti(im.pBasso);
  return {
    type: 'custom',
    name: nome,
    silent: true,
    clip: true,
    animation: false,
    data: im.canvas ? [[im.t0, alto]] : [],
    renderItem(_p, api: CustomSeriesRenderItemAPI): CustomSeriesRenderItemReturn {
      const [xa = 0, ya = 0] = api.coord([im.t0, alto]);
      const [xb = 0, yb = 0] = api.coord([im.t1, basso]);
      if (!im.canvas) return null;
      return { type: 'image', silent: true, style: { image: im.canvas, x: xa, y: ya, width: xb - xa, height: yb - ya } };
    },
    encode: { x: 0, y: 1 },
    z: 1,
  };
}

/** La cella sotto un punto (tempo, prezzo nell'unita' mostrata): il suo istante, prezzo e valore. */
export function valoreAl(m: DatiMappa, t: number, y: number, converti: (p: number) => number = (p) => p) {
  let i = -1;
  let lo = 0;
  let hi = m.tempi.length - 1;
  while (lo <= hi) {
    const k = (lo + hi) >> 1;
    if ((m.tempi[k] ?? Infinity) <= t) {
      i = k;
      lo = k + 1;
    } else hi = k - 1;
  }
  const ti = m.tempi[i];
  if (ti == null || t >= ti + m.durata) return null;
  let j = -1;
  let migliore = Infinity;
  m.prezzi.forEach((p, k) => {
    const d = Math.abs(converti(p) - y);
    if (d < migliore) {
      migliore = d;
      j = k;
    }
  });
  if (j < 0 || migliore > m.passoPrezzo * 0.75) return null;
  const v = m.valori[i]?.[j] ?? null;
  return { indice: i, t: ti, prezzo: m.prezzi[j] ?? 0, valore: v };
}
