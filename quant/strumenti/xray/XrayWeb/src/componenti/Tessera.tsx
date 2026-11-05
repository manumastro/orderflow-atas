// La tessera (A.5): un numero da leggere al volo.
//   ETICHETTA (11, maiuscolo, spento)  [badge della fonte, solo icona] [tag della riserva]
//   ● 7.731,25   (22/600, cifre tabellari; il pallino del tono a sinistra, mai il testo colorato)
//   una riga: in parole semplici o il dettaglio (12, --testo-2)
// Il pallino e' un ::before di .valore[data-tono]: non aggiunge testo, cosi' l'inner_text della .valore
// resta il numero (lo legge prova_pagina.py). Le variazioni si scrivono in inchiostro, con ▲▼ e la
// parola: niente verde o rosso per la direzione del prezzo, si leggerebbe come un segnale.

import type { ReactNode } from 'react';
import type { FonteId } from '../api/tipi';
import { FonteBadge } from './FonteBadge';

export type Tono = 'freno' | 'acceleratore' | 'compra' | 'vende';
export type Tag = 'non provato' | 'supposto' | 'sperimentale' | 'solo dal vivo';

interface Props {
  etichetta: string;
  valore: ReactNode;
  dettaglio?: ReactNode;
  /** il segno che il valore porta: un pallino del suo colore prima del numero */
  tono?: Tono | null;
  /** la fonte, obbligatoria nella fase 2: un badge a sola icona, col nome nell'aria-label */
  fonte?: FonteId | readonly FonteId[];
  /** la riserva breve accanto al numero */
  tag?: Tag | null;
  larghezza?: 2 | 3 | 4 | 6 | 12;
  /** a riga intera sul telefono (la tessera del regime) */
  larga?: boolean;
}

const avvisate = new Set<string>();

export function Tessera({ etichetta, valore, dettaglio, tono, fonte, tag, larghezza = 3, larga = false }: Props) {
  const fonti: readonly FonteId[] = fonte == null ? [] : typeof fonte === 'string' ? [fonte] : fonte;
  if (import.meta.env.DEV && import.meta.env.MODE !== 'test' && fonti.length === 0 && !avvisate.has(etichetta)) {
    avvisate.add(etichetta);
    console.warn(`Tessera "${etichetta}" senza fonte: la fase 2 la mette ovunque (A.5)`);
  }
  return (
    <section className={`scheda tessera s${larghezza}${larga ? ' larga' : ''}`} aria-label={etichetta}>
      <div className="etichetta">
        <span className="etichetta-testo">{etichetta}</span>
        <span className="etichetta-segni">
          {tag && <span className="tag">{tag}</span>}
          {fonti.map((f) => (
            <FonteBadge key={f} id={f} soloIcona />
          ))}
        </span>
      </div>
      <div className="valore" data-tono={tono ?? undefined}>
        {valore}
      </div>
      {dettaglio && <div className="dettaglio">{dettaglio}</div>}
    </section>
  );
}
