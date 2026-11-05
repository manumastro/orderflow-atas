// La preparazione delle prove (Vitest, jsdom): le asserzioni del DOM, cio' che jsdom non ha
// (matchMedia, ResizeObserver) e un ECharts finto che registra le opzioni (e le azioni, come lo zoom
// rimesso) invece di disegnare.

import '@testing-library/jest-dom/vitest';
import { afterEach, vi } from 'vitest';
import { cleanup } from '@testing-library/react';

export interface GraficoFinto {
  opzioni: unknown[];
  /** le azioni ricevute (dispatchAction): lo zoom rimesso dopo un setOption */
  azioni: unknown[];
  group: string;
  /** lo stato delle opzioni come lo darebbe ECharts (getOption); le prove possono cambiarlo */
  stato: Record<string, unknown>;
  setOption: (o: unknown, impostazioni?: unknown) => void;
  getOption: () => Record<string, unknown>;
  dispatchAction: (a: unknown) => void;
  clear: () => void;
  resize: () => void;
  dispose: () => void;
}

/** I grafici creati durante una prova, con le opzioni ricevute. */
export const grafici: GraficoFinto[] = [];

vi.mock('../viz/echarts', () => ({
  crea: (): GraficoFinto => {
    const g: GraficoFinto = {
      opzioni: [],
      azioni: [],
      group: '',
      stato: {},
      setOption(o) {
        g.opzioni.push(o);
      },
      getOption() {
        return g.stato;
      },
      dispatchAction(a) {
        g.azioni.push(a);
      },
      clear() {},
      resize() {},
      dispose() {},
    };
    grafici.push(g);
    return g;
  },
  collega: () => {},
}));

Object.defineProperty(window, 'matchMedia', {
  writable: true,
  value: (query: string) => ({
    matches: false,
    media: query,
    onchange: null,
    addListener: () => {},
    removeListener: () => {},
    addEventListener: () => {},
    removeEventListener: () => {},
    dispatchEvent: () => false,
  }),
});

class OsservatoreFinto {
  observe() {}
  unobserve() {}
  disconnect() {}
}
globalThis.ResizeObserver = OsservatoreFinto as unknown as typeof ResizeObserver;

afterEach(() => {
  cleanup();
  grafici.length = 0;
  vi.restoreAllMocks();
});
