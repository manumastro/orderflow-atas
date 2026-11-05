// Le domande al server, con la cache di TanStack Query. "Dal vivo" (l'ultimo istante dell'ultima
// giornata) si richiede ogni 15 secondi; un istante passato non cambia, e non si richiede.

import { keepPreviousData, useQuery, type UseQueryResult } from '@tanstack/react-query';
import { chiedi, nonDisponibile, type Parametri } from './client';
import type {
  RispostaBase,
  RispostaFonti,
  RispostaGiorni,
  RispostaIstanti,
  RispostaLettura,
  RispostaNqGex,
  RispostaGexTempo,
  RispostaQuattroDomande,
  RispostaStato,
  RispostaVolSerie,
  RispostaVolSorriso,
  RispostaVolTermine, RispostaVolInBreve } from './tipi';

export const RITMO_MS = 15_000;
/** una rotta che non c'e' ancora si riprova ogni minuto: quando il server la aggiunge, appare */
const RITMO_ASSENTE_MS = 60_000;

/** Una misura di X-Ray: i dati restano a schermo mentre arrivano i nuovi (niente sfarfallio). */
export function useMisura<T>(percorso: string, parametri: Parametri, vivo: boolean, attiva = true): UseQueryResult<T> {
  return useQuery({
    queryKey: [percorso, parametri],
    queryFn: ({ signal }) => chiedi<T>(percorso, parametri, signal),
    enabled: attiva,
    placeholderData: keepPreviousData,
    refetchInterval: vivo ? RITMO_MS : false,
    staleTime: vivo ? 0 : Infinity,
  });
}

/**
 * Una rotta nuova (volatilita', NQ, confronti, fonti): finche' il server risponde 404 si mostra
 * "non ancora disponibile" e si riprova ogni minuto; poi si aggiorna ogni 15 secondi.
 */
export function useNuova<T>(percorso: string, parametri: Parametri = {}, attiva = true): UseQueryResult<T> {
  return useQuery({
    queryKey: [percorso, parametri],
    queryFn: ({ signal }) => chiedi<T>(percorso, parametri, signal),
    enabled: attiva,
    placeholderData: keepPreviousData,
    retry: (volte, errore) => !nonDisponibile(errore) && volte < 2,
    refetchInterval: (q) => (nonDisponibile(q.state.error) ? RITMO_ASSENTE_MS : RITMO_MS),
  });
}

export const useStato = () =>
  useQuery({ queryKey: ['stato'], queryFn: ({ signal }) => chiedi<RispostaStato>('stato', {}, signal), refetchInterval: RITMO_MS });

export const useGiorni = (vivo: boolean) =>
  useQuery({
    queryKey: ['giorni'],
    queryFn: ({ signal }) => chiedi<RispostaGiorni>('giorni', {}, signal),
    refetchInterval: vivo ? RITMO_MS : false,
  });

export const useIstanti = (giorno: string | null, vivo: boolean) =>
  useMisura<RispostaIstanti>('istanti', { giorno }, vivo, giorno != null);

export const useBase = (giorno: string | null, vivo: boolean) =>
  useMisura<RispostaBase>('base', { giorno }, vivo, giorno != null);

export const useFonti = () => useNuova<RispostaFonti>('fonti');
/** le letture dal vivo dell'agente: si richiedono ogni 15 secondi, cosi' una lettura nuova compare da sola */
export const useLettura = (giorno?: string | null) => useNuova<RispostaLettura>('lettura', { giorno });
export const useVolSerie = (giorno: string | null) => useNuova<RispostaVolSerie>('vol/serie', { giorno }, giorno != null);
export const useVolTermine = () => useNuova<RispostaVolTermine>('vol/termine');
/** la volatilita' letta per il prezzo: livello, movimento atteso, struttura, skew (5 novembre 2026) */
export const useVolInBreve = () => useNuova<RispostaVolInBreve>('vol/in-breve');
export const useVolSorriso = (indice: 'SPX' | 'NDX') => useNuova<RispostaVolSorriso>('vol/sorriso', { indice });
export const useNqGex = (ambito: 'tutte' | '0dte') =>
  useNuova<RispostaNqGex>('nq/gex', { ambito: ambito === '0dte' ? '0dte' : undefined });

export const useGexTempo = (radice: 'NQ' | 'ES', giorno: string | null) =>
  useNuova<RispostaGexTempo>('gex/tempo', { radice, giorno }, giorno != null);

/**
 * Le quattro domande (regime, meteo, muri sul profilo, grilletto), 5 novembre 2026. ES segue giorno e istante come
 * le altre rotte di X-Ray (useMisura); NQ ha solo l'ultimo dato, e la rotta si tratta come nuova (useNuova).
 */
export function useQuattroDomande(
  radice: 'ES' | 'NQ',
  parametri: Parametri,
  vivo: boolean,
  attiva = true,
): UseQueryResult<RispostaQuattroDomande> {
  const es = radice === 'ES';
  const p: Parametri = es ? { radice, ...parametri } : { radice };
  const fermo = es && !vivo;
  return useQuery({
    queryKey: ['quattro-domande', p],
    queryFn: ({ signal }) => chiedi<RispostaQuattroDomande>('quattro-domande', p, signal),
    enabled: attiva,
    placeholderData: keepPreviousData,
    retry: (volte, errore) => !nonDisponibile(errore) && volte < 2,
    refetchInterval: (q) => (nonDisponibile(q.state.error) ? RITMO_ASSENTE_MS : fermo ? false : RITMO_MS),
    staleTime: fermo ? Infinity : 0,
  });
}
