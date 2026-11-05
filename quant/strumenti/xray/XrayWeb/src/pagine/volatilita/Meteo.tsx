// Il meteo di un indice (C.10): quanto e' alta la paura, quanto si puo' muovere il future oggi in prezzi,
// se l'assicurazione e' cara, e se le fonti stile VIX dicono la stessa cosa. Tutto calcolato dal server
// (/api/vol/in-breve); qui si scrive. I verdetti sono in inchiostro: i colori dei dati sono riservati.
//
//   VXN 25,4 · normale   [calma|normale|tesa|paura]   ▲ +0,4 da stamattina
//   [Movimento oggi] [Implicita − realizzata] [Breve − mese] [Skew]
//   ✓ Fonti stile VIX: convergono (0,5 punti) · 1 senza dato: ...
//   ▸ Perché   (le frasi del server e le definizioni: aperto in Impara, chiuso in Veloce)

import { useState, type ReactNode } from 'react';
import { Collapse, UnstyledButton } from '@mantine/core';
import { IconArrowsDiff, IconCheck, IconChevronDown, IconMinus } from '@tabler/icons-react';
import type { ConfrontoVol, Indice, VolInBreve } from '../../api/tipi';
import { doppiaOra, num, oraDoppiaCorta, segno } from '../../formato';
import { Scheda } from '../../componenti/Scheda';
import { Termine } from '../../componenti/Termine';
import { useSpiegazioni } from '../../stato/spiegazioni';
import { fonte } from '../../tema/fonti';
import { nomeEsclusa, perche, scriviSenzaDati, scriviVerdetto } from './confronto';

/** Il nome dell'indice nel titolo stabile del riquadro ("Il meteo · Nasdaq 100"). */
export const NOME_METEO: Record<Indice, string> = { NDX: 'Nasdaq 100', SPX: 'S&P 500' };

const STRUTTURA: Record<'inversa' | 'piatta' | 'normale', string> = {
  inversa: 'inversa: si teme il breve',
  piatta: 'piatta',
  normale: 'normale: calma a breve',
};

const IMPLICITA: Record<NonNullable<VolInBreve['implicitaTesto']>, string> = {
  'assicurazione cara': 'cara',
  'in linea': 'in linea',
  'si muove più del prezzato': 'si muove di più',
};

const FASCE = ['calma', 'normale', 'tesa', 'paura'] as const;

/**
 * Dove sta il valore sulla barra delle fasce, da 0 a 1: quattro tratti uguali, il valore interpolato
 * nel suo tratto. I tratti di calma e paura sono larghi quanto quello accanto.
 */
export function posizioneFascia(v: number, soglie: readonly number[]): number {
  const [a = 0, b = a + 1, c = b + 1] = soglie;
  const bordi = [a - (b - a), a, b, c, c + (c - b)];
  for (let i = 0; i < 4; i++) {
    const lo = bordi[i]!;
    const hi = bordi[i + 1]!;
    if (v <= hi || i === 3) return Math.min(1, Math.max(0, (i + Math.min(1, Math.max(0, (v - lo) / (hi - lo || 1)))) / 4));
  }
  return 1;
}

function BarraFasce({ x }: { x: VolInBreve }) {
  if (x.livello == null || x.soglie.length < 3) return null;
  const pos = posizioneFascia(x.livello, x.soglie);
  return (
    <span className="fasce" role="img" aria-label={`fasce del ${x.nomeIndice}: calma sotto ${num(x.soglie[0], 0)}, normale fino a ${num(x.soglie[1], 0)}, tesa fino a ${num(x.soglie[2], 0)}, poi paura`}>
      <span className="fasce-pista">
        {FASCE.map((f) => (
          <span key={f} className="fasce-tratto" data-attiva={x.fascia === f ? 'si' : undefined} title={f} />
        ))}
        <span className="fasce-segno" style={{ left: `${(pos * 100).toFixed(1)}%` }} aria-hidden />
      </span>
      <span className="fasce-soglie" aria-hidden>
        {x.soglie.slice(0, 3).map((s, i) => (
          <span key={s} style={{ left: `${(i + 1) * 25}%` }}>
            {num(s, 0)}
          </span>
        ))}
      </span>
    </span>
  );
}

function Numero({ etichetta, valore, sotto, titolo }: { etichetta: ReactNode; valore: string; sotto: string; titolo?: string }) {
  return (
    <div className="meteo-numero" title={titolo}>
      <span className="meteo-etichetta">{etichetta}</span>
      <span className="meteo-valore num">{valore}</span>
      <span className="meteo-sotto">{sotto}</span>
    </div>
  );
}

function IconaVerdetto({ c }: { c: ConfrontoVol }) {
  if (c.verdetto === 'convergono') return <IconCheck size={15} aria-hidden />;
  if (c.verdetto === 'divergono') return <IconArrowsDiff size={15} aria-hidden />;
  return <IconMinus size={15} aria-hidden />;
}

/** La riga delle fonti: il verdetto del server con icona e parola, e chi non ha un dato recente. */
function RigaFonti({ c }: { c: ConfrontoVol }) {
  const senza = scriviSenzaDati(c);
  const motivo = perche(c);
  const testo = `Fonti stile VIX: ${scriviVerdetto(c)}${motivo ? `, ${motivo}` : ''}${senza ? ` · ${senza}` : ''}`;
  return (
    <p className="meteo-fonti" data-verdetto={c.verdetto} title={testo}>
      <IconaVerdetto c={c} />
      <span>
        <Termine k="stile VIX">Fonti stile VIX</Termine>: <strong>{scriviVerdetto(c)}</strong>
        {motivo ? `, ${motivo}` : ''}
        {senza ? <span className="spento"> · {senza}</span> : null}
      </span>
    </p>
  );
}

/** Le frasi del server, le serie confrontate, le escluse e le definizioni. */
function Perche({ x }: { x: VolInBreve }) {
  const c = x.confronto;
  return (
    <div className="meteo-perche">
      {x.frasi.length ? (
        <ul className="frasi">
          {x.frasi.map((f) => (
            <li key={f}>{f}</li>
          ))}
        </ul>
      ) : (
        <p className="nota">Nessun dato per una lettura.</p>
      )}
      {c && c.serieConfrontate.length > 0 && (
        <p className="nota">
          Confrontate:{' '}
          {c.serieConfrontate.map((s) => `${s.nome} ${num(s.valore, 1)} (${fonte(s.fonte).breve}, ${oraDoppiaCorta(s.quando)})`).join(' · ')}.
          {c.escluse.length > 0 && <> Non confrontate: {c.escluse.map((e) => `${nomeEsclusa(e.id)}, ${e.motivo}`).join('; ')}.</>}
        </p>
      )}
      <p className="nota">
        Fasce del {x.nomeIndice}: calma sotto {num(x.soglie[0], 0)}, normale fino a {num(x.soglie[1], 0)}, tesa fino a {num(x.soglie[2], 0)}, poi
        paura: una convenzione, non provata. Il movimento atteso è una deviazione standard fino alla chiusura delle 16:00 di New York (circa
        due volte su tre il prezzo resta dentro)
        {x.movimentoGiorno != null ? `; sulla giornata intera ±${num(x.movimentoGiorno, 0)}` : ''}
        {x.volOggi != null ? `; vol 0DTE ${num(x.volOggi, 1)}%` : ''}. Breve = scadenze fra 1 e 8 giorni, mese = la più vicina ai 30 giorni
        {x.volBreve != null ? ` (${num(x.volBreve, 1)}% contro ${x.volMese == null ? '–' : `${num(x.volMese, 1)}%`})` : ''}. Skew = put al 5%
        sotto meno call al 5% sopra, a 30 giorni. Implicita − realizzata = il {x.nomeIndice} meno la volatilità vera del future nelle ultime 20
        sedute
        {x.realizzata20 != null ? ` (${num(x.realizzata20, 1)}%` : ''}
        {x.realizzata20 != null && x.realizzata5 != null ? `; ultime 5 sedute ${num(x.realizzata5, 1)}%` : ''}
        {x.realizzata20 != null ? ')' : ''}; soglie ±2 punti, una convenzione. Le fonti stile VIX convergono se stanno entro 1 punto.
      </p>
    </div>
  );
}

export function Meteo({ x }: { x: VolInBreve }) {
  const [modo] = useSpiegazioni();
  const [scelta, setScelta] = useState<boolean | null>(null);
  const aperto = scelta ?? modo === 'impara';
  const variazione = x.livello != null && x.apertura != null ? x.livello - x.apertura : null;
  const freccia = variazione == null ? '' : variazione > 0.05 ? '▲ ' : variazione < -0.05 ? '▼ ' : '';
  return (
    <Scheda
      titolo={`Il meteo · ${NOME_METEO[x.indice]}`}
      fonti={x.fonteLivello === 'catena' ? ['catena'] : [x.fonteLivello, 'catena']}
      larghezza={6}
      className="meteo"
      breve="Quanto costa assicurarsi e quanto si può muovere il future oggi: descrive il mercato, non dice da che parte andrà."
      semplice={
        <p>
          Il {x.nomeIndice} è il prezzo dell’assicurazione sul {x.indice === 'NDX' ? 'Nasdaq 100' : 'S&P 500'}: alto vuol dire paura e
          movimenti ampi. Il <Termine k="movimento atteso">movimento atteso</Termine> è la fascia in prezzi di {x.radice} dove il prezzo
          resta circa due volte su tre fino alla chiusura. Sono prezzi, non previsioni.
        </p>
      }
    >
      <div className="meteo-testa">
        <span className="meteo-livello num">
          {x.nomeIndice} {num(x.livello, 1)}
          <span className="meteo-fascia"> · {x.fascia ?? '–'}</span>
        </span>
        <BarraFasce x={x} />
        <span className="meteo-variazione" title={x.quandoLivello ? doppiaOra(x.quandoLivello) : undefined}>
          {variazione == null ? '–' : `${freccia}${segno(variazione, 1)} da stamattina`}
          {x.quandoLivello ? <span className="spento"> · {oraDoppiaCorta(x.quandoLivello)}</span> : null}
        </span>
      </div>
      <div className="meteo-numeri">
        <Numero
          etichetta={<Termine k="movimento atteso">Movimento oggi</Termine>}
          valore={x.movimentoOggi != null ? `±${num(x.movimentoOggi, 0)}` : '–'}
          sotto={x.fasciaPrezzi ? `${num(x.fasciaPrezzi[0], 0)}–${num(x.fasciaPrezzi[1], 0)} ${x.radice}` : `punti di ${x.radice}`}
        />
        <Numero
          etichetta={<Termine k="implicita − realizzata">Implicita − realizzata</Termine>}
          valore={x.implicitaMenoRealizzata != null ? segno(x.implicitaMenoRealizzata, 1) : '–'}
          sotto={x.implicitaTesto ? IMPLICITA[x.implicitaTesto] : '–'}
          titolo={x.implicitaTesto ?? undefined}
        />
        <Numero etichetta="Breve − mese" valore={x.pendenza != null ? segno(x.pendenza, 1) : '–'} sotto={x.struttura ? STRUTTURA[x.struttura] : 'manca la scadenza a un mese'} />
        <Numero etichetta={<Termine k="skew">Skew</Termine>} valore={x.skew != null ? segno(x.skew, 1) : '–'} sotto={x.skewTesto ?? '–'} />
      </div>
      {x.confronto ? <RigaFonti c={x.confronto} /> : <p className="meteo-fonti spento">Il confronto delle fonti non c’è ancora.</p>}
      <UnstyledButton className="spiega-bottone meteo-apri" onClick={() => setScelta(!aperto)} aria-expanded={aperto}>
        <IconChevronDown size={14} className={aperto ? 'giro aperto' : 'giro'} aria-hidden />
        Perché
      </UnstyledButton>
      <Collapse expanded={aperto} keepMountedMode="display-none">
        <Perche x={x} />
      </Collapse>
    </Scheda>
  );
}
