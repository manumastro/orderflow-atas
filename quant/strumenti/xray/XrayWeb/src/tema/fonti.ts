// Le quattro fonti, ciascuna con colore, icona, nome e tratto fissi in tutta la pagina: il badge
// di ogni scheda, la legenda e la serie del grafico usano gli stessi.
//
// La palette (magenta, viola, giallo, verde) passa lo script della skill dataviz sul riquadro
// #111723, a coppie tutte: fascia di luminosita', croma, normale 15,6, contrasto >= 3:1 (5 novembre
// 2026: la catena passa dal blu al viola, perche' il blu ora vuol dire "comprato"). La coppia
// peggiore per il daltonismo e' giallo-verde (cboe-yahoo, deltaE 6,9, fascia 6-8): e' ammessa solo
// con una codifica in piu', che qui e' il tratto (cboe tratteggiato, yahoo a puntini), l'icona e il
// nome scritto in fondo alla linea.

import { IconBuildingBank, IconLink, IconScan, IconWorld, type Icon } from '@tabler/icons-react';
import type { FonteId } from '../api/tipi';

export type Tratto = 'solid' | 'dashed' | 'dotted';

export interface Fonte {
  id: FonteId;
  /** il nome per la persona */
  nome: string;
  /** il nome corto, per le legende strette */
  breve: string;
  colore: string;
  Icona: Icon;
  tratto: Tratto;
  /** cos'e', in una riga */
  cosa: string;
  ritardo: string;
}

export const FONTI: Record<FonteId, Fonte> = {
  xray: {
    id: 'xray',
    nome: 'X-Ray · OptionsDepth',
    breve: 'X-Ray',
    colore: '#d55181',
    Icona: IconScan,
    tratto: 'solid',
    cosa: 'la stima del fornitore su SPX, dalla cache di ATAS',
    ritardo: 'vivo (1 min)',
  },
  catena: {
    id: 'catena',
    nome: 'Catena CME · ATAS',
    breve: 'Catena',
    colore: '#9256bd',
    Icona: IconLink,
    tratto: 'solid',
    cosa: 'le opzioni ES e NQ che ATAS riceve, calcolo nostro',
    ritardo: 'vivo (1 min)',
  },
  cboe: {
    id: 'cboe',
    nome: 'Cboe pubblico',
    breve: 'Cboe',
    colore: '#c98500',
    Icona: IconBuildingBank,
    tratto: 'dashed',
    cosa: 'la catena di QQQ e NDX dal sito del Cboe, calcolo nostro',
    ritardo: '15 min',
  },
  yahoo: {
    id: 'yahoo',
    nome: 'Indice ufficiale · Yahoo',
    breve: 'Yahoo',
    colore: '#008300',
    Icona: IconWorld,
    tratto: 'dotted',
    cosa: 'VIX e VXN pubblicati dal Cboe, via Yahoo',
    ritardo: '~15 min',
  },
};

export const ORDINE_FONTI: FonteId[] = ['xray', 'catena', 'cboe', 'yahoo'];

/** La fonte di un id, anche se il server ne mandasse una sconosciuta. */
export function fonte(id: string): Fonte {
  return (FONTI as Record<string, Fonte | undefined>)[id] ?? {
    id: id as FonteId,
    nome: id,
    breve: id,
    colore: '#898781',
    Icona: IconWorld,
    tratto: 'solid',
    cosa: 'fonte non riconosciuta',
    ritardo: '?',
  };
}
