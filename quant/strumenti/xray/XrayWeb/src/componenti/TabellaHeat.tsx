// La heat map come tabella (C.5): una riga per strike, una colonna per scadenza (o per gruppo), il valore
// nella cella a 2 cifre significative. In testa a ogni colonna la sua netta; la colonna dello strike e'
// ferma a sinistra e porta distanza e marcatori (◆ flip, ▲ freno piu' forte, ▼ acceleratore piu' forte).
// Il prezzo e' una linea FRA due righe: `tr.prezzo` e' la riga subito sotto, col cartellino in fondo a destra.
// La cella piu' forte di ogni colonna (il "nodo") e' contornata in inchiostro. Le unita' sono di
// OptionsDepth: si leggono il segno e la forma.

import { useEffect, useRef } from 'react';
import type { RispostaHeatmap } from '../api/tipi';
import { auto, distanza, doppiaOra, nome, num, sig } from '../formato';
import { inchiostroSu, massimoAssoluto, passoDi, rgbDi, segnoGamma, coloreSegno, parolaSegno, type ScalaSegno } from '../viz/grafici';

interface Props {
  dati: RispostaHeatmap;
  /** un prezzo di SPX nell'unita' scelta */
  p: (x: number) => number;
  unita: string;
  /** i colori del segno della misura: la gamma per la GEX, il flusso per DEX e VEX */
  scala?: ScalaSegno;
  /** "solo i forti": le celle sotto il 5% del massimo della loro colonna restano vuote */
  soloForti?: boolean;
}

/** Sotto questa quota del massimo della colonna, con "Solo i forti", la cella resta vuota. */
export const SOGLIA_FORTI = 0.05;

export type Marca = 'flip' | 'freno' | 'acceleratore';
export const SEGNO_MARCA: Record<Marca, string> = { flip: '◆', freno: '▲', acceleratore: '▼' };
const PAROLA_MARCA: Record<Marca, string> = { flip: 'flip', freno: 'freno più forte', acceleratore: 'acceleratore più forte' };

/** I conti della tabella, separati per provarli: scala, nodi, riga del prezzo, strike vicino, marcatori. */
export function calcolaHeat(dati: RispostaHeatmap) {
  const n = dati.colonne.length;
  const colonna = (j: number) => dati.valori.map((riga) => riga[j] ?? null);
  // la scala si fa sulle colonne vere, non sul totale (che e' la loro somma e sarebbe sempre il massimo)
  const massimi = dati.colonne.map((_, j) => massimoAssoluto(colonna(j)));
  const massimo = Math.max(0, ...massimi.slice(0, n - 1));
  const nodi = massimi.map((m, j) => (m > 0 ? dati.valori.findIndex((riga) => riga[j] != null && Math.abs(riga[j] ?? 0) === m) : -1));
  const r = dati.riferimento.prezzo;
  // le righe vanno dall'alto in basso: la linea del prezzo sta sopra il primo strike sotto il prezzo
  const rigaPrezzo = dati.strike.findIndex((s) => s <= r);
  // lo strike piu' vicino al prezzo (in grassetto)
  let vicino = -1;
  dati.strike.forEach((s, i) => {
    if (vicino < 0 || Math.abs(s - r) < Math.abs((dati.strike[vicino] ?? s) - r)) vicino = i;
  });
  // il minimo e il massimo delle colonne vere, per la legenda
  let minimoValore: number | null = null;
  let massimoValore: number | null = null;
  for (const riga of dati.valori) {
    riga.slice(0, n - 1).forEach((v) => {
      if (v == null || !Number.isFinite(v)) return;
      if (minimoValore == null || v < minimoValore) minimoValore = v;
      if (massimoValore == null || v > massimoValore) massimoValore = v;
    });
  }
  // i marcatori: ogni livello sulla riga dello strike piu' vicino, se sta entro un passo
  const passo = passoDi(dati.strike);
  const marche: Marca[][] = dati.strike.map(() => []);
  const metti = (prezzo: number | null | undefined, m: Marca) => {
    if (prezzo == null || !dati.strike.length) return;
    let i = 0;
    dati.strike.forEach((s, k) => {
      if (Math.abs(s - prezzo) < Math.abs((dati.strike[i] ?? s) - prezzo)) i = k;
    });
    if (Math.abs((dati.strike[i] ?? Infinity) - prezzo) <= passo) marche[i]?.push(m);
  };
  for (const z of dati.livelli?.flip ?? []) metti(z, 'flip');
  metti(dati.livelli?.frenoPiuForte, 'freno');
  metti(dati.livelli?.accPiuForte, 'acceleratore');
  return { massimi, massimo, nodi, rigaPrezzo, vicino, minimoValore: minimoValore as number | null, massimoValore: massimoValore as number | null, marche };
}

/** Il nome breve di una colonna: "0DTE", "Pross. 3 g", "Opex 21 g", "Totale", "Retail". */
export function nomeColonna(chiave: string, dte: number | null | undefined): string {
  const g = dte != null && dte > 0 ? ` ${dte} g` : '';
  switch (chiave) {
    case '0dte':
      return '0DTE';
    case 'next_expiry':
      return `Pross.${g}`;
    case 'opex':
      return `Opex${g}`;
    case 'totale':
      return 'Totale';
    case 'firm':
      return 'Firm';
    case 'procust':
      return 'Clienti prof.';
    default:
      return nome(chiave);
  }
}

export function TabellaHeat({ dati, p, unita, scala = segnoGamma, soloForti = true }: Props) {
  const scatola = useRef<HTMLDivElement>(null);
  const centrata = useRef(false);
  const { massimi, massimo, nodi, rigaPrezzo, vicino, marche } = calcolaHeat(dati);
  const r = dati.riferimento.prezzo;
  const ultima = dati.colonne.length - 1;

  // al primo disegno la linea del prezzo sta a meta' della tabella
  useEffect(() => {
    const el = scatola.current;
    const riga = el?.querySelector<HTMLTableRowElement>('tr.prezzo');
    if (!el || !riga || centrata.current) return;
    el.scrollTop = riga.offsetTop - el.clientHeight / 2;
    centrata.current = true;
  });

  return (
    <div className="tabella-scorre heat" ref={scatola}>
      <table>
        <thead>
          <tr>
            <th>Strike {unita}</th>
            {dati.colonne.map((c, j) => {
              const netta = dati.nette?.[j] ?? null;
              return (
                <th key={c.chiave} title={c.istante ? `${nome(c.chiave)} · dato delle ${doppiaOra(c.istante)}` : nome(c.chiave)}>
                  <span className="heat-colonna">{nomeColonna(c.chiave, c.dte)}</span>
                  <span className="heat-netta" title={netta == null ? undefined : `netta ${auto(netta)} · ${parolaSegno(netta, scala)}`}>
                    {netta != null && <span className="segno-punto" style={{ background: coloreSegno(netta, scala) }} aria-hidden />}
                    {sig(netta, 2)}
                  </span>
                </th>
              );
            })}
          </tr>
        </thead>
        <tbody>
          {dati.strike.map((s, i) => {
            const qui = marche[i] ?? [];
            return (
              <tr key={s} className={i === rigaPrezzo ? 'prezzo' : undefined}>
                <td
                  className={`strike${i === vicino ? ' vicino' : ''}`}
                  title={qui.length ? qui.map((m) => PAROLA_MARCA[m]).join(', ') : undefined}
                >
                  {num(p(s), 0)}
                  <span className="heat-dist">· {distanza(s - r, 0)}</span>
                  {qui.length > 0 && (
                    <span className="heat-marche" aria-label={qui.map((m) => PAROLA_MARCA[m]).join(', ')}>
                      {qui.map((m) => SEGNO_MARCA[m]).join('')}
                    </span>
                  )}
                </td>
                {(dati.valori[i] ?? []).map((v, j) => {
                  const scalaColonna = j === ultima ? Math.max(massimi[j] ?? 0, massimo) : massimo;
                  const debole = soloForti && v != null && Math.abs(v) < (massimi[j] ?? 0) * SOGLIA_FORTI;
                  const rgb = debole ? null : rgbDi(v, scalaColonna, scala);
                  const chiave = dati.colonne[j]?.chiave ?? String(j);
                  return (
                    <td
                      key={chiave}
                      // il cartellino del prezzo sta in fondo alla linea, a destra dell'ultima colonna (D14)
                      data-prezzo={i === rigaPrezzo && j === ultima ? `${num(p(r), 2)} prezzo` : undefined}
                      className={`cella${nodi[j] === i ? ' nodo' : ''}${debole ? ' debole' : ''}`}
                      style={rgb ? { background: `rgb(${rgb[0]},${rgb[1]},${rgb[2]})`, color: inchiostroSu(rgb) } : undefined}
                      title={`${nome(chiave)} · strike ${num(p(s), 0)}: ${auto(v)}${v != null ? ` · ${parolaSegno(v, scala)}` : ''}`}
                    >
                      {v == null || debole ? '' : sig(v, 2)}
                    </td>
                  );
                })}
              </tr>
            );
          })}
        </tbody>
      </table>
    </div>
  );
}
