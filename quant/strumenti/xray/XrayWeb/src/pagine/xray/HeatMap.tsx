// Heat map (C.5): strike per riga, scadenze (o gruppi) per colonna, il valore dentro la cella. Una tabella
// sola, alta quanto la finestra; in testa a ogni colonna la sua netta. Il colore segue il significato:
// freno e acceleratore per la GEX, comprato e venduto per DEX e VEX.

import { useMisura } from '../../api/domande';
import type { RispostaHeatmap } from '../../api/tipi';
import { sig } from '../../formato';
import { useFiltri } from '../../stato/contesto';
import { useScelte } from '../../stato/scelte';
import { Scheda } from '../../componenti/Scheda';
import { Esito, Vuoto } from '../../componenti/Esito';
import { Legenda } from '../../componenti/Legenda';
import { Divisore, Segmenti } from '../../componenti/Comandi';
import { Termine } from '../../componenti/Termine';
import { SEGNO_MARCA, TabellaHeat, calcolaHeat } from '../../componenti/TabellaHeat';
import { coloreSegno, segnoFlusso, segnoGamma } from '../../viz/grafici';
import { C } from '../../tema/colori';
import './xray1.css';

type MetricaH = 'gex' | 'dex' | 'vex';
type Ambito = '0dte' | 'all';
type Numeri = 'forti' | 'tutti';

interface Scelte {
  metrica: MetricaH;
  ambito: Ambito;
  ampiezza: string;
  numeri: Numeri;
}

export function HeatMap() {
  const f = useFiltri();
  const [s, setS] = useScelte<Scelte>('heatmap', { metrica: 'gex', ambito: '0dte', ampiezza: '1', numeri: 'forti' });
  const q = useMisura<RispostaHeatmap>('heatmap', { ...f.parametri, metrica: s.metrica, ambito: s.ambito, ampiezza: s.ampiezza }, f.vivo, f.pronto);
  const dex = s.metrica === 'dex';
  const scala = s.metrica === 'gex' ? segnoGamma : segnoFlusso;

  const comandi = (
    <>
      <Segmenti<MetricaH>
        etichetta="Misura"
        valore={s.metrica}
        cambia={(m) => setS((x) => ({ ...x, metrica: m }))}
        opzioni={[
          { valore: 'gex', testo: 'GEX' },
          { valore: 'dex', testo: 'DEX' },
          { valore: 'vex', testo: 'VEX' },
        ]}
      />
      {s.metrica === 'vex' && <span className="tag">sperimentale</span>}
      {dex && (
        <Segmenti<Ambito>
          etichetta="Scadenze"
          valore={s.ambito}
          cambia={(a) => setS((x) => ({ ...x, ambito: a }))}
          opzioni={[
            { valore: '0dte', testo: '0DTE' },
            { valore: 'all', testo: 'Tutte' },
          ]}
        />
      )}
      <Divisore />
      <Segmenti<string>
        etichetta="Ampiezza"
        valore={s.ampiezza}
        cambia={(a) => setS((x) => ({ ...x, ampiezza: a }))}
        opzioni={['1', '2', '3'].map((a) => ({ valore: a, testo: `±${a}%` }))}
      />
      <Divisore />
      <Segmenti<Numeri>
        etichetta="Numeri"
        valore={s.numeri}
        cambia={(n) => setS((x) => ({ ...x, numeri: n }))}
        opzioni={[
          { valore: 'forti', testo: 'Solo i forti' },
          { valore: 'tutti', testo: 'Tutti' },
        ]}
      />
    </>
  );

  const calcolo = q.data?.strike.length ? calcolaHeat(q.data) : null;
  const legenda = calcolo ? (
    <Legenda
      voci={[
        { colore: calcolo.minimoValore == null ? C.neutro : coloreSegno(calcolo.minimoValore, scala), forma: 'rett', testo: 'Minimo', valore: sig(calcolo.minimoValore, 2) },
        { colore: calcolo.massimoValore == null ? C.neutro : coloreSegno(calcolo.massimoValore, scala), forma: 'rett', testo: 'Massimo', valore: sig(calcolo.massimoValore, 2) },
      ]}
      scala={{ massimo: calcolo.massimo, segno: scala }}
    >
      <span>nodo = la cella più forte della colonna</span>
      <span>
        {SEGNO_MARCA.flip} flip · {SEGNO_MARCA.freno} freno più forte · {SEGNO_MARCA.acceleratore} acceleratore più forte
      </span>
    </Legenda>
  ) : undefined;

  return (
    <div className="griglia">
      <Scheda
        titolo="Esposizione per strike e per scadenza"
        fonti={['xray']}
        comandi={comandi}
        legenda={legenda}
        breve={
          dex
            ? 'Ogni colonna è un gruppo: blu se su quello strike ha comprato delta oggi, arancio se ne ha venduto.'
            : 'Ogni colonna è una scadenza: dove si concentra il peso e se le scadenze dicono la stessa cosa.'
        }
        semplice={
          dex ? (
            <p>
              Ogni colonna è un gruppo di chi scambia. Blu: il gruppo ha comprato <Termine k="DEX">delta</Termine> da inizio giornata su
              quello strike (si è caricato di rialzo); arancio: ne ha venduto. È un flusso, non una posizione. La linea bianca fra due
              righe è il prezzo; la cella contornata è la più forte della colonna.
            </p>
          ) : (
            <p>
              Ogni colonna è un gruppo di scadenze. {s.metrica === 'gex' ? (
                <>
                  Verde: lì i market maker <Termine k="freno">frenano</Termine> il prezzo; rosso: lo{' '}
                  <Termine k="acceleratore">accelerano</Termine>.
                </>
              ) : (
                <>
                  <Termine k="VEX">VEX</Termine>, sperimentale: blu, i market maker comprerebbero se la volatilità salisse; arancio,
                  venderebbero.
                </>
              )}{' '}
              Più il colore è pieno, più pesa. La linea bianca fra due righe è il prezzo; la cella contornata è la più forte della
              colonna. Se le colonne hanno i colori negli stessi posti, le scadenze dicono la stessa cosa. Non provato.
            </p>
          )
        }
        sotto="Unità di OptionsDepth: si leggono il segno e la forma. In testa la netta di ogni colonna, su tutti gli strike della famiglia (non solo la finestra). «Solo i forti» lascia vuote le celle sotto il 5% del massimo della colonna."
      >
        <Esito q={q} rotta="heatmap" altezza={500}>
          {(d) =>
            d.strike.length ? (
              <TabellaHeat dati={d} p={f.p} unita={f.unita} scala={scala} soloForti={s.numeri === 'forti'} />
            ) : (
              <Vuoto>Nessun dato a quest’ora per questa misura.</Vuoto>
            )
          }
        </Esito>
      </Scheda>
    </div>
  );
}
