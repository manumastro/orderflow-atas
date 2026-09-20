# Build E Deploy

Stato: **procedura**. Come il codice di questo repository arriva su ATAS, e le tre cose che un
`git pull` **non** porta.

```bash
cd FabioOrderFlow/src
./deploy.sh
```

Target `net10.0` senza WPF, requisito di ATAS X. Gli assembly ATAS sono risolti dal bundle
dell'applicazione su macOS e da `Program Files` su Windows.

---

## Su ATAS X Non Serve Riavviare: Basta Il Deploy

L'indicatore viene ricaricato da solo, e in replay **la posizione non si perde** — verificato il
20 settembre 2026, ricaricando tre volte il motore dei livelli con il replay fermo alle 13:00Z.

**L'id del chart cambia a ogni ricarica**, quindi non va mai cablato da nessuna parte: ne' in un
hook, ne' in un file di giornata, ne' in un comando. Lo si chiede al bridge.

(Il vecchio ATAS 8 il riavvio lo richiedeva, ed e' da li' che veniva la regola precedente, che
diceva il contrario.)

---

## Un `git pull` Non Porta L'Indicatore

Nel repo c'e' il **sorgente C#, non la DLL**: `bin/` e `obj/` sono in `.gitignore`, e ATAS carica
il binario da `%APPDATA%\ATAS X\Indicators` (Windows) o da
`~/Library/Application Support/ATAS/Indicators` (macOS).

Finche' non si esegue `deploy.sh`, sul chart resta la versione precedente — **e non e'
distinguibile a occhio da un pull che non ha funzionato.**

Non porta nemmeno la **memoria dell'agente**, che vive in `~/.claude/projects/<percorso>/memory/`,
fuori dal repo.

---

## La Memoria Di Orientamento Si Installa, Non Si Scrive A Mano

Il contenuto e' versionato in
[`memoria-di-orientamento/`](memoria-di-orientamento/) e si deposita con:

```bash
python3 FabioOrderFlow/tools/semina_memoria.py
```

che trova da solo la cartella di memoria della macchina in uso.

In memoria va **solo l'orientamento** — dove siamo, come ci siamo arrivati. Le regole stanno in
`CLAUDE.md` e nei documenti, e **una memoria che le duplica prima o poi diverge senza accorgersene**.
E' gia' successo: due voci su undici dicevano il contrario di `CLAUDE.md`.

Su una macchina nuova, la sequenza completa e cosa va rifatto a mano stanno in
[`portare-il-repo-su-un-altra-macchina.md`](portare-il-repo-su-un-altra-macchina.md).

---

## Quando L'API ATAS Non E' Chiara

**Ispeziona gli assembly con reflection** invece di dedurla dalla documentazione: `docs/atas/` non
sempre coincide con la build ATAS X installata.

Non modificare `docs/atas/api/` salvo necessita' tecnica concreta.
