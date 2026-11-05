// Il badge di una fonte: corto (icona nel colore della fonte, nome breve nell'inchiostro del testo); il
// nome intero sta nel title e nell'aria-label. Se la fonte e' vecchia, assente o in errore, il badge si
// allunga con l'icona e l'eta' ("◷ 14 h"): lo stato lo calcola il server (useStatoFonte). Nel passato
// le eta' si spengono.

import { IconAlertTriangle, IconClockHour4 } from '@tabler/icons-react';
import type { FonteId } from '../api/tipi';
import { etaLunga } from '../formato';
import { useStatoFonte } from '../stato/fonti';
import { fonte } from '../tema/fonti';

interface Props {
  id: FonteId | string;
  /** solo l'icona (le tessere): il nome sta nell'aria-label */
  soloIcona?: boolean;
  /** il nome intero al posto di quello breve (la pagina Fonti) */
  completo?: boolean;
}

export function FonteBadge({ id, soloIcona = false, completo = false }: Props) {
  const f = fonte(id);
  const stato = useStatoFonte(f.id);
  const { Icona } = f;
  const vecchia = stato?.vecchia ?? false;
  const quanto = stato ? (stato.stato === 'vecchio' ? etaLunga(stato.secondiFa) : stato.testo) : '';
  const titolo = `${f.nome}: ${f.cosa} (${f.ritardo})${stato ? ` · ${stato.testo}` : ''}`;
  return (
    <span
      className={`fonte-badge${soloIcona ? ' solo-icona' : ''}`}
      data-fonte={f.id}
      data-vecchia={vecchia ? 'si' : undefined}
      style={{ ['--fonte' as string]: f.colore }}
      title={titolo}
      aria-label={`fonte: ${f.nome}${vecchia ? `, ${stato?.testo ?? 'vecchia'}` : ''}`}
    >
      <Icona size={13} stroke={2} color={f.colore} aria-hidden />
      {!soloIcona && <span>{completo ? f.nome : f.breve}</span>}
      {vecchia && (
        <span className="fonte-eta" aria-hidden>
          {stato?.stato === 'vecchio' ? <IconClockHour4 size={12} stroke={2} /> : <IconAlertTriangle size={12} stroke={2} />}
          {quanto}
        </span>
      )}
    </span>
  );
}

/** Le fonti di un riquadro, in fila. */
export function FontiBadge({ fonti, soloIcona = false }: { fonti: readonly (FonteId | string)[]; soloIcona?: boolean }) {
  return (
    <span className="fonti-badge">
      {fonti.map((f) => (
        <FonteBadge key={f} id={f} soloIcona={soloIcona} />
      ))}
    </span>
  );
}
