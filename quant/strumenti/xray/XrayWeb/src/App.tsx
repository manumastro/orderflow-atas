// La pagina come un software (5 novembre 2026, in stile QuantData): la testata ferma in alto con due
// barre (la barra alta: marchio, gruppi, orologio, fonti, Impara|Veloce; la barra del contesto: le pagine
// del gruppo, i comandi del tempo, la riserva), la pagina scelta, il piede con la riserva intera del
// gruppo, la barra del tempo fissa in fondo (solo se la pagina usa l'istante) e, sul telefono, la barra
// in basso. Tutto lo stato sta dentro l'<header>: leggi_pagina.py ne legge l'innerText.

import { useEffect } from 'react';
import { Navigate, Route, Routes, useLocation } from 'react-router';
import { useIsFetching } from '@tanstack/react-query';
import { FiltriProvider } from './stato/filtri';
import { useFiltri } from './stato/contesto';
import { useSpiegazioni } from './stato/spiegazioni';
import { BarraAlta } from './componenti/BarraAlta';
import { BarraContesto } from './componenti/BarraContesto';
import { BarraTempo } from './componenti/BarraTempo';
import { BarraBasso } from './componenti/BarraBasso';
import { MARCHIO, PAGINE, REINDIRIZZI, gruppoDi, paginaDi } from './pagine/rotte';

function Corpo() {
  const { pathname, search, state, key } = useLocation();
  const pagina = paginaDi(pathname.replace(/^\//, ''));
  const caricando = useIsFetching({ predicate: (q) => q.queryKey[0] !== 'stato' && q.queryKey[0] !== 'fonti' }) > 0;
  const f = useFiltri();
  const [spiegazioni] = useSpiegazioni();
  const conTempo = pagina?.filtri.istante ?? false;

  // arrivando da un rimando della Lettura, si scorre fino alla scheda nominata e la si evidenzia: la scheda
  // compare quando arrivano i dati, quindi la si cerca per qualche secondo
  useEffect(() => {
    const scheda = (state as { scheda?: string } | null)?.scheda;
    if (!scheda) return;
    let volte = 0;
    const t = setInterval(() => {
      const el = [...document.querySelectorAll<HTMLElement>('#pagina section.scheda')].find((s) => s.getAttribute('aria-label') === scheda);
      if (el) {
        clearInterval(t);
        el.scrollIntoView({ behavior: 'smooth', block: 'start' });
        el.classList.add('evidenziata');
        setTimeout(() => el.classList.remove('evidenziata'), 2500);
      } else if (++volte > 60) clearInterval(t);
    }, 250);
    return () => clearInterval(t);
  }, [key, state]);

  useEffect(() => {
    document.title = pagina ? `${pagina.titolo} · ${MARCHIO}` : MARCHIO;
  }, [pagina]);

  // lo stato della pagina sul body: il CSS spegne le spiegazioni in Veloce, borda la testata nel passato,
  // e lascia lo spazio alla barra del tempo
  useEffect(() => {
    const b = document.body.dataset;
    b.spiegazioni = spiegazioni;
    b.tempo = f.passato ? 'passato' : 'vivo';
    b.barraTempo = conTempo ? 'si' : 'no';
  }, [spiegazioni, f.passato, conTempo]);

  return (
    <>
      <header className="testata" data-passato={f.passato ? 'si' : undefined}>
        <BarraAlta pagina={pagina} />
        <BarraContesto pagina={pagina} />
      </header>
      <main id="pagina" className="pagina" data-carica={caricando ? 'si' : 'no'} tabIndex={-1}>
        <Routes>
          {PAGINE.map(({ percorso, Componente }) => (
            <Route key={percorso} path={`/${percorso}`} element={<Componente />} />
          ))}
          {Object.entries(REINDIRIZZI).map(([da, a]) => (
            <Route key={da} path={`/${da}`} element={<Navigate to={{ pathname: `/${a}`, search }} replace />} />
          ))}
          <Route path="*" element={<Navigate to={{ pathname: '/adesso', search }} replace />} />
        </Routes>
      </main>
      <footer className="piede">{gruppoDi(pagina?.gruppo ?? 'xray').riserva}</footer>
      {conTempo && <BarraTempo />}
      <BarraBasso pagina={pagina} />
    </>
  );
}

export function App() {
  return (
    <FiltriProvider>
      <Corpo />
    </FiltriProvider>
  );
}
