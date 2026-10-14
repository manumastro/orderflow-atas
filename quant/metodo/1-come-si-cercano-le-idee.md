# Come Si Cercano Le Idee

La fase 1 della metodologia ([`la-metodologia-istituzionale.md`](la-metodologia-istituzionale.md#1-lidea))
dice **dove** stanno le idee (i paper, soprattutto su SSRN) e **cosa** se ne prende (il finding e il
perche', mai le regole copiate). Questo documento dice **come** l'agente li trova, li scarica e li
legge in questo repository. E' tecnica del framework, non del transcript: dove aggiunge qualcosa, lo
dichiara.

## I Passi

```text
cercare -> verificare che esista -> scaricare il PDF -> leggerlo tutto -> analisi del paper -> scheda dell'idea
```

### 1. Cercare

- **Con il titolo esatto**, tra virgolette, quando l'utente lo da'; altrimenti con le parole chiave
  del finding (`prop firm challenge first-passage`, `intraday momentum first half hour`).
- **Dove**: SSRN prima di tutto, perche' e' il posto che indica il transcript `[MC · 25:42 - 25:49]`.
  *Aggiunta del framework*: anche arXiv (sezione q-fin), RePEc/IDEAS, NBER e le pagine degli autori,
  che spesso hanno lo stesso paper scaricabile senza barriere.
- **Strumento**: la ricerca web dell'agente. I riassunti che la ricerca restituisce **non sono una
  fonte**: dicono che il paper esiste e dove sta, non cosa dice.

### 2. Verificare che esista

Per ogni paper si annotano titolo, autori, affiliazione, identificativo (su SSRN l'`abstract_id`),
date (scritto, pubblicato), e se e' un preprint o e' passato da una revisione fra pari. Un preprint
non revisionato si puo' usare, ma lo si scrive nella scheda.

### 3. Scaricare il PDF

**SSRN non si scarica con le richieste automatiche.** Misurato l'11 ottobre 2026: la lettura web e
`curl` (anche con l'intestazione di un browser) ricevono **403**, sia la pagina dell'abstract sia
`Delivery.cfm`. Lo stesso per ResearchGate. Davanti c'e' Cloudflare.

**Lo strumento e' il browser, con Playwright** (il server MCP `playwright`, configurato in
`.mcp.json`). Due regole:

1. **La verifica di sicurezza la supera una persona, mai l'agente.** Se Cloudflare mostra
   "Esecuzione della verifica di sicurezza", l'agente si ferma e chiede all'utente di cliccare la
   casella nella finestra di Chrome che Playwright ha aperto; poi prosegue nella stessa sessione.
   Provare ad aggirare un controllo anti-bot non e' una tecnica di ricerca: e' eludere una misura di
   sicurezza, e non si fa.
2. **Se la pagina chiede un account**, lo stesso: accede l'utente, l'agente continua.

La stessa tecnica serve per **le regole delle prop firm**: anche il centro assistenza di Tradeify
risponde 403 alla lettura automatica, e si legge con Playwright (11 ottobre 2026).

**Come si scarica, in pratica** (provato l'11 ottobre 2026 su tre paper SSRN):

1. `browser_navigate` sulla pagina dell'abstract; se compare la verifica, la supera l'utente;
2. il banner dei cookie copre i pulsanti: si sceglie **"Accept only necessary cookies"**;
3. il PDF si scarica **cliccando "Download This Paper"**, come farebbe una persona, dentro
   `browser_run_code_unsafe`: `page.waitForEvent('download')` insieme al clic, poi
   `download.saveAs('<percorso in quant/paper>')`. Una richiesta diretta a `Delivery.cfm` dallo
   script non serve: il sandbox non permette di scrivere file (`import('fs')` fallisce);
4. nello stesso script si legge l'abstract dalla pagina (`.abstract-text`): e' il testo esatto, da
   preferire ai riassunti della ricerca web;
5. il server MCP salva una copia di ogni download anche in `.playwright-mcp/` (fuori da git): si
   cancella;
6. alla fine si chiude il browser.

**Installare il browser di Playwright**, se manca (errore `Browser "chrome-for-testing" is not
installed`). La versione deve essere quella del server MCP in uso, che e' `@latest`:

```bash
npx -y @playwright/mcp@latest install-browser chrome-for-testing
```

Senza `@latest`, `npx` puo' usare una versione in cache e installare un browser diverso da quello
che il server cerca: l'11 ottobre 2026 ha installato `chromium-1246` mentre il server voleva
`chromium-1247`.

**Dove va il PDF**: in `quant/paper/<filone>/<chiave>/<chiave>.pdf`. La chiave e'
`autore-anno-titolo-breve`, il filone e' la domanda a cui il paper risponde (`prop-firm`,
`momento-intraday`, ...); le regole complete sono in testa all'[indice](../paper/indice-dei-paper.md).
Appena scaricato se ne calcola lo `sha256sum`, che va nella scheda: dice quale versione e' stata
letta. **I
PDF non vanno in git** (sono dei loro autori e dei loro editori): li esclude `.gitignore`. In git
va l'indice, [`quant/paper/indice-dei-paper.md`](../paper/indice-dei-paper.md), con il link e lo stato
di ogni fonte.

### 4. Leggerlo tutto

**Come**: il lettore di PDF dell'agente vuole `pdftoppm` (poppler), che su questo PC non c'e'. C'e'
invece `pdftotext`, con Git per Windows:

```bash
pdftotext -layout quant/paper/<filone>/<chiave>/<chiave>.pdf <scratchpad>/<chiave>.txt
```

Il testo si legge per intero. **Le formule escono storpiate** (le lettere greche spariscono): ogni
formula che entra in una scheda o nel codice si ricontrolla sul PDF. Il file di testo e' un
intermedio e sta nella scratchpad.

Il transcript dice di usare la macchina per "craccare" il paper: di cosa parla, quali sono i
findings, perche' dovrebbe fare soldi `[MC · 28:57 - 29:27]`, `[MC · 1:04:46 - 1:05:09]`. L'agente
**legge il PDF per intero**, non l'abstract, ed estrae:

- **il finding**, con i numeri, lo strumento, il periodo, la scala temporale e i dati usati;
- **il perche'**, cioe' il meccanismo che gli autori propongono, e le alternative che scartano;
- **cosa lo ucciderebbe**, se gli autori lo dicono, o cosa si deduce dal perche';
- **i limiti dichiarati** dagli autori, e quelli che non dichiarano: dati simulati o reali, costi,
  look-ahead, numero di varianti provate, campione fuori;
- **cosa si puo' rifare qui**, con NQ e i dati di ATAS, e cosa no.

### 5. Scrivere l'analisi del paper

Nella cartella del paper, `analisi.md`. In testa, fra due righe `---`, **la scheda bibliografica**:
chiave, sigla, titolo, autori, versione letta, fonte e identificativo, url, tipo (preprint o rivista),
revisione fra pari, pagine, pdf e il suo sha256, filone, ruolo (`idea` o `metodo`), stato, data di
lettura, idee collegate, codice degli autori. Poi **la lettura**: la domanda, il metodo, i risultati
con la sezione da cui vengono, cosa abbiamo riprodotto, cosa non regge o non si dice, cosa ne prende il
framework. Esempio: [`quant/paper/prop-firm/tomas-fernandez-2026-valuing-prop-firm-evaluation-contracts/analisi.md`](../paper/prop-firm/tomas-fernandez-2026-valuing-prop-firm-evaluation-contracts/analisi.md).
Poi si aggiorna la riga del paper nell'indice.

### 6. Scrivere la scheda dell'idea

Nello studio, `quant/studi/<idea>/1-idea.md`, con il modello [`1-come-si-scrive-un-idea.md`](1-come-si-scrive-un-idea.md).
Il paper non si copia `[MC · 26:27 - 27:22]`: la scheda prende il finding e il perche', e le regole
da provare si scrivono dopo, nei tre riquadri.
