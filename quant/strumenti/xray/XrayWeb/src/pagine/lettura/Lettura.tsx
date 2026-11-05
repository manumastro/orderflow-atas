// Lettura: il punto della situazione scritto dall'agente quando l'utente lo chiede (2 novembre 2026,
// "una pagina nel nostro x ray con la lettura che posso far partire quando te lo dico io"). La pagina non
// calcola niente: mostra la lettura com'e' (/api/lettura), nella forma lineare di
// quant/metodo/come-si-leggono-le-opzioni-dal-vivo.md, e ogni riga porta alla scheda dove si vede il dato.
//
// Dal 5 novembre 2026 (C.9 del progetto): la data sempre scritta, con l'eta' in ore e giorni; le tessere
// delle righe che hanno un'etichetta; i prezzi come una scala, dall'alto in basso, con la riga del prezzo
// di adesso al suo posto (prezzoAdesso dal server, da/a dalla lettura). Una lettura vecchia si attenua.

import { Fragment, useEffect, useState, type ReactNode } from 'react';
import { Link, useLocation } from 'react-router';
import { IconArrowRight, IconClockExclamation } from '@tabler/icons-react';
import { useLettura } from '../../api/domande';
import type { DoveLettura, Lettura as TipoLettura, PrezzoLettura, RispostaLettura } from '../../api/tipi';
import { distanza, etaLunga, etichettaGiorno, giornoNY, num, oraDoppiaCorta } from '../../formato';
import { Scheda } from '../../componenti/Scheda';
import { Tessera } from '../../componenti/Tessera';
import { Esito, Errore, Vuoto } from '../../componenti/Esito';
import { GRUPPI, paginaDi } from '../rotte';
import './lettura.css';

/** oltre questi minuti la lettura si dichiara vecchia */
const VECCHIA_MIN = 20;

export function Lettura() {
  const [giorno, setGiorno] = useState<string | null>(null);
  const q = useLettura(giorno);
  return (
    <Esito q={q} rotta="lettura">
      {(d) => <Letture d={d} giorno={giorno} scegliGiorno={setGiorno} />}
    </Esito>
  );
}

function Letture({ d, giorno, scegliGiorno }: { d: RispostaLettura; giorno: string | null; scegliGiorno: (g: string | null) => void }) {
  const valide = d.letture.filter((x) => x.lettura);
  const [scelta, setScelta] = useState<string | null>(null);
  // una lettura nuova si mostra da sola, a meno che si stia guardando una precedente
  const ultima = valide.at(-1)?.file ?? null;
  const file = scelta && valide.some((x) => x.file === scelta) ? scelta : ultima;
  const corrente = valide.find((x) => x.file === file)?.lettura;
  const rotte = d.letture.filter((x) => x.errore);

  // il selettore del giorno si vede sempre, anche con un giorno solo
  const scelte = (
    <div className="lettura-scelte" role="group" aria-label="Le letture del giorno">
      <select
        className="lettura-giorno"
        aria-label="Giorno"
        value={giorno ?? d.giorno ?? ''}
        disabled={!d.giorni.length}
        onChange={(e) => {
          setScelta(null);
          scegliGiorno(e.target.value === d.giorni[0] ? null : e.target.value);
        }}
      >
        {d.giorni.length ? (
          d.giorni.map((g) => (
            <option key={g} value={g}>
              {etichettaGiorno(g)}
            </option>
          ))
        ) : (
          <option value="">nessun giorno</option>
        )}
      </select>
      {valide.map((x) => (
        <button
          key={x.file}
          type="button"
          className={x.file === file ? 'lettura-ora attiva' : 'lettura-ora'}
          aria-pressed={x.file === file}
          onClick={() => setScelta(x.file === ultima ? null : x.file)}
        >
          {x.lettura!.oraNY}
        </button>
      ))}
      {valide.length > 0 && <span className="nota">ore di New York</span>}
    </div>
  );

  return (
    <>
      {scelte}
      {rotte.map((x) => (
        <Errore key={x.file} messaggio={`La lettura ${x.file} non si legge: ${x.errore}`} />
      ))}
      {corrente ? (
        <Mostra l={corrente} ultima={file === ultima} adesso={d.prezzoAdesso ?? null} />
      ) : (
        <Vuoto>
          Nessuna lettura {d.giorno ? `il ${etichettaGiorno(d.giorno)}` : 'ancora'}. Chiedila all’agente: “vedi ora”, o “aggiorna la lettura”.
        </Vuoto>
      )}
    </>
  );
}

type Adesso = NonNullable<RispostaLettura['prezzoAdesso']>;

function Mostra({ l, ultima, adesso }: { l: TipoLettura; ultima: boolean; adesso: Adesso | null }) {
  const secondi = useSecondiDa(l.quando);
  const vecchia = secondi > VECCHIA_MIN * 60;
  const etichettate = l.righe.filter((r) => r.etichetta);
  // il prezzo di adesso vale solo per lo stesso strumento della lettura (NQZ6 e NQ sono lo stesso)
  const stesso = adesso && (!l.strumento || adesso.strumento.slice(0, 2).toUpperCase() === l.strumento.slice(0, 2).toUpperCase()) ? adesso : null;

  return (
    <>
      {(vecchia || !ultima) && (
        <div className="lettura-avviso-vecchia" role="status">
          <IconClockExclamation size={16} aria-hidden />
          {!ultima
            ? 'Stai guardando una lettura precedente: la più recente è l’ultimo orario in alto.'
            : `Vecchia: scritta ${etaLunga(secondi)} fa. Chiedi all’agente una lettura nuova (“vedi ora”).`}
        </div>
      )}
      <div className="griglia tessere">
        <Tessera
          etichetta="Lettura del"
          larghezza={etichettate.length ? 4 : 6}
          valore={`${giornoNY(l.quando)} · ${l.oraNY}`}
          dettaglio={
            <>
              New York ({l.oraItalia} Italia) · {etaLunga(secondi)} fa
              {vecchia && (
                <span className="chip-vecchia">
                  <IconClockExclamation size={12} aria-hidden /> VECCHIA
                </span>
              )}
            </>
          }
          larga
        />
        {etichettate.map((r) => (
          <Tessera key={r.voce} etichetta={r.voce} larghezza={etichettate.length > 2 ? 3 : 4} valore={r.etichetta} dettaglio={r.dove ? <Dove dove={r.dove} breve /> : undefined} />
        ))}
      </div>
      <div className={vecchia ? 'griglia lettura-attenuata' : 'griglia'}>
        <Scheda titolo="La lettura" larghezza={12} breve="Descrive, non dà segnali: ogni riga porta al riquadro dove si vede il dato.">
          {(l.strumento || l.prezzo != null) && (
            <p className="nota">
              {l.strumento ?? 'NQ'} {l.prezzo != null ? num(l.prezzo, 2) : ''} quando è stata scritta
              {l.studi ? ` · studi del giorno: ${l.studi}` : ''}
            </p>
          )}
          <p className="lettura-breve">{l.inBreve}</p>
          <ol className="lettura-righe">
            {l.righe.map((r) => (
              <li key={r.voce}>
                <strong>{r.voce}</strong>
                <span>{r.testo}</span>
                {r.dove && <Dove dove={r.dove} />}
              </li>
            ))}
          </ol>
        </Scheda>
        <Scheda
          titolo="I prezzi"
          larghezza={12}
          breve="Dall’alto in basso, con il prezzo di adesso al suo posto: cosa cambia arrivandoci."
          sotto="Nessuno è un supporto o una resistenza provata. La distanza è dal prezzo di adesso al bordo più vicino della zona."
        >
          <Prezzi prezzi={l.prezzi} adesso={stesso} />
        </Scheda>
        <Scheda titolo="Da guardare" larghezza={6}>
          <ul className="lettura-lista">
            {l.daGuardare.map((x) => (
              <li key={x}>{x}</li>
            ))}
          </ul>
        </Scheda>
        <Scheda titolo="Dubbi e parole" larghezza={6}>
          <p className="lettura-dubbi">{l.dubbi}</p>
          <dl className="lettura-parole">
            {l.parole.map((p) => (
              <div key={p.parola}>
                <dt>{p.parola}</dt>
                <dd>{p.significa}</dd>
              </div>
            ))}
          </dl>
        </Scheda>
      </div>
    </>
  );
}

/** Il centro di un prezzo della lettura (da/a), o null se la lettura non li ha. */
const centro = (p: PrezzoLettura): number | null => (p.da != null && p.a != null ? (p.da + p.a) / 2 : (p.da ?? p.a ?? null));

/** La distanza dal prezzo al bordo piu' vicino della zona: "dentro" se ci sta. */
function distanzaZona(p: PrezzoLettura, x: number): string {
  const da = p.da ?? p.a;
  const a = p.a ?? p.da;
  if (da == null || a == null) return '–';
  const lo = Math.min(da, a);
  const hi = Math.max(da, a);
  if (x >= lo && x <= hi) return 'dentro';
  return distanza(x < lo ? lo - x : hi - x, 0);
}

/**
 * I prezzi della lettura come una scala (il modello e' ScalaPrezzi): dall'alto in basso, con la riga del
 * prezzo di adesso al suo posto. Senza da/a (le letture di prima del 5 novembre) resta la tabella di testo,
 * nell'ordine in cui l'agente l'ha scritta. Sul telefono ogni riga diventa una scheda (lettura.css).
 */
function Prezzi({ prezzi, adesso }: { prezzi: PrezzoLettura[]; adesso: Adesso | null }) {
  const numerici = prezzi.every((p) => centro(p) != null);
  const ordinati = numerici ? [...prezzi].sort((x, y) => (centro(y) ?? 0) - (centro(x) ?? 0)) : prezzi;
  const conPrezzo = numerici && adesso != null;
  const dove = conPrezzo ? ordinati.findIndex((p) => (centro(p) ?? 0) < adesso.prezzo) : -1;
  const posto = dove < 0 ? ordinati.length : dove;
  const rigaPrezzo: ReactNode = conPrezzo ? (
    <tr className="scala-prezzo lettura-adesso" key="adesso">
      <td className="lettura-prezzo">{num(adesso.prezzo, 2)}</td>
      <td className="testo" colSpan={3}>
        ◀ prezzo adesso {adesso.strumento} · {oraDoppiaCorta(adesso.quando)}
      </td>
    </tr>
  ) : null;
  return (
    <div className="tabella-scorre lettura-scala">
      <table className="lettura-tabella scala-tabella">
        <thead>
          <tr>
            <th>Prezzo</th>
            <th className="testo">Cos’è</th>
            <th className="testo">Se ci arriva</th>
            {conPrezzo && <th>Dist.</th>}
          </tr>
        </thead>
        <tbody>
          {ordinati.map((p, i) => (
            <Fragment key={`${p.prezzo}-${i}`}>
              {conPrezzo && i === posto && rigaPrezzo}
              <tr>
                <td className="lettura-prezzo">{p.prezzo}</td>
                <td className="testo" data-nome="Cos’è">
                  {p.cosa}
                </td>
                <td className="testo" data-nome="Se ci arriva">
                  {p.seCiArriva}
                </td>
                {conPrezzo && <td className="num lettura-dist">{distanzaZona(p, adesso.prezzo)}</td>}
              </tr>
            </Fragment>
          ))}
          {conPrezzo && posto === ordinati.length && rigaPrezzo}
        </tbody>
      </table>
    </div>
  );
}

/** Il rimando alla scheda dove si vede il dato: un clic apre la pagina e ci scorre sopra. */
function Dove({ dove, breve = false }: { dove: DoveLettura; breve?: boolean }) {
  const { search } = useLocation();
  if (dove.pagina === 'chart') return <span className="lettura-dove">sul chart di ATAS · {dove.scheda}</span>;
  const p = paginaDi(dove.pagina);
  if (!p) return <span className="lettura-dove">{dove.scheda}</span>;
  const gruppo = GRUPPI.find((g) => g.id === p.gruppo)?.titolo;
  const nome = gruppo && gruppo !== p.titolo ? `${gruppo} › ${p.titolo}` : p.titolo;
  return (
    <Link className="lettura-dove" to={{ pathname: `/${p.percorso}`, search }} state={{ scheda: dove.scheda }}>
      <IconArrowRight size={13} aria-hidden /> {breve ? dove.scheda : `${nome} › ${dove.scheda}`}
    </Link>
  );
}

function useSecondiDa(ms: number): number {
  const [adesso, setAdesso] = useState(() => Date.now());
  useEffect(() => {
    const t = setInterval(() => setAdesso(Date.now()), 30_000);
    return () => clearInterval(t);
  }, []);
  return Math.max(0, Math.floor((adesso - ms) / 1000));
}
