// I filtri (giorno, istante, prezzi in SPX o ES) vivono nell'indirizzo: #/adesso?giorno=..&ts=..&prezzi=ES.
// Senza giorno vale l'ultimo, senza istante e' "dal vivo", senza prezzi e' ES (dal 5 novembre 2026, come
// ATAS disegna X-Ray sul chart di ES; prima era SPX): un link riapre la
// stessa vista, e il tasto Indietro la ripristina. Le pagine leggono i filtri da useFiltri().
// "passato" (un istante scelto, o una giornata che non e' l'ultima) accende la modalita' passato della
// testata (body[data-tempo="passato"], la chip "Passato", le eta' spente).

import { useCallback, useMemo, type ReactNode } from 'react';
import { useSearchParams } from 'react-router';
import { useBase, useGiorni, useIstanti } from '../api/domande';
import { ContestoFiltri, type Filtri, type Prezzi } from './contesto';

export function FiltriProvider({ children }: { children: ReactNode }) {
  const [sp, setSp] = useSearchParams();
  const giornoChiesto = sp.get('giorno');
  const tsTesto = sp.get('ts');
  const ts = tsTesto != null && tsTesto !== '' && Number.isFinite(Number(tsTesto)) ? Number(tsTesto) : null;
  const prezziChiesti: Prezzi = sp.get('prezzi') === 'SPX' ? 'SPX' : 'ES';

  const qGiorni = useGiorni(giornoChiesto == null && ts == null);
  const giorni = useMemo(() => qGiorni.data?.giorni ?? [], [qGiorni.data]);
  const giorno = giornoChiesto != null && giorni.includes(giornoChiesto) ? giornoChiesto : (giorni[0] ?? null);
  const oggi = giorno != null && giorno === giorni[0];
  const vivo = oggi && ts == null;
  const passato = giorno != null && (ts != null || !oggi);

  const qIstanti = useIstanti(giorno, vivo);
  const qBase = useBase(giorno, vivo);
  const istanti = useMemo(() => qIstanti.data?.istanti ?? [], [qIstanti.data]);
  const base = qBase.data ?? null;
  const cassa = qIstanti.data?.cassa ?? null;
  const es = prezziChiesti === 'ES' && base?.valore != null;
  const b = base?.valore ?? 0;
  // stabile finche' non cambiano unita' e base: i grafici non si ridisegnano a ogni giro dal vivo
  const p = useCallback((x: number) => (es ? x + b : x), [es, b]);

  const cambia = useCallback(
    (fai: (s: URLSearchParams) => void) => {
      setSp(
        (prima) => {
          const s = new URLSearchParams(prima);
          fai(s);
          return s;
        },
        { replace: true },
      );
    },
    [setSp],
  );

  const scegliGiorno = useCallback(
    (g: string) =>
      cambia((s) => {
        if (g === giorni[0]) s.delete('giorno');
        else s.set('giorno', g);
        s.delete('ts');
      }),
    [cambia, giorni],
  );

  const scegliIstante = useCallback(
    (t: number | null) =>
      cambia((s) => {
        // l'ultimo istante dell'ultima giornata e' "dal vivo"
        if (t == null || (oggi && t === istanti.at(-1))) s.delete('ts');
        else s.set('ts', String(t));
      }),
    [cambia, oggi, istanti],
  );

  const scegliPrezzi = useCallback(
    (p: Prezzi) =>
      cambia((s) => {
        if (p === 'SPX') s.set('prezzi', 'SPX');
        else s.delete('prezzi');
      }),
    [cambia],
  );

  const valore = useMemo<Filtri>(
    () => ({
      giorni,
      giorno,
      ts,
      istanti,
      istante: ts ?? istanti.at(-1) ?? null,
      oggi,
      vivo,
      passato,
      cassa,
      base,
      es,
      unita: es ? 'ES' : 'SPX',
      p,
      parametri: { giorno, ts: ts ?? undefined },
      scegliGiorno,
      scegliIstante,
      scegliPrezzi,
      pronto: giorno != null && qIstanti.data != null,
      erroreGiorni: qGiorni.error,
    }),
    [giorni, giorno, ts, istanti, oggi, vivo, passato, cassa, base, es, p, scegliGiorno, scegliIstante, scegliPrezzi, qIstanti.data, qGiorni.error],
  );

  return <ContestoFiltri.Provider value={valore}>{children}</ContestoFiltri.Provider>;
}
