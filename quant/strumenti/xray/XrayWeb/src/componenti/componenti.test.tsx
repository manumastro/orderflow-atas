// Le prove dei componenti comuni e delle funzioni dei grafici (5 novembre 2026, la pagina in stile
// QuantData): Esito (404 -> "non ancora disponibile"), il badge corto delle fonti con l'eta', la heat
// map, il riquadro in Impara e in Veloce, la tessera col pallino del tono, la scala dei prezzi con la riga
// del prezzo al suo posto, lo zoom che non si azzera, e i conti dei colori (OKLab, inchiostro sopra un
// colore) e delle etichette (distanzia, fusione).

import { describe, expect, it } from 'vitest';
import { render, screen } from '@testing-library/react';
import { MantineProvider } from '@mantine/core';
import type { UseQueryResult } from '@tanstack/react-query';
import type { ReactElement } from 'react';
import { ErroreApi } from '../api/client';
import type { RispostaHeatmap } from '../api/tipi';
import { tema } from '../tema/tema';
import { FONTI } from '../tema/fonti';
import { C, SCURO } from '../tema/colori';
import { rendi, serverFinto } from '../test/strumenti';
import { grafici } from '../test/preparazione';
import { Esito } from './Esito';
import { FonteBadge } from './FonteBadge';
import { Scheda } from './Scheda';
import { Tessera } from './Tessera';
import { ScalaPrezzi, posizionePrezzo } from './ScalaPrezzi';
import { Grafico } from './Grafico';
import { BarraInline } from './BarraInline';
import { TabellaHeat, calcolaHeat } from './TabellaHeat';
import type { RigaScala } from '../api/tipi';
import { num } from '../formato';
import { distanzia, inchiostroSu, lineaPrezzo, lineeOrizzontali, oklab, rgbDi, segnoFlusso } from '../viz/grafici';
import type { Opzioni } from '../viz/echarts';
import heat from '../api/esempi/xray-heatmap.json';

const conMantine = (ui: ReactElement) => render(<MantineProvider theme={tema} forceColorScheme="dark" env="test">{ui}</MantineProvider>);

/** Una domanda finta, con i soli campi che Esito legge. */
function domanda<T>(campi: { data?: T; error?: unknown }): UseQueryResult<T> {
  return { data: campi.data, error: campi.error ?? null } as UseQueryResult<T>;
}

describe('Esito', () => {
  it('un 404 mostra "Fonte non ancora disponibile", non un errore rosso', () => {
    conMantine(<Esito q={domanda<number>({ error: new ErroreApi(404, '/api/nq/gex: 404', 'nq/gex') })} rotta="nq/gex">{() => 'dati'}</Esito>);
    expect(screen.getByText('Fonte non ancora disponibile')).toBeInTheDocument();
    expect(screen.getByText('/api/nq/gex')).toBeInTheDocument();
    expect(screen.queryByRole('alert')).toBeNull();
  });

  it('un altro errore è un avviso', () => {
    conMantine(<Esito q={domanda<number>({ error: new ErroreApi(500, 'guasto', 'adesso') })} rotta="adesso">{() => 'dati'}</Esito>);
    expect(screen.getByRole('alert')).toHaveTextContent('guasto');
  });

  it('con i dati disegna i figli', () => {
    conMantine(<Esito q={domanda({ data: 42 })} rotta="x">{(d) => `valore ${d}`}</Esito>);
    expect(screen.getByText('valore 42')).toBeInTheDocument();
  });
});

const SINTESI_CBOE_VECCHIA = {
  fonti: [],
  sintesi: [
    { fonte: 'xray', stato: 'ok', peggiore: 'xray', secondiFa: 3, flussi: ['xray'], testo: 'aggiornata 3 s' },
    { fonte: 'cboe', stato: 'vecchio', peggiore: 'cboe-qqq', secondiFa: 14 * 3600, flussi: ['cboe-qqq'], testo: 'vecchia 14 h' },
  ],
};

describe('FonteBadge', () => {
  it('è corto: icona e nome breve; il nome intero nel title, il colore della fonte', async () => {
    serverFinto({});
    rendi(
      <>
        <FonteBadge id="xray" />
        <FonteBadge id="yahoo" />
      </>,
    );
    const xray = screen.getByText('X-Ray').closest('[data-fonte]');
    expect(xray).toHaveAttribute('title', expect.stringContaining('X-Ray · OptionsDepth'));
    const yahoo = screen.getByText('Yahoo').closest('[data-fonte]');
    expect(yahoo).toHaveAttribute('data-fonte', 'yahoo');
    expect((yahoo as HTMLElement).style.getPropertyValue('--fonte')).toBe(FONTI.yahoo.colore);
  });

  it('una fonte vecchia allunga il badge con l’età, e il riquadro prende il velo', async () => {
    serverFinto({ '/api/fonti': SINTESI_CBOE_VECCHIA });
    const { container } = rendi(
      <Scheda titolo="Profilo QQQ" fonti={['cboe']}>
        corpo
      </Scheda>,
    );
    expect(await screen.findByText('14 h')).toBeInTheDocument();
    expect(container.querySelector('[data-fonte="cboe"]')).toHaveAttribute('data-vecchia', 'si');
    expect(container.querySelector('section.scheda')).toHaveAttribute('data-vecchia', 'si');
  });

  it('nel passato le età si spengono', async () => {
    serverFinto({ '/api/fonti': SINTESI_CBOE_VECCHIA });
    rendi(<FonteBadge id="cboe" />, '/adesso?giorno=2026-10-30');
    await screen.findByText('Cboe');
    await new Promise((r) => setTimeout(r, 50));
    expect(screen.queryByText('14 h')).toBeNull();
  });

  it('i colori delle fonti sono quattro, diversi, e nessuno è un colore del segno o del flusso', () => {
    const colori = Object.values(FONTI).map((f) => f.colore.toLowerCase());
    expect(new Set(colori).size).toBe(4);
    for (const c of [C.freno, C.acceleratore, C.compra, C.vende, '#199e70', '#9085e9']) expect(colori).not.toContain(c);
    expect(FONTI.catena.colore).toBe('#9256bd');
  });
});

describe('TabellaHeat', () => {
  const dati = heat as RispostaHeatmap;

  it('una riga per strike, una sola riga del prezzo, almeno un nodo', () => {
    const { container } = conMantine(<TabellaHeat dati={dati} p={(x) => x} unita="SPX" />);
    expect(container.querySelectorAll('.heat tbody tr')).toHaveLength(dati.strike.length);
    expect(container.querySelectorAll('.heat tr.prezzo')).toHaveLength(1);
    expect(container.querySelectorAll('.heat td.nodo').length).toBeGreaterThanOrEqual(1);
    // il cartellino del prezzo sta in fondo alla linea, sull'ultima cella della riga
    const cartellino = container.querySelector('tr.prezzo td:last-child')?.getAttribute('data-prezzo');
    expect(cartellino).toMatch(/prezzo$/);
    expect(cartellino).toContain(num(dati.riferimento.prezzo, 2));
    expect(container.querySelectorAll('[data-prezzo]')).toHaveLength(1);
  });

  it('la riga del prezzo è il primo strike sotto il prezzo; la scala non conta il totale', () => {
    const { rigaPrezzo, massimo, massimi } = calcolaHeat(dati);
    expect(dati.strike[rigaPrezzo]).toBeLessThanOrEqual(dati.riferimento.prezzo);
    expect(dati.strike[rigaPrezzo - 1] ?? Infinity).toBeGreaterThan(dati.riferimento.prezzo);
    expect(massimo).toBe(Math.max(...massimi.slice(0, -1)));
  });

  it('in ES gli strike si spostano della base', () => {
    const { container } = conMantine(<TabellaHeat dati={dati} p={(x) => x + 30} unita="ES" />);
    const primo = dati.strike[0] ?? 0;
    expect(container.querySelector('tbody td.strike')).toHaveTextContent(new Intl.NumberFormat('it-IT', { useGrouping: 'always' }).format(primo + 30));
    expect(screen.getByText('Strike ES')).toBeInTheDocument();
  });
});

describe('Scheda: Impara | Veloce', () => {
  const scheda = (
    <Scheda titolo="Prova" fonti={['catena']} breve="Una riga per la persona" semplice="Spiegazione intera" sotto="nota tecnica" legenda={<span>● prezzo (7.731,25)</span>}>
      contenuto
    </Scheda>
  );

  it('in Veloce (il default) si vedono titolo, legenda e riga breve; la spiegazione e la nota stanno nella "i"', () => {
    localStorage.clear();
    serverFinto({});
    const { container } = rendi(scheda);
    expect(screen.getByRole('region', { name: 'Prova' })).toBeInTheDocument();
    expect(container.querySelector('.scheda-breve')).toHaveTextContent('Una riga per la persona');
    expect(container.querySelector('.scheda-legenda')).toHaveTextContent('7.731,25');
    expect(container.querySelector('.spiega')).not.toBeVisible();
    expect(container.querySelector('.sotto')).not.toBeVisible();
    expect(screen.getByRole('button', { name: 'Spiegazione: Prova' })).toBeInTheDocument();
    expect(screen.getByText('Catena')).toBeInTheDocument();
  });

  it('in Impara si vedono anche la spiegazione intera e la nota tecnica', () => {
    localStorage.setItem('xray-spiegazioni', 'impara');
    serverFinto({});
    const { container } = rendi(scheda);
    expect(container.querySelector('.spiega')).toBeVisible();
    expect(container.querySelector('.sotto')).toBeVisible();
    localStorage.clear();
  });

});

describe('Tessera', () => {
  it('il tono è un pallino, non testo: la .valore resta il numero; la fonte è un badge a sola icona', () => {
    serverFinto({});
    const { container } = rendi(<Tessera etichetta="Regime" valore="Acceleratore" tono="acceleratore" fonte="xray" tag="non provato" />);
    const v = container.querySelector('.valore');
    expect(v).toHaveTextContent(/^Acceleratore$/);
    expect(v).toHaveAttribute('data-tono', 'acceleratore');
    expect(container.querySelector('.fonte-badge.solo-icona')).toHaveAttribute('aria-label', expect.stringContaining('X-Ray · OptionsDepth'));
    expect(screen.getByText('non provato')).toBeInTheDocument();
  });
});

describe('ScalaPrezzi', () => {
  const voce = (cosa: string, tipo: RigaScala['voci'][number]['tipo'], fonte: 'xray' | 'atas' = 'xray') => ({
    cosa,
    tipo,
    fonte,
    scadenza: fonte === 'atas' ? null : ('0dte' as const),
    peso: null,
    secondiFa: null,
  });
  const riga = (prezzo: number, voci: RigaScala['voci'], quando: RigaScala['quando'] = 'adesso'): RigaScala => ({
    prezzo,
    da: prezzo,
    a: prezzo,
    voci,
    distanza: 0,
    quando,
    seduta: quando === 'adesso' ? null : '2026-11-06',
    zona: voci.length > 1,
  });
  const righe = [
    riga(7855, [voce('freno più forte', 'freno')]),
    riga(7762, [voce('flip', 'flip'), voce('muro freno 0DTE', 'muro-freno')]),
    riga(7715, [voce('VAL cassa 04/11', 'bordo', 'atas')]),
    riga(7630, [voce('acceleratore più forte', 'acceleratore')]),
  ];

  it('la riga del prezzo sta fra il livello sopra e quello sotto, con la distanza a freccia', () => {
    serverFinto({});
    const { container } = rendi(<ScalaPrezzi righe={righe} prezzo={7731.25} unita="ES" />);
    const tr = [...container.querySelectorAll('tbody tr')];
    const i = tr.findIndex((r) => r.classList.contains('scala-prezzo'));
    expect(i).toBe(2);
    expect(tr[i]).toHaveTextContent('7.731,25');
    expect(tr[1]).toHaveTextContent('flip');
    expect(tr[1]).toHaveTextContent('muro freno 0DTE');
    expect(tr[1]).toHaveTextContent('↑31');
    expect(tr[3]).toHaveTextContent('↓16');
    expect(tr[3]).toHaveTextContent('profilo');
    expect(posizionePrezzo(righe, 9000)).toBe(0);
    expect(posizionePrezzo(righe, 1000)).toBe(4);
  });

  it('le righe di un’altra seduta sono spente e dicono la data; compatta ha tre colonne', () => {
    serverFinto({});
    const { container } = rendi(<ScalaPrezzi righe={[...righe, riga(7770, [voce('flip', 'flip')], 'prossima seduta')]} prezzo={7731.25} unita="ES" />);
    expect(container.querySelector('tr.altra-seduta')).toHaveTextContent('prev. 06/11');
    const { container: c2 } = rendi(<ScalaPrezzi righe={righe} prezzo={7731.25} unita="ES" compatta />);
    expect(c2.querySelectorAll('thead th')).toHaveLength(3);
  });
});

describe('BarraInline', () => {
  it('il numero in inchiostro, la barra dal centro nel colore del segno', () => {
    const { container } = conMantine(<BarraInline valore={-620} massimo={1000} scala={segnoFlusso} />);
    expect(container).toHaveTextContent('−620');
    const b = container.querySelector('.barra-inline-segno') as HTMLElement;
    expect(b).toHaveAttribute('data-verso', 'neg');
    expect(b.style.width).toBe('31%');
  });
});

describe('Grafico', () => {
  it('lo zoom non si azzera: un secondo setOption rimette start ed end', () => {
    serverFinto({});
    const a = { series: [] } as Opzioni;
    const b = { series: [{ type: 'line', data: [] }] } as Opzioni;
    const { rerender } = conMantine(<Grafico opzioni={a} etichetta="prova" />);
    const g = grafici.at(-1)!;
    g.stato = { dataZoom: [{ start: 60, end: 90 }] };
    rerender(
      <MantineProvider theme={tema} forceColorScheme="dark" env="test">
        <Grafico opzioni={b} etichetta="prova" />
      </MantineProvider>,
    );
    expect(g.opzioni).toHaveLength(2);
    expect(g.azioni).toContainEqual({ type: 'dataZoom', dataZoomIndex: 0, start: 60, end: 90 });
  });

  it('le opzioni possono dipendere dalla larghezza; le altezze seguono la finestra', () => {
    const larghezze: number[] = [];
    const { container } = conMantine(
      <Grafico
        opzioni={(l) => {
          larghezze.push(l);
          return { series: [] } as Opzioni;
        }}
        altezza="alto"
        etichetta="prova"
      />,
    );
    expect(larghezze.length).toBeGreaterThan(0);
    // jsdom: finestra 1024 x 768 -> alto = clamp(360, 768 - 330, 640) = 438
    expect((container.querySelector('.grafico') as HTMLElement).style.height).toBe('438px');
  });
});

describe('i conti dei grafici', () => {
  it('rgbDi interpola in OKLab: agli estremi il neutro e il polo, a metà la luminosità media', () => {
    expect(rgbDi(0, 10)).toEqual([0x2b, 0x32, 0x42]);
    expect(rgbDi(10, 10)).toEqual([0x4f, 0xae, 0x38]);
    expect(rgbDi(-10, 10)).toEqual([0xb5, 0x44, 0x3f]);
    const mezzo = rgbDi(2.5, 10)!; // radice: t = 0,5
    const L = oklab(mezzo)[0];
    expect(Math.abs(L - (oklab(C.neutro)[0] + oklab(C.freno)[0]) / 2)).toBeLessThan(0.01);
    // non e' la media in RGB (che darebbe un verde oliva piu' spento)
    expect(mezzo).not.toEqual([61, 112, 61]);
    expect(rgbDi(null, 10)).toBeNull();
    expect(rgbDi(5, 10, segnoFlusso)![2]).toBeGreaterThan(rgbDi(5, 10, segnoFlusso)![0]);
  });

  it('inchiostroSu: scuro sul verde, sul blu, sull’arancio e sul grigio del flip; chiaro sul rosso', () => {
    expect(inchiostroSu(C.freno)).toBe(SCURO);
    expect(inchiostroSu(C.acceleratore)).toBe(C.testo);
    expect(inchiostroSu(C.compra)).toBe(SCURO);
    expect(inchiostroSu(C.vende)).toBe(SCURO);
    expect(inchiostroSu(C.flip)).toBe(SCURO);
    expect(inchiostroSu('rgb(43,50,66)')).toBe(C.testo);
  });

  it('distanzia: nessuna etichetta si perde, fra due restano almeno 14 px, tutte dentro l’altezza', () => {
    const y = [7762, 7760, 7764, 7700, 7731];
    const sposta = distanzia(y, [7600, 7900], 300);
    expect(sposta).toHaveLength(y.length);
    const px = y.map((v, i) => ((7900 - v) / 300) * 300 + sposta[i]!).sort((a, b) => a - b);
    for (let i = 1; i < px.length; i++) expect(px[i]! - px[i - 1]!).toBeGreaterThanOrEqual(14 - 1e-9);
    expect(Math.max(...px)).toBeLessThanOrEqual(300);
    expect(distanzia([7700, 7800], [7600, 7900], 300)).toEqual([0, 0]);
  });

  it('lineeOrizzontali: il name tiene il valore; i vicini si fondono, la distanza ha la freccia', () => {
    const ml = lineeOrizzontali(
      [
        { y: 7731.25, colore: C.prezzo, etichetta: 'prezzo 7.731', tipo: 'prezzo' },
        { y: 7760, colore: C.flip, etichetta: 'flip 7.760', tipo: 'flip', tratto: 'dashed' },
        { y: 7764, colore: C.freno, etichetta: 'muro freno 0dte 7.764', tipo: 'livello' },
        { y: 7630, colore: C.acceleratore, etichetta: 'acceleratore più forte 7.630', tipo: 'livello' },
      ],
      { prezzo: 7731.25, fondi: 6, asse: [7600, 7900], altezzaPx: 400 },
    );
    const dati = ml.data as { name: string; yAxis: number; label: { formatter: string } }[];
    expect(dati).toHaveLength(3);
    const fusa = dati.find((d) => d.name.includes(' + '))!;
    expect(fusa.name).toBe('flip 7.760 + muro freno 0dte 7.764');
    expect(fusa.yAxis).toBe(7762);
    expect(fusa.label.formatter).toBe('FLIP + MURO FRENO 0DTE 7.762 ↑31');
    expect(dati.find((d) => d.name.startsWith('acceleratore'))!.label.formatter).toBe('ACCELERATORE PIÙ FORTE 7.630 ↓101');
    expect(dati.find((d) => d.name.startsWith('prezzo'))!.label.formatter).toBe('PREZZO 7.731');
  });

  it('lineaPrezzo: piena dove il prezzo è vivo, tratteggiata dove è ES − base, mai trasparente', () => {
    const [piena, tratteggiata] = lineaPrezzo([1, 2, 3, 4], [10, 11, 12, 13], [false, false, true, true], 'prezzo ES');
    expect(piena!.name).toBe('prezzo ES (spot vivo)');
    expect((piena!.data as [number, number | null][]).map((d) => d[1])).toEqual([null, null, 12, 13]);
    expect((tratteggiata!.data as [number, number | null][]).map((d) => d[1])).toEqual([10, 11, 12, null]);
    expect((piena!.lineStyle as { opacity?: number }).opacity).toBeUndefined();
    expect((piena!.endLabel as { show: boolean }).show).toBe(true);
  });
});
