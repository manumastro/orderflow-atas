// La barra in basso del telefono (B.3, 56 px, fissa): i cinque gruppi con icona e parola breve. Non ha
// la classe "gruppi" (prova_pagina.py conta i link di nav.gruppi). Le icone non sono quelle delle fonti.

import { NavLink, useLocation } from 'react-router';
import { GRUPPI, PAGINE, type Pagina } from '../pagine/rotte';

export function BarraBasso({ pagina }: { pagina: Pagina | undefined }) {
  const { search } = useLocation();
  const gruppo = pagina?.gruppo ?? 'xray';
  return (
    <nav className="barra-basso" aria-label="Gruppi (telefono)">
      {GRUPPI.map((g) => {
        const prima = PAGINE.find((p) => p.gruppo === g.id);
        if (!prima) return null;
        const Icona = g.icona;
        const attivo = g.id === gruppo;
        return (
          <NavLink
            key={g.id}
            to={{ pathname: `/${prima.percorso}`, search }}
            className={attivo ? 'basso-voce attiva' : 'basso-voce'}
            aria-current={attivo ? 'true' : undefined}
          >
            <Icona size={20} stroke={1.7} aria-hidden />
            <span>{g.breve}</span>
          </NavLink>
        );
      })}
    </nav>
  );
}
