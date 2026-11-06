// Volatilita', "il meteo" (C.10): in cima, per Nasdaq 100 e poi S&P 500, quanto e' alta la paura, quanto
// si puo' muovere il future oggi in prezzi, se l'assicurazione e' cara e se le fonti stile VIX convergono
// (il verdetto lo da' il server). Sotto, sei grafici in griglia a due, NDX a sinistra: nel tempo, per
// scadenza, per strike. L'ordine e' sempre NQ prima, ES dopo (M8).

import { useVolInBreve, useVolSerie, useVolSorriso, useVolTermine } from '../../api/domande';
import type { Indice } from '../../api/tipi';
import { useFiltri } from '../../stato/contesto';
import { Scheda } from '../../componenti/Scheda';
import { Esito } from '../../componenti/Esito';
import { Meteo } from './Meteo';
import { Sorriso, StrutturaTermine, VolNelTempo } from './GraficiVol';
import './volatilita.css';

/** NQ prima, ES dopo. */
const INDICI: readonly Indice[] = ['NDX', 'SPX'];

const NOME_INDICE: Record<Indice, string> = { SPX: 'S&P 500 (SPX, ES)', NDX: 'Nasdaq 100 (NDX, NQ)' };

const FONTI_TEMPO: Record<Indice, ('catena' | 'yahoo' | 'xray' | 'cboe')[]> = { SPX: ['catena', 'yahoo', 'xray'], NDX: ['catena', 'yahoo', 'cboe'] };
const FONTI_CURVE: Record<Indice, ('catena' | 'xray' | 'cboe')[]> = { SPX: ['catena', 'xray'], NDX: ['catena', 'cboe'] };

export function Volatilita() {
  const f = useFiltri();
  const qs = useVolSerie(f.giorno);
  const qt = useVolTermine();
  const qspx = useVolSorriso('SPX');
  const qndx = useVolSorriso('NDX');
  const qb = useVolInBreve();
  const sorriso = { SPX: qspx, NDX: qndx } as const;

  return (
    <div className="griglia volatilita">
      <div className="s12 griglia meteo-riga">
        <Esito q={qb} rotta="vol/in-breve" altezza={220}>
        {(d) => (
          <>
            {INDICI.map((i) => d.indici.find((x) => x.indice === i))
              .filter((x) => x != null)
              .map((x) => (
                <Meteo key={x.indice} x={x} adesso={d.quando} />
              ))}
          </>
        )}
        </Esito>
      </div>

      {INDICI.map((indice) => (
        <Scheda
          key={`tempo-${indice}`}
          titolo={`Nel tempo · ${NOME_INDICE[indice]}`}
          fonti={FONTI_TEMPO[indice]}
          larghezza={6}
          breve="La paura minuto per minuto, da ogni fonte: se le linee si muovono insieme, le fonti vedono la stessa cosa."
          semplice={
            indice === 'SPX'
              ? 'Il VIX ricostruito da noi sulle opzioni ES, il VIX ufficiale, e la volatilità al prezzo (ATM) stimata da X-Ray. L’ATM sta sotto il VIX per costruzione: si guarda la forma, non il livello.'
              : 'Il VXN (il VIX del Nasdaq 100) ricostruito da noi sulle opzioni NQ, quello ufficiale, e le ricostruzioni sulle catene pubbliche di NDX e QQQ del Cboe.'
          }
          sotto="Volatilità a 30 giorni (e simili) in punti percentuali; orari di New York, con l’ora italiana nel tooltip."
        >
          <Esito q={qs} rotta="vol/serie" altezza={300}>
            {(d) => <VolNelTempo serie={d.serie} indice={indice} />}
          </Esito>
        </Scheda>
      ))}

      {INDICI.map((indice) => (
        <Scheda
          key={`termine-${indice}`}
          titolo={`Struttura a termine · ${indice}`}
          fonti={FONTI_CURVE[indice]}
          larghezza={6}
          breve="La volatilità per scadenza: se le vicine costano più delle lontane, il mercato teme qualcosa a breve."
          semplice="La volatilità al prezzo per ogni scadenza, dalla più vicina alla più lontana. Di solito sale con la scadenza (calma); se una scadenza si gonfia, si guarda quale evento ci cade dentro (le linee tratteggiate sono gli eventi del calendario)."
          sotto="Volatilità ATM per scadenza; giorni alla scadenza su scala logaritmica. Un punto vuoto è un dato sospetto (più del doppio della mediana della curva), segnalato dal server."
        >
          <Esito q={qt} rotta="vol/termine" altezza={300}>
            {(d) => <StrutturaTermine curve={d.curve} indice={indice} eventi={d.eventi} />}
          </Esito>
        </Scheda>
      ))}

      {INDICI.map((indice) => (
        <Scheda
          key={`sorriso-${indice}`}
          titolo={`Sorriso · ${indice}`}
          fonti={FONTI_CURVE[indice]}
          larghezza={6}
          breve="Quanto costa l’assicurazione strike per strike: se la curva sale verso sinistra, proteggersi dal ribasso costa di più."
          semplice="La scadenza vicina ai 30 giorni. A sinistra gli strike sotto il prezzo: se sono più cari, il mercato paga di più per proteggersi da un ribasso. È un prezzo, non una previsione."
          sotto="Moneyness = strike / forward − 1: fonti con prezzi diversi (ES e SPX, NQ e NDX) si sovrappongono."
        >
          <Esito q={sorriso[indice]} rotta={`vol/sorriso?indice=${indice}`} altezza={300}>
            {(d) => <Sorriso curve={d.curve} indice={indice} />}
          </Esito>
        </Scheda>
      ))}
    </div>
  );
}
