// I menu delle pagine. Sul desktop il mega-menu di un gruppo (il chevron accanto a "SPX · X-Ray"): le
// pagine con icona, titolo e la riga breve, in un portale (fuori da nav.gruppi, che ha i soli cinque
// link). Sul telefono il bottone "Adesso ▾" apre un cassetto con tutte le pagine per gruppo.

import { useState } from 'react';
import { Drawer, Menu, UnstyledButton } from '@mantine/core';
import { IconChevronDown } from '@tabler/icons-react';
import { Link, NavLink, useLocation } from 'react-router';
import { GRUPPI, PAGINE, type Gruppo, type Pagina } from '../pagine/rotte';

function VocePagina({ p }: { p: Pagina }) {
  const Icona = p.icona;
  return (
    <span className="menu-pagina">
      {Icona && <Icona size={18} stroke={1.7} aria-hidden className="menu-icona" />}
      <span>
        <span className="menu-titolo">{p.titolo}</span>
        <span className="menu-breve">{p.breve}</span>
      </span>
    </span>
  );
}

/** Il chevron di un gruppo: apre le sue pagine (il mega-menu di QuantData). */
export function MenuGruppo({ gruppo }: { gruppo: Gruppo }) {
  const { search } = useLocation();
  const pagine = PAGINE.filter((p) => p.gruppo === gruppo.id);
  return (
    <Menu position="bottom-start" width={340} shadow="md" withinPortal>
      <Menu.Target>
        <UnstyledButton className="gruppo-chevron" aria-label={`Pagine di ${gruppo.titolo}`}>
          <IconChevronDown size={14} stroke={2} aria-hidden />
        </UnstyledButton>
      </Menu.Target>
      <Menu.Dropdown className="mega-menu">
        <Menu.Label>{gruppo.sotto}</Menu.Label>
        {pagine.map((p) => (
          <Menu.Item key={p.percorso} component={Link} to={{ pathname: `/${p.percorso}`, search }}>
            <VocePagina p={p} />
          </Menu.Item>
        ))}
      </Menu.Dropdown>
    </Menu>
  );
}

/** Sul telefono: "Adesso ▾" apre il cassetto con tutte le pagine per gruppo. */
export function PagineTelefono({ pagina }: { pagina: Pagina | undefined }) {
  const { search } = useLocation();
  const [aperto, setAperto] = useState(false);
  return (
    <>
      <UnstyledButton className="pagine-telefono" onClick={() => setAperto(true)} aria-label="Tutte le pagine" aria-expanded={aperto}>
        <span>{pagina?.titolo ?? 'Pagine'}</span>
        <IconChevronDown size={14} stroke={2} aria-hidden />
      </UnstyledButton>
      {aperto && (
        <Drawer opened onClose={() => setAperto(false)} position="left" size={300} title="Le pagine" classNames={{ content: 'cassetto' }}>
          <div className="cassetto-pagine">
            {GRUPPI.map((g) => (
              <div key={g.id} className="cassetto-gruppo">
                <div className="cassetto-gruppo-titolo">{g.titolo}</div>
                {PAGINE.filter((p) => p.gruppo === g.id).map((p) => (
                  <NavLink
                    key={p.percorso}
                    to={{ pathname: `/${p.percorso}`, search }}
                    className="cassetto-voce"
                    onClick={() => setAperto(false)}
                  >
                    <VocePagina p={p} />
                  </NavLink>
                ))}
              </div>
            ))}
          </div>
        </Drawer>
      )}
    </>
  );
}
