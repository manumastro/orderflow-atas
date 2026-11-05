// L'avvio: i font locali (niente CDN, la pagina funziona offline), Mantine scuro, la cache delle
// domande, il router sull'hash (#/adesso?giorno=...).

import '@fontsource/inter/latin-400.css';
import '@fontsource/inter/latin-500.css';
import '@fontsource/inter/latin-600.css';
import '@fontsource/inter/latin-700.css';
import '@fontsource/roboto-mono/latin-400.css';
import '@fontsource/roboto-mono/latin-500.css';
import '@mantine/core/styles.css';
import './stile.css';

import { StrictMode } from 'react';
import { createRoot } from 'react-dom/client';
import { MantineProvider } from '@mantine/core';
import { QueryClient, QueryClientProvider } from '@tanstack/react-query';
import { HashRouter } from 'react-router';
import { App } from './App';
import { nonDisponibile } from './api/client';
import { tema, variabili } from './tema/tema';

// gli indirizzi della pagina vecchia (#adesso?giorno=...) diventano quelli nuovi (#/adesso?giorno=...)
// (all'avvio, e quando se ne incolla uno a pagina aperta: il router segue il nuovo hash)
function correggiIndirizzo(): void {
  if (location.hash.length > 1 && !location.hash.startsWith('#/')) location.replace(`#/${location.hash.slice(1)}`);
}
correggiIndirizzo();
window.addEventListener('hashchange', correggiIndirizzo);

const cache = new QueryClient({
  defaultOptions: {
    queries: {
      // un errore si riprova una volta; un 404 (giornata o rotta che non c'e') no
      retry: (volte, errore) => volte < 1 && !nonDisponibile(errore),
      refetchOnWindowFocus: false,
    },
  },
});

const radice = document.getElementById('radice');
if (!radice) throw new Error('manca #radice');

createRoot(radice).render(
  <StrictMode>
    <MantineProvider theme={tema} cssVariablesResolver={variabili} forceColorScheme="dark">
      <QueryClientProvider client={cache}>
        <HashRouter>
          <App />
        </HashRouter>
      </QueryClientProvider>
    </MantineProvider>
  </StrictMode>,
);
