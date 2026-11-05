// I colori hanno un ruolo, uno solo (5 novembre 2026, la pagina in stile QuantData). Validati con lo
// script della skill dataviz contro il riquadro #111723, modo scuro:
//   segno   #4fae38,#b5443f            PASS  daltonismo 12,4 · normale 30,4 · contrasto 6,36 e 3,30
//   flusso  #3987e5,#d95926            PASS  daltonismo 26,8 · normale 31,8 · contrasto 4,93 e 4,62
//   fonti   #d55181,#9256bd,#c98500,#008300 (a coppie tutte)  PASS con WARN cboe-yahoo 6,9 (tratto, icona, nome)
// Le tre coppie non stanno mai nello stesso grafico: un riquadro mostra o la gamma, o il flusso, o le fonti.
//
// - verde = freno (gamma positiva dei market maker), rosso = acceleratore (negativa);
// - blu = comprato, arancio = venduto (DEX, VEX, charm);
// - il prezzo e' l'inchiostro chiaro, il flip e lo zero gamma sono grigi tratteggiati;
// - l'accento (blu notte) e' SOLO il fondo dei comandi attivi: mai una linea, un punto o un testo di dato;
// - i colori di stato (buono, attenzione, critico) vanno sempre con un'icona e una parola.

export const C = {
  pagina: '#0b0f17',
  superficie: '#111723',
  superficie2: '#182031',
  superficie3: '#1f2a3d',
  incasso: '#080b12',
  bordo: 'rgba(255,255,255,0.06)',
  bordoForte: 'rgba(255,255,255,0.14)',
  testo: '#eef1f5',
  testo2: '#b4bac6',
  spento: '#8a92a3',
  decoro: '#5d6577',
  griglia: '#1c2433',
  asse: '#2c3850',
  asseZero: '#5d6577',
  tooltip: '#182031',

  freno: '#4fae38',
  acceleratore: '#b5443f',
  compra: '#3987e5',
  vende: '#d95926',
  neutro: '#2b3242',

  prezzo: '#eef1f5',
  flip: '#aab0bb',
  /** VAL, VAH, POC, muri del ping-pong: grigio spento a puntini */
  bordoValore: '#8a92a3',

  buono: '#0ca30c',
  attenzione: '#fab219',
  critico: '#d03b3b',

  accento: '#1f3d78',
  accentoBordo: '#2d5199',
} as const;

/** Il testo scuro sopra un colore pieno chiaro (inchiostroSu). */
export const SCURO = '#0b0f17';

/** Identita' senza segno (Deriva): tre inchiostri, ognuno con il suo tratto e il nome scritto in fondo. */
export const INCHIOSTRI = [
  { colore: '#eef1f5', tratto: 'solid', spessore: 2 },
  { colore: '#b4bac6', tratto: 'dashed', spessore: 1.5 },
  { colore: '#8a92a3', tratto: 'dotted', spessore: 1.5 },
  { colore: '#b4bac6', tratto: [8, 3, 2, 3], spessore: 1.5 }, // tratto-punto, il quarto gruppo
] as const;

export const FONT = 'Inter, system-ui, -apple-system, "Segoe UI", sans-serif';
export const FONT_MONO = '"Roboto Mono", ui-monospace, monospace';
