// Le prove della pagina NQ · QQQ, la vista Exposure (C.11, 5 novembre 2026).

import { describe, expect, it } from 'vitest';
import { screen, waitFor, within } from '@testing-library/react';
import { rendi, serverFinto } from '../../test/strumenti';
import { grafici } from '../../test/preparazione';
import { C } from '../../tema/colori';
import { NqQqq } from './NqQqq';
import nqGex from '../../api/esempi/nq-gex.json';
import gexTempo from '../../api/esempi/gex-tempo-nq.json';
import quattro from '../../api/esempi/quattro-domande-nq.json';
import fonti from '../../api/esempi/fonti.json';

const tutte = { '/api/nq/gex': nqGex, '/api/gex/tempo': gexTempo, '/api/quattro-domande': quattro, '/api/fonti': fonti };

interface Serie {
  type?: string;
  name?: string;
  data?: unknown[];
  renderItem?: (p: unknown, api: unknown) => { style?: { fill?: string } };
  markLine?: { data?: { name?: string; lineStyle?: { color?: string; type?: string } }[] };
}
const serieDi = (o: unknown) => ((o as { series?: Serie[] } | undefined)?.series ?? []);

/** I colori delle barre di un profilo: renderItem chiamato su ogni dato, come farebbe ECharts. */
function coloriBarre(s: Serie): Set<string> {
  const colori = new Set<string>();
  for (const d of (s.data ?? []) as number[][]) {
    const api = { value: (i: number) => d[i], coord: ([x, y]: number[]) => [x ?? 0, y ?? 0] };
    const fill = s.renderItem?.({}, api).style?.fill;
    if (fill) colori.add(fill);
  }
  return colori;
}

describe('NQ · QQQ con le fixture del contratto', () => {
  it('le sei tessere: il regime con la riserva, gli zeri con la distanza, i muri, il movimento atteso', async () => {
    serverFinto(tutte);
    rendi(<NqQqq />, '/nq');
    const regime = await screen.findByRole('region', { name: 'Regime NQ' });
    expect(await within(regime).findByText('Acceleratore')).toBeInTheDocument();
    expect(regime.querySelector('.valore')?.getAttribute('data-tono')).toBe('acceleratore');
    expect(within(regime).getByText('supposto')).toBeInTheDocument();
    expect(within(regime).getByText('tutte −939 · 0DTE −176: concordi')).toBeInTheDocument();
    const zero = screen.getByRole('region', { name: 'Zero gamma' });
    expect(zero.querySelector('.valore')?.textContent).toBe('28.851 ↑520');
    // un'ora fa lo zero era a 28.835,74 (unOraFa): si e' mosso di +15
    expect(within(zero).getByText('▲ +15 in un’ora')).toBeInTheDocument();
    expect(screen.getByRole('region', { name: 'Zero 0DTE' }).querySelector('.valore')?.textContent).toBe('28.513 ↑182');
    const call = screen.getByRole('region', { name: 'Call wall' });
    expect(call.querySelector('.valore')?.textContent).toBe('28.700 ↑369');
    expect(call.querySelector('.valore')?.getAttribute('data-tono')).toBe('freno');
    expect(screen.getByRole('region', { name: 'Put wall' }).querySelector('.valore')?.getAttribute('data-tono')).toBe('acceleratore');
    const mov = screen.getByRole('region', { name: 'Movimento atteso oggi' });
    expect(within(mov).getByText('±330')).toBeInTheDocument();
    expect(within(mov).getByText('28.006–28.666 NQ')).toBeInTheDocument();
  });

  it('il profilo: barre verdi e rosse nel segno, lo zero gamma grigio tratteggiato, "segno supposto" in vista', async () => {
    serverFinto(tutte);
    rendi(<NqQqq />, '/nq');
    const profilo = await screen.findByRole('region', { name: 'Profilo GEX per strike, in prezzi di NQ' });
    expect(within(profilo).getByText('segno supposto')).toBeInTheDocument();
    let barre: Serie | undefined;
    await waitFor(() => {
      barre = grafici.flatMap((g) => serieDi(g.opzioni.at(-1))).find((s) => s.name === 'NQ (catena CME)');
      expect(barre).toBeDefined();
    });
    expect(coloriBarre(barre!)).toEqual(new Set([C.freno, C.acceleratore]));
    const zero = barre!.markLine?.data?.find((d) => d.name?.startsWith('zero gamma'));
    expect(zero?.name).toBe('zero gamma 29.054');
    expect(zero?.lineStyle?.color).toBe(C.flip);
    expect(zero?.lineStyle?.type).toBe('dashed');
    // la legenda porta i valori, e il prezzo di QQQ da Yahoo, anche fuori dalla cassa
    expect(within(profilo).getByText('(28.760,50)')).toBeInTheDocument();
    expect(within(profilo).getByText('687,80')).toBeInTheDocument();
    expect(within(profilo).getByText(/pre-mercato/)).toBeInTheDocument();
    expect(within(profilo).getByText('la paura: Volatilità › Il meteo · Nasdaq 100')).toBeInTheDocument();
  });

  it('i profili piccoli dicono l’età; la fonte vuota lo dice, e con un clic diventa la scelta', async () => {
    serverFinto(tutte);
    rendi(<NqQqq />, '/nq');
    const vuota = (await screen.findByText('Nessun dato.')).closest('button');
    expect(vuota).not.toBeNull();
    const qqq = screen.getByRole('button', { name: 'Apri il profilo di QQQ (Cboe)' });
    expect(within(qqq).getByText(/15 min/)).toBeInTheDocument();
    vuota?.click();
    expect(await screen.findByText('Nessun dato da questa fonte.')).toBeInTheDocument();
  });

  it('i livelli: la scala con la riga del prezzo, e la conversione in NQ nella "i"', async () => {
    serverFinto(tutte);
    rendi(<NqQqq />, '/nq');
    const livelli = await screen.findByRole('region', { name: 'I livelli' });
    expect(await within(livelli).findByText('prezzo NQ')).toBeInTheDocument();
    expect(within(livelli).getByText('VAH cassa mer 04')).toBeInTheDocument();
    expect(within(livelli).getByText(/QQQ × 41,52 \(NQ \/ QQQ alla stessa ora\)/)).toBeInTheDocument();
    expect(within(livelli).getByText(/Nessun muro entro 25 punti da un bordo del valore/)).toBeInTheDocument();
    // le schede di prima sono tolte
    expect(screen.queryByRole('region', { name: 'Le fonti a confronto' })).toBeNull();
    expect(screen.queryByRole('region', { name: 'VXN, la paura sul Nasdaq 100' })).toBeNull();
    expect(screen.queryByRole('region', { name: 'NQ adesso' })).toBeNull();
  });

  it('la GEX viva nel tempo: i nomi in fondo con il valore, le barre della GEX attorno allo zero', async () => {
    serverFinto(tutte);
    rendi(<NqQqq />, '/nq');
    await screen.findByRole('region', { name: 'La GEX viva di NQ nel tempo' });
    let serie: Serie[] = [];
    await waitFor(() => {
      serie = grafici.flatMap((g) => serieDi(g.opzioni.at(-1))).filter((s) => s.name === 'zero gamma' || s.name === 'GEX totale');
      expect(serie.length).toBe(2);
    });
    const zero = serie.find((s) => s.name === 'zero gamma') as Serie & { endLabel?: { formatter?: string } };
    expect(zero.endLabel?.formatter).toBe('zero gamma 28.851');
  });
});

describe('NQ · QQQ con il server di oggi (le rotte rispondono 404)', () => {
  it('"non ancora disponibile", nessun avviso rosso', async () => {
    serverFinto({});
    rendi(<NqQqq />, '/nq');
    expect((await screen.findAllByText('Fonte non ancora disponibile')).length).toBeGreaterThan(0);
    expect(screen.queryByRole('alert')).toBeNull();
  });
});
