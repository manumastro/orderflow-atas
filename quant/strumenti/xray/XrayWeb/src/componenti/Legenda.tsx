// La legenda in HTML sopra il grafico, CON I VALORI (A.4): "● Nome (valore)". Il valore e' quello
// attuale, o quello sotto il cursore mentre il mouse e' sul grafico (Grafico, onCursore). Un valore
// mancante e' "–", mai zero. L'identita' non e' mai solo colore: c'e' sempre la parola. La scala
// divergente porta le parole del segno (freno/acceleratore, o comprato/venduto).

import type { ReactNode } from 'react';
import { num } from '../formato';
import type { FonteId } from '../api/tipi';
import { fonte, type Tratto } from '../tema/fonti';
import { segnoGamma, type ScalaSegno } from '../viz/grafici';

export interface Voce {
  colore: string;
  testo: string;
  forma?: 'linea' | 'rett' | 'punto';
  tratto?: Tratto;
  opacita?: number;
  /** la fonte della serie: davanti al nome compare la sua icona */
  fonte?: FonteId | string;
  /** una serie senza dati: in legenda, spenta, con "nessun dato" */
  vuota?: boolean;
  /** il valore gia' scritto ("7.731,25", "↑29"); null = "–". Assente = la voce non ha valore */
  valore?: string | null;
}

export interface Scala {
  massimo: number;
  positivo?: string;
  negativo?: string;
  /** i colori del segno: la gamma (default) o il flusso */
  segno?: ScalaSegno;
  /** l'unita' della scala ("unità di OptionsDepth, intensità relativa") */
  unita?: string;
}

interface Props {
  voci?: Voce[];
  /** le fonti, ciascuna con icona, nome e tratto */
  fonti?: (FonteId | string)[];
  scala?: Scala;
  children?: ReactNode;
}

export function ScalaDivergente({ massimo, segno = segnoGamma, positivo = segno.parolaPos, negativo = segno.parolaNeg, unita }: Scala) {
  const cifre = massimo >= 10 ? 0 : 2;
  return (
    <span className="chiave scala-chiave">
      <span>
        {negativo} −{num(massimo, cifre)}
      </span>
      <span className="scala" style={{ background: `linear-gradient(90deg, ${segno.neg}, ${segno.neutro}, ${segno.pos})` }} aria-hidden />
      <span>
        +{num(massimo, cifre)} {positivo}
      </span>
      {unita && <span className="scala-unita">{unita}</span>}
    </span>
  );
}

export function Chiave({ colore, testo, forma = 'linea', tratto = 'solid', opacita, fonte: idFonte, vuota = false, valore }: Voce) {
  const stile =
    forma === 'linea'
      ? { borderTop: `2px ${tratto} ${colore}`, opacity: opacita }
      : { background: colore, opacity: opacita };
  const f = idFonte ? fonte(idFonte) : null;
  return (
    <span className={vuota ? 'chiave vuota' : 'chiave'} data-fonte={f?.id}>
      <span className={forma} style={stile} aria-hidden />
      {f && <f.Icona size={13} stroke={2} color={f.colore} aria-hidden />}
      <span>
        {testo}
        {vuota ? ' · nessun dato' : ''}
      </span>
      {valore !== undefined && <span className="chiave-valore">({valore ?? '–'})</span>}
    </span>
  );
}

export function ChiaveFonte({ id }: { id: FonteId | string }) {
  const f = fonte(id);
  const { Icona } = f;
  return (
    <span className="chiave" data-fonte={f.id}>
      <span className="linea" style={{ borderTop: `2px ${f.tratto} ${f.colore}` }} aria-hidden />
      <Icona size={13} stroke={2} color={f.colore} aria-hidden />
      <span>{f.nome}</span>
    </span>
  );
}

export function Legenda({ voci = [], fonti = [], scala, children }: Props) {
  return (
    <div className="legenda">
      {voci.map((v) => (
        <Chiave key={v.testo} {...v} />
      ))}
      {fonti.map((f) => (
        <ChiaveFonte key={f} id={f} />
      ))}
      {scala && <ScalaDivergente {...scala} />}
      {children}
    </div>
  );
}
