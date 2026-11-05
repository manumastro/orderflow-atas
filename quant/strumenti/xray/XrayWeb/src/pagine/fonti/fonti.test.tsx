// Le prove della pagina Fonti, la diagnosi (C.12, 5 novembre 2026).

import { describe, expect, it } from 'vitest';
import { screen, within } from '@testing-library/react';
import { RISPOSTE_BASE, rendi, serverFinto } from '../../test/strumenti';
import { Fonti, statoFamiglia } from './Fonti';
import fonti from '../../api/esempi/fonti.json';

describe('Fonti con le fixture del contratto', () => {
  it('le tessere: quante fonti sono aggiornate, e cosa guardare', async () => {
    serverFinto({ '/api/fonti': fonti });
    rendi(<Fonti />, '/fonti');
    const agg = await screen.findByRole('region', { name: 'Fonti aggiornate' });
    expect(await within(agg).findByText('1 su 4')).toBeInTheDocument();
    const guarda = screen.getByRole('region', { name: 'Da guardare' });
    expect(within(guarda).getByText('3 fonti')).toBeInTheDocument();
    expect(within(guarda).getByText(/Cboe: Catena NDX: errore · Yahoo: VXN assente/)).toBeInTheDocument();
  });

  it('la tabella raggruppata: 4 righe di testa, lo stato in parole, "dove si vede"', async () => {
    serverFinto({ '/api/fonti': fonti });
    const { container } = rendi(<Fonti />, '/fonti');
    const tab = await screen.findByRole('region', { name: 'Lo stato di ogni fonte' });
    expect(await within(tab).findByText('vecchia 40 min')).toBeInTheDocument();
    expect(container.querySelectorAll('tr.fonte-gruppo').length).toBe(4);
    expect(within(tab).getAllByText('in errore').length).toBeGreaterThan(0);
    expect(within(tab).getAllByText('· HTTP 403 dal sito del Cboe', { exact: false }).length).toBeGreaterThan(0);
    expect(within(tab).getByText('NQ · QQQ › Profilo (QQQ), I livelli')).toBeInTheDocument();
    // la riga "yahoo" di prima (VIX e VXN insieme) non si mostra: nascondeva il VXN assente
    expect(within(tab).queryByText('yahoo', { selector: '.mono' })).toBeNull();
    expect(within(tab).getByText('yahoo-vxn')).toBeInTheDocument();
  });

  it('le famiglie stanno in <details> chiusi per indicatore, e sono nel DOM', async () => {
    serverFinto({ '/api/fonti': fonti });
    const { container } = rendi(<Fonti />, '/fonti');
    expect(await screen.findByText('gex__mm_0dte')).toBeInTheDocument();
    const d = container.querySelectorAll('details.famiglia');
    expect(d.length).toBe(1);
    expect(d[0]?.hasAttribute('open')).toBe(false);
  });

  it('una famiglia senza dati, accanto alle sue sotto-famiglie, è "non usata"', () => {
    const fam = [
      { nome: 'forecast', minuti: null, indicatore: 'x' },
      { nome: 'forecast__gamma', minuti: 0.3, indicatore: 'x' },
    ];
    expect(statoFamiglia(fam[0]!, fam)).toBe('non usata');
    expect(statoFamiglia(fam[1]!, fam)).toBe('ok');
    expect(statoFamiglia({ nome: 'y', minuti: null, indicatore: 'z' }, [])).toBe('assente');
    expect(statoFamiglia({ nome: 'y', minuti: 12, indicatore: 'z' }, [])).toBe('vecchio');
  });
});

describe('Fonti con il server di oggi (la rotta delle fonti risponde 404)', () => {
  it('la tabella delle fonti non c’è ancora, il lettore sì', async () => {
    serverFinto({});
    rendi(<Fonti />, '/fonti');
    expect(await screen.findByText('Fonte non ancora disponibile')).toBeInTheDocument();
    expect(await screen.findByText('gex__mm_0dte')).toBeInTheDocument();
    const lettore = screen.getByRole('region', { name: 'Lettore di X-Ray' });
    expect(within(lettore).getByText('vivo')).toBeInTheDocument();
    expect(RISPOSTE_BASE['/api/stato']).toBeDefined();
  });
});
