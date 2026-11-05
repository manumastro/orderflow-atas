// Le prove della pagina Volatilita', "il meteo" (C.10, 5 novembre 2026).

import { describe, expect, it } from 'vitest';
import { screen, waitFor, within } from '@testing-library/react';
import { rendi, serverFinto } from '../../test/strumenti';
import { grafici } from '../../test/preparazione';
import { Volatilita } from './Volatilita';
import { posizioneFascia } from './Meteo';
import { giorniAlla, limitiVol, massimoTondo, valoreAl } from './GraficiVol';
import { scriviSenzaDati, scriviVerdetto } from './confronto';
import type { RispostaVolInBreve } from '../../api/tipi';
import volSerie from '../../api/esempi/vol-serie.json';
import volTermine from '../../api/esempi/vol-termine.json';
import sorrisoSpx from '../../api/esempi/vol-sorriso-spx.json';
import volInBreve from '../../api/esempi/vol-in-breve.json';

const tutte = { '/api/vol/in-breve': volInBreve, '/api/vol/serie': volSerie, '/api/vol/termine': volTermine, '/api/vol/sorriso': sorrisoSpx };

interface OpzioniFinte {
  series?: { name?: string; type?: string; endLabel?: { show?: boolean }; markLine?: { data?: { name?: string }[] } }[];
}
const ultime = () => grafici.map((g) => g.opzioni.at(-1) as OpzioniFinte | undefined).filter((o): o is OpzioniFinte => o != null);

describe('il meteo', () => {
  it('Nasdaq 100 prima, S&P 500 dopo; il livello con la fascia, il movimento in prezzi, i verdetti in parole', async () => {
    serverFinto(tutte);
    rendi(<Volatilita />, '/volatilita');
    const ndx = await screen.findByRole('region', { name: 'Il meteo · Nasdaq 100' });
    const spx = screen.getByRole('region', { name: 'Il meteo · S&P 500' });
    // l'ordine: NQ prima (M8)
    expect(ndx.compareDocumentPosition(spx) & Node.DOCUMENT_POSITION_FOLLOWING).toBeTruthy();
    expect(within(ndx).getByText(/VXN 25,1/, { selector: '.meteo-livello' })).toBeInTheDocument();
    expect(within(ndx).getByText(/normale/, { selector: '.meteo-fascia' })).toBeInTheDocument();
    expect(within(ndx).getByText('±353')).toBeInTheDocument();
    expect(within(ndx).getByText('28.052–28.758 NQ')).toBeInTheDocument();
    expect(within(ndx).getByText('+3,4')).toBeInTheDocument();
    expect(within(ndx).getByText('cara')).toBeInTheDocument();
    expect(within(ndx).getByText('put care')).toBeInTheDocument();
    // le frasi del server non si buttano: stanno dietro "Perché", nel DOM
    expect(within(ndx).getByText(/VXN a 25,1: volatilità normale/)).toBeInTheDocument();
    // le sei tessere per un numero di prima non ci sono piu'
    expect(screen.queryByRole('region', { name: /Fonti a confronto/ })).toBeNull();
    expect(screen.queryByRole('region', { name: /Cosa dice adesso/ })).toBeNull();
  });

  it('su S&P 500 le fonti stile VIX convergono: l’ATM di X-Ray è esclusa dal confronto', async () => {
    // Prima la pagina scriveva "divergono" su SPX: metteva nel confronto anche l'ATM di X-Ray, che sta 3-4
    // punti sotto il VIX per costruzione (la volatilita' al prezzo, senza le ali). La regola
    // (come-si-leggono-le-opzioni-dal-vivo.md, passo 1) confronta solo le misure stile VIX: il verdetto
    // lo da' il server, e con la fixture e' "convergono" (0,4 punti fra VIX ricostruito e VIX).
    serverFinto(tutte);
    rendi(<Volatilita />, '/volatilita');
    const spx = await screen.findByRole('region', { name: 'Il meteo · S&P 500' });
    const riga = spx.querySelector('.meteo-fonti');
    expect(riga?.getAttribute('data-verdetto')).toBe('convergono');
    expect(within(spx).getByText('convergono (0,4 punti)')).toBeInTheDocument();
    expect(within(spx).getByText(/Non confrontate: ATM X-Ray, misura al prezzo \(ATM\), non stile VIX/)).toBeInTheDocument();
    const ndx = screen.getByRole('region', { name: 'Il meteo · Nasdaq 100' });
    expect(within(ndx).getByText(/1 senza dato: Vol 30g QQQ \(Cboe\) \(nessun punto oggi\)/)).toBeInTheDocument();
  });

  it('i verdetti si scrivono in inchiostro, senza i colori del segno', async () => {
    serverFinto(tutte);
    const { container } = rendi(<Volatilita />, '/volatilita');
    await screen.findByRole('region', { name: 'Il meteo · Nasdaq 100' });
    expect(container.innerHTML).not.toMatch(/#4fae38|#b5443f|verde acqua|viola/i);
  });
});

describe('i grafici della volatilità', () => {
  it('sei grafici; la legenda ha i valori e solo le serie con dati, le vuote in una riga', async () => {
    serverFinto(tutte);
    rendi(<Volatilita />, '/volatilita');
    await screen.findByRole('region', { name: 'Il meteo · Nasdaq 100' });
    const tempo = await screen.findByRole('region', { name: 'Nel tempo · Nasdaq 100 (NDX, NQ)' });
    expect(await within(tempo).findByText(/senza dati oggi: Vol 30g QQQ \(Cboe\)/)).toBeInTheDocument();
    expect(within(tempo).queryByText(/Vol 30g QQQ \(Cboe\) · nessun dato/)).toBeNull();
    // ogni voce porta il valore: "VXN ricostruito (25,1%)"
    expect(tempo.querySelectorAll('.legenda .chiave-valore').length).toBeGreaterThanOrEqual(3);
    expect(grafici.length).toBeGreaterThanOrEqual(6);
    for (const t of ['Struttura a termine · NDX', 'Struttura a termine · SPX', 'Sorriso · NDX', 'Sorriso · SPX', 'Nel tempo · S&P 500 (SPX, ES)']) {
      expect(screen.getByRole('region', { name: t })).toBeInTheDocument();
    }
  });

  it('la struttura a termine porta gli eventi del calendario come linee col nome', async () => {
    serverFinto(tutte);
    rendi(<Volatilita />, '/volatilita');
    await screen.findByRole('region', { name: 'Struttura a termine · NDX' });
    const eventi = () =>
      ultime()
        .flatMap((o) => o.series ?? [])
        .filter((s) => s.name === 'eventi')
        .flatMap((s) => s.markLine?.data ?? []);
    await waitFor(() => expect(eventi().length).toBeGreaterThan(0));
    expect(eventi()[0]?.name).toMatch(/^evento 05\/11 sussidi di disoccupazione/);
  });

  it('i conti dei grafici: la fascia, i limiti, il massimo tondo, i giorni di un evento', () => {
    expect(posizioneFascia(25.05, [20, 26, 38])).toBeGreaterThan(0.25);
    expect(posizioneFascia(25.05, [20, 26, 38])).toBeLessThan(0.5);
    expect(posizioneFascia(5, [20, 26, 38])).toBe(0);
    expect(posizioneFascia(80, [20, 26, 38])).toBe(1);
    expect(limitiVol([16.2, 17.9])).toEqual([15, 19]);
    expect(massimoTondo(151)).toBe(200);
    expect(massimoTondo(90)).toBe(100);
    const punti = [{ scadenza: '2026-11-06', giorni: 1.34, vol: 20 }];
    expect(giorniAlla('2026-11-06', punti)).toBe(1.34);
    expect(giorniAlla('2026-11-13', punti)).toBeCloseTo(8.34, 5);
    expect(valoreAl([[1, 10], [2, null], [3, 12]], 2)).toBe(10);
    expect(valoreAl([[1, 10], [2, null], [3, 12]], null)).toBe(12);
  });

  it('il confronto si scrive: il verdetto con l’ampiezza, chi non ha dati', () => {
    const c = (volInBreve as RispostaVolInBreve).indici[1]!.confronto!;
    expect(scriviVerdetto(c)).toBe('convergono (0,3 punti)');
    expect(scriviSenzaDati(c)).toBe('1 senza dato: Vol 30g QQQ (Cboe) (nessun punto oggi)');
  });
});

describe('con il server di oggi (le rotte rispondono 404)', () => {
  it('Volatilità: "non ancora disponibile", nessun avviso rosso', async () => {
    serverFinto({});
    rendi(<Volatilita />, '/volatilita');
    expect((await screen.findAllByText('Fonte non ancora disponibile')).length).toBeGreaterThan(0);
    expect(screen.queryByRole('alert')).toBeNull();
  });
});
