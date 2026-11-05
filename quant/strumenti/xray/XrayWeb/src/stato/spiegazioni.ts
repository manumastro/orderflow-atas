// Impara | Veloce: una regola sola per tutta la pagina (5 novembre 2026). Veloce (il default) mostra di
// ogni riquadro il titolo, la legenda con i valori e la riga breve; Impara mostra anche la spiegazione
// intera e la nota tecnica. La "i" le apre sempre. La scelta si ricorda nel browser (xray-spiegazioni) e
// si riflette in body[data-spiegazioni]; le chiavi per riquadro di prima (xray-spiega:*) non si leggono piu'.

import { useLocalStorage } from '@mantine/hooks';

export type Spiegazioni = 'impara' | 'veloce';

export const CHIAVE_SPIEGAZIONI = 'xray-spiegazioni';

export function useSpiegazioni(): [Spiegazioni, (s: Spiegazioni) => void] {
  const [valore, imposta] = useLocalStorage<Spiegazioni>({
    key: CHIAVE_SPIEGAZIONI,
    defaultValue: 'veloce',
    getInitialValueInEffect: false,
    // il valore e' una parola, non JSON: si legge anche se scritto a mano
    serialize: (v) => v,
    deserialize: (v) => (v === 'impara' ? 'impara' : 'veloce'),
  });
  return [valore === 'impara' ? 'impara' : 'veloce', imposta];
}
