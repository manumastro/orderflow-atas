// Un grafico ECharts: si crea al montaggio, segue la larghezza del contenitore (ResizeObserver),
// si libera allo smontaggio. Le opzioni nuove sostituiscono serie, assi e griglie, e tengono lo zoom
// (prima `notMerge` lo azzerava a ogni giro dal vivo, G1).
//
// - `opzioni` puo' essere una funzione della larghezza: i margini si adattano (griglia() di grafici.ts);
// - `altezza` 'alto' | 'medio' | 'basso' segue la finestra (D.2); un numero resta per le pagine di prima;
// - `puntatore`: per le mappe disegnate come immagine (viz/mappa.ts) il tooltip non viene dai segni ma
//   dalla posizione: il grafico passa il punto in coordinate dei dati, la funzione da' l'HTML;
// - `onCursore`: il valore sotto il cursore, null quando esce: la legenda al cursore. E' la x (il tempo),
//   o la y (il prezzo) con `cursoreSu="y"` nei profili orizzontali;
// - `tooltipSoloQui`: nei grafici legati il tooltip compare solo nel grafico sotto il mouse.
//
// L'istanza e' appesa al nodo (`__grafico`): la lettura dell'agente (scripts/leggi_pagina.py) legge da li'.

import { useEffect, useRef, useState } from 'react';
import { collega, crea, type Istanza, type Opzioni } from '../viz/echarts';
import type { Puntatore } from '../viz/grafici';
import { FONT } from '../tema/colori';

export type { Puntatore };

export type Altezza = number | 'alto' | 'medio' | 'basso' | { min: number; max: number; schermo: number };

interface Props {
  /** null = niente da disegnare: il grafico si svuota */
  opzioni: Opzioni | null | ((larghezza: number) => Opzioni | null);
  altezza?: Altezza;
  /** i grafici dello stesso gruppo condividono cursore e zoom */
  gruppo?: string;
  /** cosa mostra, per chi non vede il grafico */
  etichetta: string;
  puntatore?: Puntatore;
  /** il cursore: il valore dell'asse `cursoreSu` sotto il mouse, null quando esce; per la legenda al cursore */
  onCursore?: (valore: number | null) => void;
  /** quale asse da' il valore a `onCursore`: 'x' (il tempo, di norma) o 'y' (il prezzo dei profili orizzontali) */
  cursoreSu?: 'x' | 'y';
  /** nei grafici legati: il tooltip compare solo se il mouse e' su questo */
  tooltipSoloQui?: boolean;
}

const limita = (v: number, lo: number, hi: number) => Math.min(hi, Math.max(lo, v));

/** L'altezza in px per la finestra data (D.2). */
export function altezzaPx(a: Altezza, finestra: { larghezza: number; altezza: number }): number {
  if (typeof a === 'number') return a;
  const h = finestra.altezza;
  if (typeof a === 'object') return limita(h * a.schermo, a.min, a.max);
  if (a === 'alto') return finestra.larghezza < 768 ? 420 : limita(h - 330, 360, 640);
  if (a === 'medio') return limita(h * 0.38, 240, 380);
  return limita(h * 0.19, 150, 170);
}

function useFinestra() {
  const leggi = () => ({ larghezza: typeof window === 'undefined' ? 1600 : window.innerWidth, altezza: typeof window === 'undefined' ? 900 : window.innerHeight });
  const [f, setF] = useState(leggi);
  useEffect(() => {
    const cambia = () => setF(leggi());
    window.addEventListener('resize', cambia);
    return () => window.removeEventListener('resize', cambia);
  }, []);
  return f;
}

type Zoom = { start?: number; end?: number };

/** Applica le opzioni tenendo lo zoom (D.3). */
export function applica(g: Istanza, o: Opzioni): void {
  const prima = typeof g.getOption === 'function' ? (g.getOption() as { dataZoom?: Zoom[] } | undefined) : undefined;
  const zoom = (prima?.dataZoom ?? []).map((z) => [z.start, z.end] as const);
  g.setOption({ textStyle: { fontFamily: FONT }, ...o }, { replaceMerge: ['series', 'xAxis', 'yAxis', 'grid'] });
  zoom.forEach(([start, end], i) => {
    if (start != null && end != null && (start !== 0 || end !== 100)) g.dispatchAction({ type: 'dataZoom', dataZoomIndex: i, start, end });
  });
}

export function Grafico({ opzioni, altezza = 420, gruppo, etichetta, puntatore, onCursore, cursoreSu = 'x', tooltipSoloQui = false }: Props) {
  const nodo = useRef<HTMLDivElement>(null);
  const tip = useRef<HTMLDivElement>(null);
  const istanza = useRef<Istanza | null>(null);
  const funzione = useRef<Puntatore | undefined>(puntatore);
  const cursore = useRef(onCursore);
  const soloQui = useRef(tooltipSoloQui);
  const asseCursore = useRef(cursoreSu);
  const finestra = useFinestra();
  const [larghezza, setLarghezza] = useState(0);
  const h = altezzaPx(altezza, finestra);

  useEffect(() => {
    funzione.current = puntatore;
    cursore.current = onCursore;
    soloQui.current = tooltipSoloQui;
    asseCursore.current = cursoreSu;
  }, [puntatore, onCursore, tooltipSoloQui, cursoreSu]);

  useEffect(() => {
    const el = nodo.current;
    if (!el) return;
    const g = crea(el);
    istanza.current = g;
    // l'istanza appesa al nodo: la lettura dell'agente (scripts/leggi_pagina.py) legge da qui le serie e le
    // linee esattamente come sono disegnate, invece di ricalcolarle
    (el as HTMLDivElement & { __grafico?: Istanza }).__grafico = g;
    setLarghezza(el.clientWidth);
    const osservatore =
      typeof ResizeObserver === 'undefined'
        ? null
        : new ResizeObserver(() => {
            g.resize();
            setLarghezza(el.clientWidth);
          });
    osservatore?.observe(el);

    const zr = typeof g.getZr === 'function' ? g.getZr() : null;
    let dentro = false;
    const mostraTooltip = (si: boolean) => {
      if (!soloQui.current || si === dentro) return;
      dentro = si;
      g.setOption({ tooltip: { showContent: si } });
    };
    // il tooltip dalla posizione, solo dentro la griglia
    const nascondi = () => {
      if (tip.current) tip.current.hidden = true;
    };
    const muovi = (e: { offsetX: number; offsetY: number }) => {
      mostraTooltip(true);
      const f = funzione.current;
      const t = tip.current;
      if (!f || !t) return;
      const punto = [e.offsetX, e.offsetY];
      if (!g.containPixel('grid', punto)) return nascondi();
      const [x, y] = g.convertFromPixel('grid', punto) as number[];
      const qui = asseCursore.current === 'y' ? y : x;
      if (qui != null) cursore.current?.(qui);
      const html = x == null || y == null ? null : f(x, y);
      if (!html) return nascondi();
      t.innerHTML = html;
      t.hidden = false;
      const larga = el.clientWidth;
      const sinistra = e.offsetX + 16 + t.offsetWidth > larga ? e.offsetX - 16 - t.offsetWidth : e.offsetX + 16;
      t.style.transform = `translate(${Math.max(0, sinistra)}px, ${Math.max(0, e.offsetY - t.offsetHeight - 12)}px)`;
    };
    const esci = () => {
      nascondi();
      mostraTooltip(false);
      cursore.current?.(null);
    };
    // la legenda al cursore: il valore dell'asse sotto il mouse (anche dai grafici legati)
    const asse = (e: { axesInfo?: { axisDim?: string; value?: unknown }[] }) => {
      const info = e.axesInfo ?? [];
      const v = (info.find((a) => a.axisDim === asseCursore.current) ?? info[0])?.value;
      if (v != null && Number.isFinite(Number(v))) cursore.current?.(Number(v));
    };
    zr?.on('mousemove', muovi);
    zr?.on('globalout', esci);
    // al tocco (telefono) il tooltip della posizione si apre come col mouse
    zr?.on('click', muovi);
    if (typeof g.on === 'function') g.on('updateAxisPointer', asse as never);

    return () => {
      zr?.off('mousemove', muovi);
      zr?.off('globalout', esci);
      zr?.off('click', muovi);
      if (typeof g.off === 'function') g.off('updateAxisPointer', asse as never);
      osservatore?.disconnect();
      g.dispose();
      istanza.current = null;
    };
  }, []);

  useEffect(() => {
    const g = istanza.current;
    if (!g || !gruppo) return;
    g.group = gruppo;
    collega(gruppo);
  }, [gruppo]);

  useEffect(() => {
    const g = istanza.current;
    if (!g) return;
    const o = typeof opzioni === 'function' ? opzioni(larghezza || nodo.current?.clientWidth || 800) : opzioni;
    if (o) {
      applica(g, o);
      if (soloQui.current) g.setOption({ tooltip: { showContent: false } });
    } else g.clear();
  }, [opzioni, larghezza]);

  useEffect(() => {
    istanza.current?.resize();
  }, [h]);

  return (
    <div className="grafico-scatola">
      <div ref={nodo} className="grafico" style={{ height: h }} role="img" aria-label={etichetta} />
      {puntatore && <div ref={tip} className="puntatore" hidden />}
    </div>
  );
}
