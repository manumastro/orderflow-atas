// Le prove di X-Ray 2 (Livelli nel tempo, Deriva, Previsione), sulle risposte vere del server (fixture di
// api/esempi). La fase 2 del 5 novembre 2026: tessere con i valori, titoli stabili (C.13), legenda con i
// valori, la Deriva a quattro pannelli, i minuti anomali che interrompono la linea, le frecce della Previsione.

import { describe, expect, it } from 'vitest';
import { fireEvent, screen, waitFor, within } from '@testing-library/react';
import { rendi, serverFinto } from '../../test/strumenti';
import { grafici } from '../../test/preparazione';
import { Livelli } from './Livelli';
import { Deriva } from './Deriva';
import { Previsione } from './Previsione';
import livelli from '../../api/esempi/xray-livelli.json';
import adesso from '../../api/esempi/xray-adesso.json';
import deriva from '../../api/esempi/xray-deriva.json';
import previsione from '../../api/esempi/xray-previsione.json';
import type { RispostaAdesso, RispostaDeriva, RispostaPrevisione } from '../../api/tipi';
import { auto, num } from '../../formato';

const BASE = 30.45;

/** Le ultime opzioni di un grafico finto, con le serie. */
const ultime = (g: (typeof grafici)[number]) =>
  g.opzioni.filter((o) => (o as { series?: unknown }).series).at(-1) as { series: { name?: string; type?: string; data?: unknown[]; markPoint?: { data: unknown[] } }[]; yAxis?: { min?: number; max?: number } };

/** Nessun testo dei colori di prima ("verde acqua", "viola"): le parole sono freno e acceleratore. */
function senzaColoriVecchi() {
  expect(document.body.textContent).not.toMatch(/verde acqua|viola/i);
}

describe('X-Ray · Livelli nel tempo', () => {
  const a = adesso as RispostaAdesso;

  it('le quattro tessere della risposta, il grafico grande, la scala dei livelli di adesso', async () => {
    serverFinto({ '/api/livelli': livelli, '/api/adesso': adesso });
    rendi(<Livelli />, '/livelli?giorno=2026-10-30');
    const regime = await screen.findByRole('region', { name: 'Regime' });
    await within(regime).findByText('Acceleratore');
    expect(regime.querySelector('.valore')).toHaveAttribute('data-tono', 'acceleratore');
    const flip = screen.getByRole('region', { name: 'Flip più vicino' });
    expect(flip).toHaveTextContent(num(a.flipVicino!.prezzo + BASE, 0));
    expect(flip).toHaveTextContent('sopra: freno');
    expect(screen.getByRole('region', { name: 'Freno più forte' })).toHaveTextContent(num(7798.5 + BASE, 0));
    expect(screen.getByRole('region', { name: 'Acceleratore più forte' })).toHaveTextContent(num(7406 + BASE, 0));

    const grande = screen.getByRole('region', { name: 'Il prezzo e i livelli della giornata' });
    // la legenda porta i valori: il prezzo di adesso, in ES
    await within(grande).findByText(`(${num(a.riferimento.prezzo + BASE, 2)})`);
    expect(grande.querySelectorAll('.grafico')).toHaveLength(1);
    // lo sfondo di default e' Nessuno: nessuna domanda a intervalli
    expect(within(grande).getByRole('radio', { name: 'Nessuno' })).toBeChecked();

    const scala = screen.getByRole('region', { name: 'La scala dei livelli' });
    expect(scala.querySelector('tr.scala-prezzo')).not.toBeNull();
    // solo le righe di adesso: la prossima seduta sta in Previsione
    expect(scala.querySelectorAll('tr.altra-seduta')).toHaveLength(0);
    senzaColoriVecchi();
  });

  it('il prezzo e i flip: il prezzo in due serie (spot vivo e ES − base), le tracce dei flip una per flip', async () => {
    serverFinto({ '/api/livelli': livelli, '/api/adesso': adesso });
    rendi(<Livelli />, '/livelli?giorno=2026-10-30');
    await screen.findByRole('region', { name: 'Regime' });
    await waitFor(() => expect(grafici.some((g) => ultime(g)?.series.some((s) => s.name === 'flip'))).toBe(true));
    const o = ultime(grafici.find((g) => ultime(g)?.series.some((s) => s.name === 'flip'))!);
    expect(o.series.filter((s) => s.name === 'flip')).toHaveLength(livelli.zeri.length);
    expect(o.series.map((s) => s.name)).toEqual(expect.arrayContaining(['prezzo ES (spot vivo)', 'prezzo ES (ES − base)']));
  });
});

describe('X-Ray · Deriva', () => {
  const d = deriva as RispostaDeriva;

  it('quattro pannelli, niente piu’ barre della gamma, le tessere dell’ultimo istante', async () => {
    serverFinto({ '/api/deriva': deriva });
    rendi(<Deriva />, '/deriva?giorno=2026-11-02');
    const gamma = await screen.findByRole('region', { name: 'Gamma al prezzo' });
    await within(gamma).findByText(auto(d.adesso!.gammaAlPrezzo));
    expect(gamma.querySelector('.valore')).toHaveAttribute('data-tono', 'acceleratore');
    expect(screen.getByRole('region', { name: 'GEX netta adesso' })).toHaveTextContent(auto(d.adesso!.gex.totale));
    expect(screen.getByRole('region', { name: 'Retail sugli 0DTE' })).toHaveTextContent(auto(d.adesso!.retail0dte));
    expect(screen.getByRole('region', { name: 'Pressione adesso' })).toHaveTextContent('verso non dichiarato');

    for (const t of ['Il prezzo e il regime', 'GEX netta per scadenza', 'Delta accumulato per gruppo', 'Pressione 0DTE'])
      expect(screen.getByRole('region', { name: t })).toBeInTheDocument();
    expect(document.querySelectorAll('.grafico')).toHaveLength(4);
    await waitFor(() => expect(grafici.filter((g) => ultime(g)).length).toBe(4));
    // l'asse del gamma non esiste piu': nessuna serie a barre, la gamma al prezzo e' la fascia sotto il prezzo
    for (const g of grafici) expect(ultime(g).series.some((s) => s.type === 'bar')).toBe(false);
    expect(ultime(grafici[0]!).series.some((s) => s.name?.startsWith('regime'))).toBe(true);
    // la GEX: lo zero sempre nell'asse
    const gex = ultime(grafici[1]!);
    expect(gex.yAxis!.min!).toBeLessThanOrEqual(0);
    expect(gex.yAxis!.max!).toBeGreaterThanOrEqual(0);
    senzaColoriVecchi();
  });

  it('un minuto anomalo della fixture interrompe la linea, con il segno', async () => {
    serverFinto({ '/api/deriva': deriva });
    rendi(<Deriva />, '/deriva?giorno=2026-11-02');
    await screen.findByRole('region', { name: 'Delta accumulato per gruppo' });
    await waitFor(() => expect(grafici.filter((g) => ultime(g)).length).toBe(4));
    // dexZeroDte.broker, indice 22: "finestra"
    const broker = ultime(grafici[2]!).series.find((s) => s.name === 'Broker')!;
    expect(d.dexZeroDte.broker[22]).not.toBeNull();
    expect((broker.data![22] as [number, number | null])[1]).toBeNull();
    expect((broker.data![21] as [number, number | null])[1]).not.toBeNull();
    expect(broker.markPoint!.data).toHaveLength(1);
    expect(screen.getByRole('region', { name: 'Delta accumulato per gruppo' })).toHaveTextContent(
      `${d.anomali.filter((x) => x.serie.startsWith('dexZeroDte.')).length} minuti anomali`,
    );
  });
});

describe('X-Ray · Previsione', () => {
  const p = previsione as RispostaPrevisione;

  it('le tessere dalla sintesi, i livelli della prossima seduta, la mappa', async () => {
    serverFinto({ '/api/previsione': previsione, '/api/adesso': adesso });
    rendi(<Previsione />, '/previsione?giorno=2026-11-02');
    const flip = await screen.findByRole('region', { name: 'Flip previsto' });
    await within(flip).findByText(num(p.sintesi!.flipFinale! + BASE, 0));
    expect(screen.getByRole('region', { name: 'Regime previsto al prezzo' })).toHaveTextContent('Acceleratore');
    expect(screen.getByRole('region', { name: 'Calcolata alle' })).toHaveTextContent(`${p.asOfs.length} di ${p.asOfs.length}`);
    expect(screen.getByRole('region', { name: 'La mappa prevista' }).querySelectorAll('.grafico')).toHaveLength(1);
    const prossima = screen.getByRole('region', { name: 'Livelli della prossima seduta' });
    await waitFor(() => expect(prossima.querySelectorAll('tbody tr:not(.scala-prezzo)').length).toBeGreaterThan(0));
    expect(prossima).toHaveTextContent('Seduta di');
    senzaColoriVecchi();
  });

  it('◀ e ▶ della previsione chiamano scegliIstante: si chiede la previsione precedente', async () => {
    const chiamate = serverFinto({ '/api/previsione': previsione, '/api/adesso': adesso });
    rendi(<Previsione />, '/previsione?giorno=2026-11-02');
    const calcolata = await screen.findByRole('region', { name: 'Calcolata alle' });
    await within(calcolata).findByText(new RegExp(`${p.asOfs.length} di ${p.asOfs.length}`));
    const avanti = within(calcolata).getByRole('button', { name: 'Previsione successiva' });
    expect(avanti).toBeDisabled();
    fireEvent.click(within(calcolata).getByRole('button', { name: 'Previsione precedente' }));
    const prima = p.asOfs[p.asOfs.length - 2]!;
    await waitFor(() => expect(chiamate.some((u) => u.includes('/api/previsione') && u.includes(`asOf=${prima}`))).toBe(true));
  });
});
