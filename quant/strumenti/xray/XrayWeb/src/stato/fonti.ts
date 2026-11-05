// Lo stato di una fonte per i riquadri: l'eta' del badge e il velo dei dati vecchi. Si costruisce su
// useFonti() (la sintesi che il server calcola, E.4): nessuna domanda nuova al server. Nel passato le
// eta' "fa" si spengono: non dicono niente di un istante scelto.

import { useContext } from 'react';
import { useFonti } from '../api/domande';
import type { FonteId, StatoFonte } from '../api/tipi';
import type { SintesiFonte } from '../api/tipi';
import { ContestoFiltri } from './contesto';

/** La sintesi per fonte, se il server la da' gia' (prima di E.4 non c'e'). */
export function sintesiDi(dati: unknown): SintesiFonte[] | null {
  const s = (dati as { sintesi?: unknown } | null | undefined)?.sintesi;
  return Array.isArray(s) ? (s as SintesiFonte[]) : null;
}

export interface StatoDiFonte {
  stato: StatoFonte['stato'];
  secondiFa: number | null;
  testo: string;
  /** vecchia, assente o in errore: il riquadro prende il velo */
  vecchia: boolean;
}

export function useStatoFonte(id: FonteId | string): StatoDiFonte | null {
  const q = useFonti();
  const filtri = useContext(ContestoFiltri);
  if (filtri?.passato) return null;
  const s = sintesiDi(q.data)?.find((x) => x.fonte === id);
  if (!s) return null;
  return { stato: s.stato, secondiFa: s.secondiFa, testo: s.testo, vecchia: s.stato === 'vecchio' || s.stato === 'assente' || s.stato === 'errore' };
}
