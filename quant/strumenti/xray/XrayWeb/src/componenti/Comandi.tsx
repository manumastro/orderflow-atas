// I comandi dei riquadri, nell'intestazione a destra, in misura xs: segmenti per al massimo 5 scelte
// (il segmento scelto e' la pillola blu notte dell'accento), una tendina nativa oltre. I gruppi di
// comandi si separano con Divisore. Le etichette restano quelle che le prove citano (B.7).

import { NativeSelect, SegmentedControl } from '@mantine/core';

export interface Opzione<V extends string> {
  valore: V;
  testo: string;
  disattiva?: boolean;
}

interface Props<V extends string> {
  opzioni: Opzione<V>[];
  valore: V;
  cambia: (v: V) => void;
  etichetta: string;
}

export function Segmenti<V extends string>({ opzioni, valore, cambia, etichetta }: Props<V>) {
  return (
    <SegmentedControl<V>
      size="xs"
      color="accento"
      aria-label={etichetta}
      value={valore}
      onChange={cambia}
      data={opzioni.map((o) => ({ value: o.valore, label: o.testo, disabled: o.disattiva ?? false }))}
      className="segmenti"
    />
  );
}

export function Tendina<V extends string>({ opzioni, valore, cambia, etichetta }: Props<V>) {
  return (
    <NativeSelect
      size="xs"
      aria-label={etichetta}
      value={valore}
      onChange={(e) => cambia(e.currentTarget.value as V)}
      data={opzioni.map((o) => ({ value: o.valore, label: o.testo }))}
      className="tendina"
    />
  );
}

/** Il filo verticale di 1 px fra due gruppi di comandi. */
export function Divisore() {
  return <span className="comandi-divisore" aria-hidden />;
}
