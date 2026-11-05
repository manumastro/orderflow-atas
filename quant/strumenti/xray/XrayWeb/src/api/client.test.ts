// Le prove del client: l'indirizzo, la risposta, e il 404 che vuol dire "non ancora disponibile".

import { describe, expect, it, vi } from 'vitest';
import { chiedi, ErroreApi, indirizzo, nonDisponibile } from './client';
import { serverFinto } from '../test/strumenti';

describe('il client', () => {
  it('compone l’indirizzo e salta i parametri vuoti', () => {
    expect(indirizzo('adesso', { giorno: '2026-11-02', ts: undefined, x: null, y: '' })).toBe('/api/adesso?giorno=2026-11-02');
    expect(indirizzo('vol/sorriso', { indice: 'NDX' })).toBe('/api/vol/sorriso?indice=NDX');
    expect(indirizzo('giorni')).toBe('/api/giorni');
  });

  it('legge una risposta JSON', async () => {
    serverFinto({ '/api/giorni': { giorni: ['2026-11-02'] } });
    await expect(chiedi<{ giorni: string[] }>('giorni')).resolves.toEqual({ giorni: ['2026-11-02'] });
  });

  it('una rotta che il server non ha ancora: ErroreApi 404, riconosciuto come non disponibile', async () => {
    serverFinto({});
    const e = await chiedi('vol/serie').catch((x: unknown) => x);
    expect(e).toBeInstanceOf(ErroreApi);
    expect((e as ErroreApi).stato).toBe(404);
    expect(nonDisponibile(e)).toBe(true);
  });

  it('un errore del server porta il suo messaggio, e non è "non disponibile"', async () => {
    vi.spyOn(globalThis, 'fetch').mockResolvedValue(new Response(JSON.stringify({ errore: 'giornata 2026-01-01 non presente' }), { status: 500 }));
    const e = await chiedi('adesso').catch((x: unknown) => x);
    expect((e as Error).message).toBe('giornata 2026-01-01 non presente');
    expect(nonDisponibile(e)).toBe(false);
  });
});
