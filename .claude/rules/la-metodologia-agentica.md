---
paths:
  - ".claude/skills/**"
  - ".claude/agents/**"
  - "studio/**"
---

# La Metodologia Agentica

Si carica quando si toccano skill, agenti o lo studio. Il documento e'
`studio/la-metodologia-agentica.md` (le dodici regole); i due sistemi che la applicano e le lezioni
del primo giro, `studio/il-sistema-di-studio.md`.

- **Prima si raccoglie, poi si legge**: un collettore deterministico prende tutti i dati, gli agenti
  d'analisi partono dopo, su file, **senza Bash** (`tools:` lo impone). Una misura che manca e' una
  lacuna del collettore: si dichiara e si aggiunge al codice, non si rifa' a mano.
- **Ogni calcolo si fa una volta, e lo studio e' veloce** (15 ottobre 2026): una sola GEX per giro e
  studio, il giro una volta dopo la GEX, nessun agente che ricalcola; lo studio intero sotto i dieci minuti.
- **Una fase, un agente** (`model: opus`, `maxTurns`, skill in `skills:`), **una skill** che rimanda
  ai documenti del metodo. In parallelo cio' che e' indipendente; il confronto lo fa solo la sintesi.
- **L'orchestratore non studia**; **chi verifica non e' chi ha scritto**; la persona entra solo per
  Cloudflare, login, scelte. Dentro gli agenti valgono le regole del repo.
- **Le risorse condivise hanno un lucchetto** (un chart di ATAS, una cache).
- **Descrizioni con `: ` fra virgolette** nel frontmatter degli agenti, se no l'agente non si carica in
  silenzio. Si valida con `claude plugin validate .claude/skills` e `.claude/agents`; si carica in una
  sessione nuova; si prova per intero con `claude -p` prima di dirlo pronto.
