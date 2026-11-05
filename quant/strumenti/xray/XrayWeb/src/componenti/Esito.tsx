// Cosa si vede mentre una domanda e' in corso, quando fallisce, o quando la rotta non c'e' ancora.
// Un 404 di una rotta nuova non e' un errore rosso: e' "fonte non ancora disponibile".

import type { ReactNode } from 'react';
import { Skeleton } from '@mantine/core';
import { IconAlertTriangle, IconClockHour4 } from '@tabler/icons-react';
import type { UseQueryResult } from '@tanstack/react-query';
import { nonDisponibile } from '../api/client';

export function NonDisponibile({ rotta, children }: { rotta: string; children?: ReactNode }) {
  return (
    <div className="non-disponibile" role="status" data-rotta={rotta}>
      <IconClockHour4 size={20} stroke={1.8} aria-hidden />
      <div>
        <strong>Fonte non ancora disponibile</strong>
        <p>
          Il server non ha ancora la rotta <code>/api/{rotta}</code>. La pagina riprova da sola ogni minuto: quando il server
          la aggiunge, i dati compaiono qui.
        </p>
        {children}
      </div>
    </div>
  );
}

export function Errore({ messaggio }: { messaggio: string }) {
  return (
    <div className="avviso" role="alert">
      <IconAlertTriangle size={18} aria-hidden />
      <span>{messaggio}</span>
    </div>
  );
}

export function Vuoto({ children }: { children: ReactNode }) {
  return <p className="vuoto">{children}</p>;
}

interface Props<T> {
  q: UseQueryResult<T>;
  /** la rotta, per il riquadro "non ancora disponibile" */
  rotta: string;
  altezza?: number;
  children: (dati: T) => ReactNode;
}

/** Mostra i dati di una domanda, o il suo stato: in arrivo, non ancora disponibile, errore. */
export function Esito<T>({ q, rotta, altezza = 200, children }: Props<T>) {
  if (q.data !== undefined) return <>{children(q.data)}</>;
  if (q.error) {
    if (nonDisponibile(q.error)) return <NonDisponibile rotta={rotta} />;
    return <Errore messaggio={q.error instanceof Error ? q.error.message : String(q.error)} />;
  }
  return <Skeleton height={altezza} radius={6} className="attesa" />;
}
