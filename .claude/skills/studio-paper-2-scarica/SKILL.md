---
name: studio-paper-2-scarica
description: Fase 2 dello studio dei paper - scaricare. Porta i PDF scelti in quant/paper/<filone>/<chiave>/, con lo sha256; i PDF liberi con curl, SSRN con Playwright; davanti a Cloudflare o a un account si ferma e chiede la persona.
argument-hint: "<file della ricerca>"
---

# Fase 2: Scaricare

Il passo 3 di [`quant/metodo/1-come-si-cercano-le-idee.md`](../../../quant/metodo/1-come-si-cercano-le-idee.md):
aprilo, contiene la procedura Playwright provata su SSRN.

## Per Ogni Chiave Scelta Nel File Della Ricerca

1. **La cartella**: `quant/paper/<filone>/<chiave>/`. Il PDF: `<chiave>.pdf`.
2. **Il PDF libero prima**: se il file della ricerca ha un url diretto (arXiv, NBER, pagina
   dell'autore), `curl -L -A "<user agent di un browser>" -o <percorso> <url>`. Controlla che sia un
   PDF vero (`file`, o i primi byte `%PDF`) e non una pagina HTML d'errore.
3. **SSRN e le pagine con Cloudflare**: Playwright (server MCP `playwright`), come nel documento:
   la pagina dell'abstract, "Accept only necessary cookies", il clic su "Download This Paper" dentro
   `browser_run_code_unsafe` con `page.waitForEvent('download')` e `download.saveAs(...)`; dalla stessa
   pagina l'abstract esatto (`.abstract-text`). Alla fine chiudi il browser e cancella le copie in
   `.playwright-mcp/`.
4. **Se compare una verifica di sicurezza o un login**: **fermati**. Non provare ad aggirarla, non
   cambiare user agent per evitarla, non cercare mirror non ufficiali. Rispondi
   `BLOCCATO: <chiave>, <url>, <cosa serve>` e aspetta: l'orchestratore chiede all'utente di
   superarla nella finestra aperta, poi ti riprende.
5. **Lo sha256**: `sha256sum <pdf>`; va nel file della ricerca, accanto alla chiave.
6. **Lo stato**: `scaricato` nel file della ricerca e nell'indice; una riga datata nel **Diario**.

Un paper che non si riesce a scaricare resta `trovato`, con il motivo. I PDF non vanno in git.

Rispondi all'orchestratore con: le chiavi scaricate con lo sha256, quelle bloccate e perche'.
