// Il riquadro (A.4), uguale in tutte le pagine:
//   titolo 13/600 · badge corti delle fonti (con l'eta' se vecchie) · chip della riserva propria · (i) ···· comandi
//   la legenda CON I VALORI (12 px, sempre)
//   la riga breve in parole semplici (12 px, un'ellissi se non ci sta)
//   [solo in Impara] la spiegazione intera (.spiega), poi la nota tecnica (.sotto)
//   il corpo: grafico o tabella
// Il titolo e' la frase stabile: e' l'aria-label della section, e lo leggono leggi_pagina.py e la Lettura.
// In Veloce la spiegazione e la nota restano nel DOM (nascoste, `hidden`) e stanno nella "i".

import type { ReactNode } from 'react';
import type { FonteId } from '../api/tipi';
import { useStatoFonte } from '../stato/fonti';
import { useSpiegazioni } from '../stato/spiegazioni';
import { FontiBadge } from './FonteBadge';
import { Aiuto } from './Aiuto';
import { ChipRiservaPropria } from './ChipRiserva';

export type Larghezza = 2 | 3 | 4 | 5 | 6 | 7 | 8 | 12;

interface Props {
  titolo: string;
  fonti?: readonly (FonteId | string)[];
  /** la riga breve: una frase sola su cosa mostra e cosa vuol dire per il prezzo; niente numeri */
  breve?: ReactNode;
  /** in parole semplici: cosa mostra, cosa vuol dire per il prezzo, quanto e' provato (in Impara e nella "i") */
  semplice?: ReactNode;
  /** la nota tecnica (in Impara, in fondo; sempre nella "i") */
  sotto?: ReactNode;
  /** la riserva propria del riquadro, breve: "segno supposto" (una chip nell'intestazione) */
  riserva?: string;
  /** la riserva intera, nella "i" e nella chip */
  riservaIntera?: ReactNode;
  /** la legenda con i valori (Legenda), sotto l'intestazione */
  legenda?: ReactNode;
  comandi?: ReactNode;
  larghezza?: Larghezza;
  className?: string;
  children?: ReactNode;
}

/** Il riquadro e' "vecchio" se una delle sue fonti lo e': un velo sul corpo e data-vecchia="si". */
function useVecchia(fonti: readonly (FonteId | string)[]): boolean {
  const a = useStatoFonte(fonti[0] ?? '');
  const b = useStatoFonte(fonti[1] ?? '');
  return a?.stato === 'vecchio' || (fonti.length > 1 && b?.stato === 'vecchio');
}

export function Scheda({
  titolo,
  fonti = [],
  breve,
  semplice,
  sotto,
  riserva,
  riservaIntera,
  legenda,
  comandi,
  larghezza = 12,
  className,
  children,
}: Props) {
  const [modo] = useSpiegazioni();
  const impara = modo === 'impara';
  const vecchia = useVecchia(fonti);
  const conAiuto = semplice != null || sotto != null || riservaIntera != null;
  return (
    <section
      className={`scheda s${larghezza}${className ? ` ${className}` : ''}`}
      aria-label={titolo}
      data-vecchia={vecchia ? 'si' : undefined}
    >
      <div className="scheda-testa">
        <div className="scheda-titolo">
          <h2>{titolo}</h2>
          {fonti.length > 0 && <FontiBadge fonti={fonti} />}
          {riserva && <ChipRiservaPropria testo={riserva} intera={riservaIntera} />}
          {conAiuto && <Aiuto titolo={titolo} semplice={semplice} sotto={sotto} riserva={riservaIntera} />}
        </div>
        {comandi && <div className="scheda-comandi">{comandi}</div>}
      </div>
      {legenda && <div className="scheda-legenda">{legenda}</div>}
      {breve && <p className="scheda-breve">{breve}</p>}
      {semplice && (
        <div className="spiega" hidden={!impara}>
          {semplice}
        </div>
      )}
      <div className="scheda-corpo">{children}</div>
      {sotto && (
        <p className="sotto" hidden={!impara}>
          {sotto}
        </p>
      )}
    </section>
  );
}
