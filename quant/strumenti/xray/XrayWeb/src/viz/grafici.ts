// I pezzi comuni dei grafici, cosi' ogni pagina eredita lo stesso comportamento (5 novembre 2026, la
// pagina in stile QuantData). Le regole (skill dataviz): un asse per grafico, segni sottili, griglia a
// filo, tooltip su ogni segno, legenda quando le serie sono piu' di una, il testo con gli inchiostri del
// testo e mai col colore della serie.
//
// Due vincoli che legge scripts/leggi_pagina.py: Grafico.tsx appende l'istanza al nodo (`__grafico`), e
// ogni linea di riferimento (markLine) tiene `name` = etichetta minuscola CON il valore ("flip 7.760").

import type {
  CustomSeriesOption,
  CustomSeriesRenderItemAPI,
  CustomSeriesRenderItemParams,
  CustomSeriesRenderItemReturn,
  GridComponentOption,
  LineSeriesOption,
  MarkLineComponentOption,
  TooltipComponentOption,
  XAXisComponentOption,
  YAXisComponentOption,
} from 'echarts';
import { C, FONT, INCHIOSTRI, SCURO } from '../tema/colori';
import { distanza, num, oraNY } from '../formato';

export const TESTO = { color: C.testo2, fontFamily: FONT, fontSize: 12 };

type AsseX = XAXisComponentOption;
type AsseY = YAXisComponentOption;

const etichetta = { color: C.spento, fontSize: 11, fontFamily: FONT };

/** Dal punto (x, y nelle unita' degli assi) all'HTML del tooltip, o null per nasconderlo. */
export type Puntatore = (x: number, y: number) => string | null;

// ------------------------------------------------------------------ il segno: due compiti, due coppie

export interface ScalaSegno {
  pos: string;
  neg: string;
  neutro: string;
  parolaPos: string;
  parolaNeg: string;
}

/** La gamma dei market maker: verde freno, rosso acceleratore. */
export const segnoGamma: ScalaSegno = { pos: C.freno, neg: C.acceleratore, neutro: C.neutro, parolaPos: 'freno', parolaNeg: 'acceleratore' };
/** Il flusso (DEX, VEX, charm): blu comprato, arancio venduto. */
export const segnoFlusso: ScalaSegno = { pos: C.compra, neg: C.vende, neutro: C.neutro, parolaPos: 'comprato', parolaNeg: 'venduto' };

export const coloreSegno = (v: number, s: ScalaSegno = segnoGamma): string => (v >= 0 ? s.pos : s.neg);
/** La parola del segno: mai "positivo" o "negativo" da soli. */
export const parolaSegno = (v: number, s: ScalaSegno = segnoGamma): string => (v >= 0 ? s.parolaPos : s.parolaNeg);

// ------------------------------------------------------------------ assi

/** Un asse di valori: segni recessivi, griglia sottile. Con min e max dati, i loro numeri (non tondi) non si scrivono. */
export function asseValori(extra: Record<string, unknown> = {}): AsseX & AsseY {
  const estremi = typeof extra.min === 'number' || typeof extra.max === 'number';
  return {
    type: 'value',
    axisLine: { lineStyle: { color: C.asse } },
    axisTick: { show: false },
    axisLabel: { ...etichetta, ...(estremi ? { showMinLabel: false, showMaxLabel: false } : {}) },
    splitLine: { lineStyle: { color: C.griglia, width: 1 } },
    nameTextStyle: { color: C.spento, fontSize: 11 },
    ...extra,
  } as AsseX & AsseY;
}

/** L'intervallo fra le tacche: un multiplo del passo degli strike che dia da 6 a 10 tacche. */
export function intervalloPrezzi(min: number, max: number, passo: number): number {
  const ampiezza = Math.abs(max - min);
  if (!(passo > 0) || !(ampiezza > 0)) return passo > 0 ? passo : 1;
  for (const k of [1, 2, 5, 10, 20, 25, 50, 100, 200, 250, 500, 1000]) {
    if (ampiezza / (passo * k) <= 10) return passo * k;
  }
  return passo * 1000;
}

/** L'asse dei prezzi (verticale): numeri senza decimali, senza il primo e l'ultimo (tagliati). */
export function assePrezzi(min: number, max: number, extra: Record<string, unknown> & { passo?: number } = {}): AsseY {
  const { passo, ...resto } = extra;
  return asseValori({
    min,
    max,
    ...(passo ? { interval: intervalloPrezzi(min, max, passo) } : {}),
    axisLabel: { ...etichetta, formatter: (v: number) => num(v, 0), showMinLabel: false, showMaxLabel: false },
    ...resto,
  });
}

/** Sotto questa larghezza della finestra (il telefono) l'asse del tempo non scrive il nome del fuso. */
const TELEFONO = 768;

/**
 * L'asse del tempo, in ora di New York. Il nome del fuso sta DENTRO il grafico, in basso a destra sopra la
 * linea dell'asse, allineato alla sua fine: fuori, a destra, ECharts 6 lo spostava oltre il bordo e si
 * tagliava. `nameMoveOverlap: false` tiene l'allineamento; `z` alto lo porta sopra le mappe. Sul telefono
 * il nome si toglie qui, per tutte le pagine (lo dice gia' la legenda o il piede).
 */
export function asseTempo(min: number, max: number, extra: Record<string, unknown> = {}): AsseX {
  const telefono = typeof window !== 'undefined' && window.innerWidth < TELEFONO;
  return {
    type: 'time',
    min,
    max,
    name: 'ora di New York',
    nameLocation: 'end',
    nameGap: 0,
    nameMoveOverlap: false,
    nameTextStyle: {
      color: C.testo2,
      fontSize: 11,
      fontFamily: FONT,
      align: 'right',
      verticalAlign: 'bottom',
      padding: [1, 4, 1, 4],
      backgroundColor: 'rgba(11,15,23,0.65)',
      borderRadius: 3,
    },
    z: 10,
    axisLine: { lineStyle: { color: C.asse } },
    axisTick: { show: false },
    axisLabel: { ...etichetta, formatter: (ms: number) => oraNY(ms), hideOverlap: true },
    splitLine: { show: false },
    ...extra,
    ...(telefono ? { name: '' } : {}),
  } as AsseX;
}

/** La linea dello zero, piu' marcata della griglia: `asse` e' l'asse dei valori ('x' nei profili). */
export function asseZero(asse: 'x' | 'y' = 'y'): MarkLineComponentOption {
  return {
    symbol: 'none',
    silent: true,
    animation: false,
    data: [{ ...(asse === 'x' ? { xAxis: 0 } : { yAxis: 0 }), name: 'zero' }],
    lineStyle: { color: C.asseZero, width: 1, type: 'solid' },
    label: { show: false },
  };
}

/**
 * I margini per la larghezza (Grafico la conosce): dai 600 px a destra lo spazio per il cartellino (64) o
 * per i nomi in fondo alle linee (150); sotto, quasi niente (i nomi vanno nella legenda). A sinistra lo
 * spazio lo decidono le etichette dell'asse.
 */
export function griglia(
  larghezza: number,
  opz: { destra?: 'cartellino' | 'etichette' | 'niente'; sinistra?: number; sopra?: number; sotto?: number } = {},
): GridComponentOption {
  const stretto = larghezza > 0 && larghezza < 600;
  const destra = opz.destra ?? 'cartellino';
  const right = stretto ? (destra === 'cartellino' ? 16 : 12) : destra === 'cartellino' ? 64 : destra === 'etichette' ? 150 : 16;
  return {
    left: opz.sinistra ?? 8,
    right,
    top: opz.sopra ?? 12,
    bottom: opz.sotto ?? 8,
    outerBoundsMode: 'same',
    outerBoundsContain: 'axisLabel',
  };
}

/** Sotto i 600 px niente nomi in fondo alle linee: vanno nella legenda. */
export const etichetteFinali = (larghezza: number): boolean => !(larghezza > 0 && larghezza < 600);

// ------------------------------------------------------------------ tooltip e croce

export function tooltip(extra: TooltipComponentOption = {}): TooltipComponentOption {
  return {
    backgroundColor: C.tooltip,
    borderColor: C.bordoForte,
    borderWidth: 1,
    textStyle: { color: C.testo, fontSize: 12, fontFamily: FONT },
    extraCssText: 'box-shadow: 0 6px 24px rgba(0,0,0,.45); border-radius: 8px;',
    confine: true,
    ...extra,
  };
}

/** La croce del cursore, una sola per tutti: filo pieno, cartellini sugli assi. */
export function croce(
  asse: 'x' | 'y' | 'xy' = 'xy',
  scrivi: { x?: (v: number) => string; y?: (v: number) => string } = {},
): Record<string, unknown> {
  const filo = { color: C.testo2, opacity: 0.6, width: 1, type: 'solid' };
  return {
    type: asse === 'xy' ? 'cross' : 'line',
    ...(asse === 'xy' ? {} : { axis: asse }),
    lineStyle: filo,
    crossStyle: filo,
    label: {
      show: true,
      backgroundColor: C.superficie2,
      borderColor: C.bordoForte,
      borderWidth: 1,
      color: C.testo,
      fontFamily: FONT,
      fontSize: 11,
      padding: [3, 6],
      formatter: (p: { axisDimension?: string; value?: unknown }) => {
        const v = Number(p.value);
        const f = p.axisDimension === 'x' ? scrivi.x : scrivi.y;
        return f ? f(v) : num(v, 0);
      },
    },
  };
}

export function escape(s: string): string {
  return s.replace(/[&<>"']/g, (c) => ({ '&': '&amp;', '<': '&lt;', '>': '&gt;', '"': '&quot;', "'": '&#39;' })[c] ?? c);
}

/** Riga di tooltip: il valore forte, il nome dopo, una lineetta del colore della serie. */
export function rigaTip(colore: string | null, nome: string, valore: string): string {
  const tratto = colore
    ? `<span style="display:inline-block;width:12px;height:2px;background:${colore};margin-right:6px;vertical-align:3px"></span>`
    : '';
  return `<div>${tratto}<b>${escape(valore)}</b> <span style="color:${C.testo2}">${escape(nome)}</span></div>`;
}

/** La testa grigia di un tooltip. */
export function testaTip(testo: string): string {
  return `<div style="color:${C.testo2}">${escape(testo)}</div>`;
}

/** Il parametro di un formatter del tooltip, con i soli campi usati. */
export interface PuntoTip {
  value: unknown;
  name: string;
  seriesName?: string;
  seriesType?: string;
  color?: unknown;
  dataIndex: number;
}

/** I parametri di un formatter come lista (trigger "axis") o come uno (trigger "item"). */
export function puntiTip(p: unknown): PuntoTip[] {
  return (Array.isArray(p) ? p : [p]) as PuntoTip[];
}

export function valoriDi(p: PuntoTip): number[] {
  return Array.isArray(p.value) ? p.value.map((x) => Number(x)) : [Number(p.value)];
}

export const coloreTip = (p: PuntoTip): string | null => (typeof p.color === 'string' ? p.color : null);

// ------------------------------------------------------------------ colori: OKLab, contrasto

type Rgb = [number, number, number];
type Lab = [number, number, number];

function rgb(hex: string): Rgb {
  const h = hex.replace('#', '');
  return [0, 2, 4].map((i) => parseInt(h.slice(i, i + 2), 16)) as Rgb;
}

/** Un colore come '#rrggbb', 'rgb(r,g,b)' o terna. */
function comeRgb(c: Rgb | string): Rgb {
  if (Array.isArray(c)) return c;
  const m = /^rgba?\(\s*(\d+)[,\s]+(\d+)[,\s]+(\d+)/i.exec(c);
  if (m) return [Number(m[1]), Number(m[2]), Number(m[3])];
  return rgb(c);
}

const lineare = (c: number) => {
  const x = c / 255;
  return x <= 0.04045 ? x / 12.92 : ((x + 0.055) / 1.055) ** 2.4;
};
const gamma = (x: number) => {
  const c = x <= 0.0031308 ? 12.92 * x : 1.055 * x ** (1 / 2.4) - 0.055;
  return Math.round(Math.min(1, Math.max(0, c)) * 255);
};

/** Da sRGB a OKLab (Ottosson): la distanza fra colori come la vede l'occhio. */
export function oklab(c: Rgb | string): Lab {
  const [r, g, b] = comeRgb(c).map(lineare) as Rgb;
  const l = Math.cbrt(0.4122214708 * r + 0.5363325363 * g + 0.0514459929 * b);
  const m = Math.cbrt(0.2119034982 * r + 0.6806995451 * g + 0.1073969566 * b);
  const s = Math.cbrt(0.0883024619 * r + 0.2817188376 * g + 0.6299787005 * b);
  return [
    0.2104542553 * l + 0.793617785 * m - 0.0040720468 * s,
    1.9779984951 * l - 2.428592205 * m + 0.4505937099 * s,
    0.0259040371 * l + 0.7827717662 * m - 0.808675766 * s,
  ];
}

function daOklab([L, A, B]: Lab): Rgb {
  const l = (L + 0.3963377774 * A + 0.2158037573 * B) ** 3;
  const m = (L - 0.1055613458 * A - 0.0638541728 * B) ** 3;
  const s = (L - 0.0894841775 * A - 1.291485548 * B) ** 3;
  return [
    gamma(4.0767416621 * l - 3.3077115913 * m + 0.2309699292 * s),
    gamma(-1.2684380046 * l + 2.6097574011 * m - 0.3413193965 * s),
    gamma(-0.0041960863 * l - 0.7034186147 * m + 1.707614701 * s),
  ];
}

const poli = new Map<ScalaSegno, { neutro: Lab; pos: Lab; neg: Lab }>();
function poliDi(s: ScalaSegno) {
  let p = poli.get(s);
  if (!p) {
    p = { neutro: oklab(s.neutro), pos: oklab(s.pos), neg: oklab(s.neg) };
    poli.set(s, p);
  }
  return p;
}

/**
 * Il colore di un valore su [-massimo, +massimo]: dal centro grigio al polo della scala, interpolato in
 * OKLab (non in RGB: i toni medi restano verdi e rossi, non oliva). La radice tiene visibili i valori
 * piccoli accanto a un picco.
 */
export function rgbDi(valore: number | null | undefined, massimo: number, scala: ScalaSegno = segnoGamma): Rgb | null {
  if (valore == null || !Number.isFinite(valore) || !massimo) return null;
  const t = Math.min(1, Math.sqrt(Math.abs(valore) / massimo));
  const p = poliDi(scala);
  const a = p.neutro;
  const b = valore >= 0 ? p.pos : p.neg;
  return daOklab([0, 1, 2].map((i) => a[i]! + (b[i]! - a[i]!) * t) as Lab);
}

export function coloreDi(valore: number | null | undefined, massimo: number, scala: ScalaSegno = segnoGamma): string | null {
  const c = rgbDi(valore, massimo, scala);
  return c ? `rgb(${c[0]},${c[1]},${c[2]})` : null;
}

/** La luminanza relativa WCAG. */
function luminanza(c: Rgb): number {
  const [r, g, b] = c.map(lineare) as Rgb;
  return 0.2126 * r + 0.7152 * g + 0.0722 * b;
}

/** Il contrasto WCAG fra due colori (da 1 a 21). */
export function contrasto(a: Rgb | string, b: Rgb | string): number {
  const la = luminanza(comeRgb(a));
  const lb = luminanza(comeRgb(b));
  return (Math.max(la, lb) + 0.05) / (Math.min(la, lb) + 0.05);
}

/** Il testo sopra un colore pieno: chiaro se il contrasto WCAG e' >= 4,5:1, altrimenti scuro (#0b0f17). */
export function inchiostroSu(colore: Rgb | string): string {
  return contrasto(C.testo, colore) >= 4.5 ? C.testo : SCURO;
}

export function massimoAssoluto(valori: Iterable<number | null | undefined>): number {
  let m = 0;
  for (const x of valori) if (x != null && Number.isFinite(x)) m = Math.max(m, Math.abs(x));
  return m;
}

/** Il 98° percentile dei valori assoluti: un solo picco non spegne il resto della mappa. */
export function scala98(valori: (number | null)[][]): number {
  const ordinati = valori
    .flat()
    .filter((x): x is number => x != null && Number.isFinite(x))
    .map(Math.abs)
    .sort((a, b) => a - b);
  return ordinati[Math.floor(ordinati.length * 0.98)] || massimoAssoluto(ordinati) || 1;
}

/** Il passo tipico fra prezzi (la mediana delle differenze). */
export function passoDi(prezzi: readonly number[]): number {
  const s = [...prezzi].sort((a, b) => a - b);
  const d: number[] = [];
  for (let i = 1; i < s.length; i++) {
    const x = s[i] ?? 0;
    const y = s[i - 1] ?? 0;
    if (x > y) d.push(x - y);
  }
  d.sort((a, b) => a - b);
  return d[d.length >> 1] ?? 1;
}

// ------------------------------------------------------------------ il prezzo e i cartellini

/** Il cartellino: il valore sul bordo destro, pieno nel colore della linea, il testo che si legge sopra. */
export function cartellino(colore: string): Record<string, unknown> {
  return {
    position: 'end',
    backgroundColor: colore,
    color: inchiostroSu(colore),
    padding: [2, 5],
    borderRadius: 3,
    fontWeight: 600,
    fontSize: 11,
    fontFamily: FONT,
  };
}

/**
 * Il prezzo: 2 px di inchiostro con un alone scuro, sempre sopra (z 10). Due serie: "<nome> (spot vivo)"
 * piena dove `vivo`, "<nome> (ES − base)" tratteggiata dove no. Mai trasparente. Il cartellino col valore
 * sta in fondo alla serie che ha l'ultimo punto.
 */
export function lineaPrezzo(tempi: readonly number[], prezzi: readonly (number | null)[], vivo: readonly boolean[], nome: string): LineSeriesOption[] {
  const n = tempi.length;
  const piena: [number, number | null][] = [];
  const tratteggiata: [number, number | null][] = [];
  let ultimo = -1;
  for (let i = 0; i < n; i++) {
    const p = prezzi[i] ?? null;
    const t = tempi[i] ?? 0;
    if (p != null) ultimo = i;
    const v = vivo[i] ?? false;
    const prima = i > 0 ? (vivo[i - 1] ?? false) : v;
    const dopo = i < n - 1 ? (vivo[i + 1] ?? false) : v;
    // un tratto pieno unisce due punti vivi; uno tratteggiato tocca almeno un punto non vivo
    const inPiena = v && (prima || dopo || n === 1);
    const inTratteggiata = !v || !prima || !dopo;
    piena.push([t, p != null && inPiena ? p : null]);
    tratteggiata.push([t, p != null && inTratteggiata ? p : null]);
  }
  const ultimaPiena = ultimo >= 0 && (vivo[ultimo] ?? false);
  const stile = (tipo: 'solid' | 'dashed') => ({
    color: C.prezzo,
    width: 2,
    type: tipo === 'dashed' ? [6, 3] : 'solid',
    shadowColor: 'rgba(0,0,0,.85)',
    shadowBlur: 3,
  });
  const fine = (mostra: boolean) =>
    mostra
      ? { show: true, ...cartellino(C.prezzo), formatter: (p: { value?: unknown }) => num(Array.isArray(p.value) ? Number(p.value[1]) : Number(p.value), 2) }
      : { show: false };
  const base = { type: 'line' as const, showSymbol: false, connectNulls: false, z: 10, emphasis: { disabled: true }, itemStyle: { color: C.prezzo } };
  return [
    { ...base, name: `${nome} (spot vivo)`, data: piena, lineStyle: stile('solid') as LineSeriesOption['lineStyle'], endLabel: fine(ultimaPiena) },
    { ...base, name: `${nome} (ES − base)`, data: tratteggiata, lineStyle: stile('dashed') as LineSeriesOption['lineStyle'], endLabel: fine(!ultimaPiena && ultimo >= 0) },
  ];
}

// ------------------------------------------------------------------ le linee orizzontali

export interface LineaOrizzontale {
  y: number;
  colore: string;
  /** il `name` della markLine: minuscolo, CON il valore ("flip 7.760"); lo legge leggi_pagina.py */
  etichetta: string;
  /** quello che si vede nel cartellino: "FLIP 7.760 ↑29" (di solito l'etichetta in maiuscolo) */
  testo?: string;
  spessore?: number;
  tratto?: 'solid' | 'dashed' | 'dotted';
  opacita?: number;
  tipo?: 'prezzo' | 'flip' | 'livello' | 'bordo';
}

/**
 * Distanzia le etichette in verticale: per ogni y lo spostamento in px (positivo = in giu'), perche' fra
 * due restino almeno `minimo` px. Non ne toglie nessuna, e le tiene dentro l'altezza.
 */
export function distanzia(y: readonly number[], asse: [number, number], altezzaPx: number, minimo = 14): number[] {
  const [lo, hi] = asse;
  const campo = hi - lo || 1;
  const pixel = y.map((v) => ((hi - v) / campo) * altezzaPx);
  const ordine = pixel.map((p, i) => [p, i] as const).sort((a, b) => a[0] - b[0]);
  const q = ordine.map(([p]) => p);
  for (let k = 1; k < q.length; k++) q[k] = Math.max(q[k]!, q[k - 1]! + minimo);
  // se l'ultima esce dal fondo, si risale
  if (q.length && q[q.length - 1]! > altezzaPx) {
    q[q.length - 1] = altezzaPx;
    for (let k = q.length - 2; k >= 0; k--) q[k] = Math.min(q[k]!, q[k + 1]! - minimo);
  }
  const sposta = new Array<number>(y.length).fill(0);
  ordine.forEach(([p, i], k) => (sposta[i] = q[k]! - p));
  return sposta;
}

/** Il nome di una linea senza il valore in fondo: "flip 7.760" -> "flip". */
const senzaValore = (s: string) => s.replace(/\s+[−+-]?[\d.,]+\s*$/u, '').trim();

const PRIORITA: Record<string, number> = { prezzo: 0, flip: 1, livello: 2, bordo: 3 };

/** Fonde le linee piu' vicine di `soglia` fra loro (non il prezzo): una zona sola, nessun nome perso. */
export function fondiLinee(linee: readonly LineaOrizzontale[], soglia: number, cifre = 0): LineaOrizzontale[] {
  const prezzi = linee.filter((l) => l.tipo === 'prezzo');
  const altre = linee.filter((l) => l.tipo !== 'prezzo').sort((a, b) => a.y - b.y);
  const gruppi: LineaOrizzontale[][] = [];
  for (const l of altre) {
    const g = gruppi.at(-1);
    const ultima = g?.at(-1);
    if (g && ultima && l.y - ultima.y <= soglia) g.push(l);
    else gruppi.push([l]);
  }
  const fuse = gruppi.map((g) => {
    if (g.length === 1) return g[0]!;
    const capo = [...g].sort((a, b) => (PRIORITA[a.tipo ?? 'livello'] ?? 2) - (PRIORITA[b.tipo ?? 'livello'] ?? 2))[0]!;
    const y = (g[0]!.y + g.at(-1)!.y) / 2;
    const nomi = g.map((l) => senzaValore(l.testo ?? l.etichetta.toUpperCase()));
    return {
      ...capo,
      y,
      etichetta: g.map((l) => l.etichetta).join(' + '),
      testo: `${nomi.join(' + ')} ${num(y, cifre)}`,
    };
  });
  return [...prezzi, ...fuse];
}

/**
 * Le linee orizzontali di riferimento come markLine, col cartellino. Con `prezzo`, la distanza con la
 * freccia nel testo ("↑29"); con `fondi` (punti), le linee vicine diventano una ("FLIP + MURO FRENO 0DTE
 * 7.762 ↑31", e i `name` uniti con " + "); con `asse` e `altezzaPx`, i cartellini si distanziano.
 *
 * Senza opzioni il cartellino sta dentro la griglia (insideEndTop), come prima: le pagine di oggi non
 * lasciano lo spazio a destra. Con `asse` sta fuori, sul bordo destro (griglia() con destra 'cartellino').
 */
export function lineeOrizzontali(
  linee: LineaOrizzontale[],
  opz: { asse?: [number, number]; altezzaPx?: number; prezzo?: number; fondi?: number; cifre?: number } = {},
): MarkLineComponentOption {
  const cifre = opz.cifre ?? 0;
  const tutte = opz.fondi != null && opz.fondi > 0 ? fondiLinee(linee, opz.fondi, cifre) : linee;
  const fuori = opz.asse != null;
  const sposta = opz.asse && opz.altezzaPx ? distanzia(tutte.map((l) => l.y), opz.asse, opz.altezzaPx) : tutte.map(() => 0);
  return {
    silent: false,
    symbol: 'none',
    animation: false,
    data: tutte.map((l, i) => {
      const base = l.testo ?? l.etichetta.toUpperCase();
      const dist = opz.prezzo != null && l.tipo !== 'prezzo' ? ` ${distanza(l.y - opz.prezzo, 0)}` : '';
      const testo = `${base}${dist}`;
      return {
        yAxis: l.y,
        name: l.etichetta,
        lineStyle: { color: l.colore, width: l.spessore ?? 1, type: l.tratto ?? 'solid', opacity: l.opacita ?? 1 },
        label: fuori
          ? { show: true, ...cartellino(l.colore), formatter: testo, offset: [0, sposta[i] ?? 0] }
          : { show: true, ...cartellino(l.colore), position: 'insideEndTop', formatter: testo },
      };
    }),
  } as MarkLineComponentOption;
}

/** Una linea verticale a un istante (o a un valore di x), senza etichetta. */
export function lineaVerticale(x: number, colore: string = C.testo2): MarkLineComponentOption {
  return { symbol: 'none', silent: true, animation: false, data: [{ xAxis: x }], lineStyle: { color: colore, width: 1.5 }, label: { show: false } };
}

/** Una serie vuota che porta solo le linee di riferimento. */
export function serieLinee(markLine: MarkLineComponentOption, nome = 'riferimenti'): LineSeriesOption {
  return { type: 'line', name: nome, data: [], markLine };
}

/** Lo stile di linea di un inchiostro (Deriva: identita' senza segno, con il nome in fondo). */
export const INCHIOSTRI_LINEA = (i: number) => {
  const k = INCHIOSTRI[i % INCHIOSTRI.length]!;
  return { color: k.colore, width: k.spessore, type: k.tratto as 'solid' | 'dashed' | 'dotted' | number[] };
};

// ------------------------------------------------------------------ profili e barre

/** Nei profili il bersaglio e' tutta la riga dello strike, non la barra di 3 px. */
export function puntatoreStrike(prezzi: readonly number[], valori: readonly (number | null)[], formatta: (i: number) => string | null): Puntatore {
  const passo = passoDi(prezzi);
  return (_x, y) => {
    let migliore = -1;
    let d = Infinity;
    prezzi.forEach((p, i) => {
      const e = Math.abs(p - y);
      if (e < d) {
        d = e;
        migliore = i;
      }
    });
    if (migliore < 0 || d > passo * 0.6 || valori[migliore] === undefined) return null;
    return formatta(migliore);
  };
}

const valore = (api: CustomSeriesRenderItemAPI, i: number): number => Number(api.value(i));

/**
 * Barre orizzontali su assi numerici (valore in x, prezzo in y): il profilo per prezzo o per strike.
 * Il colore dice solo il segno (di `scala`, la gamma se manca), la lunghezza il valore. Lo spessore e'
 * l'80% del passo (almeno 2 px); arrotondate solo sul capo libero.
 */
export function serieBarre(a: {
  prezzi: readonly number[];
  valori: readonly (number | null)[];
  spessore: number;
  colore?: (v: number, i: number) => string;
  scala?: ScalaSegno;
  /** l'opacita' di ogni barra (la put nella vista "Call e put": 0,45) */
  opacita?: (i: number) => number;
  /** un bordo di 1 px nello stesso colore (la barra tenue resta leggibile) */
  bordo?: boolean;
  nome?: string;
  converti?: (p: number) => number;
  z?: number;
}): CustomSeriesOption {
  const converti = a.converti ?? ((p: number) => p);
  const colore = a.colore ?? ((v: number) => coloreSegno(v, a.scala ?? segnoGamma));
  const dati: [number, number, number][] = [];
  a.prezzi.forEach((p, i) => {
    const v = a.valori[i];
    if (v != null) dati.push([v, converti(p), i]);
  });
  const mezzo = (a.spessore * 0.8) / 2;
  return {
    type: 'custom',
    name: a.nome ?? 'valore',
    data: dati,
    renderItem(_params: CustomSeriesRenderItemParams, api: CustomSeriesRenderItemAPI): CustomSeriesRenderItemReturn {
      const x = valore(api, 0);
      const y = valore(api, 1);
      const i = valore(api, 2);
      const [ax = 0, ay = 0] = api.coord([0, y + mezzo]);
      const [bx = 0, by = 0] = api.coord([x, y - mezzo]);
      const alta = Math.max(2, by - ay);
      const c = colore(x, i);
      return {
        type: 'rect',
        shape: {
          x: Math.min(ax, bx),
          y: (ay + by) / 2 - alta / 2,
          width: Math.max(1, Math.abs(bx - ax)),
          height: alta,
          r: x >= 0 ? [0, 2, 2, 0] : [2, 0, 0, 2],
        },
        style: { fill: c, opacity: a.opacita ? a.opacita(i) : 1, ...(a.bordo ? { stroke: c, lineWidth: 1 } : {}) },
        emphasis: { style: { stroke: C.testo, lineWidth: 1 } },
      };
    },
    encode: { x: 0, y: 1 },
    z: a.z ?? 2,
  };
}

/** La fascia del segno (Deriva): 8 px in fondo alla griglia, colorata minuto per minuto. */
export function fasciaSegno(tempi: readonly number[], valori: readonly (number | null)[], scala: ScalaSegno = segnoGamma): CustomSeriesOption {
  const dati: [number, number, number][] = [];
  tempi.forEach((t, i) => {
    const v = valori[i];
    const fine = tempi[i + 1] ?? t + (i > 0 ? t - (tempi[i - 1] ?? t) : 60_000);
    if (v != null && Number.isFinite(v)) dati.push([t, v, fine]);
  });
  return {
    type: 'custom',
    name: `regime (${scala.parolaPos} / ${scala.parolaNeg})`,
    data: dati,
    silent: true,
    renderItem(params: CustomSeriesRenderItemParams, api: CustomSeriesRenderItemAPI): CustomSeriesRenderItemReturn {
      const area = params.coordSys as unknown as { x: number; y: number; width: number; height: number };
      const [x0 = 0] = api.coord([valore(api, 0), 0]);
      const [x1 = 0] = api.coord([valore(api, 2), 0]);
      return {
        type: 'rect',
        shape: { x: x0, y: area.y + area.height - 8, width: Math.max(1, x1 - x0), height: 8 },
        style: { fill: coloreSegno(valore(api, 1), scala) },
      };
    },
    encode: { x: 0 },
    z: 3,
  };
}

/** Lo zoom nel tempo: rotella e trascinamento dentro, cursore sotto. */
export function zoomTempo() {
  return [
    { type: 'inside' as const, xAxisIndex: 0 },
    {
      type: 'slider' as const,
      xAxisIndex: 0,
      height: 18,
      bottom: 8,
      borderColor: C.asse,
      fillerColor: 'rgba(180,186,198,0.12)',
      handleStyle: { color: C.superficie3, borderColor: C.spento },
      moveHandleStyle: { color: C.superficie3 },
      dataBackground: { lineStyle: { color: C.decoro }, areaStyle: { color: C.superficie2 } },
      textStyle: { color: C.spento },
      labelFormatter: (v: number) => oraNY(v),
    },
  ];
}
