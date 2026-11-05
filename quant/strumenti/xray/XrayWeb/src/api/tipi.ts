// I tipi delle risposte del server XrayLive (porta 8790). Ricavati dalle risposte vere del
// 2 novembre 2026 e da XrayLive/Server/Misure.cs (le rotte X-Ray), e dal contratto delle rotte
// nuove (volatilita', NQ, confronti, fonti). JSON camelCase, orari in ms UTC, prezzi in SPX
// salvo dove e' scritto.

/** Un istante in millisecondi UTC. */
export type Ms = number;
/** Un valore che il server puo' non avere (NaN e infiniti arrivano come null). */
export type Forse<T> = T | null;

export type Scadenza = '0dte' | 'next_expiry' | 'opex';
export type Gruppo = 'firm' | 'broker' | 'procust' | 'nonprocust';
export type Metrica = 'gex' | 'dex' | 'vex';
export type Greca = 'gamma' | 'charm';
export type Regime = 'freno' | 'acceleratore' | 'neutro';

// ------------------------------------------------------------------ stato, giorni, base

export interface StatoLettore {
  presente: boolean;
  vivo: boolean;
  secondiFa: Forse<number>;
  ultimoChunkSecondiFa: Forse<number>;
  chunk: Forse<number>;
  errori: Forse<number>;
  ultimoErrore: Forse<string>;
  archivioMB: Forse<number>;
  giornate: Forse<number>;
}

export interface StatoFamiglia {
  nome: string;
  minuti: Forse<number>;
  indicatore: string;
}

export interface StatoServer {
  indirizzo: string;
  avviato: string;
  dati: string;
  pagina: string;
  bridge: string;
  bridgeErrore: Forse<string>;
}

export interface RispostaStato {
  lettore: StatoLettore;
  famiglie: StatoFamiglia[];
  server: StatoServer;
}

export interface RispostaGiorni {
  giorni: string[];
}

export interface RispostaIstanti {
  giorno: string;
  istanti: Ms[];
  cassa: { apertura: Ms; chiusura: Ms };
}

export interface RispostaBase {
  valore: Forse<number>;
  campioni: number;
  giorno: Forse<string>;
  fonte: string;
}

// ------------------------------------------------------------------ X-Ray

/** Il prezzo di SPX a cui si leggono le misure: lo spot in cassa, fuori ES meno la base. */
export interface Riferimento {
  prezzo: number;
  fonte: string;
  vivo: boolean;
  spotX: Forse<number>;
  es: Forse<number>;
}

export interface Picco {
  strike: number;
  peso: Forse<number>;
  sotto: Forse<number>;
  sopra: Forse<number>;
}

export interface Outlook {
  istante: Ms;
  da: Ms;
  fino: Ms;
  zeri: number[];
  positivi: Picco[];
  negativi: Picco[];
  /** la seduta che la previsione copre, AAAA-MM-GG di New York (5 novembre 2026) */
  seduta: Forse<string>;
}

export interface Muro {
  strike: number;
  valore: Forse<number>;
}

export type Esposizione =
  | { scadenza: Scadenza; presente: false }
  | {
      scadenza: Scadenza;
      presente: true;
      istante: Ms;
      dte: number;
      scade: Ms;
      netto: Forse<number>;
      call: Forse<number>;
      put: Forse<number>;
      entroUnoPercento: Forse<number>;
      muroFreno: Forse<Muro>;
      muroAcceleratore: Forse<Muro>;
    };

export interface DexGruppo {
  gruppo: Gruppo;
  zeroDte: Forse<number>;
  zeroDteTrentaMinuti: Forse<number>;
  tutte: Forse<number>;
  tutteTrentaMinuti: Forse<number>;
}

export interface RispostaAdesso {
  giorno: string;
  ts: Ms;
  riferimento: Riferimento;
  base: RispostaBase;
  /** da: il primo istante da cui il segno al prezzo e' lo stesso di adesso (5 novembre 2026) */
  gamma: { valore: Forse<number>; regime: Forse<Regime>; istante: Forse<Ms>; da: Forse<Ms> };
  charm: { valore: Forse<number>; istante: Forse<Ms> };
  flip: number[];
  /** il flip piu' vicino al prezzo e il segno della gamma un passo di griglia sopra e sotto (5 novembre 2026) */
  flipVicino: Forse<{ prezzo: number; distanza: number; sopra: Forse<Regime>; sotto: Forse<Regime> }>;
  picchiPositivi: Picco[];
  picchiNegativi: Picco[];
  outlook: Forse<Outlook>;
  /** la scala dei livelli in SPX, con i bordi del valore convertiti (− base), dall'alto in basso (5 novembre 2026) */
  scala: RigaScala[];
  gex: Esposizione[];
  vex: Esposizione[];
  dex: DexGruppo[];
  /** verso e unita restano null finche' il segno di pressure__0dte non e' scritto in x-ray-in-vivo.md */
  pressione: Forse<{
    delta: Forse<number>;
    trades: Forse<number>;
    trentaMinutiPrima: Forse<number>;
    istante: Ms;
    verso: Forse<'comprano' | 'vendono'>;
    unita: Forse<string>;
  }>;
}

export type RispostaProfilo =
  | { giorno: string; ts: Ms; greca: Greca; presente: false }
  | {
      giorno: string;
      ts: Ms;
      greca: Greca;
      presente: true;
      istante: Ms;
      riferimento: Riferimento;
      prezzi: number[];
      valori: Forse<number>[];
      alPrezzo: Forse<number>;
      zeri: number[];
      picchiPositivi: Picco[];
      picchiNegativi: Picco[];
    };

export type RispostaStrike =
  | { giorno: string; ts: Ms; famiglia: string; presente: false }
  | {
      giorno: string;
      ts: Ms;
      famiglia: string;
      presente: true;
      istante: Ms;
      dte: number;
      scade: Ms;
      riferimento: Riferimento;
      strike: number[];
      call: Forse<number>[];
      put: Forse<number>[];
      netto: Forse<number>[];
      totale: { call: Forse<number>; put: Forse<number>; netto: Forse<number> };
      /** il netto piu' positivo e piu' negativo entro il 5% dal riferimento, come l'esposizione di adesso (5 novembre 2026) */
      muri: { positivo: Forse<Muro>; negativo: Forse<Muro> };
      entroUnoPercento: Forse<number>;
      /** solo per la GEX, dai livelli dell'istante; [] altrimenti */
      flip: number[];
    };

export interface ColonnaHeat {
  chiave: Scadenza | Gruppo | 'totale';
  istante: Forse<Ms>;
  dte: Forse<number>;
  scade: Forse<Ms>;
}

export interface RispostaHeatmap {
  giorno: string;
  ts: Ms;
  metrica: Metrica;
  ambito: '0dte' | 'all';
  riferimento: Riferimento;
  colonne: ColonnaHeat[];
  /** dall'alto in basso: il primo e' lo strike piu' alto */
  strike: number[];
  /** una riga per strike, una colonna per colonna (l'ultima e' il totale) */
  valori: Forse<number>[][];
  /** una per colonna (l'ultima = la somma), su TUTTI gli strike della famiglia all'istante, non solo la finestra:
   *  per gex/vex coincide con adesso.gex[i].netto; per dex e' il cumulato del gruppo (5 novembre 2026) */
  nette: Forse<number>[];
  livelli: { flip: number[]; frenoPiuForte: Forse<number>; accPiuForte: Forse<number> };
}

/** Una linea nel tempo: coppie [istante, prezzo]. */
export type Traccia = [Ms, number][];

export interface RispostaIntervalli {
  giorno: string;
  greca: Greca;
  passo: number;
  tempi: Ms[];
  prezzi: number[];
  valori: Forse<number>[][];
  riferimento: Forse<number>[];
  vivo: boolean[];
  zeri: Traccia[];
}

export interface RispostaIntervalliStrike {
  giorno: string;
  metrica: 'gex' | 'vex';
  scadenza: Scadenza;
  passo: number;
  tempi: Ms[];
  strike: number[];
  valori: Forse<number>[][];
  riferimento: Forse<number>[];
  vivo: boolean[];
}

export interface RispostaDeriva {
  giorno: string;
  passo: number;
  tempi: Ms[];
  riferimento: Forse<number>[];
  vivo: boolean[];
  gex: Record<Scadenza, Forse<number>[]>;
  dexZeroDte: Record<Gruppo, Forse<number>[]>;
  dexTutte: Record<Gruppo, Forse<number>[]>;
  pressione: Forse<number>[];
  gammaAlPrezzo: Forse<number>[];
  /** l'ultimo istante in numeri, per le tessere (5 novembre 2026) */
  adesso: Forse<{
    ts: Ms;
    gammaAlPrezzo: Forse<number>;
    regime: Forse<Regime>;
    regimeDa: Forse<Ms>;
    gex: Record<Scadenza | 'totale', Forse<number>>;
    retail0dte: Forse<number>;
    /** il valore di un'ora prima (non la differenza) */
    retail0dteUnOraFa: Forse<number>;
    pressione: Forse<number>;
    /** il valore di 30 minuti prima (non la differenza) */
    pressioneTrentaMinutiFa: Forse<number>;
  }>;
  /** i minuti anomali delle serie cumulate del DEX: la pagina interrompe la linea, i valori restano */
  anomali: MinutoAnomalo[];
  unita: { gex: string; dex: string; pressione: string };
}

/** Un minuto anomalo di una serie cumulata del DEX: serie = "dexTutte.firm", indice = la posizione in tempi. */
export interface MinutoAnomalo {
  serie: string;
  indice: number;
  ts: Ms;
  /** il salto (Δ) del punto; per "partenza" il primo valore */
  salto: Forse<number>;
  motivo: 'finestra' | 'scadenza' | 'salto' | 'partenza';
}

export interface RispostaLivelli {
  giorno: string;
  passo: number;
  tempi: Ms[];
  riferimento: Forse<number>[];
  vivo: boolean[];
  zeri: Traccia[];
  /** i picchi nel tempo: [istante, prezzo, peso] */
  picchi: { tipo: 'positivo' | 'negativo'; punti: [Ms, number, Forse<number>][] }[];
  adesso: Forse<{ zeri: number[]; positivi: Picco[]; negativi: Picco[] }>;
  outlook: Forse<Outlook>;
}

export interface RispostaPrevisione {
  giorno: string;
  tipo: 'forecast' | 'nextday';
  greca: Greca;
  asOfs: Ms[];
  asOf: Ms;
  tempi: Ms[];
  prezzi: number[];
  valori: Forse<number>[][];
  riferimento: Forse<Riferimento>;
  /** per ogni tempo, i prezzi dove i valori cambiano segno (interpolati), uniti in tracce (5 novembre 2026) */
  zeri: Traccia[];
  /** per tipo=nextday: la seduta prevista (AAAA-MM-GG); null per forecast */
  seduta: Forse<string>;
  /** per tipo=forecast: le 16:00 di New York − 15 minuti; null per nextday */
  taglioScadenza: Forse<Ms>;
  /** il valore previsto al prezzo del calcolo, per ogni tempo */
  alPrezzo: Forse<number>[];
  /** l'ultimo tempo, e il primo cambio di segno di alPrezzo */
  sintesi: Forse<{ flipFinale: Forse<number>; regimeFinale: Forse<Regime>; cambiaAlle: Forse<Ms> }>;
}

// ------------------------------------------------------------------ le fonti (rotte nuove)

/** Le quattro fonti: un id fisso, usato ovunque (colore, icona, nome in tema/fonti.ts). */
export type FonteId = 'xray' | 'catena' | 'cboe' | 'yahoo';
export type Indice = 'SPX' | 'NDX';

export interface StatoFonte {
  /** l'id del flusso: xray, catena-es, catena-nq, gex-nq, gex-es, cboe-qqq, cboe-ndx, yahoo-vix, yahoo-vxn */
  id: string;
  fonte: FonteId;
  nome: string;
  ultimo: Forse<Ms>;
  secondiFa: Forse<number>;
  stato: 'ok' | 'vecchio' | 'errore' | 'assente' | 'in corso';
  errore: Forse<string>;
  nota: Forse<string>;
}

/** Una riga per fonte: lo stato peggiore dei suoi flussi (errore > assente > vecchio > in corso > ok), 5 novembre 2026. */
export interface SintesiFonte {
  fonte: FonteId;
  stato: StatoFonte['stato'];
  /** l'id del flusso peggiore */
  peggiore: string;
  /** del flusso peggiore */
  secondiFa: Forse<number>;
  flussi: string[];
  /** "aggiornata 40 s", "vecchia 14 h", "VXN ufficiale assente" */
  testo: string;
}

export interface RispostaFonti {
  adesso?: Ms;
  /** dal 5 novembre 2026 anche yahoo-vix, yahoo-vxn e yahoo-qqq; la riga "yahoo" resta finche' la pagina Fonti non la toglie */
  fonti: StatoFonte[];
  /** assente nelle risposte di prima del 5 novembre 2026 */
  sintesi?: SintesiFonte[];
}

export interface SerieVol {
  id: string;
  nome: string;
  indice: Indice;
  fonte: FonteId;
  descrizione?: Forse<string>;
  /** [istante, volatilita' in punti percentuali] */
  punti: [Ms, Forse<number>][];
  /** stile VIX (formula del Cboe a 30 giorni, o l'indice) o al prezzo (ATM): solo le prime si confrontano (5 novembre 2026) */
  misura?: 'stile-vix' | 'atm';
}

export interface RispostaVolSerie {
  giorno: string;
  serie: SerieVol[];
}

export interface CurvaTermine {
  id: string;
  nome?: Forse<string>;
  indice: Indice;
  fonte: FonteId;
  quando?: Forse<Ms>;
  /** sospetto: vol > 2 × la mediana delle vol della stessa curva, un dato sporco (5 novembre 2026) */
  punti: { scadenza: string; giorni: number; vol: Forse<number>; sospetto?: boolean }[];
}

export interface RispostaVolTermine {
  quando: Forse<Ms>;
  curve: CurvaTermine[];
  /** gli eventi del calendario (docs/research/calendario/eventi.json) da oggi a 70 giorni */
  eventi?: { giorno: string; nome: string }[];
}

export interface CurvaSorriso {
  id: string;
  nome?: Forse<string>;
  fonte: FonteId;
  scadenza?: Forse<string>;
  giorni?: Forse<number>;
  forward?: Forse<number>;
  /** moneyness = strike / forward − 1, in % */
  punti: { moneyness: number; strike: number; vol: Forse<number> }[];
}

export interface RispostaVolSorriso {
  indice: Indice;
  quando: Forse<Ms>;
  curve: CurvaSorriso[];
}

export interface Conversione {
  da: string;
  tipo: 'rapporto' | 'base';
  valore: number;
  nota?: Forse<string>;
}

export interface GexFonte {
  id: string;
  nome: string;
  fonte: FonteId;
  quando?: Forse<Ms>;
  secondiFa?: Forse<number>;
  conversione: Forse<Conversione>;
  totale?: Forse<number>;
  zeroGamma?: Forse<number>;
  callWall?: Forse<number>;
  putWall?: Forse<number>;
  scadenze?: Forse<string>;
  /** un avviso sul dato di questa fonte (per esempio lo 0DTE di un altro giorno) */
  nota?: Forse<string>;
  /** in prezzi di NQ, GEX in milioni di $ per 1% (convenzione A) */
  strike: { prezzo: number; gex: Forse<number> }[];
}

/** Il prezzo vero di QQQ (Yahoo), anche nel pre-mercato e nel dopo-mercato, accanto a quello del file del Cboe. */
export interface PrezzoQqq {
  prezzo: number;
  quando: Ms;
  secondiFa: number;
  sessione: 'pre' | 'cassa' | 'dopo' | 'chiuso';
  chiusuraPrima: Forse<number>;
  /** in percento sulla chiusura di ieri */
  variazione: Forse<number>;
  /** NQ / QQQ adesso */
  rapportoNQ: Forse<number>;
  prezzoCboe: Forse<number>;
  quandoCboe: Forse<Ms>;
  fonte: 'yahoo';
}

export interface RispostaNqGex {
  quando: Forse<Ms>;
  prezzoNQ: Forse<number>;
  fonti: GexFonte[];
  /** assente nelle risposte di prima del 5 novembre 2026 */
  qqq?: Forse<PrezzoQqq>;
  /** il punto di gex-NQ.jsonl di un'ora prima dell'ultimo, come xray_query.gex_viva (5 novembre 2026) */
  unOraFa?: Forse<PuntoGexTempo>;
  /** da /api/vol/in-breve, NDX */
  movimentoOggi?: Forse<number>;
  /** prezzo ± 2 × movimentoOggi; senza movimento, prezzo ± 2% */
  finestra?: Forse<[number, number]>;
  /** la scala dei livelli di NQ (soglia 25 punti), in prezzi di NQ */
  scala?: RigaScala[];
}

export interface CurvaConfronto {
  id: string;
  nome: string;
  fonte: FonteId;
  unita: string;
  /** in prezzi di ES; normalizzata = valore / massimo assoluto */
  punti: { prezzo: number; valore: Forse<number>; normalizzata: Forse<number> }[];
}

export interface RispostaGexConfronto {
  quando: Forse<Ms>;
  prezzoES: Forse<number>;
  curve: CurvaConfronto[];
  accordo: Forse<{ segnoUguale: Forse<number>; strike: number; nota?: Forse<string> }>;
}

/** Un minuto della GEX viva (/api/gex/tempo): prezzo del future, tutte le scadenze e lo 0DTE. */
export interface PuntoGexTempo {
  ts: Ms;
  future: Forse<number>;
  totale: Forse<number>;
  zero: Forse<number>;
  callWall: Forse<number>;
  putWall: Forse<number>;
  totale0: Forse<number>;
  zero0: Forse<number>;
  callWall0: Forse<number>;
  putWall0: Forse<number>;
}

export interface RispostaGexTempo {
  radice: 'NQ' | 'ES';
  giorno: string;
  fonte: FonteId;
  descrizione?: Forse<string>;
  punti: PuntoGexTempo[];
}

// ---------------------------------------------------------------- la lettura dal vivo (/api/lettura)
// Scritta dall'agente su richiesta (scripts/scrivi_lettura.py), nella forma lineare di
// quant/metodo/come-si-leggono-le-opzioni-dal-vivo.md. Il server la restituisce com'e'.

export interface DoveLettura {
  /** il percorso della pagina ("nq", "volatilita"...), o "chart" per il chart di ATAS */
  pagina: string;
  scheda: string;
}

export interface RigaLettura {
  voce: string;
  testo: string;
  dove?: Forse<DoveLettura>;
  /** una parola breve per la tessera ("NQ: acceleratore"), al massimo 24 caratteri */
  etichetta?: Forse<string>;
}

export interface PrezzoLettura {
  prezzo: string;
  cosa: string;
  seCiArriva: string;
  /** il prezzo in numeri: una zona da < a, un prezzo da = a */
  da?: Forse<number>;
  a?: Forse<number>;
}

export interface Lettura {
  versione: number;
  quando: Ms;
  oraNY: string;
  oraItalia: string;
  strumento?: Forse<string>;
  prezzo?: Forse<number>;
  studi?: Forse<string>;
  inBreve: string;
  righe: RigaLettura[];
  prezzi: PrezzoLettura[];
  daGuardare: string[];
  dubbi: string;
  parole: { parola: string; significa: string }[];
}

export interface FileLettura {
  file: string;
  lettura?: Lettura;
  errore?: string;
}

export interface RispostaLettura {
  giorno: Forse<string>;
  giorni: string[];
  letture: FileLettura[];
  /** il prezzo di adesso dello strumento dell'ultima lettura, dal bridge (5 novembre 2026) */
  prezzoAdesso?: Forse<{ strumento: string; prezzo: number; quando: Ms }>;
}

/** La volatilita' letta per il prezzo, per indice (/api/vol/in-breve, 5 novembre 2026). */
export interface VolInBreve {
  indice: Indice;
  radice: 'ES' | 'NQ';
  nomeIndice: string;
  livello: Forse<number>;
  quandoLivello: Forse<Ms>;
  fonteLivello: FonteId;
  fascia: Forse<'calma' | 'normale' | 'tesa' | 'paura'>;
  /** le tre soglie fra calma, normale, tesa e paura: una convenzione, non provata */
  soglie: number[];
  apertura: Forse<number>;
  prezzo: Forse<number>;
  /** in punti del future, fino alla scadenza di oggi (una deviazione standard) */
  movimentoOggi: Forse<number>;
  scadenzaOggi: Forse<string>;
  volOggi: Forse<number>;
  movimentoGiorno: Forse<number>;
  volBreve: Forse<number>;
  volMese: Forse<number>;
  pendenza: Forse<number>;
  struttura: Forse<'inversa' | 'piatta' | 'normale'>;
  skew: Forse<number>;
  skewTesto: Forse<string>;
  /** volatilita' realizzata del future, in %: 20 sedute chiusura su chiusura, e 5 sedute a 30 minuti (5 novembre 2026) */
  realizzata20?: Forse<number>;
  realizzata5?: Forse<number>;
  /** il livello (VIX, VXN) meno la realizzata a 20 sedute: sopra +2 l'assicurazione e' cara, sotto -2 il mercato si muove piu' del prezzato */
  implicitaMenoRealizzata?: Forse<number>;
  implicitaTesto?: Forse<'assicurazione cara' | 'in linea' | 'si muove più del prezzato'>;
  frasi: string[];
  /** prezzo ± movimentoOggi (5 novembre 2026) */
  fasciaPrezzi?: Forse<[number, number]>;
  /** il verdetto, solo fra le misure stile VIX con un punto negli ultimi 30 minuti */
  confronto?: ConfrontoVol;
}

export interface ConfrontoVol {
  verdetto: 'convergono' | 'divergono' | 'non si confronta';
  /** max − min fra le serie confrontate, in punti */
  ampiezza: Forse<number>;
  sogliaPunti: 1;
  serieConfrontate: { id: string; nome: string; fonte: FonteId; valore: number; quando: Ms }[];
  /** "Cboe fuori dalla cassa", "nessun punto oggi", "ultimo punto di 2 h fa" */
  senzaDati: { id: string; nome: string; motivo: string }[];
  /** "misura al prezzo (ATM), non stile VIX" */
  escluse: { id: string; motivo: string }[];
}

export interface RispostaVolInBreve {
  quando: Ms;
  indici: VolInBreve[];
}

// ---------------------------------------------------------------- la scala dei livelli (5 novembre 2026)
// XrayLive/Server/Scala.cs: le voci vicine (6 punti per ES, anche in SPX; 25 per NQ) fuse in zone, dall'alto in basso.

export type TipoLivello =
  | 'flip'
  | 'freno'
  | 'acceleratore'
  | 'muro-freno'
  | 'muro-acceleratore'
  | 'zero'
  | 'zero-0dte'
  | 'call-wall'
  | 'put-wall'
  | 'bordo';

export interface VoceScala {
  /** "flip", "muro freno 0DTE", "freno più forte", "VAL cassa mer 04", "call wall (Cboe QQQ)" */
  cosa: string;
  tipo: TipoLivello;
  /** 'atas' per i bordi del valore: vengono dal chart, non da una fonte di opzioni */
  fonte: FonteId | 'atas';
  scadenza: Forse<Scadenza | '0dte-nq' | 'tutte'>;
  peso: Forse<number>;
  /** l'eta' della fonte di questa voce (NQ) */
  secondiFa: Forse<number>;
}

export interface RigaScala {
  /** centro ed estremi della zona (uguali se la voce e' una) */
  prezzo: number;
  da: number;
  a: number;
  voci: VoceScala[];
  /** prezzo − riferimento */
  distanza: number;
  quando: 'adesso' | 'prossima seduta';
  seduta: Forse<string>;
  /** voci.length > 1 */
  zona: boolean;
}

// ---------------------------------------------------------------- le quattro domande (/api/quattro-domande, 5 novembre 2026)
// La regola sta in XrayLive/Server/QuattroDomande.cs (portata da xray_query.quattro_domande, che ora la legge da qui).
// TUTTI i prezzi di questa risposta sono nel future della radice (ES o NQ).

export type VersoGamma = 'freno' | 'acceleratore';

export interface RegimeSintetico {
  radice: 'ES' | 'NQ';
  base: 'stima OptionsDepth su SPX' | 'convenzione A, supposta';
  /** ES: segno della gamma osservata al prezzo; NQ: segno della GEX totale */
  alPrezzo: Forse<VersoGamma>;
  /** ES: segno della netta di gex__mm_0dte; NQ: segno di totale0 */
  zeroDte: Forse<VersoGamma>;
  /** ES: segno della somma delle tre nette di X-Ray; NQ: uguale ad alPrezzo */
  tutte: Forse<VersoGamma>;
  valori: { alPrezzo: Forse<number>; zeroDte: Forse<number>; tutte: Forse<number> };
  /** "segnali opposti" se alPrezzo e zeroDte ci sono e differiscono */
  lettura: Forse<'freno' | 'acceleratore' | 'segnali opposti'>;
  /** la frase per quella lettura (quella di Python, parola per parola) */
  frase: Forse<string>;
  /** "dice quanto, non dove; ..." piu' l'esito della prova del regime */
  provato: string;
  // --- in piu' rispetto al progetto: servono a xray_query.py per il dizionario di prima
  /** da dove viene il regime, in parole */
  fonte: string;
  /** ES: la netta di ogni scadenza; NQ: null */
  gexPerScadenza: Forse<Partial<Record<Scadenza, Forse<number>>>>;
  /** ES: il flip piu' vicino al prezzo, in ES; NQ: null */
  flipPiuVicino: Forse<number>;
  /** NQ: lo zero gamma di tutte le scadenze e dello 0DTE; ES: null */
  zero: Forse<{ tutte: Forse<number>; zeroDte: Forse<number> }>;
  /** NQ: l'istante dell'ultimo punto della GEX viva */
  quando?: Forse<Ms>;
}

export interface MeteoSintetico {
  indice: Indice;
  nomeIndice: string;
  fonteLivello: FonteId;
  quando: Forse<Ms>;
  livello: Forse<number>;
  fascia: VolInBreve['fascia'];
  movimentoOggi: Forse<number>;
  fasciaPrezzi: Forse<[number, number]>;
  movimentoGiorno: Forse<number>;
  /** in piu' rispetto al progetto: la realizzata a 20 sedute, il termine di paragone dell'implicita */
  realizzata20: Forse<number>;
  implicitaMenoRealizzata: Forse<number>;
  implicitaTesto: Forse<NonNullable<VolInBreve['implicitaTesto']>>;
  pendenza: Forse<number>;
  struttura: VolInBreve['struttura'];
  skew: Forse<number>;
  skewTesto: Forse<string>;
}

export interface Bordo {
  /** "VAL cassa mer 04", "muro alto del ping-pong" */
  nome: string;
  tipo: 'VAL' | 'VAH' | 'POC' | 'muro alto' | 'muro basso';
  seduta: Forse<string>;
  prezzo: number;
}

export interface MuroVicino {
  muro: string;
  prezzo: number;
  bordo: string;
  prezzoBordo: number;
  distanza: number;
}

export interface MuriSulProfilo {
  /** ES 6, NQ 25 */
  sogliaPunti: number;
  bordi: Bordo[];
  muri: { muro: string; prezzo: number; dalPrezzo: Forse<number> }[];
  vicini: MuroVicino[];
  /** cosa manca, per esempio "manca il chart M30 di ES nel bridge" */
  nota: Forse<string>;
}

export interface RispostaQuattroDomande {
  radice: 'ES' | 'NQ';
  giorno: Forse<string>;
  ts: Forse<Ms>;
  /** TUTTI i prezzi di questa risposta sono in ES (o NQ) */
  unita: 'ES' | 'NQ';
  /** per convertire in SPX: prezzo − base */
  baseEsSpx: Forse<number>;
  prezzo: Forse<number>;
  /** il documento della regola (in piu' rispetto al progetto) */
  fonte: string;
  regime: Forse<RegimeSintetico>;
  /** null nel passato: errori ["il meteo c'è solo dal vivo"] */
  meteo: Forse<MeteoSintetico>;
  muri: Forse<MuriSulProfilo>;
  grilletto: string;
  errori: string[];
}
