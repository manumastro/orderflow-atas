// Le prove di X-Ray 1 (Adesso, Per strike, Heat map), sulle risposte vere del server (fixture in api/esempi/).
// Adesso: la prima tessera e' il prezzo a 2 decimali (la legge prova_pagina.py), il regime dice "Segnali
// opposti" quando al prezzo e sullo 0DTE il verso e' diverso. Heat map: 6 colonne col DEX, una sola linea del
// prezzo, che sta FRA due righe.

import { describe, expect, it } from 'vitest';
import { screen, within } from '@testing-library/react';
import { rendi, serverFinto } from '../../test/strumenti';
import { grafici } from '../../test/preparazione';
import adesso from '../../api/esempi/xray-adesso.json';
import quattro from '../../api/esempi/quattro-domande-es.json';
import strike from '../../api/esempi/xray-strike.json';
import heatmap from '../../api/esempi/xray-heatmap.json';
import { Adesso } from './Adesso';
import { PerStrike } from './PerStrike';
import { HeatMap } from './HeatMap';
import { calcolaHeat, nomeColonna } from '../../componenti/TabellaHeat';
import type { RispostaAdesso, RispostaHeatmap, RispostaQuattroDomande } from '../../api/tipi';
import { num } from '../../formato';

const a = adesso as RispostaAdesso;
const q = quattro as unknown as RispostaQuattroDomande;
const h = heatmap as RispostaHeatmap;

/** Un profilo osservato piccolo attorno al prezzo della fixture: meta' freno sopra, acceleratore sotto. */
const profilo = {
  giorno: a.giorno,
  ts: a.ts,
  greca: 'gamma',
  presente: true,
  istante: a.ts,
  riferimento: a.riferimento,
  prezzi: [7550, 7560, 7570, 7580, 7590, 7600, 7610],
  valori: [-40, -20, -5, 3, 12, 30, 8],
  alPrezzo: -137.67,
  zeri: [7560.11],
  picchiPositivi: a.picchiPositivi,
  picchiNegativi: a.picchiNegativi,
};

/** Tutti i testi di una markLine nelle opzioni registrate dall'ECharts finto. */
function nomiLinee(): string[] {
  const nomi: string[] = [];
  for (const g of grafici)
    for (const o of g.opzioni as { series?: { markLine?: { data?: { name?: string }[] } }[] }[])
      for (const s of o.series ?? []) for (const d of s.markLine?.data ?? []) if (d.name) nomi.push(d.name);
  return nomi;
}

const risposteAdesso = (extra: Record<string, unknown> = {}) => ({
  '/api/adesso': adesso,
  '/api/profilo': { giorno: a.giorno, ts: a.ts, greca: 'gamma', presente: false },
  '/api/quattro-domande': quattro,
  ...extra,
});

describe('X-Ray · Adesso, le quattro domande', () => {
  it('la prima tessera è il prezzo, a 2 decimali, in SPX se scelto', async () => {
    serverFinto(risposteAdesso());
    const { container } = rendi(<Adesso />, '/adesso?giorno=2026-10-30&prezzi=SPX');
    const prezzo = await screen.findByRole('region', { name: 'Prezzo' });
    expect(prezzo).toHaveTextContent(num(a.riferimento.prezzo, 2));
    expect(container.querySelector('.tessera .valore')?.textContent).toBe(num(a.riferimento.prezzo, 2));
    expect(screen.getByText('Il profilo osservato non c’è', { exact: false })).toBeInTheDocument();
  });

  it('senza scelta i prezzi sono in ES, come sul chart di ATAS: il prezzo si sposta della base', async () => {
    serverFinto(risposteAdesso());
    rendi(<Adesso />, '/adesso?giorno=2026-10-30');
    const prezzo = await screen.findByRole('region', { name: 'Prezzo' });
    await within(prezzo).findByText(num(a.riferimento.prezzo + 30.45, 2));
    expect(prezzo).toHaveTextContent('ES ·');
  });

  it('le sei tessere e i tre riquadri con i titoli stabili; niente frasi lunghe a vista', async () => {
    serverFinto(risposteAdesso());
    rendi(<Adesso />, '/adesso?giorno=2026-10-30&prezzi=SPX');
    await screen.findByRole('region', { name: 'Regime' });
    for (const t of ['Prezzo', 'Regime', 'Flip più vicino', 'Meteo', 'Muri sul valore', 'Pressione 0DTE'])
      expect(screen.getByRole('region', { name: t })).toBeInTheDocument();
    for (const t of ['Freno e acceleratore, prezzo per prezzo', 'La scala dei prezzi', 'Chi ha comprato delta oggi'])
      expect(screen.getByRole('region', { name: t })).toBeInTheDocument();
    // le schede di prima non ci sono piu'
    expect(screen.queryByRole('region', { name: 'In parole semplici' })).toBeNull();
    expect(screen.queryByRole('region', { name: 'GEX dei market maker per scadenza' })).toBeNull();
    expect(screen.queryByRole('region', { name: 'Gamma al prezzo' })).toBeNull();
  });

  it('"Segnali opposti" quando al prezzo e sullo 0DTE il verso è diverso, con le tre pastiglie e "non provato"', async () => {
    serverFinto(risposteAdesso());
    rendi(<Adesso />, '/adesso?giorno=2026-10-30&prezzi=SPX');
    const regime = await screen.findByRole('region', { name: 'Regime' });
    await within(regime).findByText('Segnali opposti');
    expect(regime).toHaveTextContent('al prezzo');
    expect(regime).toHaveTextContent('0DTE');
    expect(regime).toHaveTextContent('tutte');
    expect(regime).toHaveTextContent('non provato');
    // nessun pallino del tono: il regime non e' netto
    expect(regime.querySelector('.valore')?.getAttribute('data-tono')).toBeNull();
  });

  it('un regime netto prende il pallino del suo segno', async () => {
    const netto = { ...q, regime: { ...q.regime!, lettura: 'freno', alPrezzo: 'freno', zeroDte: 'freno' } };
    serverFinto(risposteAdesso({ '/api/quattro-domande': netto }));
    rendi(<Adesso />, '/adesso?giorno=2026-10-30&prezzi=SPX');
    const regime = await screen.findByRole('region', { name: 'Regime' });
    await within(regime).findByText('Freno');
    expect(regime.querySelector('.valore')?.getAttribute('data-tono')).toBe('freno');
  });

  it('il meteo e i muri sul valore vengono dalle quattro domande, nell\'unità scelta', async () => {
    serverFinto(risposteAdesso());
    rendi(<Adesso />, '/adesso?giorno=2026-10-30&prezzi=ES');
    const meteo = await screen.findByRole('region', { name: 'Meteo' });
    await within(meteo).findByText('±44');
    expect(meteo).toHaveTextContent('VIX 16,9 normale');
    expect(meteo).toHaveTextContent('assicurazione cara');
    const muri = screen.getByRole('region', { name: 'Muri sul valore' });
    expect(muri).toHaveTextContent('nessuno entro 6');
  });

  it('la scala dei prezzi ha la riga del prezzo, e la striscia del regime per scadenza', async () => {
    serverFinto(risposteAdesso());
    rendi(<Adesso />, '/adesso?giorno=2026-10-30&prezzi=SPX');
    const scala = await screen.findByRole('region', { name: 'La scala dei prezzi' });
    await within(scala).findByText(`prezzo SPX`);
    expect(scala.querySelectorAll('tr.scala-prezzo')).toHaveLength(1);
    expect(within(scala).getByLabelText('Regime per scadenza')).toHaveTextContent('0DTE');
  });

  it('chi ha comprato delta: una riga per gruppo, barre nel colore del flusso', async () => {
    serverFinto(risposteAdesso());
    rendi(<Adesso />, '/adesso?giorno=2026-10-30&prezzi=SPX');
    const delta = await screen.findByRole('region', { name: 'Chi ha comprato delta oggi' });
    await within(delta).findByText('Retail');
    expect(delta.querySelectorAll('tbody tr')).toHaveLength(4);
    expect(delta).toHaveTextContent('flusso sommato da inizio giornata, non posizione');
    const colori = [...delta.querySelectorAll<HTMLElement>('.barra-inline-segno')].map((x) => x.style.background);
    expect(colori.length).toBeGreaterThan(0);
    for (const c of colori) expect(['rgb(57, 135, 229)', 'rgb(217, 89, 38)']).toContain(c);
  });

  it('il profilo: legenda con i valori, linee col valore nel nome (prezzo, flip, picchi)', async () => {
    serverFinto(risposteAdesso({ '/api/profilo': profilo }));
    rendi(<Adesso />, '/adesso?giorno=2026-10-30&prezzi=SPX');
    const scheda = await screen.findByRole('region', { name: 'Freno e acceleratore, prezzo per prezzo' });
    await within(scheda).findByRole('img', { name: 'La gamma dei market maker prezzo per prezzo' });
    expect(scheda.querySelector('.scheda-legenda')).toHaveTextContent(`prezzo SPX(${num(a.riferimento.prezzo, 2)})`);
    expect(scheda.querySelector('.scheda-legenda')).toHaveTextContent('gamma al prezzo(−137,7)');
    const nomi = nomiLinee();
    expect(nomi.some((n) => n.startsWith('prezzo '))).toBe(true);
    expect(nomi.some((n) => n.includes('flip 7.560'))).toBe(true);
  });

  it('prima della cassa una riga dice per quale seduta valgono i livelli (e non è un .avviso)', async () => {
    const prima = { ...a, riferimento: { ...a.riferimento, vivo: false, fonte: 'ES meno la base' } };
    serverFinto(
      risposteAdesso({
        '/api/adesso': prima,
        '/api/istanti': { giorno: '2026-10-30', istanti: [1793620800000, 1793620860000], cassa: { apertura: 1793629800000, chiusura: 1793653200000 } },
      }),
    );
    const { container } = rendi(<Adesso />, '/adesso?giorno=2026-10-30&prezzi=SPX');
    await screen.findByText('Pre-cassa', { exact: false });
    expect(container.querySelector('.avviso-cassa')).toHaveTextContent('lun 02/11');
    expect(container.querySelector('.avviso')).toBeNull();
  });

  it('nessun colore di prima nei testi', async () => {
    serverFinto(risposteAdesso({ '/api/profilo': profilo }));
    const { container } = rendi(<Adesso />, '/adesso?giorno=2026-10-30&prezzi=SPX');
    await screen.findByRole('region', { name: 'Regime' });
    expect(container.textContent).not.toMatch(/verde acqua|viola|gialle/i);
  });
});

describe('X-Ray · Per strike', () => {
  it('le quattro tessere della GEX, i due riquadri, le linee e i colori del segno', async () => {
    serverFinto({ '/api/strike': strike });
    rendi(<PerStrike />, '/strike?giorno=2026-10-30&prezzi=SPX');
    const netta = await screen.findByRole('region', { name: 'Netta della scadenza' });
    expect(netta).toHaveTextContent('−2,1');
    expect(netta.querySelector('.valore')?.getAttribute('data-tono')).toBe('acceleratore');
    expect(screen.getByRole('region', { name: 'Entro l’1%' })).toHaveTextContent('+0,63');
    expect(screen.getByRole('region', { name: 'Muro freno' })).toHaveTextContent('7.525↓59');
    expect(screen.getByRole('region', { name: 'Muro acceleratore' })).toHaveTextContent('7.600↑16');
    const vicini = screen.getByRole('region', { name: 'Strike vicini al prezzo' });
    expect(vicini).toHaveTextContent('Sopra, dal più vicino');
    expect(vicini).toHaveTextContent('Sotto, dal più vicino');
    expect(vicini.querySelectorAll('tr.riga-prezzo')).toHaveLength(1);
    // il primo sopra e' il piu' vicino: 7.585
    expect(vicini.querySelectorAll('tbody tr')[1]).toHaveTextContent('7.585');
    expect(vicini.querySelectorAll('td.forte')).toHaveLength(3);
    const scheda = screen.getByRole('region', { name: 'Esposizione per strike' });
    await within(scheda).findByRole('img', { name: 'Esposizione per strike' });
    const nomi = nomiLinee();
    expect(nomi.some((n) => n.includes('flip 7.560'))).toBe(true);
    expect(nomi.some((n) => n.includes('muro freno 7.525'))).toBe(true);
    expect(scheda.querySelector('.scheda-legenda')).toHaveTextContent('freno');
    expect(scheda.querySelector('.scheda-legenda')).toHaveTextContent('acceleratore');
    expect(scheda.textContent).not.toMatch(/verde acqua|viola/i);
  });

  it('le etichette dei segmenti restano quelle che la prova della pagina clicca', async () => {
    serverFinto({ '/api/strike': strike });
    rendi(<PerStrike />, '/strike?giorno=2026-10-30&prezzi=SPX');
    await screen.findByRole('region', { name: 'Esposizione per strike' });
    for (const t of ['GEX', 'DEX', 'VEX', 'Netto', 'Call e put', '0DTE']) expect(screen.getAllByText(t, { exact: true }).length).toBeGreaterThan(0);
  });
});

describe('X-Ray · Heat map', () => {
  it('calcolaHeat: la linea del prezzo sta sopra il primo strike sotto il prezzo; lo strike più vicino; i marcatori', () => {
    const c = calcolaHeat(h);
    const r = h.riferimento.prezzo;
    expect(h.strike[c.rigaPrezzo]).toBeLessThanOrEqual(r);
    expect(h.strike[c.rigaPrezzo - 1]).toBeGreaterThan(r);
    expect(h.strike[c.vicino]).toBe(7585);
    // il flip 7.560,11 cade sulla riga 7.560
    expect(c.marche[h.strike.indexOf(7560)]).toContain('flip');
    expect(c.massimo).toBeGreaterThan(0);
    expect(c.minimoValore).toBeLessThan(0);
  });

  it('i nomi brevi delle colonne', () => {
    expect(nomeColonna('0dte', 0)).toBe('0DTE');
    expect(nomeColonna('next_expiry', 3)).toBe('Pross. 3 g');
    expect(nomeColonna('opex', 21)).toBe('Opex 21 g');
    expect(nomeColonna('totale', null)).toBe('Totale');
  });

  it('una sola linea del prezzo, fra due righe, e la netta in testa a ogni colonna', async () => {
    serverFinto({ '/api/heatmap': heatmap });
    const { container } = rendi(<HeatMap />, '/heatmap?giorno=2026-10-30&prezzi=SPX');
    await screen.findByRole('region', { name: 'Esposizione per strike e per scadenza' });
    await screen.findByText('0DTE', { selector: '.heat-colonna' });
    expect(container.querySelectorAll('.heat tr.prezzo')).toHaveLength(1);
    expect(container.querySelector('.heat tr.prezzo td:last-child')?.getAttribute('data-prezzo')).toBe(`${num(h.riferimento.prezzo, 2)} prezzo`);
    expect(container.textContent).not.toContain('◀ prezzo');
    expect(container.querySelectorAll('.heat thead th')).toHaveLength(1 + h.colonne.length);
    expect(container.querySelector('.heat thead')).toHaveTextContent('−2,1');
    expect(container.querySelectorAll('.heat td.nodo').length).toBeGreaterThan(0);
    // la colonna dello strike porta la distanza
    expect(container.querySelector('.heat td.strike.vicino')).toHaveTextContent('7.585· ↑1');
    expect(screen.getByText('Minimo')).toBeInTheDocument();
  });

  it('col DEX 6 colonne: lo strike, 4 gruppi e il totale', async () => {
    const dex: RispostaHeatmap = {
      ...h,
      metrica: 'dex',
      colonne: ['firm', 'broker', 'procust', 'nonprocust', 'totale'].map((k) => ({ chiave: k as RispostaHeatmap['colonne'][number]['chiave'], istante: k === 'totale' ? null : h.ts, dte: null, scade: null })),
      valori: h.valori.map((r) => [r[0] ?? null, r[1] ?? null, r[2] ?? null, r[0] ?? null, r[3] ?? null]),
      nette: [1, -2, 3, -4, -2],
    };
    serverFinto({ '/api/heatmap': dex });
    const { container } = rendi(<HeatMap />, '/heatmap?giorno=2026-10-30&prezzi=SPX');
    await screen.findByText('Retail', { selector: '.heat-colonna' });
    expect(container.querySelectorAll('.heat thead th')).toHaveLength(6);
    expect(container.querySelectorAll('.heat tr.prezzo')).toHaveLength(1);
  });
});
