// La barra dentro la cella di una tabella (A.6): divergente dal centro della cella, nel colore del
// segno (gamma) o del flusso; il numero accanto, in inchiostro; la lunghezza proporzionale al massimo
// della colonna. Il segno si legge anche senza colore: dal verso della barra e dal "−" del numero.

import { sig } from '../formato';
import { coloreSegno, parolaSegno, segnoGamma, type ScalaSegno } from '../viz/grafici';

interface Props {
  valore: number | null | undefined;
  /** il massimo assoluto della colonna: la barra piena */
  massimo: number;
  /** i colori del segno: la gamma (default) o il flusso */
  scala?: ScalaSegno;
  /** il numero gia' scritto (default: due cifre significative col segno) */
  testo?: string;
}

export function BarraInline({ valore, massimo, scala = segnoGamma, testo }: Props) {
  const v = valore != null && Number.isFinite(valore) ? valore : null;
  const quota = v == null || !(massimo > 0) ? 0 : Math.min(1, Math.abs(v) / massimo);
  const scritto = testo ?? sig(v);
  return (
    <span className="barra-inline" title={v == null ? 'nessun dato' : `${scritto} · ${parolaSegno(v, scala)}`}>
      <span className="barra-inline-pista" aria-hidden>
        {v != null && quota > 0 && (
          <span
            className="barra-inline-segno"
            data-verso={v >= 0 ? 'pos' : 'neg'}
            style={{ width: `${(quota * 50).toFixed(1)}%`, background: coloreSegno(v, scala) }}
          />
        )}
      </span>
      <span className="barra-inline-numero num">{scritto}</span>
    </span>
  );
}
