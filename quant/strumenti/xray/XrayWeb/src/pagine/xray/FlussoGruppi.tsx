// Chi ha comprato delta oggi, letto contro lo storico (9 novembre 2026, richiesta dell'utente: "piu' volta al
// prendere una decisione", sulla logica statistica e sui paper). Il numero grezzo dice poco: sulle 35 sedute
// d'archivio il flusso del retail e' quasi lo specchio del prezzo (correlazione -0,85 dentro la seduta). Per questo il
// riquadro, per ogni gruppo, risponde a due domande, che il server calcola (/api/flusso):
// - a quest'ora, quanto e' insolito? il percentile fra le sedute passate alla stessa ora dall'apertura;
// - oltre il prezzo? la sorpresa: il flusso meno quello che il movimento del prezzo farebbe attendere.
// Sotto, quattro piccoli grafici: la linea di oggi dentro la banda delle sedute passate (10-90%).
// Nessuna lettura e' provata come previsione: la ricerca e' quant/paper/ricerche/2026-11-09-flusso-delle-opzioni-per-gruppo.md.

import type { ReactNode } from 'react';
import type { BandaFlusso, FlussoGruppo, Gruppo, RispostaFlusso } from '../../api/tipi';
import { giornoNY, num, oraNY, segno, sig } from '../../formato';
import { C } from '../../tema/colori';
import { Grafico } from '../../componenti/Grafico';
import { BarraInline } from '../../componenti/BarraInline';
import { Vuoto } from '../../componenti/Esito';
import type { Opzioni } from '../../viz/echarts';
import { asseValori, asseZero, croce, griglia, massimoAssoluto, rigaTip, segnoFlusso, testaTip, tooltip } from '../../viz/grafici';

export type Ambito = '0dte' | 'all';

/** "gio 05/11" da AAAA-MM-GG (come giornoSeduta di Adesso, qui per non importare la pagina). */
const giornoSeduta = (g: string | null): string => (g ? giornoNY(Date.parse(`${g}T17:00:00Z`)) : '–');

export const ORDINE_GRUPPI: Gruppo[] = ['nonprocust', 'procust', 'firm', 'broker'];
const NOME: Record<Gruppo, string> = {
  nonprocust: 'Retail',
  procust: 'Clienti professionali',
  firm: 'Firm (proprietari)',
  broker: 'Broker-dealer',
};
const NOME_BREVE: Record<Gruppo, string> = { nonprocust: 'Retail', procust: 'Prof.', firm: 'Firm', broker: 'Broker' };

/** Il colore del verso del flusso: un pallino, mai il testo. */
const pallino = (verso: 'compra' | 'vende' | null) => (
  <span className="segno-punto" style={{ background: verso === 'compra' ? C.compra : verso === 'vende' ? C.vende : C.decoro }} aria-hidden />
);

/** "A quest'ora": dove sta oggi fra le sedute passate alla stessa ora. */
export function fraLeSedute(g: FlussoGruppo, soglie: { bassa: number; alta: number }): { verso: 'compra' | 'vende' | null; testo: string } {
  const p = g.percentile;
  if (p == null) return { verso: null, testo: g.oggi == null ? '–' : 'storico troppo corto' };
  if (p < soglie.bassa) return { verso: 'vende', testo: `più venditore del solito: sotto il ${num(100 - p, 0)}% delle sedute` };
  if (p > soglie.alta) return { verso: 'compra', testo: `più compratore del solito: sopra il ${num(p, 0)}% delle sedute` };
  return { verso: null, testo: `nella norma: sopra il ${num(p, 0)}% delle sedute` };
}

/** "Oltre il prezzo": il flusso contro quello che il movimento del prezzo dall'apertura farebbe attendere. */
export function oltreIlPrezzo(g: FlussoGruppo, soglie: { bassa: number; alta: number }): { verso: 'compra' | 'vende' | null; testo: string } {
  const p = g.percentileSorpresa;
  if (p == null) return { verso: null, testo: '–' };
  if (p < soglie.bassa) return { verso: 'vende', testo: '▼ ha venduto più di quanto il prezzo faccia attendere' };
  if (p > soglie.alta) return { verso: 'compra', testo: '▲ ha comprato più di quanto il prezzo faccia attendere' };
  return { verso: null, testo: '≈ quanto il prezzo fa attendere' };
}

/** Quanto il prezzo spiega il flusso del gruppo, in parole: "il prezzo ne spiega il 75%, di solito contro". */
export function quantoSpiega(g: FlussoGruppo): string | null {
  if (g.spiegaPrezzo == null) return null;
  const verso = g.versoPrezzo === 'contro' ? 'va contro il prezzo' : 'va con il prezzo';
  return `il prezzo ne spiega il ${num(g.spiegaPrezzo * 100, 0)}% · di solito ${verso}`;
}

export function FlussoGruppi({ d, ambito }: { d: RispostaFlusso; ambito: Ambito }) {
  const righe = ORDINE_GRUPPI.map((g) => d.gruppi.find((x) => x.gruppo === g && x.ambito === ambito)).filter(
    (x): x is FlussoGruppo => x != null,
  );
  if (!d.cassa) return <Vuoto>La cassa non è ancora aperta: il confronto parte dalle 09:30 di New York.</Vuoto>;
  if (!righe.some((r) => r.oggi != null)) return <Vuoto>Nessun dato del delta per gruppo a quest’ora.</Vuoto>;
  const massimo = massimoAssoluto(righe.map((r) => r.oggi));
  const retail = righe.find((r) => r.gruppo === 'nonprocust');
  // le sedute con il dato di questo ambito (gli 0DTE hanno meno storico di tutte le scadenze)
  const confronti = Math.max(0, ...righe.map((r) => r.confronti));
  return (
    <div className="flusso">
      <p className="flusso-contesto">
        Dalle 09:30: prezzo {d.prezzoDallApertura == null ? '–' : `${segno(d.prezzoDallApertura, 0)} punti di SPX`} · confronto con{' '}
        {confronti} sedute (fino a {giornoSeduta(d.al)}) alle {oraNY(d.ts)} di New York
        {retail?.spiegaPrezzo != null && (
          <>
            {' '}
            · <b>il prezzo spiega il {num(retail.spiegaPrezzo * 100, 0)}% del flusso del retail</b>: conta la colonna “Oltre il prezzo”
          </>
        )}
      </p>
      <div className="tabella-scorre tabella-flusso">
        <table>
          <thead>
            <tr>
              <th>Gruppo</th>
              <th>Oggi, dalle 09:30</th>
              <th>A quest’ora, contro le altre sedute</th>
              <th>Oltre il prezzo</th>
              <th data-priorita="2">Ultimi 30 min</th>
            </tr>
          </thead>
          <tbody>
            {righe.map((r) => {
              const fra = fraLeSedute(r, d.soglie);
              const oltre = oltreIlPrezzo(r, d.soglie);
              const spiega = quantoSpiega(r);
              return (
                <tr key={r.gruppo}>
                  <td className="nome-gruppo">{NOME[r.gruppo]}</td>
                  <td>
                    <BarraInline valore={r.oggi} massimo={massimo} scala={segnoFlusso} />
                    {r.sbilancio != null && <span className="flusso-sotto">sbilancio {segno(r.sbilancio, 2)}</span>}
                  </td>
                  <td>
                    <span className="flusso-lettura">
                      {pallino(fra.verso)}
                      {fra.testo}
                    </span>
                  </td>
                  <td>
                    <span className="flusso-lettura">
                      {pallino(oltre.verso)}
                      {oltre.testo}
                    </span>
                    {spiega && <span className="flusso-sotto">{spiega}</span>}
                  </td>
                  <td className="trenta" data-priorita="2">
                    {r.trentaMinuti == null ? '–' : `${r.trentaMinuti >= 0 ? '▲' : '▼'} ${sig(r.trentaMinuti, 2)}`}
                  </td>
                </tr>
              );
            })}
          </tbody>
        </table>
      </div>
      <div className="flusso-bande">
        {righe.map((r) => (
          <Banda key={r.gruppo} gruppo={r.gruppo} b={d.bande[`${r.gruppo}_${ambito}`]} apertura={d.apertura} />
        ))}
      </div>
      <p className="flusso-riserva">
        Nessun gruppo è provato come previsione del prezzo: né nei paper (su SPX nessun gruppo anticipa l’indice) né sulle nostre
        sedute. Soglie 20% e 80%: una convenzione, non provata.
      </p>
    </div>
  );
}

const ALTEZZA_BANDA = { min: 130, max: 170, schermo: 0.16, maxVerticale: 230 };

function Banda({ gruppo, b, apertura }: { gruppo: Gruppo; b: BandaFlusso | undefined; apertura: number }): ReactNode {
  const ora = (m: number) => oraNY(apertura + m * 60_000);
  const opzioni = (larghezza: number): Opzioni | null => {
    if (!b || !b.minuti.length) return null;
    const punti = (s: (number | null)[]) => b.minuti.map((m, i) => [m, s[i]] as [number, number | null]);
    const ampiezza = b.minuti.map((m, i) => [m, b.p90[i] != null && b.p10[i] != null ? b.p90[i]! - b.p10[i]! : null] as [number, number | null]);
    return {
      animation: false,
      grid: griglia(larghezza, { destra: 'niente', sopra: 8, sotto: 4 }),
      tooltip: tooltip({
        trigger: 'axis',
        axisPointer: croce('x', { x: (v) => ora(v) }),
        formatter: (p: unknown) => {
          const i = Array.isArray(p) && p.length ? (p[0] as { dataIndex: number }).dataIndex : -1;
          if (i < 0) return '';
          return (
            testaTip(`${NOME[gruppo]} · ${ora(b.minuti[i]!)} NY`) +
            rigaTip(C.testo, 'oggi', b.oggi[i] == null ? '–' : num(b.oggi[i], 0)) +
            rigaTip(C.spento, 'metà delle sedute', b.p50[i] == null ? '–' : num(b.p50[i], 0)) +
            rigaTip(null, 'banda 10-90%', b.p10[i] == null ? '–' : `${num(b.p10[i], 0)} … ${num(b.p90[i], 0)}`)
          );
        },
      }),
      xAxis: asseValori({
        min: 0,
        max: 390,
        interval: 60,
        axisLabel: { color: C.spento, fontSize: 10, formatter: (v: number) => ora(v) },
        splitLine: { show: false },
      }),
      yAxis: asseValori({ axisLabel: { color: C.spento, fontSize: 10, formatter: (v: number) => num(v, 0) } }),
      series: [
        { type: 'line', name: 'p10', data: punti(b.p10), stack: 'banda', stackStrategy: 'all', symbol: 'none', lineStyle: { opacity: 0 }, silent: true },
        {
          type: 'line',
          name: 'banda',
          data: ampiezza,
          stack: 'banda',
          stackStrategy: 'all',
          symbol: 'none',
          lineStyle: { opacity: 0 },
          areaStyle: { color: 'rgba(180,186,198,0.16)' },
          silent: true,
        },
        { type: 'line', name: 'metà delle sedute', data: punti(b.p50), symbol: 'none', lineStyle: { color: C.spento, width: 1, type: 'dashed' } },
        {
          type: 'line',
          name: 'oggi',
          data: punti(b.oggi),
          symbol: 'none',
          connectNulls: false,
          lineStyle: { color: C.testo, width: 2 },
          markLine: asseZero('y'),
        },
      ],
    } as Opzioni;
  };
  return (
    <div className="flusso-banda">
      <h3>{NOME_BREVE[gruppo]}</h3>
      <Grafico opzioni={opzioni} altezza={ALTEZZA_BANDA} etichetta={`Il flusso di oggi del gruppo ${NOME[gruppo]} dentro la banda delle sedute passate`} />
    </div>
  );
}
