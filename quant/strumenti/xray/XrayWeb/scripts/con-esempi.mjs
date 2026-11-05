// npm run dev:esempi -- [argomenti di vite]: vite in sviluppo con XRAY_ESEMPI=1, cioe' le rotte
// nuove che il server non ha ancora rispondono con le fixture di src/api/esempi/ (vite.config.ts).
// Un file a parte perche' su Windows "XRAY_ESEMPI=1 vite" non si scrive cosi'.
import { spawn } from 'node:child_process';
import { fileURLToPath } from 'node:url';

const vite = fileURLToPath(new URL('../node_modules/vite/bin/vite.js', import.meta.url));
const figlio = spawn(process.execPath, [vite, ...process.argv.slice(2)], {
  stdio: 'inherit',
  env: { ...process.env, XRAY_ESEMPI: '1' },
});
figlio.on('exit', (codice) => process.exit(codice ?? 0));
