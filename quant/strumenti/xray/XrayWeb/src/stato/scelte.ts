// Le scelte di una pagina (misura, scadenza, vista...) restano mentre si gira fra le pagine, come
// nella pagina di prima: si tengono in memoria per la sessione, non nell'indirizzo.

import { useCallback, useState } from 'react';

const memoria = new Map<string, unknown>();

type Cambio<T> = T | ((prima: T) => T);

export function useScelte<T extends object>(chiave: string, iniziali: T): [T, (c: Cambio<T>) => void] {
  const [valore, setValore] = useState<T>(() => (memoria.get(chiave) as T | undefined) ?? iniziali);
  const imposta = useCallback(
    (c: Cambio<T>) =>
      setValore((prima) => {
        const dopo = typeof c === 'function' ? c(prima) : c;
        memoria.set(chiave, dopo);
        return dopo;
      }),
    [chiave],
  );
  return [valore, imposta];
}
