// La barra alta (B.1, 48 px): il marchio, il menu dei cinque gruppi (il gruppo X-Ray con il suo
// mega-menu), l'orologio di New York e d'Italia, lo stato delle quattro fonti, Impara | Veloce.
// nav.gruppi ha solo i cinque link (li conta prova_pagina.py): il chevron e' un bottone fuori dagli <a>.
// Sul telefono (44 px) il marchio e' solo l'icona, i gruppi spariscono (restano nel DOM) e "Adesso ▾"
// apre tutte le pagine.

import { SegmentedControl } from '@mantine/core';
import { IconRadar } from '@tabler/icons-react';
import { Link, NavLink, useLocation } from 'react-router';
import { GRUPPI, MARCHIO, PAGINE, type Pagina } from '../pagine/rotte';
import { useSpiegazioni, type Spiegazioni } from '../stato/spiegazioni';
import { MenuGruppo, PagineTelefono } from './MenuPagine';
import { Orologio } from './Orologio';
import { SaluteFonti } from './SaluteFonti';

export function SceltaSpiegazioni({ className = 'spiegazioni' }: { className?: string }) {
  const [modo, setModo] = useSpiegazioni();
  return (
    <SegmentedControl<Spiegazioni>
      size="xs"
      color="accento"
      className={className}
      aria-label="Spiegazioni"
      value={modo}
      onChange={setModo}
      data={[
        { value: 'impara', label: 'Impara' },
        { value: 'veloce', label: 'Veloce' },
      ]}
    />
  );
}

export function BarraAlta({ pagina }: { pagina: Pagina | undefined }) {
  const { search } = useLocation();
  const gruppo = pagina?.gruppo ?? 'xray';
  return (
    <div className="barra-alta">
      <Link to={{ pathname: '/adesso', search }} className="marchio" aria-label={MARCHIO}>
        <IconRadar size={20} stroke={1.8} aria-hidden />
        <span className="marchio-nome">{MARCHIO}</span>
      </Link>
      <PagineTelefono pagina={pagina} />
      <nav className="gruppi" aria-label="Gruppi">
        {GRUPPI.map((g) => {
          const prima = PAGINE.find((p) => p.gruppo === g.id);
          if (!prima) return null;
          const attivo = g.id === gruppo;
          return (
            <span key={g.id} className={attivo ? 'gruppo-voce attiva' : 'gruppo-voce'}>
              <NavLink
                to={{ pathname: `/${prima.percorso}`, search }}
                className={attivo ? 'gruppo attivo' : 'gruppo'}
                aria-current={attivo ? 'true' : undefined}
                title={g.sotto}
              >
                {g.titolo}
              </NavLink>
              {PAGINE.filter((p) => p.gruppo === g.id).length > 1 && <MenuGruppo gruppo={g} />}
            </span>
          );
        })}
      </nav>
      <span className="barra-spazio" />
      <Orologio />
      <SaluteFonti />
      <SceltaSpiegazioni />
    </div>
  );
}
