// Il contesto dei filtri: il tipo, l'hook e una funzione pura. Separato dal provider perche' il
// ricaricamento a caldo di Vite vuole file che esportano solo componenti, o niente componenti.

import { createContext, useContext } from 'react';
import type { RispostaBase } from '../api/tipi';

export type Prezzi = 'SPX' | 'ES';

export interface Filtri {
  giorni: string[];
  /** il giorno scelto (null finche' non arrivano le giornate) */
  giorno: string | null;
  /** l'istante scelto; null = l'ultimo (dal vivo, o la fine di una giornata passata) */
  ts: number | null;
  istanti: number[];
  /** l'istante che si sta guardando: quello scelto, o l'ultimo */
  istante: number | null;
  oggi: boolean;
  /** l'ultimo istante dell'ultima giornata: si aggiorna ogni 15 secondi */
  vivo: boolean;
  /** si guarda il passato: un istante scelto, o una giornata che non e' l'ultima (la testata lo dice) */
  passato: boolean;
  /** la cassa della giornata (09:30-16:00 di New York, in ms UTC), da /api/istanti */
  cassa: { apertura: number; chiusura: number } | null;
  base: RispostaBase | null;
  /** vero se i prezzi si mostrano in ES (scelto, e la base c'e') */
  es: boolean;
  unita: Prezzi;
  /** un prezzo di SPX nell'unita' scelta */
  p: (x: number) => number;
  /** i parametri giorno e ts per le rotte di X-Ray */
  parametri: { giorno: string | null; ts: number | undefined };
  scegliGiorno: (g: string) => void;
  scegliIstante: (t: number | null) => void;
  scegliPrezzi: (p: Prezzi) => void;
  /** pronto quando ci sono le giornate e gli istanti */
  pronto: boolean;
  erroreGiorni: unknown;
}

export const ContestoFiltri = createContext<Filtri | null>(null);

export function useFiltri(): Filtri {
  const f = useContext(ContestoFiltri);
  if (!f) throw new Error('useFiltri fuori da FiltriProvider');
  return f;
}

/** L'indice dell'ultimo istante <= t (ricerca binaria); 0 se sono tutti dopo. */
export function indicePrima(lista: readonly number[], t: number): number {
  let lo = 0;
  let hi = lista.length - 1;
  let r = 0;
  while (lo <= hi) {
    const m = (lo + hi) >> 1;
    if ((lista[m] ?? Number.POSITIVE_INFINITY) <= t) {
      r = m;
      lo = m + 1;
    } else hi = m - 1;
  }
  return r;
}
