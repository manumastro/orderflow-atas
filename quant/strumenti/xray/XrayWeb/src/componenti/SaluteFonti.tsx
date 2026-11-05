// Lo stato delle quattro fonti nella barra alta (B.4): una pastiglia per fonte, con l'icona nel colore
// della fonte, l'icona dello stato (✓ ◷ ⚠) e la parola o l'eta'. Lo stato di una fonte e' il peggiore dei
// suoi flussi, e lo calcola il server (/api/fonti -> sintesi). Il clic apre i flussi di quella fonte e il
// link a Fonti; il lettore di X-Ray e' la prima riga del popover di X-Ray. Sul tablet solo le icone, sul
// telefono una pastiglia sola ("✓ tutte", "◷ 1 vecchia"). Nel passato: "stai guardando il passato".
// Se il server non ha ancora la sintesi, X-Ray mostra lo stato del lettore e le altre "–".

import { Popover, UnstyledButton } from '@mantine/core';
import { IconAlertTriangle, IconCheck, IconClockHour4, IconHistory } from '@tabler/icons-react';
import { Link, useLocation } from 'react-router';
import { useFonti, useStato } from '../api/domande';
import type { FonteId, RispostaStato, SintesiFonte, StatoFonte } from '../api/tipi';
import { eta, etaLunga, num } from '../formato';
import { useFiltri } from '../stato/contesto';
import { sintesiDi } from '../stato/fonti';
import { fonte, ORDINE_FONTI } from '../tema/fonti';

type Esito = 'ok' | 'vecchio' | 'male' | 'ignoto';

const esitoDi = (s: StatoFonte['stato'] | undefined): Esito =>
  s == null ? 'ignoto' : s === 'ok' || s === 'in corso' ? 'ok' : s === 'vecchio' ? 'vecchio' : 'male';

function IconaStato({ esito }: { esito: Esito }) {
  if (esito === 'ok') return <IconCheck size={13} stroke={2.2} className="stato-ok" aria-hidden />;
  if (esito === 'vecchio') return <IconClockHour4 size={13} stroke={2} className="stato-vecchio" aria-hidden />;
  if (esito === 'male') return <IconAlertTriangle size={13} stroke={2} className="stato-male" aria-hidden />;
  return null;
}

/** Il testo dello stato del lettore di X-Ray (lo StatoLettore di prima). */
export function testoLettore(d: RispostaStato | undefined, errore: unknown): { testo: string; esito: Esito; titolo: string } {
  if (errore) return { testo: 'server non raggiungibile', esito: 'male', titolo: '' };
  const l = d?.lettore;
  if (!l) return { testo: 'collegamento…', esito: 'ignoto', titolo: '' };
  const fa = l.ultimoChunkSecondiFa;
  const esito: Esito = !l.vivo ? 'male' : fa != null && fa > 600 ? 'vecchio' : 'ok';
  const testo = !l.presente
    ? 'lettore mai partito'
    : !l.vivo
      ? `lettore FERMO da ${num((l.secondiFa ?? 0) / 60, 0)} min`
      : `lettore vivo · ultimo dato ${eta(fa)} fa`;
  const titolo = `archivio ${num(l.archivioMB, 0)} MB, ${l.errori ?? 0} errori${d?.server.bridgeErrore ? ` · ${d.server.bridgeErrore}` : ''}`;
  return { testo, esito, titolo };
}

function Pastiglia({
  id,
  sintesi,
  flussi,
  lettore,
  passato,
}: {
  id: FonteId;
  sintesi: SintesiFonte | null;
  flussi: StatoFonte[];
  lettore: ReturnType<typeof testoLettore> | null;
  passato: boolean;
}) {
  const { search } = useLocation();
  const f = fonte(id);
  const { Icona } = f;
  const esito: Esito = sintesi ? esitoDi(sintesi.stato) : lettore ? lettore.esito : 'ignoto';
  let parola = '–';
  if (passato) parola = '';
  else if (sintesi) {
    if (sintesi.stato === 'vecchio') parola = etaLunga(sintesi.secondiFa);
    else if ((sintesi.stato === 'ok' || sintesi.stato === 'in corso') && sintesi.secondiFa != null) parola = eta(sintesi.secondiFa);
    else parola = sintesi.testo;
  } else if (lettore) parola = lettore.esito === 'ok' ? 'vivo' : lettore.testo;
  return (
    <Popover width={340} position="bottom-end" shadow="md" withinPortal>
      <Popover.Target>
        <UnstyledButton
          className="pastiglia"
          data-fonte={id}
          data-esito={esito}
          aria-label={`${f.breve}: ${sintesi?.testo ?? lettore?.testo ?? 'stato non disponibile'}`}
        >
          <Icona size={14} stroke={2} color={f.colore} aria-hidden />
          <span className="pastiglia-nome">{f.breve}</span>
          {!passato && <IconaStato esito={esito} />}
          {parola && <span className="pastiglia-testo">{parola}</span>}
        </UnstyledButton>
      </Popover.Target>
      <Popover.Dropdown className="aiuto-pannello">
        <div className="aiuto-testo">
          <strong className="aiuto-titolo">
            {f.nome} <span className="spento">· {f.ritardo}</span>
          </strong>
          <p className="aiuto-sotto">{f.cosa}</p>
          <ul className="flussi">
            {lettore && (
              <li title={lettore.titolo}>
                <IconaStato esito={lettore.esito} /> {lettore.testo}
              </li>
            )}
            {flussi.map((x) => (
              <li key={x.id}>
                <IconaStato esito={esitoDi(x.stato)} /> <b>{x.nome}</b> · {x.stato}
                {x.secondiFa != null ? ` · ${etaLunga(x.secondiFa)}` : ''}
                {x.nota ? <span className="spento"> · {x.nota}</span> : null}
              </li>
            ))}
          </ul>
          <Link to={{ pathname: '/fonti', search }} className="aiuto-link">
            Apri la pagina Fonti
          </Link>
        </div>
      </Popover.Dropdown>
    </Popover>
  );
}

export function SaluteFonti() {
  const f = useFiltri();
  const qFonti = useFonti();
  const qStato = useStato();
  const sintesi = sintesiDi(qFonti.data);
  const flussi = qFonti.data?.fonti ?? [];
  const lettore = testoLettore(qStato.data, qStato.error);
  const righe = ORDINE_FONTI.map((id) => ({ id, s: sintesi?.find((x) => x.fonte === id) ?? null }));
  const brutte = righe.filter((r) => r.s != null && esitoDi(r.s.stato) !== 'ok');
  let riassunto: string;
  let esitoRiassunto: Esito;
  if (!sintesi) {
    riassunto = lettore.esito === 'ok' ? 'X-Ray vivo' : lettore.testo;
    esitoRiassunto = lettore.esito;
  } else if (brutte.length === 0) {
    riassunto = 'tutte';
    esitoRiassunto = 'ok';
  } else {
    riassunto = `${brutte.length} ${brutte.length === 1 ? 'vecchia' : 'vecchie'}`;
    esitoRiassunto = brutte.some((r) => esitoDi(r.s?.stato) === 'male') ? 'male' : 'vecchio';
  }
  return (
    <div className="salute" role="status" aria-live="polite" aria-label="Stato delle fonti">
      {f.passato && (
        <span className="salute-passato">
          <IconHistory size={14} stroke={1.8} aria-hidden /> stai guardando il passato
        </span>
      )}
      <span className="salute-pastiglie">
        {righe.map(({ id, s }) => (
          <Pastiglia
            key={id}
            id={id}
            sintesi={s}
            flussi={flussi.filter((x) => x.fonte === id)}
            lettore={id === 'xray' ? lettore : null}
            passato={f.passato}
          />
        ))}
      </span>
      <span className="salute-una pastiglia" data-esito={esitoRiassunto}>
        <IconaStato esito={esitoRiassunto} />
        <span className="pastiglia-testo">{riassunto}</span>
      </span>
    </div>
  );
}
