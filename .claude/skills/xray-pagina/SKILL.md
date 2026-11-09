---
name: xray-pagina
description: "La pagina locale di X-Ray e delle fonti (quant/strumenti/xray/XrayWeb): Vite, React, TypeScript, Mantine, ECharts. Usala quando si cambia, si estende, si compila o si prova la pagina http://127.0.0.1:8790/: una pagina o una fonte nuova, i colori, le spiegazioni, le prove."
---

# La Pagina Di X-Ray: XrayWeb

La pagina disegna, **non calcola**: i numeri arrivano gia' pronti dal server `XrayLive --serve`
(porta 8790, C#). Il significato dei dati sta in `quant/metodo/x-ray-in-vivo.md`, lo strumento in
`quant/strumenti/xray/x-ray-lettore-e-dashboard.md`. Il server serve `XrayWeb/dist`.

## Lo Stack E Perche'

| | perche' |
|---|---|
| **Vite + React 19 + TypeScript strict** | componenti piccoli e tipi precisi; nessun `any` (ESLint lo vieta) |
| **Mantine** (tema scuro) | i comandi (segmenti, tendine, cursori) gia' accessibili; la disposizione e' in `src/stile.css` |
| **ECharts** (solo i moduli usati, `src/viz/echarts.ts`) | canvas: le prove contano i pixel; mappe e profili a rettangoli |
| **TanStack Query** | la cache, e "Dal vivo" ogni 15 s; un istante passato non si richiede |
| **React Router, HashRouter** | gli indirizzi `#/adesso?giorno=..&ts=..&prezzi=ES`; i filtri vivono nei search params |
| **@fontsource** Inter e Roboto Mono | font locali: la pagina funziona offline |

Versioni **esatte** nel `package.json` (niente `^`), rilasciate da almeno due settimane, controllate
con `npm view <pacchetto> time`. TypeScript resta alla 6.0 finche' `typescript-eslint` non supporta la 7.

## La Forma: In Stile QuantData

Dal **5 novembre 2026, per decisione dell'utente**, la pagina e' un software in stile QuantData, non un
documento. Le regole che restano sono qui.

- **La testata** (`header.testata`, ferma in alto, due righe, 88 px):
  - **la barra alta** (`BarraAlta`): il marchio (`MARCHIO` in `rotte.tsx`, "Opzioni dal vivo"), il menu dei
    cinque gruppi (`nav.gruppi`; il gruppo X-Ray apre un mega-menu con le sei pagine), l'orologio di New York e
    d'Italia, lo stato delle quattro fonti (`SaluteFonti`: icona, stato, eta'), il comando **Impara | Veloce**;
  - **la barra del contesto** (`BarraContesto`): le pagine del gruppo come linguette (`nav.voci`) e la "i"
    della pagina, i comandi del tempo (`ComandiTempo`: giorno, istante, Dal vivo, Prezzi ES|SPX con la base
    nella "ⓘ") e la chip della riserva breve del gruppo (`ChipRiserva`). Un comando che la pagina non usa resta
    al suo posto, spento, con la ragione scritta: la barra non salta.
- **La barra del tempo** (`BarraTempo`), fissa in fondo, 44 px, solo nelle pagine che usano l'istante: un
  cursore con le ore di New York, la cassa 09:30-16:00 come fascia piu' chiara, ◀ ▶ (con Maiusc: 15 minuti).
- **La modalita' passato**: con un istante o un giorno passato la testata prende un bordo ambra e la chip
  "◷ Passato"; sempre icona e parola.
- **Il telefono** (sotto 768 px): testata di 80 px, le pagine in un cassetto, i filtri nel cassetto ⚙, la barra
  del tempo sostituita da ◀ ▶, e **una barra in basso** (`BarraBasso`, `nav.barra-basso`) con i cinque gruppi.
  Nessuno scorrimento orizzontale a 390 px.
- **Lo schermo verticale** (dal 9 novembre 2026, richiesta dell'utente: un monitor 1200 per 1920 in verticale;
  da 768 px, piu' alto che largo): `verticale()` in `Grafico.tsx` e `@media (orientation: portrait)` in
  `stile.css`, la stessa soglia. La testata ha il marchio senza nome e l'orologio corto, e la barra del contesto va
  su due righe, le linguette sopra e i comandi del tempo sotto. Accanto al grafico grande la colonna dei
  livelli prende 5 colonne su 12 (dai 1100 px). I grafici crescono fino alla barra del tempo:
  - `'alto'` lascia sotto lo spazio per un riquadro basso (il delta di Adesso);
  - `'pieno'` e' per il grafico senza riquadri sotto (Per strike, Livelli, Previsione);
  - `'basso'` divide lo schermo fra i quattro pannelli della Deriva;
  - un'altezza a oggetto puo' avere `maxVerticale`.

  Su NQ il profilo resta a 600 px, perche' la GEX nel tempo stia nella prima schermata. Fuori dal verticale
  niente cambia.
- **Ogni pagina**: una riga di tessere (la risposta), poi una griglia di riquadri. Il grafico grande ha
  `altezza="alto"` e sta nella prima schermata a 1600 per 900. Mai un'altezza fissa in pixel.
- **Il riquadro** (`Scheda`), uguale ovunque: titolo stabile, badge corto della fonte (con l'eta' se vecchia),
  chip della riserva propria, "i", comandi a destra; sotto, **la legenda con i valori** (quelli sotto il
  cursore mentre il mouse e' sul grafico; un valore mancante e' "–", mai zero), **la riga breve**, il corpo.
- **La tessera** (`Tessera`): etichetta, valore, una riga; la fonte e' obbligatoria; il tono e' un pallino, mai
  il testo colorato; il tag ("non provato", "supposto", "sperimentale", "solo dal vivo") e' una pillola.

Le dieci pagine, in `src/pagine/rotte.tsx`:

| gruppo | pagine (percorso) |
|---|---|
| Lettura | Lettura (`#/lettura`) |
| SPX · X-Ray | Adesso (`#/adesso`), Per strike (`#/strike`), Heat map (`#/heatmap`), Livelli nel tempo (`#/livelli`; `#/tempo` rimanda qui), Deriva (`#/deriva`), Previsione (`#/previsione`) |
| Volatilita' | Volatilita' (`#/volatilita`) |
| NQ · QQQ | NQ · QQQ (`#/nq`) |
| Fonti | Fonti (`#/fonti`) |

## Le Cartelle

```text
src/api/         client.ts (chiedi, ErroreApi, 404), domande.ts (gli hook), tipi.ts (un tipo per risposta), esempi/ (fixture)
src/componenti/  il guscio: BarraAlta, BarraContesto, BarraTempo, BarraBasso, MenuPagine, ComandiTempo, SaluteFonti,
                 ChipRiserva, Orologio
                 il riquadro e le sue parti: Scheda, Tessera, FonteBadge, Legenda, Aiuto (la "i"), Termine (il glossario),
                 Comandi (Segmenti, Tendina, Divisore), Esito
                 i dati: Grafico, ScalaPrezzi (la colonna dei livelli), BarraInline (la barra dentro la cella), TabellaHeat
src/pagine/      rotte.tsx (MARCHIO, GRUPPI, PAGINE, REINDIRIZZI); xray/, lettura/, volatilita/, nq/, fonti/, ognuna
                 con le sue pagine, il suo <gruppo>.css e il suo file di prova
src/stato/       filtri.tsx e contesto.ts (giorno, istante, prezzi dall'indirizzo; la modalita' passato), scelte.ts
                 (le scelte di pagina), spiegazioni.ts (Impara | Veloce), fonti.ts (lo stato di una fonte, per badge ed eta')
src/tema/        colori.ts (C, INCHIOSTRI: un colore, un ruolo), fonti.ts (le quattro fonti), tema.ts (Mantine)
src/viz/         echarts.ts (moduli), grafici.ts (assi, tooltip, segnoGamma e segnoFlusso, linee con il cartellino,
                 distanzia, inchiostroSu), mappa.ts (la mappa come immagine), fonti.ts (stile per fonte)
src/glossario.ts le glosse dei termini (due righe: cosa vuol dire per il prezzo, quanto e' provato)
src/formato.ts   numeri all'italiana, ora di New York e italiana, distanze con la freccia
```

## Come Si Aggiunge Una Pagina

1. **Il calcolo va nel server** (`XrayLive/Server/`): una rotta in `Servitore.cs`, con la sua prova in
   `XrayLive.Test`. La pagina non calcola.
2. Il tipo della risposta in `src/api/tipi.ts`, ricavato dalla risposta vera (`curl`) e dal C#.
3. L'hook in `src/api/domande.ts`: `useMisura` per le rotte di X-Ray, `useNuova` per una rotta che il
   server potrebbe non avere ancora (404 -> "fonte non ancora disponibile", riprova ogni minuto).
4. Il componente in `src/pagine/<gruppo>/`, con le tessere in cima e `Esito` attorno a ogni dato (attesa, 404,
   errore); gli stili propri in `<gruppo>.css`, mai in `stile.css`.
5. La riga in `src/pagine/rotte.tsx`: percorso, gruppo, filtri usati, `breve` (la riga del mega-menu),
   `spiega` (la frase della "i" della pagina), icona.
6. Le fixture in `src/api/esempi/` e le prove nel file del gruppo. **Le prove sono divise per gruppo**:
   `pagine/xray/xray1.test.tsx` (Adesso, Per strike, Heat map), `pagine/xray/xray2.test.tsx` (Livelli nel tempo,
   Deriva, Previsione), poi `lettura/lettura.test.tsx`, `volatilita/volatilita.test.tsx`, `nq/nq.test.tsx`,
   `fonti/fonti.test.tsx`; i componenti comuni in `componenti/componenti.test.tsx`. La pagina va nella lista di
   `quant/strumenti/xray/scripts/prova_pagina.py`.

## La Pagina La Legge Anche L'Agente

Dal 2 novembre 2026 la lettura dal vivo delle opzioni parte dalla pagina (`scripts/leggi_pagina.py`), per
richiesta dell'utente: l'agente deve leggere cio' che l'utente vede. Per questo:
- **`Grafico.tsx` appende l'istanza di ECharts al nodo** (`__grafico`): non toglierla, lo script legge da li'
  serie e `markLine` (il `name` di una linea resta minuscolo con il valore, "flip 7.760");
- **ogni riquadro ha un titolo stabile** (`aria-label` della `section.scheda`), che non cambia con i comandi:
  la lettura cita il riquadro per nome;
- **la legenda con i valori sta dentro la `section`**, e lo stato delle fonti, l'istante e la riserva stanno
  nell'`header`: lo script li legge da li'. Nasconde invece `.spiega`, `.sotto` e `.scheda-breve`;
- **la pagina Lettura** (`src/pagine/lettura/`) mostra la lettura che l'agente scrive con
  `scripts/scrivi_lettura.py`; le sue voci e i suoi controlli stanno in quello script, e la forma in
  `come-si-leggono-le-opzioni-dal-vivo.md`, "La pagina Lettura": si cambiano insieme;
- un riquadro nuovo o rinominato si aggiunge alla tabella "Dove si legge nella pagina" di
  `quant/metodo/come-si-leggono-le-opzioni-dal-vivo.md`.

## Le Fonti E I Colori

**Un colore, un ruolo** (5 novembre 2026, decisione dell'utente). I valori stanno **solo** in
`src/tema/colori.ts` (i ruoli) e `src/tema/fonti.ts` (le fonti); le scale del segno in `src/viz/grafici.ts`
(`segnoGamma`, `segnoFlusso`).

| colore | ruolo, uno solo |
|---|---|
| verde `#4fae38` | **freno**: gamma positiva dei market maker (su NQ supposta, convenzione A); anche il call wall di NQ |
| rosso `#b5443f` | **acceleratore**: gamma negativa; anche il put wall di NQ |
| blu `#3987e5` | **comprato**: il flusso positivo (DEX, VEX, charm) |
| arancio `#d95926` | **venduto**: il flusso negativo |
| `#2b3242` | il centro di ogni scala divergente |
| inchiostro `#eef1f5` | **il prezzo** (2 px, con alone; tratteggiato fuori cassa) e il testo |
| grigio `#aab0bb`, **tratteggiato** | **il flip e lo zero gamma** (lo zero dello 0DTE a puntini) |
| grigio spento `#8a92a3`, a puntini | **i bordi del valore**: VAL, VAH, POC, muri del ping-pong |
| tre inchiostri con tratti diversi (`INCHIOSTRI`) | identita' senza segno: le scadenze e i gruppi nella Deriva, con il nome scritto in fondo |
| magenta `#d55181` | fonte **X-Ray** |
| **viola `#9256bd`** | fonte **catena CME** (era blu: il blu ora vuol dire "comprato") |
| giallo `#c98500`, tratteggiato | fonte **Cboe** |
| verde scuro `#008300`, a puntini | fonte **Yahoo** |
| blu notte `#1f3d78` | **l'accento: solo il fondo dei comandi attivi** (segmento, pillola, voce scelta). Mai una linea, un punto, un testo o un bordo di dato |
| `#0ca30c`, `#fab219`, `#d03b3b` | **stato** (buono, attenzione e passato, critico): sempre con icona e parola |

**Le regole**:
- **Un riquadro mostra o la gamma (verde e rosso) o il flusso (blu e arancio)**, mai tutti e due: il rosso
  dell'acceleratore e l'arancio del venduto sono vicini. La legenda dice sempre la parola ("freno",
  "acceleratore", "comprato", "venduto").
- **Un grafico mostra o il segno o le fonti, mai tutti e due.** Su NQ le barre del profilo sono nel colore del
  segno e la fonte la dice il badge, con la riserva "segno supposto" sempre in vista: ogni profilo ha una fonte
  sola. Yahoo non sta mai in un grafico con il segno.
- **Il segno si legge anche senza colore**: dal verso della barra, dal "−" nei numeri, dalla parola.
- **Il prezzo non si colora per la direzione**: le variazioni sono in inchiostro, con ▲▼ e la parola. Verde o
  rosso sul prezzo si leggerebbero come un segnale.
- **La vista "Call e put"** di Per strike: le due barre nel colore del segno della misura, la call piena, la put
  tenue (45%, con il bordo).
- Le quattro fonti hanno id fissi (`xray`, `catena`, `cboe`, `yahoo`); badge (`FonteBadge`), legenda
  (`ChiaveFonte`, `voceFonte`) e serie (`lineaFonte`) li leggono da `tema/fonti.ts`. Due serie della stessa
  fonte: stesso colore, simboli diversi (`stiliPerFonte`).
- **Un colore nuovo si valida prima** con la skill `dataviz` (caricarla) e il suo script, contro il riquadro
  `#111723`:
  - segno: `node <dataviz>/scripts/validate_palette.js "#4fae38,#b5443f" --mode dark --surface "#111723"` (PASS);
  - flusso: `"#3987e5,#d95926"` (PASS);
  - fonti: `"#d55181,#9256bd,#c98500,#008300" --pairs all` (PASS con il WARN noto cboe-yahoo, deltaE 6,9, coperto
    da tratto, icona e nome).

  Per il daltonismo il rosso deve essere piu' scuro del verde: per questo il verde e il rosso puri di QuantData
  non passano.
- I colori di prima che non devono tornare: `#199e70` (verde acqua), `#9085e9` (viola dell'acceleratore), il
  giallo come flip, `COLORI_SCADENZA`, `COLORI_GRUPPO`.

## Le Regole Di Scrittura

- **Prima la persona, senza muro di testo.** Ogni riquadro con una spiegazione ha **la riga breve**
  (`breve`): una frase sola su cosa mostra e cosa vuol dire per il prezzo, senza numeri (i numeri stanno nella
  legenda). Poi `semplice` (cosa mostra, cosa vuol dire per il prezzo, quanto e' provato) e `sotto` (il
  tecnico).
- **Impara | Veloce**, un comando solo per tutta la pagina, nella barra alta (`stato/spiegazioni.ts`, chiave
  `xray-spiegazioni`, **Veloce di default**). Veloce mostra titolo, legenda e riga breve; Impara anche
  `semplice` e `sotto`. **La "i"** (`Aiuto`) apre sempre spiegazione, nota e riserva: un popover sul desktop,
  un cassetto sul telefono.
- **I termini** si scrivono `<Termine k="flip">flip</Termine>`: la glossa sta in `src/glossario.ts`, due righe
  al massimo (cosa vuol dire per il prezzo, quanto e' provato). L'utente sta imparando le opzioni: ogni termine
  si spiega.
- **La riserva sempre in vista, in breve**: la chip del gruppo (`riservaBreve` in `rotte.tsx`), i tag delle
  tessere, la chip della riserva propria nei riquadri che ne hanno una; quella intera nella "i" e nel piede.
- Tutto in italiano: testi, nomi, commenti. Numeri con `formato.ts` (`num`, `segno`, `auto`): livelli di ES a 0
  decimali con la distanza a freccia (↑29), il prezzo a 2; orari di New York con l'ora italiana accanto.
- Nessun segnale operativo in nessun testo: niente LONG, SHORT, ingressi, stop o bersagli.

## Compilare, Provare, Guardare

```bash
cd quant/strumenti/xray/XrayWeb
npm ci                      # le dipendenze esatte
npm run dev                 # 5173, /api -> 127.0.0.1:8790
npm run dev:esempi          # come dev, ma le rotte nuove rispondono con src/api/esempi/
npm run build               # tsc -b + vite build -> dist/ (base ./)
npm test                    # Vitest: formato, client, componenti, e le pagine per gruppo (fixture e 404)
npm test -- src/pagine/xray # solo le prove di un gruppo
npm run lint
npx vite preview --port 4173
python ../scripts/prova_pagina.py --url http://127.0.0.1:4173 --foto <scratchpad>/foto
```

Poi si guarda **a occhio** ogni pagina con il Playwright MCP (desktop 1600 per 900, verticale 1200 per 1760,
telefono 390 per 844):
etichette che non si sovrappongono, il grafico grande nella prima schermata, la barra del tempo, i badge con
l'eta', nessuno scorrimento orizzontale. Si corregge cio' che non si legge, si chiude il browser. Le foto vanno nella scratchpad, mai nel repo; `node_modules/` e
`dist/` sono in `.gitignore`.
