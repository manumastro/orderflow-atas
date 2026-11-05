// Il confronto fra fonti della volatilita', solo da scrivere: il verdetto lo calcola il server
// (/api/vol/in-breve, `confronto`), solo fra le misure stile VIX con un punto negli ultimi 30 minuti
// (5 novembre 2026; la regola e' in come-si-leggono-le-opzioni-dal-vivo.md, passo 1). La misura al
// prezzo (ATM) sta 3-4 punti sotto il VIX per costruzione: non si confronta, e il server la esclude.

import type { ConfrontoVol } from '../../api/tipi';
import { num } from '../../formato';

export type Verdetto = ConfrontoVol['verdetto'];

/** I nomi delle serie escluse, che il server manda solo con l'id. */
const NOMI_ESCLUSE: Record<string, string> = {
  'atm-xray': 'ATM X-Ray',
  'iv30-ndx': 'IV30 NDX (Cboe)',
  'iv30-qqq': 'IV30 QQQ (Cboe)',
};

export const nomeEsclusa = (id: string): string => NOMI_ESCLUSE[id] ?? id;

/** "convergono (0,5 punti)", "divergono (3,3 punti)", "non si confronta". */
export function scriviVerdetto(c: ConfrontoVol): string {
  if (c.verdetto === 'non si confronta' || c.ampiezza == null) return c.verdetto;
  return `${c.verdetto} (${num(c.ampiezza, 1)} ${c.ampiezza === 1 ? 'punto' : 'punti'})`;
}

/** "2 senza dato: VXN (nessun punto oggi), VXN da NDX (Cboe fuori dalla cassa)", o null. */
export function scriviSenzaDati(c: ConfrontoVol): string | null {
  if (!c.senzaDati.length) return null;
  return `${c.senzaDati.length} senza dato: ${c.senzaDati.map((s) => `${s.nome} (${s.motivo})`).join(', ')}`;
}

/** Perche' il verdetto manca: una serie sola, o nessuna. */
export function perche(c: ConfrontoVol): string | null {
  if (c.verdetto !== 'non si confronta') return null;
  return c.serieConfrontate.length === 1 ? 'una sola fonte con un dato recente' : 'nessuna fonte con un dato recente';
}
