// Fonti, la diagnosi (C.12): c'e' qualcosa di fermo, e quali riquadri ne soffrono? In cima le tessere
// (quante fonti sono aggiornate, cosa guardare, il lettore di X-Ray e il bridge di ATAS); sotto, lo stato
// di ogni fonte in una tabella raggruppata (una riga di testa per fonte, con lo stato peggiore che
// calcola il server, e i suoi flussi con "dove si vede"); poi il lettore e le famiglie di X-Ray.
// Lo stato e' sempre icona + parola, mai il colore da solo.

import { Fragment, type ReactNode } from 'react';
import { Link } from 'react-router';
import { IconAlertTriangle, IconCircleCheck, IconCircleDashed, IconClockExclamation, IconHourglass } from '@tabler/icons-react';
import { useFonti, useStato } from '../../api/domande';
import type { FonteId, RispostaFonti, RispostaStato, StatoFonte } from '../../api/tipi';
import { doppiaOra, eta, etaLunga, num } from '../../formato';
import { C } from '../../tema/colori';
import { FONTI, ORDINE_FONTI } from '../../tema/fonti';
import { Scheda } from '../../componenti/Scheda';
import { Tessera } from '../../componenti/Tessera';
import { Esito } from '../../componenti/Esito';
import { doveSiVede } from './dove';
import './fonti.css';

type Stato = StatoFonte['stato'] | 'non usata';

const STATI: Record<Stato, { testo: string; colore: string; Icona: typeof IconCircleCheck }> = {
  ok: { testo: 'aggiornata', colore: C.buono, Icona: IconCircleCheck },
  vecchio: { testo: 'vecchia', colore: C.attenzione, Icona: IconClockExclamation },
  errore: { testo: 'in errore', colore: C.critico, Icona: IconAlertTriangle },
  assente: { testo: 'assente', colore: C.spento, Icona: IconCircleDashed },
  'in corso': { testo: 'primo giro in corso', colore: C.spento, Icona: IconHourglass },
  'non usata': { testo: 'non usata', colore: C.spento, Icona: IconCircleDashed },
};

/** Lo stato con icona, parola e colore: mai il colore da solo. */
export function StatoBadge({ stato, testo }: { stato: Stato; testo?: string }) {
  const s = STATI[stato] ?? STATI.assente;
  return (
    <span className="stato-badge" data-stato={stato} style={{ color: s.colore }}>
      <s.Icona size={15} aria-hidden />
      <span>{testo ?? s.testo}</span>
    </span>
  );
}

/** La riga "yahoo" di prima (VIX e VXN insieme) nasconde il flusso rotto dietro quello buono: si toglie. */
const flussi = (d: RispostaFonti): StatoFonte[] => d.fonti.filter((x) => x.id !== 'yahoo');

export function Fonti() {
  const qf = useFonti();
  const qs = useStato();
  return (
    <>
      <div className="griglia tessere">
        <TessereFonti d={qf.data} />
        <TessereLettore s={qs.data} />
      </div>
      <div className="griglia">
        <Scheda
          titolo="Lo stato di ogni fonte"
          fonti={ORDINE_FONTI}
          larghezza={12}
          breve="Ogni fonte con il suo stato peggiore, e sotto i suoi flussi: una fonte vecchia vuol dire che i riquadri dove si vede mostrano il passato."
          semplice="Ogni riga è un flusso di dati: quando è arrivato l’ultimo, se è aggiornato, vecchio o in errore, e in quali riquadri della pagina si vede. Lo stato di una fonte è il peggiore dei suoi flussi, e lo calcola il server."
        >
          <Esito q={qf} rotta="fonti">
            {(d) => <TabellaFonti d={d} />}
          </Esito>
        </Scheda>
        <Scheda
          titolo="Il lettore di X-Ray e il server"
          fonti={['xray']}
          larghezza={6}
          breve="Il programma che legge la cache di ATAS e serve questa pagina: se è fermo, X-Ray mostra dati vecchi."
          semplice="Il programma che legge la cache di ATAS e serve questa pagina. Se è fermo, le pagine di X-Ray mostrano dati vecchi."
        >
          <Esito q={qs} rotta="stato">
            {(s) => <Lettore s={s} />}
          </Esito>
        </Scheda>
        <Scheda
          titolo="Le famiglie di X-Ray"
          fonti={['xray']}
          larghezza={6}
          breve="Un indicatore X-Ray per riga: si apre sulle sue famiglie. Uno fermo da molti minuti è un indicatore chiuso su ES."
          semplice="Ogni famiglia arriva solo se il suo indicatore X-Ray è aperto su un chart ES. Una famiglia ferma da molti minuti vuol dire un indicatore chiuso."
          sotto="Età dell'ultimo chunk di ogni famiglia; vecchia oltre 10 minuti. Una famiglia senza dati il cui indicatore manda le sotto-famiglie è “non usata”."
        >
          <Esito q={qs} rotta="stato">
            {(s) => <Famiglie s={s} />}
          </Esito>
        </Scheda>
      </div>
    </>
  );
}

// ------------------------------------------------------------------ le tessere

function TessereFonti({ d }: { d: RispostaFonti | undefined }) {
  const sintesi = d?.sintesi ?? null;
  const ok = sintesi?.filter((s) => s.stato === 'ok') ?? [];
  const male = sintesi?.filter((s) => s.stato !== 'ok') ?? [];
  return (
    <>
      <Tessera
        etichetta="Fonti aggiornate"
        larghezza={3}
        fonte={ok.length ? ok.map((x) => x.fonte) : ORDINE_FONTI}
        valore={sintesi ? `${ok.length} su ${sintesi.length}` : '–'}
        dettaglio={sintesi ? (male.length === 0 ? 'tutte aggiornate' : male.length === 1 ? 'una è da guardare' : `${male.length} sono da guardare`) : 'la sintesi del server non è arrivata'}
      />
      <Tessera
        etichetta="Da guardare"
        larghezza={3}
        fonte={male.length ? male.map((s) => s.fonte) : ORDINE_FONTI}
        valore={sintesi ? (male.length ? `${male.length} ${male.length === 1 ? 'fonte' : 'fonti'}` : 'niente') : '–'}
        dettaglio={male.length ? male.map((s) => `${FONTI[s.fonte]?.breve ?? s.fonte}: ${s.testo}`).join(' · ') : 'nessun dato vecchio o assente'}
      />
    </>
  );
}

function TessereLettore({ s }: { s: RispostaStato | undefined }) {
  const l = s?.lettore;
  return (
    <>
      <Tessera
        etichetta="Lettore di X-Ray"
        larghezza={3}
        fonte="xray"
        valore={!l ? '–' : !l.presente ? 'mai partito' : l.vivo ? 'vivo' : 'fermo'}
        dettaglio={l?.ultimoChunkSecondiFa != null ? `ultimo dato ${eta(l.ultimoChunkSecondiFa)} fa` : 'nessun dato letto'}
      />
      <Tessera
        etichetta="Bridge di ATAS"
        larghezza={3}
        fonte="catena"
        valore={!s ? '–' : s.server.bridgeErrore ? 'in errore' : 'raggiungibile'}
        dettaglio={!s ? '–' : (s.server.bridgeErrore ?? s.server.bridge)}
      />
    </>
  );
}

// ------------------------------------------------------------------ la tabella raggruppata

function TabellaFonti({ d }: { d: RispostaFonti }) {
  const righe = flussi(d);
  return (
    <div className="tabella-scorre">
      <table className="tabella-fonti">
        <thead>
          <tr>
            <th className="testo">Flusso</th>
            <th className="testo">Stato</th>
            <th>Età</th>
            <th className="testo">Dove si vede</th>
            <th className="testo" data-priorita="2">
              Nota
            </th>
          </tr>
        </thead>
        <tbody>
          {ORDINE_FONTI.map((id) => (
            <Gruppo key={id} id={id} d={d} righe={righe.filter((x) => x.fonte === id)} />
          ))}
        </tbody>
      </table>
    </div>
  );
}

function Gruppo({ id, d, righe }: { id: FonteId; d: RispostaFonti; righe: StatoFonte[] }) {
  const f = FONTI[id];
  const s = d.sintesi?.find((x) => x.fonte === id);
  return (
    <Fragment>
      <tr className="fonte-gruppo" data-fonte={id}>
        <td className="testo" colSpan={5}>
          <span className="gruppo-testa">
            <f.Icona size={16} color={f.colore} aria-hidden />
            <strong>{f.nome}</strong>
            {s ? <StatoBadge stato={s.stato} testo={s.testo} /> : <span className="spento">stato –</span>}
            <span className="gruppo-cosa">
              {f.cosa} · ritardo previsto {f.ritardo}
            </span>
          </span>
        </td>
      </tr>
      {righe.length === 0 && (
        <tr>
          <td className="testo spento" colSpan={5}>
            nessun flusso di questa fonte
          </td>
        </tr>
      )}
      {righe.map((x) => (
        <tr key={x.id} data-stato={x.stato}>
          <td className="testo flusso">
            {x.nome} <span className="nota mono">{x.id}</span>
            {(x.nota || x.errore) && (
              <span className="nota-telefono">
                {x.nota}
                {x.errore ? <span className="errore-testo"> · {x.errore}</span> : null}
              </span>
            )}
          </td>
          <td className="testo">
            <StatoBadge stato={x.stato} />
          </td>
          <td title={x.ultimo ? doppiaOra(x.ultimo) : undefined}>{x.secondiFa == null ? '–' : etaLunga(x.secondiFa)}</td>
          <td className="testo dove">
            {doveSiVede(x.id).length
              ? doveSiVede(x.id).map((w, i) => (
                  <Fragment key={w.testo}>
                    {i > 0 && <br />}
                    {w.pagina ? <Link to={`/${w.pagina}`}>{w.testo}</Link> : w.testo}
                  </Fragment>
                ))
              : '–'}
          </td>
          <td className="avvolto" data-priorita="2">
            {x.nota}
            {x.errore ? <span className="errore-testo"> · {x.errore}</span> : null}
          </td>
        </tr>
      ))}
    </Fragment>
  );
}

// ------------------------------------------------------------------ il lettore e le famiglie

function Lettore({ s }: { s: RispostaStato }) {
  const l = s.lettore;
  const righe: [string, ReactNode][] = [
    ['Lettore', <StatoBadge key="l" stato={!l.presente ? 'assente' : l.vivo ? 'ok' : 'errore'} testo={!l.presente ? 'mai partito' : l.vivo ? 'vivo' : 'fermo'} />],
    ['Ultimo giro', l.secondiFa == null ? '–' : `${eta(l.secondiFa)} fa`],
    ['Ultimo dato', l.ultimoChunkSecondiFa == null ? '–' : `${eta(l.ultimoChunkSecondiFa)} fa`],
    ['Chunk letti', num(l.chunk, 0)],
    ['Errori', `${num(l.errori, 0)}${l.ultimoErrore ? ` · ${l.ultimoErrore}` : ''}`],
    ['Archivio', `${num(l.archivioMB, 0)} MB · ${num(l.giornate, 0)} giornate`],
    ['Server', s.server.indirizzo],
    ['Avviato', s.server.avviato ? doppiaOra(Date.parse(s.server.avviato)) : '–'],
    ['Bridge di ATAS', s.server.bridgeErrore ? <span className="errore-testo">{s.server.bridgeErrore}</span> : `${s.server.bridge} · ok`],
  ];
  return (
    <div className="tabella-scorre">
      <table>
        <tbody>
          {righe.map(([k, v]) => (
            <tr key={k}>
              <td className="testo nota">{k}</td>
              <td className="testo">{v}</td>
            </tr>
          ))}
        </tbody>
      </table>
    </div>
  );
}

type Famiglia = RispostaStato['famiglie'][number];

const ORDINE_STATO: Record<Stato, number> = { errore: 0, assente: 1, vecchio: 2, 'in corso': 3, ok: 4, 'non usata': 5 };

/**
 * Lo stato di una famiglia: aggiornata entro 10 minuti, poi vecchia. Una famiglia senza dati il cui
 * indicatore manda altre famiglie (forecast, nextday, observed: i dati arrivano nelle sotto-famiglie)
 * e' "non usata", non "assente".
 */
export function statoFamiglia(x: Famiglia, sorelle: readonly Famiglia[]): Stato {
  if (x.minuti == null) return sorelle.some((y) => y !== x && y.minuti != null) ? 'non usata' : 'assente';
  return x.minuti <= 10 ? 'ok' : 'vecchio';
}

function Famiglie({ s }: { s: RispostaStato }) {
  const gruppi = new Map<string, Famiglia[]>();
  for (const x of s.famiglie) gruppi.set(x.indicatore, [...(gruppi.get(x.indicatore) ?? []), x]);
  return (
    <div className="famiglie">
      {[...gruppi.entries()].map(([indicatore, fam]) => {
        const stati = fam.map((x) => statoFamiglia(x, fam));
        const peggiore = [...stati].sort((a, b) => ORDINE_STATO[a] - ORDINE_STATO[b])[0] ?? 'assente';
        const vecchia = Math.max(...fam.map((x) => x.minuti ?? 0));
        return (
          <details key={indicatore} className="famiglia" data-stato={peggiore}>
            <summary>
              <span className="famiglia-nome">{indicatore}</span>
              <StatoBadge stato={peggiore} />
              <span className="spento">
                {fam.length} {fam.length === 1 ? 'famiglia' : 'famiglie'}
                {vecchia > 0 ? ` · la più vecchia ${eta(vecchia * 60)} fa` : ''}
              </span>
            </summary>
            <table>
              <tbody>
                {fam.map((x, i) => (
                  <tr key={x.nome}>
                    <td className="mono">{x.nome}</td>
                    <td className="testo">
                      <StatoBadge stato={stati[i]!} />
                    </td>
                    <td>{x.minuti == null ? '–' : `${eta(x.minuti * 60)} fa`}</td>
                  </tr>
                ))}
              </tbody>
            </table>
          </details>
        );
      })}
    </div>
  );
}
