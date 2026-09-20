---
name: il-contesto-sta-nel-repo
description: Cosa va tenuto in memoria e cosa nel repo: qui l'orientamento, li' le regole
metadata:
  type: project
---

**La divisione, e va rispettata in entrambe le direzioni.**

| | dove | perche' |
|---|---|---|
| **le regole, il metodo, le procedure** | nel **repo** | viaggiano con `git`, si versionano, e `CLAUDE.md` le impone con un obbligo di lettura |
| **l'orientamento: dove siamo, come ci siamo arrivati** | **qui** | serve prima di aver letto qualcosa, a inizio sessione |

**Perche' non il contrario.** Il 20 settembre 2026, spostando il repo dal Mac a un PC, la memoria
non e' arrivata: vive in `~/.claude/projects/<percorso>/memory/`, fuori da `git`, con un nome
derivato dal percorso e quindi diverso su ogni macchina. Ma il problema vero e' emerso
controllandola: degli undici file, otto duplicavano documenti gia' nel repo e **due erano
scaduti** — dicevano il contrario di `CLAUDE.md`, che nel frattempo era cambiato (la deroga sul
gate orario, e il dossier descritto come fonte che comanda). **Una memoria che diverge dal repo
non si accorge di divergere.**

**Questi file sono una copia, non l'originale.** Stanno versionati in
`docs/research/memoria-di-orientamento/` e si installano con
`python3 FabioOrderFlow/tools/semina_memoria.py`, che trova da solo la cartella di memoria della
macchina in uso. Modificarli qui a mano crea proprio la divergenza descritta sopra: si modifica il
file nel repo e si rilancia il comando.

**How to apply:** un fatto con conseguenza operativa — una regola, una soglia, una procedura — si
scrive **nel repo**, nel documento pertinente, con la riga datata in `FabioOrderFlow/progress.txt`.
Qui restano solo [[dove-siamo]] e [[la-storia-del-progetto]], che vanno **aggiornati** quando
cambia la fase, non lasciati invecchiare.
