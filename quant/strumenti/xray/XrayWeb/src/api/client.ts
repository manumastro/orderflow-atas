// Le chiamate al server: GET /api/<percorso>?<parametri>, risposta JSON. Un errore porta lo
// stato HTTP: il 404 di una rotta nuova vuol dire "fonte non ancora disponibile", non un guasto.

export type Parametri = Record<string, string | number | null | undefined>;

export class ErroreApi extends Error {
  readonly stato: number;
  readonly percorso: string;

  constructor(stato: number, messaggio: string, percorso: string) {
    super(messaggio);
    this.name = 'ErroreApi';
    this.stato = stato;
    this.percorso = percorso;
  }
}

/** Vero se l'errore e' un 404: la rotta (o il dato) non c'e' ancora. */
export function nonDisponibile(e: unknown): boolean {
  return e instanceof ErroreApi && e.stato === 404;
}

export function indirizzo(percorso: string, parametri: Parametri = {}): string {
  const q = new URLSearchParams();
  for (const [k, v] of Object.entries(parametri)) if (v != null && v !== '') q.set(k, String(v));
  const s = q.toString();
  return `/api/${percorso}${s ? `?${s}` : ''}`;
}

function messaggioDi(corpo: unknown): string | null {
  if (corpo != null && typeof corpo === 'object' && 'errore' in corpo) {
    const e = (corpo as { errore: unknown }).errore;
    if (typeof e === 'string') return e;
  }
  return null;
}

export async function chiedi<T>(percorso: string, parametri: Parametri = {}, segnale?: AbortSignal): Promise<T> {
  const url = indirizzo(percorso, parametri);
  const r = await fetch(url, { cache: 'no-store', signal: segnale ?? null });
  const testo = await r.text();
  let corpo: unknown = null;
  try {
    corpo = testo ? JSON.parse(testo) : null;
  } catch {
    if (r.ok) throw new ErroreApi(r.status, `${url}: risposta non JSON`, percorso);
  }
  if (!r.ok) throw new ErroreApi(r.status, messaggioDi(corpo) ?? `${url}: ${r.status}`, percorso);
  return corpo as T;
}
