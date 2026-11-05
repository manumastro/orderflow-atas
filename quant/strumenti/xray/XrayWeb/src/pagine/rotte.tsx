// Le pagine e i gruppi della navigazione (5 novembre 2026: dieci pagine, Livelli e Nel tempo diventano
// "Livelli nel tempo"). Ogni pagina dice quali filtri usa: la barra del contesto spegne gli altri, con la
// ragione scritta. Ogni gruppo porta la sua riserva: breve nella chip, intera nel piede.

import type { ComponentType } from 'react';
import {
  IconChartBar,
  IconChartCandle,
  IconChartHistogram,
  IconChartLine,
  IconCrystalBall,
  IconGauge,
  IconGridDots,
  IconNotes,
  IconPlugConnected,
  IconStack2,
  IconWaveSine,
  type Icon,
} from '@tabler/icons-react';
import { Adesso } from './xray/Adesso';
import { PerStrike } from './xray/PerStrike';
import { HeatMap } from './xray/HeatMap';
import { Deriva } from './xray/Deriva';
import { Livelli } from './xray/Livelli';
import { Previsione } from './xray/Previsione';
import { Volatilita } from './volatilita/Volatilita';
import { NqQqq } from './nq/NqQqq';
import { Fonti } from './fonti/Fonti';
import { Lettura } from './lettura/Lettura';

/** Il nome del sito: si cambia qui, in una riga (D19). */
export const MARCHIO = 'Opzioni dal vivo';

export type GruppoId = 'lettura' | 'xray' | 'volatilita' | 'nq' | 'fonti';

export interface Gruppo {
  id: GruppoId;
  titolo: string;
  sotto: string;
  icona: Icon;
  /** la parola della barra in basso del telefono */
  breve: string;
  /** la riserva intera: il piede della pagina */
  riserva: string;
  /** la riserva breve: la chip della barra del contesto */
  riservaBreve: string;
}

export interface Filtri {
  giorno: boolean;
  istante: boolean;
  prezzi: boolean;
}

export interface Pagina {
  percorso: string;
  titolo: string;
  gruppo: GruppoId;
  Componente: ComponentType;
  filtri: Filtri;
  /** una riga nel mega-menu ("la giornata minuto per minuto") */
  breve: string;
  /** la frase dell'Intro di prima, nella "i" della pagina */
  spiega: string;
  icona?: Icon;
}

export const GRUPPI: Gruppo[] = [
  {
    id: 'lettura',
    titolo: 'Lettura',
    sotto: 'il punto della situazione, scritto dall’agente su richiesta',
    icona: IconNotes,
    breve: 'Lettura',
    riserva:
      'Scritta dall’agente su richiesta, dai dati di questa pagina e del chart di ATAS: è il punto della situazione, non un segnale. Nessuna regola operativa è approvata, e nessun livello delle opzioni è provato su NQ.',
    riservaBreve: 'descrive, non dà segnali',
  },
  {
    id: 'xray',
    titolo: 'SPX · X-Ray',
    sotto: 'la stima di OptionsDepth sui market maker di SPX',
    icona: IconChartHistogram,
    breve: 'X-Ray',
    riserva:
      'Stima di OptionsDepth su SPX (dati Cboe con il tipo di partecipante), letta dalla cache di ATAS: non è NQ, non è una misura, e nessun livello è provato come supporto o resistenza. Prezzi in ES come sul chart di ATAS (SPX più la base ES-SPX); in SPX con il filtro Prezzi. Orari di New York.',
    riservaBreve: 'stima OptionsDepth · non provato',
  },
  {
    id: 'volatilita',
    titolo: 'Volatilità',
    sotto: 'la paura del mercato, da quattro fonti',
    icona: IconWaveSine,
    breve: 'Volat.',
    riserva:
      'La volatilità implicita è il prezzo dell’assicurazione, non una previsione del movimento. Ricostruita da noi su ES e NQ, stimata da X-Ray, pubblicata dal Cboe: le fonti si confrontano, nessuna è provata come segnale. Orari di New York.',
    riservaBreve: 'prezzo dell’assicurazione · non previsione',
  },
  {
    id: 'nq',
    titolo: 'NQ · QQQ',
    sotto: 'la gamma di NQ da tre fonti',
    icona: IconChartCandle,
    breve: 'NQ',
    riserva:
      'GEX di NQ da tre fonti, tutte in prezzi di NQ e nella convenzione A (dealer lunghi call, corti put): è un’ipotesi sul segno, non una misura. Le scale sono diverse: si confrontano forma, segno e livelli. Nessun livello è provato.',
    riservaBreve: 'segno supposto (convenzione A) · non provato',
  },
  {
    id: 'fonti',
    titolo: 'Fonti',
    sotto: 'lo stato di ogni fonte',
    icona: IconPlugConnected,
    breve: 'Fonti',
    riserva: 'Ogni fonte dice quando è arrivato l’ultimo dato. Un dato vecchio si dichiara, non si legge come dato di adesso.',
    riservaBreve: 'un dato vecchio si dichiara',
  },
];

const tutti: Filtri = { giorno: true, istante: true, prezzi: true };
const nessuno: Filtri = { giorno: false, istante: false, prezzi: false };

export const PAGINE: Pagina[] = [
  {
    percorso: 'lettura',
    titolo: 'Lettura',
    gruppo: 'lettura',
    Componente: Lettura,
    filtri: nessuno,
    breve: 'il punto della situazione, scritto dall’agente',
    spiega:
      'Il punto della situazione, scritto dall’agente quando glielo chiedi (“vedi ora”, “aggiorna la lettura”). Si aggiorna da solo appena l’agente ne scrive una nuova. Descrive, non dà segnali.',
    icona: IconNotes,
  },
  {
    percorso: 'adesso',
    titolo: 'Adesso',
    gruppo: 'xray',
    Componente: Adesso,
    filtri: tutti,
    breve: 'le quattro domande, adesso',
    spiega:
      'Dove sta il prezzo rispetto alle zone in cui le coperture dei market maker frenano o accelerano il movimento, e chi è posizionato come.',
    icona: IconGauge,
  },
  {
    percorso: 'strike',
    titolo: 'Per strike',
    gruppo: 'xray',
    Componente: PerStrike,
    filtri: tutti,
    breve: 'l’esposizione di una scadenza, strike per strike',
    spiega:
      'L’esposizione dei market maker (o il delta di un gruppo) su ogni strike, attorno al prezzo. Una barra lunga vicina al prezzo è uno strike che pesa.',
    icona: IconChartBar,
  },
  {
    percorso: 'heatmap',
    titolo: 'Heat map',
    gruppo: 'xray',
    Componente: HeatMap,
    filtri: tutti,
    breve: 'strike per scadenza, con il numero nella cella',
    spiega:
      'Tutte le scadenze (o tutti i gruppi) insieme, strike per strike: si vede subito dove si concentra il peso e se le scadenze dicono la stessa cosa.',
    icona: IconGridDots,
  },
  {
    percorso: 'livelli',
    titolo: 'Livelli nel tempo',
    gruppo: 'xray',
    // Livelli e Nel tempo uniti (fase 2, P2): lo sfondo della mappa e' un comando del grafico
    Componente: Livelli,
    filtri: tutti,
    breve: 'flip e muri durante la giornata, sul prezzo',
    spiega:
      'I livelli chiave di OptionsDepth: i flip (dove il regime cambia) e i picchi (dove il freno o l’acceleratore sono più forti), come si sono mossi durante la giornata, con il prezzo sopra.',
    icona: IconStack2,
  },
  {
    percorso: 'deriva',
    titolo: 'Deriva',
    gruppo: 'xray',
    Componente: Deriva,
    filtri: tutti,
    breve: 'la giornata minuto per minuto',
    spiega:
      'La giornata minuto per minuto: come si sono mossi insieme il prezzo, il regime, la gamma dei market maker e il delta scambiato dai gruppi. Passa il mouse su un pannello: il cursore si muove su tutti.',
    icona: IconChartLine,
  },
  {
    percorso: 'previsione',
    titolo: 'Previsione',
    gruppo: 'xray',
    Componente: Previsione,
    filtri: tutti,
    breve: 'cosa prevede OptionsDepth per il resto della seduta e per domani',
    spiega:
      'Come OptionsDepth si aspetta che cambino freno e acceleratore nelle prossime ore (o nella prossima seduta), se le posizioni restano quelle di adesso. Cambia col tempo che passa e con la charm, non con chi apre o chiude.',
    icona: IconCrystalBall,
  },
  {
    percorso: 'volatilita',
    titolo: 'Volatilità',
    gruppo: 'volatilita',
    Componente: Volatilita,
    filtri: { giorno: true, istante: false, prezzi: false },
    breve: 'il meteo: quanto costa assicurarsi',
    spiega:
      'Quanto il mercato paga per assicurarsi contro i movimenti, su Nasdaq 100 e S&P 500: da quattro fonti messe una sopra l’altra, per vedere se dicono la stessa cosa.',
    icona: IconWaveSine,
  },
  {
    percorso: 'nq',
    titolo: 'NQ · QQQ',
    gruppo: 'nq',
    Componente: NqQqq,
    filtri: nessuno,
    breve: 'la vista Exposure di NQ',
    spiega:
      'La gamma dei dealer su NQ vista da tre fonti: le opzioni NQ che riceve ATAS, e le catene pubbliche di QQQ e NDX del Cboe, tutte portate in prezzi di NQ.',
    icona: IconChartCandle,
  },
  {
    percorso: 'fonti',
    titolo: 'Fonti',
    gruppo: 'fonti',
    Componente: Fonti,
    filtri: nessuno,
    breve: 'la diagnosi delle fonti',
    spiega: 'Da dove viene ogni numero della pagina, e quanto è fresco. Un dato vecchio si dichiara, non si legge come dato di adesso.',
    icona: IconPlugConnected,
  },
];

/** I vecchi indirizzi che rimandano a una pagina nuova, con la stessa query (D9). */
export const REINDIRIZZI: Record<string, string> = { tempo: 'livelli' };

/** La pagina di un percorso, anche di uno vecchio (tempo -> livelli). */
export const paginaDi = (percorso: string): Pagina | undefined => {
  const p = REINDIRIZZI[percorso] ?? percorso;
  return PAGINE.find((x) => x.percorso === p);
};

export const gruppoDi = (id: GruppoId): Gruppo => GRUPPI.find((g) => g.id === id) ?? GRUPPI[1]!;
