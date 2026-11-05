/// <reference types="vitest/config" />
// La pagina si compila in dist/ con indirizzi relativi (./): il server XrayLive la serve da li'.
// In sviluppo e in anteprima /api va al server vero su 127.0.0.1:8790.
//
// Con XRAY_ESEMPI=1 (npm run dev:esempi) le rotte nuove che il server non ha ancora (volatilita',
// NQ, confronti, fonti) rispondono con le fixture di src/api/esempi/: serve solo a vedere le
// pagine prima che il server le abbia. Le rotte di X-Ray vanno sempre al server vero.
import { readFileSync } from 'node:fs';
import { fileURLToPath } from 'node:url';
import type { Connect, Plugin } from 'vite';
import { defineConfig } from 'vite';
import react from '@vitejs/plugin-react';

const server = process.env.XRAY_SERVER ?? 'http://127.0.0.1:8790';
const proxy = { '/api': { target: server, changeOrigin: false } };

function esempio(url: string): string | null {
  const [percorso = '', q = ''] = url.split('?');
  const indice = new URLSearchParams(q).get('indice');
  const file: Record<string, string> = {
    '/api/fonti': 'fonti',
    '/api/vol/serie': 'vol-serie',
    '/api/vol/termine': 'vol-termine',
    '/api/vol/sorriso': indice === 'NDX' ? 'vol-sorriso-ndx' : 'vol-sorriso-spx',
    '/api/nq/gex': 'nq-gex',
    '/api/es/gex-confronto': 'es-gex-confronto',
  };
  const nome = file[percorso];
  return nome ? fileURLToPath(new URL(`./src/api/esempi/${nome}.json`, import.meta.url)) : null;
}

function esempi(): Plugin {
  const middleware: Connect.NextHandleFunction = (req, res, next) => {
    const file = req.url ? esempio(req.url) : null;
    if (!file) return next();
    res.setHeader('Content-Type', 'application/json; charset=utf-8');
    res.setHeader('Cache-Control', 'no-store');
    res.end(readFileSync(file));
  };
  return {
    name: 'xray-esempi',
    configureServer: (s) => void s.middlewares.use(middleware),
    configurePreviewServer: (s) => void s.middlewares.use(middleware),
  };
}

export default defineConfig({
  base: './',
  plugins: [react(), ...(process.env.XRAY_ESEMPI === '1' ? [esempi()] : [])],
  server: { host: '127.0.0.1', port: 5173, strictPort: false, proxy },
  preview: { host: '127.0.0.1', port: 4173, strictPort: false, proxy },
  build: { outDir: 'dist', emptyOutDir: true, chunkSizeWarningLimit: 1600, sourcemap: false },
  test: {
    environment: 'jsdom',
    setupFiles: ['./src/test/preparazione.ts'],
    css: false,
    // il fuso della macchina non deve cambiare le prove delle ore: si prova da un fuso qualunque
    env: { TZ: 'Asia/Tokyo' },
  },
});
