// Gli attrezzi delle prove: un server finto (fetch) che risponde per percorso, e la pagina montata
// con tutti i fornitori (Mantine, cache delle domande, router, filtri).

import type { ReactElement } from 'react';
import { render } from '@testing-library/react';
import { MantineProvider } from '@mantine/core';
import { QueryClient, QueryClientProvider } from '@tanstack/react-query';
import { MemoryRouter } from 'react-router';
import { vi } from 'vitest';
import { FiltriProvider } from '../stato/filtri';
import { tema } from '../tema/tema';

/** Le risposte di base di X-Ray: una giornata, tre istanti, la base. */
export const RISPOSTE_BASE: Record<string, unknown> = {
  '/api/giorni': { giorni: ['2026-11-02', '2026-10-30'] },
  '/api/istanti': {
    giorno: '2026-11-02',
    istanti: [1793633400000, 1793633460000, 1793633520000],
    cassa: { apertura: 1793629800000, chiusura: 1793653200000 },
  },
  '/api/base': { valore: 30.45, campioni: 389, giorno: '2026-10-30', fonte: 'mediana' },
  '/api/stato': {
    lettore: { presente: true, vivo: true, secondiFa: 3, ultimoChunkSecondiFa: 5, chunk: 100, errori: 0, ultimoErrore: null, archivioMB: 800, giornate: 45 },
    famiglie: [{ nome: 'gex__mm_0dte', minuti: 0.5, indicatore: 'GEX Heatmap' }],
    server: { indirizzo: 'http://127.0.0.1:8790/', avviato: '2026-11-02T09:42:33Z', dati: 'x', pagina: 'y', bridge: 'http://127.0.0.1:8787', bridgeErrore: null },
  },
};

/**
 * Un fetch finto: per ogni percorso (senza la query) la risposta, o un numero = stato HTTP senza
 * corpo. Un percorso che non c'e' risponde 404, come le rotte che il server non ha ancora.
 */
export function serverFinto(risposte: Record<string, unknown | number>) {
  const chiamate: string[] = [];
  const tutte = { ...RISPOSTE_BASE, ...risposte };
  vi.spyOn(globalThis, 'fetch').mockImplementation(async (input) => {
    const url = typeof input === 'string' ? input : input instanceof URL ? input.href : input.url;
    chiamate.push(url);
    const percorso = url.split('?')[0] ?? url;
    const r = tutte[percorso];
    if (r === undefined) return new Response('', { status: 404 });
    if (typeof r === 'number') return new Response('', { status: r });
    return new Response(JSON.stringify(r), { status: 200, headers: { 'Content-Type': 'application/json' } });
  });
  return chiamate;
}

export function rendi(ui: ReactElement, indirizzo = '/') {
  const cache = new QueryClient({ defaultOptions: { queries: { retry: false } } });
  return render(
    <MantineProvider theme={tema} forceColorScheme="dark" env="test">
      <QueryClientProvider client={cache}>
        <MemoryRouter initialEntries={[indirizzo]}>
          <FiltriProvider>{ui}</FiltriProvider>
        </MemoryRouter>
      </QueryClientProvider>
    </MantineProvider>,
  );
}
