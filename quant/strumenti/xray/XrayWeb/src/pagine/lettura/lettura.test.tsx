// Le prove della pagina Lettura (spostate da pagine.test.tsx il 5 novembre 2026, poi estese dalla fase 2:
// la data sempre scritta, le tessere delle righe con l'etichetta, la riga del prezzo di adesso al suo posto).

import { describe, expect, it } from 'vitest';
import { screen, within } from '@testing-library/react';
import { rendi, serverFinto } from '../../test/strumenti';
import { Lettura } from './Lettura';
import lettura from '../../api/esempi/lettura.json';
import { giornoNY } from '../../formato';

const quando = lettura.letture[0]!.lettura!.quando;

describe('Lettura, la lettura dell’agente', () => {
  it('mostra in breve, le cinque righe con il rimando alla scheda, i prezzi e il file rotto come errore', async () => {
    serverFinto({ '/api/lettura': lettura });
    rendi(<Lettura />, '/lettura');
    expect(await screen.findByText(/fermo dentro il valore di oggi/)).toBeInTheDocument();
    const testo = screen.getByRole('region', { name: 'La lettura' });
    for (const voce of ['Cosa è cambiato', 'Il prezzo', 'La paura', 'I dealer', 'ES']) expect(within(testo).getByText(voce)).toBeInTheDocument();
    expect(screen.getByRole('link', { name: /NQ · QQQ › Le fonti a confronto/ })).toBeInTheDocument();
    expect(screen.getByText('sul chart di ATAS · pannello')).toBeInTheDocument();
    expect(screen.getByText('28.960-29.030')).toBeInTheDocument();
    expect(screen.getByRole('alert')).toHaveTextContent('1110.json');
  });

  it('la data e’ sempre scritta, la lettura vecchia si dichiara, il giorno si sceglie anche se e’ uno solo', async () => {
    serverFinto({ '/api/lettura': lettura });
    rendi(<Lettura />, '/lettura');
    const del = await screen.findByRole('region', { name: 'Lettura del' });
    expect(del).toHaveTextContent(giornoNY(quando));
    expect(del).toHaveTextContent('11:08');
    expect(del).toHaveTextContent('17:08 Italia');
    expect(del).toHaveTextContent('VECCHIA');
    expect(del).not.toHaveTextContent(/\d{3,} min fa/);
    expect(screen.getByRole('status')).toHaveTextContent('Vecchia');
    expect(screen.getByRole('combobox', { name: 'Giorno' })).toBeInTheDocument();
  });

  it('una tessera per ogni riga con l’etichetta', async () => {
    serverFinto({ '/api/lettura': lettura });
    rendi(<Lettura />, '/lettura');
    expect(await screen.findByRole('region', { name: 'I dealer' })).toHaveTextContent('NQ: acceleratore');
    expect(screen.getByRole('region', { name: 'ES' })).toHaveTextContent('ES: acceleratore');
    expect(screen.getByRole('region', { name: 'La paura' })).toHaveTextContent('Paura: VXN 27, stabile');
    // le righe senza etichetta non hanno tessera
    expect(screen.queryByRole('region', { name: 'Cosa è cambiato' })).toBeNull();
  });

  it('la riga del prezzo di adesso sta al suo posto, fra i prezzi sopra e quelli sotto', async () => {
    const dentro = { ...lettura, prezzoAdesso: { ...lettura.prezzoAdesso, prezzo: 28700 } };
    serverFinto({ '/api/lettura': dentro });
    rendi(<Lettura />, '/lettura');
    const prezzi = await screen.findByRole('region', { name: 'I prezzi' });
    const righe = [...prezzi.querySelectorAll('tbody tr')];
    const i = righe.findIndex((r) => r.classList.contains('lettura-adesso'));
    expect(i).toBeGreaterThan(0);
    expect(righe[i]).toHaveTextContent('prezzo adesso NQZ6');
    expect(righe[i - 1]).toHaveTextContent('28.778-28.788');
    expect(righe[i + 1]).toHaveTextContent('28.665-28.685');
    // la distanza dal bordo piu' vicino della zona
    expect(righe[i + 1]).toHaveTextContent('↓15');
  });

  it('il prezzo sotto tutti i livelli: la riga in fondo', async () => {
    serverFinto({ '/api/lettura': lettura });
    rendi(<Lettura />, '/lettura');
    const prezzi = await screen.findByRole('region', { name: 'I prezzi' });
    const righe = [...prezzi.querySelectorAll('tbody tr')];
    expect(righe.at(-1)).toHaveClass('lettura-adesso');
  });

  it('senza letture dice come chiederla', async () => {
    serverFinto({ '/api/lettura': { giorno: null, giorni: [], letture: [] } });
    rendi(<Lettura />, '/lettura');
    expect(await screen.findByText(/Chiedila all’agente/)).toBeInTheDocument();
  });
});
