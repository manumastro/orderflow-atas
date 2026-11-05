// Dove si vede ogni flusso di dati (C.12): una mappa statica, cosi' una fonte vecchia dice anche quali
// riquadri non leggere. "Pagina › Riquadro", con i titoli stabili delle schede (C.13). Un flusso nuovo
// del server si aggiunge qui; uno che manca si scrive "–".

export interface Dove {
  /** il percorso della pagina (per il rimando), senza "#/"; null se non si vede in una pagina */
  pagina: string | null;
  testo: string;
}

export const DOVE: Record<string, Dove[]> = {
  xray: [
    { pagina: 'adesso', testo: 'SPX · X-Ray › tutte le pagine' },
    { pagina: 'volatilita', testo: 'Volatilità › Nel tempo · S&P 500 (ATM X-Ray), Struttura a termine · SPX, Sorriso · SPX' },
  ],
  'catena-es': [{ pagina: 'volatilita', testo: 'Volatilità › Il meteo · S&P 500, Nel tempo · S&P 500 (VIX ricostruito), Struttura a termine · SPX, Sorriso · SPX' }],
  'catena-nq': [
    { pagina: 'volatilita', testo: 'Volatilità › Il meteo · Nasdaq 100, Nel tempo · Nasdaq 100 (VXN ricostruito), Struttura a termine · NDX, Sorriso · NDX' },
    { pagina: 'nq', testo: 'NQ · QQQ › Movimento atteso oggi' },
  ],
  'gex-nq': [{ pagina: 'nq', testo: 'NQ · QQQ › le tessere, Profilo (NQ), I livelli, La GEX viva di NQ nel tempo' }],
  'gex-es': [{ pagina: null, testo: 'nessuna pagina: su ES e SPX vale X-Ray; la legge lo studio' }],
  'cboe-qqq': [
    { pagina: 'nq', testo: 'NQ · QQQ › Profilo (QQQ), I livelli' },
    { pagina: 'volatilita', testo: 'Volatilità › Nel tempo · Nasdaq 100, Struttura a termine · NDX, Sorriso · NDX' },
  ],
  'cboe-ndx': [
    { pagina: 'nq', testo: 'NQ · QQQ › Profilo (NDX), I livelli' },
    { pagina: 'volatilita', testo: 'Volatilità › Nel tempo · Nasdaq 100, Struttura a termine · NDX, Sorriso · NDX' },
  ],
  'yahoo-vix': [{ pagina: 'volatilita', testo: 'Volatilità › Il meteo · S&P 500, Nel tempo · S&P 500 (VIX)' }],
  'yahoo-vxn': [{ pagina: 'volatilita', testo: 'Volatilità › Il meteo · Nasdaq 100, Nel tempo · Nasdaq 100 (VXN)' }],
  'yahoo-qqq': [{ pagina: 'nq', testo: 'NQ · QQQ › Profilo (la riga di QQQ e NQ/QQQ)' }],
};

export const doveSiVede = (id: string): Dove[] => DOVE[id] ?? [];
