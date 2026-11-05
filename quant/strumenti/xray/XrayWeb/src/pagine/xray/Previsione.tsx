// Previsione (5 novembre 2026, C.8 del progetto): la mappa che OptionsDepth calcola in avanti, per il
// resto della seduta o per la prossima, con la traccia del flip previsto (previsione.zeri, dal server).
// Un solo comando del tempo: l'istante della barra sceglie la previsione; le frecce della tessera
// "Calcolata alle" passano alla previsione precedente o successiva (f.scegliIstante). In cima la risposta
// (previsione.sintesi); a destra i livelli della prossima seduta (adesso.scala), che prima stavano in Livelli.

import { useCallback, useMemo, useState } from 'react';
import type { LineSeriesOption } from 'echarts';
import { ActionIcon } from '@mantine/core';
import { IconChevronLeft, IconChevronRight } from '@tabler/icons-react';
import { useMediaQuery } from '@mantine/hooks';
import { useMisura } from '../../api/domande';
import type { RispostaAdesso, RispostaPrevisione } from '../../api/tipi';
import { distanza, doppiaOra, etichettaGiorno, giornoCorto, giornoNY, num, oraDoppiaCorta, oraIT, oraNY } from '../../formato';
import { indicePrima, useFiltri, type Filtri } from '../../stato/contesto';
import { useScelte } from '../../stato/scelte';
import { C } from '../../tema/colori';
import { Grafico, altezzaPx } from '../../componenti/Grafico';
import { Legenda, type Voce } from '../../componenti/Legenda';
import { Scheda } from '../../componenti/Scheda';
import { Tessera } from '../../componenti/Tessera';
import { ScalaPrezzi, coloreLivello } from '../../componenti/ScalaPrezzi';
import { Esito, Vuoto } from '../../componenti/Esito';
import { Divisore, Segmenti } from '../../componenti/Comandi';
import { Termine } from '../../componenti/Termine';
import type { Opzioni } from '../../viz/echarts';
import {
  assePrezzi,
  asseTempo,
  cartellino,
  coloreDi,
  etichetteFinali,
  griglia,
  intervalloPrezzi,
  lineeOrizzontali,
  passoDi,
  rigaTip,
  scala98,
  serieLinee,
  testaTip,
  type LineaOrizzontale,
} from '../../viz/grafici';
import { disegnaMappa, serieMappa, valoreAl, type DatiMappa } from '../../viz/mappa';
import './xray2.css';

type Tipo = 'forecast' | 'nextday';
type GrecaP = 'gamma' | 'charm';

interface Scelte {
  tipo: Tipo;
  greca: GrecaP;
  ampiezza: string;
}

const parole = (greca: GrecaP) => (greca === 'charm' ? { pos: 'comprano col tempo', neg: 'vendono col tempo' } : { pos: 'freno', neg: 'acceleratore' });
const regimeParola = (r: string | null | undefined) =>
  r === 'freno' ? 'Freno' : r === 'acceleratore' ? 'Acceleratore' : r === 'neutro' ? 'Neutro' : '–';

/** Il flip previsto all'istante t: lo zero piu' vicino al prezzo del calcolo fra le tracce. */
function flipAl(d: RispostaPrevisione, t: number, rif: number | null): number | null {
  const passo = d.tempi.length > 1 ? (d.tempi[1] ?? 0) - (d.tempi[0] ?? 0) : 300_000;
  let migliore: number | null = null;
  for (const z of d.zeri) {
    let p: [number, number] | null = null;
    for (const q of z) if (q[0] <= t && t - q[0] <= passo * 1.5) p = q;
    if (p && (migliore == null || rif == null || Math.abs(p[1] - rif) < Math.abs(migliore - rif))) migliore = p[1];
  }
  return migliore;
}

export function Previsione() {
  const f = useFiltri();
  const telefono = useMediaQuery('(max-width: 767px)') ?? false;
  const [s, setS] = useScelte<Scelte>('previsione', { tipo: 'forecast', greca: 'gamma', ampiezza: '1.5' });
  // l'istante della barra sceglie la previsione: dal vivo, l'ultima
  const q = useMisura<RispostaPrevisione>('previsione', { giorno: f.giorno, tipo: s.tipo, greca: s.greca, asOf: f.ts ?? undefined }, f.vivo, f.pronto);
  const qa = useMisura<RispostaAdesso>('adesso', f.parametri, f.vivo, f.pronto);
  const [cursore, setCursore] = useState<number | null>(null);

  const d = q.data;
  const P = f.p;
  const rif = d?.riferimento?.prezzo ?? null;
  const sintesi = d?.sintesi ?? null;
  const fine = d?.tempi.at(-1) ?? null;
  const k = d ? d.asOfs.indexOf(d.asOf) : -1;
  const vai = (j: number) => {
    const a = d?.asOfs[j];
    if (a != null) f.scegliIstante(a);
  };
  const seduta = d?.seduta ?? null;
  const p = parole(s.greca);

  // la legenda: il flip previsto e il valore al prezzo, sotto il cursore o all'ultimo tempo
  const tLeg = cursore ?? fine;
  const iLeg = d && tLeg != null ? indicePrima(d.tempi, tLeg) : -1;
  const alPrezzo = d && iLeg >= 0 ? (d.alPrezzo[iLeg] ?? null) : null;
  const flipLeg = d && tLeg != null ? flipAl(d, tLeg, rif) : null;
  const voci: Voce[] = [
    { colore: C.prezzo, testo: `prezzo al calcolo`, valore: rif == null ? null : num(P(rif), 2) },
    { colore: C.flip, testo: 'flip previsto', tratto: 'dashed', valore: flipLeg == null ? null : num(P(flipLeg), 0) },
    {
      colore: alPrezzo == null ? C.neutro : alPrezzo >= 0 ? C.freno : C.acceleratore,
      testo: 'previsto al prezzo',
      forma: 'rett',
      valore: alPrezzo == null ? null : `${num(alPrezzo, Math.abs(alPrezzo) >= 10 ? 0 : 2)} · ${alPrezzo >= 0 ? p.pos : p.neg}`,
    },
  ];

  const comandi = (
    <>
      <Segmenti<Tipo>
        etichetta="Orizzonte"
        valore={s.tipo}
        cambia={(v) => setS((x) => ({ ...x, tipo: v }))}
        opzioni={[
          { valore: 'forecast', testo: 'Resto della seduta' },
          { valore: 'nextday', testo: 'Prossima seduta' },
        ]}
      />
      <Segmenti<GrecaP>
        etichetta="Greca"
        valore={s.greca}
        cambia={(v) => setS((x) => ({ ...x, greca: v }))}
        opzioni={[
          { valore: 'gamma', testo: 'Gamma' },
          { valore: 'charm', testo: 'Charm' },
        ]}
      />
      <Divisore />
      <Segmenti<string>
        etichetta="Ampiezza"
        valore={s.ampiezza}
        cambia={(v) => setS((x) => ({ ...x, ampiezza: v }))}
        opzioni={[
          { valore: '0.75', testo: '±0,75%' },
          { valore: '1.5', testo: '±1,5%' },
          { valore: '3', testo: '±3%' },
        ]}
      />
    </>
  );

  const prossima = (
    <Scheda
      titolo="Livelli della prossima seduta"
      fonti={['xray']}
      larghezza={4}
      className="livelli-scala"
      breve="Come OptionsDepth vede i livelli della prossima seduta, se le posizioni restano quelle di adesso."
      semplice="Flip e picchi della superficie outlook di OptionsDepth: la loro previsione dei livelli per la seduta indicata, con le posizioni di adesso. È il loro modello, non una misura, e nessun livello è provato."
    >
      <Esito q={qa} rotta="adesso">
        {(a) => {
          const righe = a.scala.filter((r) => r.quando === 'prossima seduta');
          const giorno = righe.find((r) => r.seduta)?.seduta ?? a.outlook?.seduta ?? null;
          return righe.length ? (
            <>
              {giorno && <p className="nota">Seduta di {etichettaGiorno(giorno)}</p>}
              <ScalaPrezzi righe={righe} prezzo={a.riferimento.prezzo} unita={f.unita} converti={P} mostraQuando={false} compatta={telefono} />
            </>
          ) : (
            <Vuoto>Nessuna previsione per la prossima seduta a questo istante.</Vuoto>
          );
        }}
      </Esito>
    </Scheda>
  );

  return (
    <>
      <div className="griglia tessere">
        <Tessera
          etichetta="Flip previsto"
          fonte="xray"
          larghezza={4}
          valore={
            sintesi?.flipFinale == null ? (
              '–'
            ) : (
              <>
                {num(P(sintesi.flipFinale), 0)}
                {rif != null && <span className="valore-dist"> {distanza(sintesi.flipFinale - rif, 0)}</span>}
              </>
            )
          }
          dettaglio={fine != null ? `alle ${oraDoppiaCorta(fine)}${seduta ? ` · seduta di ${giornoNY(fine)}` : ''} · distanza dal prezzo al calcolo` : undefined}
        />
        <Tessera
          etichetta="Regime previsto al prezzo"
          fonte="xray"
          larghezza={4}
          tag="non provato"
          tono={sintesi?.regimeFinale === 'freno' || sintesi?.regimeFinale === 'acceleratore' ? sintesi.regimeFinale : null}
          valore={regimeParola(sintesi?.regimeFinale)}
          dettaglio={
            sintesi?.cambiaAlle != null
              ? `cambia alle ${oraDoppiaCorta(sintesi.cambiaAlle)}`
              : fine != null
                ? `${sintesi?.regimeFinale ?? '–'} fino alle ${oraDoppiaCorta(fine)}, al prezzo del calcolo`
                : undefined
          }
          larga
        />
        <Tessera
          etichetta="Calcolata alle"
          fonte="xray"
          larghezza={4}
          valore={d ? `${oraNY(d.asOf)} NY` : '–'}
          dettaglio={
            <span className="dettaglio-riga">
              <span>{d ? `${oraIT(d.asOf)} Italia · ${giornoNY(d.asOf)} · ${k + 1} di ${d.asOfs.length}` : ''}</span>
              <span className="frecce-previsione">
                <ActionIcon size="sm" variant="default" aria-label="Previsione precedente" disabled={!d || k <= 0} onClick={() => vai(k - 1)}>
                  <IconChevronLeft size={14} />
                </ActionIcon>
                <ActionIcon size="sm" variant="default" aria-label="Previsione successiva" disabled={!d || k < 0 || k >= d.asOfs.length - 1} onClick={() => vai(k + 1)}>
                  <IconChevronRight size={14} />
                </ActionIcon>
              </span>
            </span>
          }
        />
      </div>
      <div className="griglia">
        <Scheda
          titolo="La mappa prevista"
          fonti={['xray']}
          larghezza={8}
          comandi={comandi}
          legenda={
            <Legenda voci={voci} scala={d ? { massimo: scala98(d.valori), positivo: p.pos, negativo: p.neg, unita: 'unità di OptionsDepth, intensità relativa' } : undefined}>
              {seduta && <span className="nota">seduta di {etichettaGiorno(seduta)}</span>}
              {cursore != null && <span className="nota">alle {oraDoppiaCorta(cursore)}</span>}
            </Legenda>
          }
          breve={
            <>
              Dove OptionsDepth prevede che il prezzo verrà frenato o accelerato, con la traccia del <Termine k="flip">flip</Termine> previsto.
            </>
          }
          semplice="Il tempo in orizzontale, il prezzo in verticale: il colore dice dove OptionsDepth prevede freno (verde) o acceleratore (rosso), se le posizioni restano quelle di adesso. La linea grigia tratteggiata è il flip previsto, la riga bianca il prezzo quando la previsione è stata calcolata. Gli ultimi minuti prima delle 16:00 sono velati: lo 0DTE scade e i numeri lì non si leggono. È il loro modello: nessuna previsione è stata verificata."
          sotto="Una previsione nuova arriva circa ogni minuto (asOf): la sceglie l’istante della barra del tempo. Scala tagliata al 98° percentile. L’Ampiezza ritaglia la vista attorno al prezzo del calcolo."
        >
          <Esito q={q} rotta="previsione">
            {(dati) => <Mappa d={dati} f={f} scelte={s} scala={qa.data?.scala ?? []} onCursore={setCursore} />}
          </Esito>
        </Scheda>
        {prossima}
      </div>
    </>
  );
}

interface PropsMappa {
  d: RispostaPrevisione;
  f: Filtri;
  scelte: Scelte;
  scala: RispostaAdesso['scala'];
  onCursore: (t: number | null) => void;
}

/** Il grafico grande: un po' piu' basso di "alto", perche' tessere, legenda e grafico stiano nella prima schermata. */
const ALTEZZA = { min: 340, max: 640, schermo: 0.6 };

function Mappa({ d, f, scelte, scala, onCursore }: PropsMappa) {
  const P = f.p;
  const telefono = useMediaQuery('(max-width: 767px)') ?? false;
  const durata = d.tempi.length > 1 ? (d.tempi[1] ?? 0) - (d.tempi[0] ?? 0) : 300_000;
  // l'immagine si rifa' solo se cambia la previsione (un altro asOf): dal vivo, ogni 15 s, di solito no
  const mappa = useMemo(() => {
    if (!d.tempi.length || !d.prezzi.length) return null;
    const dati: DatiMappa = {
      tempi: d.tempi,
      prezzi: d.prezzi,
      valori: d.valori,
      durata,
      passoPrezzo: passoDi(d.prezzi),
      massimo: scala98(d.valori),
    };
    return { dati, immagine: disegnaMappa(dati) };
  }, [d.tempi, d.prezzi, d.valori, durata]);

  const opzioni = useCallback(
    (larghezza: number): Opzioni | null => {
      if (!mappa) return null;
      const { immagine } = mappa;
      const r = d.riferimento?.prezzo ?? null;
      // la vista: il prezzo del calcolo +- l'ampiezza, dentro la mappa
      const a = Number(scelte.ampiezza) / 100;
      const centro = r ?? (immagine.pAlto + immagine.pBasso) / 2;
      // gli estremi ai multipli dell'intervallo delle tacche: le etichette cadono su numeri tondi
      const lo = P(Math.max(immagine.pBasso, centro * (1 - a)));
      const hi = P(Math.min(immagine.pAlto, centro * (1 + a)));
      const passo = intervalloPrezzi(lo, hi, 5);
      const basso = Math.floor(lo / passo) * passo;
      const alto = Math.ceil(hi / passo) * passo;
      // l'altezza utile della griglia, per distanziare i cartellini
      const hGriglia = altezzaPx(ALTEZZA, { larghezza: window.innerWidth, altezza: window.innerHeight }) - 40;

      const linee: LineaOrizzontale[] = [];
      if (r != null) linee.push({ y: P(r), colore: C.prezzo, spessore: 2, tipo: 'prezzo', etichetta: `prezzo alle ${oraNY(d.asOf)} ${num(P(r), 2)}`, testo: num(P(r), 2) });
      if (scelte.tipo === 'nextday')
        for (const riga of scala.filter((x) => x.quando === 'prossima seduta'))
          for (const v of riga.voci) {
            const y = P(riga.prezzo);
            if (y < basso || y > alto) continue;
            // il cartellino corto (sta nei 64 px a destra); il nome intero resta nel `name`
            const corto = v.tipo === 'flip' ? 'FLIP' : coloreLivello(v.tipo) === C.freno ? 'FRENO' : coloreLivello(v.tipo) === C.acceleratore ? 'ACC.' : v.cosa.toUpperCase();
            linee.push({ y, colore: coloreLivello(v.tipo), tratto: 'dotted', tipo: v.tipo === 'flip' ? 'flip' : 'livello', etichetta: `${v.cosa} ${num(y, 0)}`, testo: `${corto} ${num(y, 0)}` });
          }

      const flip = d.zeri.map(
        (z, i): LineSeriesOption => ({
          type: 'line',
          name: 'flip previsto',
          id: `flip${i}`,
          // al centro della colonna della mappa, come l'immagine
          data: z.map(([t, y]) => [t + durata / 2, P(y)]),
          showSymbol: false,
          silent: true,
          lineStyle: { color: C.flip, width: 1.5, type: 'dashed', shadowColor: 'rgba(0,0,0,.85)', shadowBlur: 3 },
          itemStyle: { color: C.flip },
          z: 6,
        }),
      );

      const taglio = d.taglioScadenza;
      const velo: LineSeriesOption[] =
        taglio != null && taglio < immagine.t1
          ? [
              {
                type: 'line',
                name: '0DTE in scadenza',
                data: [],
                markArea: {
                  silent: true,
                  itemStyle: { color: C.pagina, opacity: 0.6 },
                  label: { show: etichetteFinali(larghezza), position: 'insideTop', color: C.testo2, fontSize: 11, formatter: '0DTE in scadenza' },
                  data: [[{ xAxis: taglio }, { xAxis: immagine.t1 }]],
                },
              },
            ]
          : [];

      // il cartellino del flip previsto alla fine: lontano almeno 14 px da quello del prezzo
      const flipFine = d.sintesi?.flipFinale != null ? P(d.sintesi.flipFinale) : null;
      let spostaFlip = 0;
      if (flipFine != null && r != null) {
        const pxPerPunto = hGriglia / (alto - basso || 1);
        const dist = (flipFine - P(r)) * pxPerPunto;
        if (Math.abs(dist) < 16) spostaFlip = (dist >= 0 ? -16 : 16) + dist;
      }
      return {
        animation: false,
        grid: griglia(larghezza, { destra: 'cartellino', sopra: 10, sotto: 8 }),
        xAxis: asseTempo(immagine.t0, immagine.t1, d.seduta ? { name: `ora di New York · ${giornoCorto(d.seduta)}` } : {}),
        yAxis: assePrezzi(basso, alto, { passo: 5 }),
        tooltip: { show: false },
        series: [
          serieMappa(immagine, P),
          ...velo,
          ...flip,
          serieLinee(lineeOrizzontali(
              linee,
              // sotto i 600 px i cartellini stanno dentro la griglia: a destra non c'e' spazio
              etichetteFinali(larghezza)
                ? { asse: [basso, alto], altezzaPx: hGriglia, fondi: 6 }
                : { fondi: 6 },
            ), 'riferimenti'),
          // il cartellino del flip previsto alla fine, sul bordo destro
          ...(flipFine != null && flipFine >= basso && flipFine <= alto && etichetteFinali(larghezza)
            ? [
                {
                  type: 'line' as const,
                  name: 'flip previsto alla fine',
                  data: [] as [number, number][],
                  markPoint: {
                    silent: true,
                    symbol: 'circle',
                    symbolSize: 1,
                    data: [{ coord: [immagine.t1, flipFine], name: `flip previsto ${num(flipFine, 0)}` }],
                    label: { show: true, ...cartellino(C.flip), position: 'right', offset: [0, spostaFlip], formatter: `FLIP ${num(flipFine, 0)}` },
                  },
                } as LineSeriesOption,
              ]
            : []),
        ],
      };
    },
    [mappa, d, P, scelte.ampiezza, scelte.tipo, scala, durata],
  );

  const puntatore = useMemo(() => {
    if (!mappa) return undefined;
    const p = parole(scelte.greca);
    return (x: number, y: number) => {
      const c = valoreAl(mappa.dati, x, y, P);
      if (!c) return null;
      const testa = testaTip(`${d.seduta ? `${giornoNY(c.t)} ` : ''}${doppiaOra(c.t)} · ${f.unita} ${num(P(c.prezzo), 0)}`);
      if (c.valore == null) return testa;
      const fl = flipAl(d, x, d.riferimento?.prezzo ?? null);
      return (
        testa +
        rigaTip(coloreDi(c.valore, mappa.dati.massimo), c.valore >= 0 ? p.pos : p.neg, num(c.valore, Math.abs(c.valore) >= 10 ? 0 : 2)) +
        (fl == null ? '' : rigaTip(C.flip, 'flip previsto', num(P(fl), 0))) +
        (d.taglioScadenza != null && c.t >= d.taglioScadenza ? `<div style="color:${C.spento}">0DTE in scadenza: non si legge</div>` : '')
      );
    };
  }, [mappa, d, P, f.unita, scelte.greca]);

  const cursore = useCallback((t: number | null) => onCursore(t == null ? null : (d.tempi[indicePrima(d.tempi, t)] ?? null)), [d.tempi, onCursore]);

  if (!mappa) return <Vuoto>Nessuna previsione a questo istante: lo Strike Profile in modalità previsione non era aperto su ES.</Vuoto>;
  return <Grafico opzioni={opzioni} altezza={telefono ? 'alto' : ALTEZZA} etichetta="La mappa prevista di freno e acceleratore" puntatore={puntatore} onCursore={cursore} />;
}
