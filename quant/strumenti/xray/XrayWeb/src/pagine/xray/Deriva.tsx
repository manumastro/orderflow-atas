// Deriva (5 novembre 2026, C.7 del progetto): la giornata minuto per minuto in quattro pannelli legati,
// uno sotto l'altro con lo stesso tempo. In cima i numeri dell'ultimo istante (deriva.adesso), calcolati
// dal server. Ogni pannello ha il suo asse; il tempo e' scritto solo nell'ultimo. Il cursore e' legato:
// il tooltip compare solo nel pannello sotto il mouse, i valori al cursore vanno nelle legende di tutti.
// I minuti anomali del delta accumulato (deriva.anomali) li marca il server: la linea si interrompe, con
// un segno, e la scala si calcola senza quei punti. I valori non si toccano.

import { useCallback, useMemo, useState, type ReactNode } from 'react';
import type { CustomSeriesOption, LineSeriesOption } from 'echarts';
import { useMisura } from '../../api/domande';
import type { Gruppo, MinutoAnomalo, RispostaDeriva, Scadenza } from '../../api/tipi';
import { auto, doppiaOra, nome, num, oraDoppiaCorta, oraNY } from '../../formato';
import { indicePrima, useFiltri, type Filtri } from '../../stato/contesto';
import { useScelte } from '../../stato/scelte';
import { C, INCHIOSTRI } from '../../tema/colori';
import { Grafico, type Altezza } from '../../componenti/Grafico';
import { Legenda, type Voce } from '../../componenti/Legenda';
import { Scheda } from '../../componenti/Scheda';
import { Tessera } from '../../componenti/Tessera';
import { Esito, Vuoto } from '../../componenti/Esito';
import { Segmenti } from '../../componenti/Comandi';
import { Termine } from '../../componenti/Termine';
import type { Opzioni } from '../../viz/echarts';
import {
  INCHIOSTRI_LINEA,
  asseTempo,
  asseValori,
  asseZero,
  croce,
  distanzia,
  etichetteFinali,
  fasciaSegno,
  lineaPrezzo,
  parolaSegno,
  puntiTip,
  rigaTip,
  testaTip,
  tooltip,
  valoriDi,
} from '../../viz/grafici';
import './xray2.css';

const GRUPPO = 'deriva';
const SCADENZE: Scadenza[] = ['0dte', 'next_expiry', 'opex'];
const NOMI_SCADENZA: Record<Scadenza, string> = { '0dte': '0DTE', next_expiry: 'prossima', opex: 'opex' };
/** il Retail per primo: e' l'inchiostro pieno (INCHIOSTRI[0]); gli altri tre seguono */
const GRUPPI: Gruppo[] = ['nonprocust', 'firm', 'broker', 'procust'];
const NOMI_GRUPPO: Record<Gruppo, string> = { nonprocust: 'Retail', firm: 'Firm', broker: 'Broker', procust: 'Prof.' };
const MOTIVI: Record<MinutoAnomalo['motivo'], string> = {
  finestra: 'il minuto copre una finestra diversa da 60 s',
  scadenza: 'passaggio di scadenza alle 16:00',
  salto: 'salto molto piu’ grande del solito',
  partenza: 'la somma parte gia’ lontana da zero',
};

type Ambito = 'zero' | 'tutte';
type Serie = LineSeriesOption | CustomSeriesOption;

/** Il margine sinistro e destro uguale in tutti i pannelli: i tempi stanno uno sotto l'altro. */
const SINISTRA = 64;
const destra = (larghezza: number) => (etichetteFinali(larghezza) ? 96 : 12);

/** Un valore col segno; lo zero esatto si scrive 0 (non 0,000). */
const v0 = (v: number | null | undefined): string => (v === 0 ? '0' : auto(v));

/** Una variazione in inchiostro: ▲ +120, ▼ −180 (mai verde o rosso: si leggerebbe come un segnale). */
function variazione(adesso: number | null | undefined, prima: number | null | undefined): string {
  if (adesso == null || prima == null) return '–';
  const v = adesso - prima;
  return `${v > 0 ? '▲' : v < 0 ? '▼' : '='} ${v0(v)}`;
}

const tonoGamma = (v: number | null | undefined) => (v == null ? null : v >= 0 ? 'freno' : 'acceleratore');
const tonoFlusso = (v: number | null | undefined) => (v == null ? null : v >= 0 ? 'compra' : 'vende');

export function Deriva() {
  const f = useFiltri();
  const [s, setS] = useScelte<{ passo: string; ambito: Ambito }>('deriva', { passo: '1', ambito: 'zero' });
  const q = useMisura<RispostaDeriva>('deriva', { giorno: f.giorno, passo: s.passo }, f.vivo, f.pronto);
  const a = q.data?.adesso ?? null;

  return (
    <>
      <div className="griglia tessere">
        <Tessera
          etichetta="Gamma al prezzo"
          fonte="xray"
          tag="non provato"
          tono={tonoGamma(a?.gammaAlPrezzo)}
          valore={auto(a?.gammaAlPrezzo)}
          dettaglio={a ? `${a.regime ?? '–'}${a.regimeDa != null ? ` da ${oraDoppiaCorta(a.regimeDa)}` : ''}` : undefined}
          larga
        />
        <Tessera
          etichetta="GEX netta adesso"
          fonte="xray"
          tono={tonoGamma(a?.gex.totale)}
          valore={auto(a?.gex.totale)}
          dettaglio={a ? `0DTE ${v0(a.gex['0dte'])} · pross. ${v0(a.gex.next_expiry)} · opex ${v0(a.gex.opex)}` : undefined}
        />
        <Tessera
          etichetta="Retail sugli 0DTE"
          fonte="xray"
          tono={tonoFlusso(a?.retail0dte)}
          valore={auto(a?.retail0dte)}
          dettaglio={a ? `da inizio giornata · ${variazione(a.retail0dte, a.retail0dteUnOraFa)} in un’ora` : undefined}
        />
        <Tessera
          etichetta="Pressione adesso"
          fonte="xray"
          valore={auto(a?.pressione)}
          dettaglio={a ? `${variazione(a.pressione, a.pressioneTrentaMinutiFa)} in 30 min · verso non dichiarato` : undefined}
        />
      </div>
      <Esito q={q} rotta="deriva">
        {(d) =>
          d.tempi.length ? (
            <Pannelli d={d} f={f} ambito={s.ambito} passo={s.passo} scegli={(x) => setS((v) => ({ ...v, ...x }))} />
          ) : (
            <Vuoto>Nessun dato in questa giornata.</Vuoto>
          )
        }
      </Esito>
    </>
  );
}

interface PropsPannelli {
  d: RispostaDeriva;
  f: Filtri;
  ambito: Ambito;
  passo: string;
  scegli: (x: Partial<{ passo: string; ambito: Ambito }>) => void;
}

/** Gli estremi di un asse che contiene sempre lo zero, con un po' di margine. */
function conZero(valori: Iterable<number | null | undefined>): [number, number] {
  let lo = 0;
  let hi = 0;
  for (const v of valori) {
    if (v == null || !Number.isFinite(v)) continue;
    lo = Math.min(lo, v);
    hi = Math.max(hi, v);
  }
  const m = (hi - lo || 1) * 0.06;
  return [lo - (lo < 0 ? m : 0), hi + (hi > 0 ? m : 0)];
}

function Pannelli({ d, f, ambito, passo, scegli }: PropsPannelli) {
  const P = f.p;
  const t = d.tempi;
  const ts = f.ts;
  const [indice, setIndice] = useState<number | null>(null);
  const cursore = useCallback((x: number | null) => setIndice(x == null ? null : indicePrima(t, x)), [t]);

  const dex = ambito === 'zero' ? d.dexZeroDte : d.dexTutte;
  const chiaveDex = ambito === 'zero' ? 'dexZeroDte' : 'dexTutte';

  // i minuti anomali della serie scelta: gruppo -> indici
  const anomali = useMemo(() => {
    const m = new Map<Gruppo, Map<number, MinutoAnomalo>>();
    for (const x of d.anomali) {
      const [serie, g] = x.serie.split('.') as [string, Gruppo];
      if (serie !== chiaveDex) continue;
      if (!m.has(g)) m.set(g, new Map());
      m.get(g)!.set(x.indice, x);
    }
    return m;
  }, [d.anomali, chiaveDex]);

  // le serie del delta con i minuti anomali tolti (null: la linea si interrompe)
  const dexPulito = useMemo(() => {
    const r = {} as Record<Gruppo, (number | null)[]>;
    for (const g of GRUPPI) {
      const via = anomali.get(g);
      r[g] = (dex[g] ?? []).map((v, i) => (via?.has(i) ? null : v));
    }
    return r;
  }, [dex, anomali]);

  const opzioni = useMemo(() => {
    const coppie = (valori: readonly (number | null)[]): [number, number | null][] => t.map((x, i) => [x, valori[i] ?? null]);
    const istante: LineSeriesOption[] = ts
      ? [{ type: 'line', name: 'istante', data: [], markLine: { symbol: 'none', silent: true, animation: false, data: [{ xAxis: ts }], lineStyle: { color: C.testo2, width: 1.5 }, label: { show: false } } }]
      : [];
    const base = (
      larghezza: number,
      ultimo: boolean,
      serie: Serie[],
      y: Record<string, unknown>,
      scrivi: (v: number) => string,
      extraTip: (i: number) => string = () => '',
    ): Opzioni => ({
      animation: false,
      grid: { left: SINISTRA, right: destra(larghezza), top: 8, bottom: ultimo ? 24 : 6 },
      xAxis: asseTempo(t[0] ?? 0, t.at(-1) ?? 0, {
        axisLabel: { show: ultimo, color: C.spento, fontSize: 11, formatter: (ms: number) => oraNY(ms), hideOverlap: true },
        name: ultimo ? 'ora di New York' : '',
        splitLine: { show: true, lineStyle: { color: C.griglia, width: 1 } },
      }),
      yAxis: asseValori({ ...y, splitNumber: 3, axisLabel: { color: C.spento, fontSize: 11, formatter: scrivi, showMinLabel: false, showMaxLabel: false } }),
      tooltip: tooltip({
        trigger: 'axis',
        axisPointer: croce('xy', { x: (v) => oraDoppiaCorta(v), y: scrivi }),
        formatter: (p: unknown) => {
          const lista = puntiTip(p);
          const x = valoriDi(lista[0] ?? { value: [0], name: '', dataIndex: 0 })[0] ?? 0;
          const righe = lista
            .filter((q) => Array.isArray(q.value) && q.value[1] != null && q.seriesName !== 'istante')
            .map((q) => rigaTip(typeof q.color === 'string' ? q.color : null, q.seriesName ?? '', scrivi(valoriDi(q)[1] ?? 0)))
            .join('');
          return testaTip(doppiaOra(x)) + righe + extraTip(indicePrima(t, x));
        },
      }),
      dataZoom: [{ type: 'inside', xAxisIndex: 0 }],
      series: [...serie, ...istante],
    });

    // 1. il prezzo, e sotto la fascia del segno della gamma al prezzo
    const prezzi = d.riferimento.map((r) => (r == null ? null : P(r)));
    const validi = prezzi.filter((x): x is number => x != null);
    const lo = Math.min(...validi);
    const hi = Math.max(...validi);
    const spazio = (hi - lo || 1) * 0.12;
    const prezzo = (larghezza: number) =>
      base(
        larghezza,
        false,
        [
          // sotto i 600 px il cartellino non ha spazio a destra: il valore sta nella legenda
          ...lineaPrezzo(t, prezzi, d.vivo, `prezzo ${f.unita}`).map((x) => (etichetteFinali(larghezza) ? x : { ...x, endLabel: { show: false } })),
          fasciaSegno(t, d.gammaAlPrezzo),
        ],
        // lo spazio in fondo per la fascia di 8 px
        { min: lo - spazio * 1.6, max: hi + spazio * 0.4 },
        (v) => num(v, 0),
        (i) => {
          const g = d.gammaAlPrezzo[i];
          return g == null ? '' : rigaTip(null, `gamma al prezzo · ${parolaSegno(g)}`, auto(g));
        },
      );

    // 2. la GEX netta per scadenza: tre inchiostri, il nome in fondo, lo zero sempre in vista
    const [gLo, gHi] = conZero(SCADENZE.flatMap((k) => d.gex[k] ?? []));
    const gex = (larghezza: number) => {
      const fine = etichetteFinali(larghezza);
      const ultimi = SCADENZE.map((k) => (d.gex[k] ?? []).findLast((v) => v != null) ?? 0);
      const sposta = distanzia(ultimi, [gLo, gHi], altezzaPannello - 14);
      return base(
        larghezza,
        false,
        SCADENZE.map(
          (k, i): LineSeriesOption => ({
            type: 'line',
            name: NOMI_SCADENZA[k],
            data: coppie(d.gex[k] ?? []),
            showSymbol: false,
            lineStyle: INCHIOSTRI_LINEA(i),
            itemStyle: { color: INCHIOSTRI[i]!.colore },
            endLabel: fine ? { show: true, formatter: NOMI_SCADENZA[k], color: C.testo2, fontSize: 11, offset: [4, sposta[i] ?? 0] } : { show: false },
            ...(i === 0 ? { markLine: asseZero('y') } : {}),
          }),
        ),
        { min: gLo, max: gHi },
        v0,
      );
    };

    // 3. il delta accumulato per gruppo, con i minuti anomali interrotti e segnati
    const [dLo, dHi] = conZero(GRUPPI.flatMap((g) => dexPulito[g]));
    const delta = (larghezza: number) => {
      const fine = etichetteFinali(larghezza);
      const ultimi = GRUPPI.map((g) => dexPulito[g].findLast((v) => v != null) ?? 0);
      const sposta = distanzia(ultimi, [dLo, dHi], altezzaPannello - 14);
      return base(
        larghezza,
        false,
        GRUPPI.map((g, i): LineSeriesOption => {
          const via = anomali.get(g);
          const segni = [...(via?.values() ?? [])].map((x) => {
            // il segno sta sull'ultimo punto buono prima del minuto anomalo, dentro la scala
            const prima = dexPulito[g].slice(0, x.indice).findLast((v) => v != null) ?? 0;
            return { coord: [x.ts, Math.min(dHi, Math.max(dLo, prima))], name: `${NOMI_GRUPPO[g]}: ${MOTIVI[x.motivo]}`, value: x.motivo };
          });
          return {
            type: 'line',
            name: NOMI_GRUPPO[g],
            data: coppie(dexPulito[g]),
            showSymbol: false,
            connectNulls: false,
            lineStyle: INCHIOSTRI_LINEA(i),
            itemStyle: { color: INCHIOSTRI[i]!.colore },
            endLabel: fine ? { show: true, formatter: NOMI_GRUPPO[g], color: C.testo2, fontSize: 11, offset: [4, sposta[i] ?? 0] } : { show: false },
            ...(i === 0 ? { markLine: asseZero('y') } : {}),
            ...(segni.length
              ? {
                  markPoint: {
                    symbol: 'circle',
                    symbolSize: 1,
                    silent: true,
                    data: segni,
                    label: { show: true, formatter: '⚠', color: C.testo, fontSize: 12, position: 'top', distance: 2 },
                    itemStyle: { color: C.testo2 },
                  },
                }
              : {}),
          };
        }),
        { min: dLo, max: dHi },
        v0,
        (i) =>
          GRUPPI.map((g) => anomali.get(g)?.get(i))
            .filter((x): x is MinutoAnomalo => x != null)
            .map((x) => rigaTip(null, `⚠ ${NOMI_GRUPPO[x.serie.split('.')[1] as Gruppo]}: ${MOTIVI[x.motivo]}`, auto(dex[x.serie.split('.')[1] as Gruppo]?.[i])))
            .join(''),
      );
    };

    // 4. la pressione 0DTE: un inchiostro, lo zero
    const [pLo, pHi] = conZero(d.pressione);
    const pressione = (larghezza: number) =>
      base(
        larghezza,
        true,
        [{ type: 'line', name: 'pressione 0DTE', data: coppie(d.pressione), showSymbol: false, lineStyle: INCHIOSTRI_LINEA(0), itemStyle: { color: INCHIOSTRI[0].colore }, markLine: asseZero('y') }],
        { min: pLo, max: pHi },
        v0,
      );

    return { prezzo, gex, delta, pressione };
  }, [d, t, ts, P, f.unita, dexPulito, anomali, dex]);

  // i valori nelle legende: sotto il cursore, o l'ultimo istante
  const i = indice ?? t.length - 1;
  // senza cursore, l'ultimo valore che c'e' (dopo le 16:00 alcune serie si fermano)
  const al = (serie: readonly (number | null)[] | undefined) => (serie == null ? null : indice != null ? (serie[indice] ?? null) : (serie.findLast((v) => v != null) ?? null));
  const quando = indice != null ? <span className="nota">alle {oraDoppiaCorta(t[i])}</span> : null;
  const g = al(d.gammaAlPrezzo);
  const nAnomali = [...anomali.values()].reduce((n, m) => n + m.size, 0);

  const pannello = (titolo: string, breve: ReactNode, semplice: string, voci: Voce[], o: (l: number) => Opzioni, altezza: Altezza, comandi?: ReactNode, extra?: ReactNode) => (
    <Scheda
      titolo={titolo}
      fonti={['xray']}
      breve={breve}
      semplice={semplice}
      comandi={comandi}
      legenda={
        <Legenda voci={voci}>
          {extra}
          {quando}
        </Legenda>
      }
    >
      <Grafico opzioni={o} altezza={altezza} gruppo={GRUPPO} etichetta={titolo} onCursore={cursore} tooltipSoloQui />
    </Scheda>
  );

  const intervallo = (
    <Segmenti<string>
      etichetta="Intervallo"
      valore={passo}
      cambia={(v) => scegli({ passo: v })}
      opzioni={[
        { valore: '1', testo: '1 min' },
        { valore: '5', testo: '5 min' },
        { valore: '15', testo: '15 min' },
      ]}
    />
  );

  return (
    <div className="griglia deriva-pannelli">
      {pannello(
        'Il prezzo e il regime',
        <>
          Il prezzo, e sotto una fascia: verde dove la gamma al prezzo <Termine k="freno">frena</Termine>, rossa dove{' '}
          <Termine k="acceleratore">accelera</Termine>.
        </>,
        'La linea bianca è il prezzo (piena in cassa, dove è lo spot di SPX; tratteggiata fuori, dove è ES meno la base). La fascia in fondo dice, minuto per minuto, se la gamma dei market maker proprio al prezzo frenava (verde) o accelerava (rosso) il movimento. È una stima di OptionsDepth, non provata.',
        [
          { colore: C.prezzo, testo: `prezzo ${f.unita}`, valore: al(d.riferimento) == null ? null : num(P(al(d.riferimento)!), 2) },
          { colore: C.freno, testo: 'freno', forma: 'rett' },
          { colore: C.acceleratore, testo: 'acceleratore', forma: 'rett' },
          { colore: g == null ? C.neutro : g >= 0 ? C.freno : C.acceleratore, testo: 'gamma al prezzo', forma: 'rett', valore: g == null ? null : `${auto(g)} · ${parolaSegno(g)}` },
        ],
        opzioni.prezzo,
        'basso',
        intervallo,
      )}
      {pannello(
        'GEX netta per scadenza',
        <>
          Sopra lo zero, nel complesso i market maker frenano; sotto, accelerano. Una linea per scadenza.
        </>,
        'La GEX netta dei market maker, separata per le opzioni che scadono oggi (0DTE), alla prossima scadenza e all’opex del mese: si vede come cambia durante la giornata. Le tre linee sono inchiostri diversi, senza colore di segno: il segno si legge dalla posizione rispetto allo zero.',
        SCADENZE.map((k, n) => ({ colore: INCHIOSTRI[n]!.colore, tratto: INCHIOSTRI[n]!.tratto as 'solid' | 'dashed' | 'dotted', testo: NOMI_SCADENZA[k], valore: v0(al(d.gex[k])) })),
        opzioni.gex,
        'basso',
        undefined,
        <span className="nota">{d.unita.gex}</span>,
      )}
      {pannello(
        'Delta accumulato per gruppo',
        <>
          Il <Termine k="DEX">delta</Termine> che ogni gruppo ha comprato (+) o venduto (−) da inizio giornata: un flusso, non una posizione.
        </>,
        'La somma, minuto dopo minuto, del delta scambiato da ogni tipo di partecipante: Retail, Firm (proprietari), Broker-dealer e clienti professionali. Dice chi ha comprato o venduto rialzo oggi, non chi è posizionato come. I minuti segnati con ⚠ sono anomali (il server li marca: una finestra diversa da un minuto, il passaggio di scadenza, un salto fuori misura): la linea si interrompe lì.',
        GRUPPI.map((k, n) => {
          const v = al(dex[k]);
          return {
            colore: INCHIOSTRI[n]!.colore,
            tratto: (typeof INCHIOSTRI[n]!.tratto === 'string' ? INCHIOSTRI[n]!.tratto : 'dashed') as 'solid' | 'dashed' | 'dotted',
            testo: nome(k),
            valore: v == null ? null : `${auto(v)}${anomali.get(k)?.has(i) ? ' ⚠' : ''}`,
          };
        }),
        opzioni.delta,
        'basso',
        <Segmenti<Ambito>
          etichetta="Scadenze del delta"
          valore={ambito}
          cambia={(v) => scegli({ ambito: v })}
          opzioni={[
            { valore: 'zero', testo: 'Solo oggi (0DTE)' },
            { valore: 'tutte', testo: 'Tutte le scadenze' },
          ]}
        />,
        <span className="nota anomali-nota">
          {nAnomali ? `⚠ ${nAnomali} ${nAnomali === 1 ? 'minuto anomalo' : 'minuti anomali'}, linea interrotta · ` : ''}
          {d.unita.dex}
        </span>,
      )}
      {pannello(
        'Pressione 0DTE',
        'Il delta che i market maker devono coprire sulle opzioni che scadono oggi; il verso del segno non è dichiarato.',
        'La Hedge Pressure di OptionsDepth sulle 0DTE: quanto delta i market maker dovrebbero comprare o vendere per restare coperti. Il significato del segno non è ancora scritto nei documenti, quindi la linea non ha colore di comprato o venduto.',
        [{ colore: INCHIOSTRI[0].colore, testo: 'pressione 0DTE', valore: auto(al(d.pressione)) }],
        opzioni.pressione,
        { min: 174, max: 194, schermo: 0.215 },
        undefined,
        <span className="nota">{d.unita.pressione}</span>,
      )}
    </div>
  );
}

/** L'altezza utile di un pannello "basso" per distanziare i nomi in fondo alle linee (circa). */
const altezzaPannello = 150;
