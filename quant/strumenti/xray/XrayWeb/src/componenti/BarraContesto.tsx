// La barra del contesto (B.1, 40 px): a sinistra le pagine del gruppo come linguette (nav.voci) e la "i"
// della pagina; poi i comandi del tempo; a destra la riserva breve del gruppo. In modalita' passato
// compare la chip "◷ Passato · gio 05/11 · 10:30 NY · 16:30 IT". Sul telefono (36 px) restano
// l'istante con ◀ ▶, "Dal vivo" e il ⚙ dei filtri; le linguette spariscono (restano nel DOM).

import { useEffect, useRef } from 'react';
import { IconHistory } from '@tabler/icons-react';
import { NavLink, useLocation } from 'react-router';
import { giornoNY, oraDoppiaCorta } from '../formato';
import { useFiltri } from '../stato/contesto';
import { PAGINE, gruppoDi, type Pagina } from '../pagine/rotte';
import { Aiuto } from './Aiuto';
import { ChipRiserva } from './ChipRiserva';
import { ComandiTempo } from './ComandiTempo';

const NESSUNO = { giorno: false, istante: false, prezzi: false };

export function ChipPassato() {
  const f = useFiltri();
  if (!f.passato) return null;
  const testo = `Passato · ${f.istante ? `${giornoNY(f.istante)} · ${oraDoppiaCorta(f.istante)}` : (f.giorno ?? '')}`;
  return (
    <span className="chip-passato" role="status" title={testo}>
      <IconHistory size={14} stroke={1.8} aria-hidden />
      {testo}
    </span>
  );
}

export function BarraContesto({ pagina }: { pagina: Pagina | undefined }) {
  const { search, pathname } = useLocation();
  const gruppo = gruppoDi(pagina?.gruppo ?? 'xray');
  const sorelle = PAGINE.filter((p) => p.gruppo === gruppo.id);
  const voci = useRef<HTMLElement>(null);

  // la linguetta attiva si porta in vista da sola (tablet: le voci scorrono di lato)
  useEffect(() => {
    const a = voci.current?.querySelector<HTMLElement>('a.active');
    if (a && typeof a.scrollIntoView === 'function') a.scrollIntoView({ inline: 'nearest', block: 'nearest' });
  }, [pathname]);

  return (
    <div className="barra-contesto">
      <nav className="voci" aria-label="Pagine" ref={voci}>
        {sorelle.map((p) => (
          <NavLink key={p.percorso} to={{ pathname: `/${p.percorso}`, search }} className="voce">
            {p.titolo}
          </NavLink>
        ))}
      </nav>
      {pagina && <Aiuto titolo={pagina.titolo} semplice={pagina.spiega} sotto={gruppo.sotto} className="pagina-aiuto" />}
      <span className="contesto-divisore" aria-hidden />
      <ChipPassato />
      <ComandiTempo filtri={pagina?.filtri ?? NESSUNO} riserva={gruppo.riserva} />
      <span className="barra-spazio" />
      <ChipRiserva breve={gruppo.riservaBreve} intera={gruppo.riserva} />
    </div>
  );
}
