// La scala dei prezzi (A.7): "la colonna dei livelli chiave" delle piattaforme, il modello di tutte le
// tabelle di livelli (Adesso, Livelli nel tempo, Previsione, NQ, Lettura).
//
//   Prezzo   Cos'e'                                   Dist.   Quando
//   7.855    ● freno piu' forte                       ↑124    oggi
//   7.762    ● flip + ● muro freno 0DTE (zona ...)    ↑31     oggi
//   ━ 7.731,25  PREZZO ━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━
//   7.715    ┄ VAL cassa 04/11                        ↓16     profilo
//
// Le righe arrivano dal server gia' ordinate e fuse in zone (E.3); la riga del prezzo la inserisce il
// componente al suo posto: e' disegno, non calcolo. `prezzo` e le righe sono nella stessa unita' (quella
// del server); `converti` li porta entrambi nell'unita' mostrata (SPX -> ES).

import { Fragment } from 'react';
import { distanza as scriviDistanza, eta, giornoCorto, num } from '../formato';
import { useStatoFonte } from '../stato/fonti';
import { C } from '../tema/colori';
import { FonteBadge } from './FonteBadge';
import type { FonteId, Forse, RigaScala, TipoLivello, VoceScala } from '../api/tipi';

interface Props {
  righe: readonly RigaScala[];
  /** il prezzo di riferimento, nell'unita' delle righe */
  prezzo: number;
  /** l'unita' mostrata ("ES", "SPX", "NQ") */
  unita: string;
  converti?: (x: number) => number;
  /** la colonna "Quando" (oggi, 1 g, prossima seduta) */
  mostraQuando?: boolean;
  /** in NQ: il badge della fonte e la sua eta' per riga */
  mostraFonte?: boolean;
  /** sul telefono: tre colonne (prezzo, cos'e', distanza) */
  compatta?: boolean;
  /** le cifre dei livelli (ES e NQ 0; D12) */
  cifre?: number;
}

/** Il colore del ruolo di un livello (A.2). */
export function coloreLivello(tipo: TipoLivello): string {
  switch (tipo) {
    case 'freno':
    case 'muro-freno':
    case 'call-wall':
      return C.freno;
    case 'acceleratore':
    case 'muro-acceleratore':
    case 'put-wall':
      return C.acceleratore;
    case 'bordo':
      return C.bordoValore;
    default:
      return C.flip;
  }
}

/** "oggi", "pross.", "opex", "profilo", "prev. 06/11". */
export function quandoDi(r: RigaScala): string {
  if (r.quando === 'prossima seduta') return r.seduta ? `prev. ${giornoCorto(r.seduta)}` : 'prev.';
  const v = r.voci[0];
  if (!v) return '–';
  if (v.fonte === 'atas') return 'profilo';
  switch (v.scadenza) {
    case '0dte':
    case '0dte-nq':
      return 'oggi';
    case 'next_expiry':
      return 'pross.';
    case 'opex':
      return 'opex';
    case 'tutte':
      return 'tutte';
    default:
      return 'oggi';
  }
}

function Voce({ v, prossima }: { v: VoceScala; prossima: boolean }) {
  const colore = coloreLivello(v.tipo);
  const bordo = v.tipo === 'bordo';
  return (
    <span className="scala-voce" data-tipo={v.tipo}>
      <span
        className={bordo ? 'scala-segno trattino' : prossima ? 'scala-segno anello' : 'scala-segno'}
        style={bordo ? { borderTopColor: colore } : prossima ? { borderColor: colore } : { background: colore }}
        aria-hidden
      />
      {v.cosa}
    </span>
  );
}

/**
 * La fonte di una riga: l'icona (il nome sta nel title) e l'eta' una volta sola. Se la fonte e' vecchia il
 * badge scrive gia' la sua eta', e quella della riga non si ripete; la colonna resta stretta anche in s4.
 */
function FonteRiga({ id, secondiFa }: { id: FonteId; secondiFa: Forse<number> }) {
  const stato = useStatoFonte(id);
  return (
    <span className="scala-fonte">
      <FonteBadge id={id} soloIcona />
      {secondiFa != null && !stato?.vecchia && <span className="scala-eta">{eta(secondiFa)}</span>}
    </span>
  );
}

/**
 * L'indice della prima riga sotto il prezzo: la riga del prezzo va subito prima. Le righe vanno dalla
 * piu' alta alla piu' bassa; le righe della prossima seduta non spostano la riga del prezzo.
 */
export function posizionePrezzo(righe: readonly RigaScala[], prezzo: number): number {
  const i = righe.findIndex((r) => r.prezzo < prezzo);
  return i < 0 ? righe.length : i;
}

export function ScalaPrezzi({ righe, prezzo, unita, converti = (x) => x, mostraQuando = true, mostraFonte = false, compatta = false, cifre = 0 }: Props) {
  const ordinate = [...righe].sort((a, b) => b.prezzo - a.prezzo);
  const dove = posizionePrezzo(ordinate, prezzo);
  const colonne = compatta ? 3 : 3 + (mostraQuando ? 1 : 0) + (mostraFonte ? 1 : 0);
  const rigaPrezzo = (
    <tr className="scala-prezzo" key="prezzo">
      <td className="num">{num(converti(prezzo), 2)}</td>
      <td colSpan={colonne - 1} className="testo">
        prezzo {unita}
      </td>
    </tr>
  );
  return (
    <div className="tabella-scorre scala-prezzi">
      <table className={compatta ? 'scala-tabella compatta' : 'scala-tabella'}>
        <thead>
          <tr>
            <th>Prezzo {unita}</th>
            <th className="testo">Cos’è</th>
            <th>Dist.</th>
            {!compatta && mostraQuando && <th className="testo">Quando</th>}
            {!compatta && mostraFonte && <th className="testo">Fonte</th>}
          </tr>
        </thead>
        <tbody>
          {ordinate.map((r, i) => {
            const prossima = r.quando === 'prossima seduta';
            const fonte = r.voci.find((v) => v.fonte !== 'atas');
            return (
              <Fragment key={`${r.quando}-${r.prezzo}-${i}`}>
                {i === dove && rigaPrezzo}
                <tr className={prossima ? 'altra-seduta' : undefined} data-zona={r.zona ? 'si' : undefined}>
                  <td className="num">{num(converti(r.prezzo), cifre)}</td>
                  <td className="testo cosa">
                    {r.voci.map((v, k) => (
                      <Fragment key={`${v.cosa}-${k}`}>
                        {k > 0 && <span className="scala-piu"> + </span>}
                        <Voce v={v} prossima={prossima} />
                      </Fragment>
                    ))}
                    {r.zona && num(converti(r.da), cifre) !== num(converti(r.a), cifre) && (
                      <span className="scala-zona">
                        {' '}
                        (zona {num(converti(r.da), cifre)}–{num(converti(r.a), cifre)})
                      </span>
                    )}
                  </td>
                  <td className="num">{scriviDistanza(r.prezzo - prezzo, 0)}</td>
                  {!compatta && mostraQuando && <td className="testo quando">{quandoDi(r)}</td>}
                  {!compatta && mostraFonte && (
                    <td className="testo fonte">{fonte && fonte.fonte !== 'atas' && <FonteRiga id={fonte.fonte} secondiFa={fonte.secondiFa} />}</td>
                  )}
                </tr>
              </Fragment>
            );
          })}
          {dove === ordinate.length && rigaPrezzo}
        </tbody>
      </table>
    </div>
  );
}
