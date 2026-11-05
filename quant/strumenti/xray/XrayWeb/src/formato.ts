// I numeri all'italiana e gli orari: di New York (il fuso del mercato) e italiani (chi guarda).
// Il client non calcola le misure: le riceve dal server e le scrive.

const formati = new Map<number, Intl.NumberFormat>();

function formato(cifre: number): Intl.NumberFormat {
  let f = formati.get(cifre);
  if (!f) {
    f = new Intl.NumberFormat('it-IT', { minimumFractionDigits: cifre, maximumFractionDigits: cifre, useGrouping: 'always' });
    formati.set(cifre, f);
  }
  return f;
}

const valido = (v: number | null | undefined): v is number => v != null && Number.isFinite(v);

/** Un numero con le cifre date, il trattino se manca. */
export function num(v: number | null | undefined, cifre = 1): string {
  return valido(v) ? formato(cifre).format(v) : '–';
}

/** Un numero col segno davanti (+ o il meno tipografico). */
export function segno(v: number | null | undefined, cifre = 1): string {
  if (!valido(v)) return '–';
  return (v > 0 ? '+' : v < 0 ? '−' : '') + formato(cifre).format(Math.abs(v));
}

/** Cifre sensate per un valore: le greche di OptionsDepth vanno da 0,001 a qualche centinaio. */
export function cifreDi(v: number | null | undefined): number {
  const a = Math.abs(v ?? 0);
  return a >= 100 ? 0 : a >= 10 ? 1 : a >= 1 ? 2 : 3;
}

/** Col segno e con le cifre adatte al valore. */
export function auto(v: number | null | undefined): string {
  return valido(v) ? segno(v, cifreDi(v)) : '–';
}

/** Una quota (0,64) in percentuale (64%). */
export function percento(v: number | null | undefined, cifre = 0): string {
  return valido(v) ? `${formato(cifre).format(v * 100)}%` : '–';
}

const oraNYf = new Intl.DateTimeFormat('it-IT', { timeZone: 'America/New_York', hour: '2-digit', minute: '2-digit', hourCycle: 'h23' });
const oraITf = new Intl.DateTimeFormat('it-IT', { timeZone: 'Europe/Rome', hour: '2-digit', minute: '2-digit', hourCycle: 'h23' });
const giornoNYf = new Intl.DateTimeFormat('it-IT', { timeZone: 'America/New_York', weekday: 'short', day: '2-digit', month: '2-digit' });

/** L'ora di New York di un istante in ms UTC. */
export const oraNY = (ms: number | null | undefined): string => (ms ? oraNYf.format(ms) : '–');
/** L'ora italiana di un istante in ms UTC. */
export const oraIT = (ms: number | null | undefined): string => (ms ? oraITf.format(ms) : '–');
/** Il giorno (di New York) di un istante: "lun 02/11". */
export const giornoNY = (ms: number | null | undefined): string => (ms ? giornoNYf.format(ms) : '–');

/** "16:45 New York · 22:45 Italia". */
export const doppiaOra = (ms: number | null | undefined): string => (ms ? `${oraNY(ms)} New York · ${oraIT(ms)} Italia` : '–');

/** Una data AAAA-MM-GG come "lun 02/11/2026" (la data e' gia' quella della seduta). */
export function etichettaGiorno(g: string): string {
  const [a, m, d] = g.split('-').map(Number);
  if (a == null || m == null || d == null) return g;
  const data = new Date(Date.UTC(a, m - 1, d, 12));
  return new Intl.DateTimeFormat('it-IT', { timeZone: 'UTC', weekday: 'short', day: '2-digit', month: '2-digit', year: 'numeric' }).format(data);
}

/** "2026-11-27" come "27/11". */
export function giornoCorto(g: string): string {
  return g.length >= 10 ? `${g.slice(8, 10)}/${g.slice(5, 7)}` : g;
}

/** Quanto tempo fa, in parole: "42 s", "7 min", "3 h". */
export function eta(secondi: number | null | undefined): string {
  if (!valido(secondi)) return '–';
  if (secondi < 90) return `${num(secondi, 0)} s`;
  if (secondi < 90 * 60) return `${num(secondi / 60, 0)} min`;
  if (secondi < 48 * 3600) return `${num(secondi / 3600, 0)} h`;
  return `${num(secondi / 86400, 0)} giorni`;
}

/** Quanto tempo fa, anche in giorni: "42 s", "7 min", "3 h", "2 giorni e 19 h". */
export function etaLunga(secondi: number | null | undefined): string {
  if (!valido(secondi)) return '–';
  if (secondi < 48 * 3600) return eta(secondi);
  const giorni = Math.floor(secondi / 86400);
  const ore = Math.round((secondi - giorni * 86400) / 3600);
  const g = `${giorni} ${giorni === 1 ? 'giorno' : 'giorni'}`;
  return ore > 0 && ore < 24 ? `${g} e ${ore} h` : ore >= 24 ? `${giorni + 1} giorni` : g;
}

/** La distanza da un prezzo, con la freccia: "↑29", "↓101", "0". */
export function distanza(v: number | null | undefined, cifre = 0): string {
  if (!valido(v)) return '–';
  const testo = formato(cifre).format(Math.abs(v));
  if (Number(Math.abs(v).toFixed(cifre)) === 0) return formato(cifre).format(0);
  return `${v > 0 ? '↑' : '↓'}${testo}`;
}

/** Un numero a poche cifre significative, col segno: "+14", "−0,27", "+1.200". */
export function sig(v: number | null | undefined, cifre = 2): string {
  if (!valido(v)) return '–';
  if (v === 0) return '0';
  const tondo = Number(Math.abs(v).toPrecision(cifre));
  const ordine = Math.floor(Math.log10(tondo));
  const decimali = Math.max(0, cifre - 1 - ordine);
  return (v > 0 ? '+' : '−') + formato(decimali).format(tondo);
}

const oraSecondiNYf = new Intl.DateTimeFormat('it-IT', {
  timeZone: 'America/New_York',
  hour: '2-digit',
  minute: '2-digit',
  second: '2-digit',
  hourCycle: 'h23',
});

/** L'ora di New York con i secondi: "06:42:05" (l'orologio della testata). */
export const oraSecondi = (ms: number | null | undefined): string => (ms ? oraSecondiNYf.format(ms) : '–');

/** "10:30 NY · 16:30 IT": le due ore dove lo spazio e' poco. */
export const oraDoppiaCorta = (ms: number | null | undefined): string => (ms ? `${oraNY(ms)} NY · ${oraIT(ms)} IT` : '–');

/** "lun 02/11 · 11:08 New York (17:08 Italia)". */
export const giornoEOra = (ms: number | null | undefined): string =>
  ms ? `${giornoNY(ms)} · ${oraNY(ms)} New York (${oraIT(ms)} Italia)` : '–';

/** Il nome delle cose, una volta sola. */
const NOMI: Record<string, string> = {
  '0dte': '0DTE (scade oggi)',
  next_expiry: 'Prossima scadenza',
  opex: 'Opex mensile',
  all: 'Tutte le scadenze',
  totale: 'Totale',
  firm: 'Firm (proprietari)',
  broker: 'Broker-dealer',
  procust: 'Clienti professionali',
  nonprocust: 'Retail',
  mm: 'Market maker',
  gex: 'GEX',
  dex: 'DEX',
  vex: 'VEX',
  gamma: 'Gamma',
  charm: 'Charm',
};

export const nome = (k: string): string => NOMI[k] ?? k;
