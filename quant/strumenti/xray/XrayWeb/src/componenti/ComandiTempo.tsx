// I comandi del tempo nella barra del contesto (B.1, al posto di Filtri): Giorno ◀ [select] ▶, l'istante
// con ◀ ▶, "Dal vivo", Prezzi ES|SPX con la base nella "ⓘ". I comandi che la pagina non usa restano al
// loro posto, spenti, con la ragione scritta al posto del valore: la barra non salta.
// Le frecce della tastiera spostano l'istante di un passo, con Maiusc di 15 minuti (non dentro input,
// select, cursori e segmenti). Sul telefono giorno e prezzi vanno nel cassetto dei filtri (⚙), con
// id="giorno-tel": mai un secondo id="giorno".

import { useEffect, useState, type ReactNode } from 'react';
import { ActionIcon, Button, Drawer, NativeSelect, Popover, SegmentedControl } from '@mantine/core';
import { IconAdjustments, IconChevronLeft, IconChevronRight, IconInfoCircle } from '@tabler/icons-react';
import { doppiaOra, etichettaGiorno, oraDoppiaCorta, segno } from '../formato';
import { indicePrima, useFiltri, type Filtri as StatoFiltri } from '../stato/contesto';
import type { Filtri } from '../pagine/rotte';
import { SceltaSpiegazioni } from './BarraAlta';

export const RAGIONE_SPENTO = 'questa pagina mostra l’ultimo dato';

/** L'indice dell'istante a `minuti` dal corrente (negativo = indietro), dentro la giornata. */
export function spostaIstante(istanti: readonly number[], indice: number, minuti: number): number {
  const n = istanti.length;
  if (!n) return -1;
  const t = istanti[indice] ?? istanti[n - 1]!;
  const meta = t + minuti * 60_000;
  if (minuti < 0) return Math.max(0, indicePrima(istanti, meta));
  const j = istanti.findIndex((x) => x >= meta);
  return j < 0 ? n - 1 : j;
}

/** Il testo della base ES − SPX, per la "ⓘ" dei prezzi. */
export function testoBase(f: StatoFiltri): string {
  const b = f.base;
  if (b?.valore == null) return 'base ES − SPX non disponibile: serve il bridge di ATAS con un chart ES a 1 minuto';
  const giorno = b.giorno && b.giorno !== f.giorno ? ` del ${b.giorno.slice(8, 10)}/${b.giorno.slice(5, 7)}` : '';
  return `base ${segno(b.valore, 2)} punti · ${b.campioni} minuti${giorno} · ${b.fonte}`;
}

function SceltaPrezzi({ f, attiva }: { f: StatoFiltri; attiva: boolean }) {
  const senzaBase = f.base?.valore == null;
  return (
    <SegmentedControl
      size="xs"
      color="accento"
      aria-label="Prezzi in"
      className="segmenti prezzi"
      value={f.es ? 'ES' : 'SPX'}
      onChange={(v) => f.scegliPrezzi(v === 'ES' ? 'ES' : 'SPX')}
      disabled={!attiva}
      data={[
        { value: 'ES', label: 'ES', disabled: senzaBase },
        { value: 'SPX', label: 'SPX' },
      ]}
      title={senzaBase ? 'Serve la base ES-SPX: il bridge di ATAS con un chart ES a 1 minuto' : undefined}
    />
  );
}

function Giorno({ f, id, attiva }: { f: StatoFiltri; id: string; attiva: boolean }) {
  const i = f.giorno == null ? -1 : f.giorni.indexOf(f.giorno);
  if (!attiva) return <span className="spento-ragione">ultimo dato</span>;
  return (
    <span className="giorno-riga">
      <ActionIcon
        variant="subtle"
        color="gray"
        size="sm"
        aria-label="Giorno prima"
        disabled={i < 0 || i >= f.giorni.length - 1}
        onClick={() => f.scegliGiorno(f.giorni[i + 1] ?? f.giorni[0]!)}
      >
        <IconChevronLeft size={15} />
      </ActionIcon>
      <NativeSelect
        id={id}
        size="xs"
        aria-label="Giorno"
        className="giorno-scelta"
        value={f.giorno ?? ''}
        onChange={(e) => f.scegliGiorno(e.currentTarget.value)}
        data={f.giorni.map((g, k) => ({ value: g, label: etichettaGiorno(g) + (k === 0 ? ' · ultimo' : '') }))}
      />
      <ActionIcon
        variant="subtle"
        color="gray"
        size="sm"
        aria-label="Giorno dopo"
        disabled={i <= 0}
        onClick={() => f.scegliGiorno(f.giorni[i - 1] ?? f.giorni[0]!)}
      >
        <IconChevronRight size={15} />
      </ActionIcon>
    </span>
  );
}

export function ComandiTempo({ filtri, riserva }: { filtri: Filtri; riserva?: ReactNode }) {
  const f = useFiltri();
  const [cassetto, setCassetto] = useState(false);
  const n = f.istanti.length;
  const indice = f.ts == null ? n - 1 : indicePrima(f.istanti, f.ts);
  const vai = (minuti: number) => {
    const j = spostaIstante(f.istanti, indice, minuti);
    if (j >= 0) f.scegliIstante(f.istanti[j] ?? null);
  };

  // le frecce spostano l'istante di un minuto, con Maiusc di 15 (non quando si scrive o si usa un altro comando)
  useEffect(() => {
    if (!filtri.istante) return;
    const tasto = (e: KeyboardEvent) => {
      if (e.key !== 'ArrowLeft' && e.key !== 'ArrowRight') return;
      const t = e.target instanceof Element ? e.target : null;
      if (t?.closest('input, select, textarea, [role="slider"], [role="radiogroup"]')) return;
      if (!n) return;
      const passo = e.shiftKey ? 15 : 1;
      const avanti = e.key === 'ArrowRight';
      const j = e.shiftKey ? spostaIstante(f.istanti, indice, avanti ? passo : -passo) : Math.min(n - 1, Math.max(0, indice + (avanti ? 1 : -1)));
      f.scegliIstante(f.istanti[j] ?? null);
    };
    window.addEventListener('keydown', tasto);
    return () => window.removeEventListener('keydown', tasto);
  }, [filtri.istante, n, indice, f]);

  const alVivo = f.ts == null && f.oggi;
  const testoVivo = alVivo ? 'Dal vivo' : f.oggi ? 'Torna al vivo' : 'Fine giornata';

  return (
    <div className="comandi-tempo">
      <div className="ct-campo ct-giorno" data-spento={filtri.giorno ? undefined : 'si'}>
        <span className="campo-nome">Giorno</span>
        <Giorno f={f} id="giorno" attiva={filtri.giorno} />
      </div>
      <div className="ct-campo ct-istante" data-spento={filtri.istante ? undefined : 'si'}>
        <span className="campo-nome">Istante</span>
        {filtri.istante && (
          <ActionIcon variant="subtle" color="gray" size="sm" aria-label="Un minuto prima" disabled={indice <= 0} onClick={() => vai(-1)}>
            <IconChevronLeft size={15} />
          </ActionIcon>
        )}
        <span className="campo-valore num" id="istante-testo">
          {!filtri.istante ? RAGIONE_SPENTO : f.istante ? doppiaOra(f.istante) : 'nessun istante'}
        </span>
        <span className="campo-valore-corto num" aria-hidden>
          {filtri.istante && f.istante ? oraDoppiaCorta(f.istante) : ''}
        </span>
        {filtri.istante && (
          <ActionIcon variant="subtle" color="gray" size="sm" aria-label="Un minuto dopo" disabled={indice >= n - 1} onClick={() => vai(1)}>
            <IconChevronRight size={15} />
          </ActionIcon>
        )}
      </div>
      <Button
        id="vivo"
        size="xs"
        variant="default"
        className="bottone-vivo"
        data-vivo={alVivo ? 'si' : undefined}
        aria-pressed={f.ts == null}
        disabled={!filtri.istante}
        onClick={() => f.scegliIstante(null)}
        leftSection={alVivo ? <span className="punto vivo" aria-hidden /> : undefined}
      >
        {testoVivo}
      </Button>
      <div className="ct-campo ct-prezzi" data-spento={filtri.prezzi ? undefined : 'si'}>
        <span className="campo-nome">Prezzi</span>
        <SceltaPrezzi f={f} attiva={filtri.prezzi} />
        <Popover width={300} position="bottom" shadow="md" withinPortal>
          <Popover.Target>
            <ActionIcon variant="subtle" color="gray" size="sm" aria-label="La base ES − SPX">
              <IconInfoCircle size={15} stroke={1.8} />
            </ActionIcon>
          </Popover.Target>
          <Popover.Dropdown className="aiuto-pannello">
            <div className="aiuto-testo">
              <strong className="aiuto-titolo">Prezzi in ES o in SPX</strong>
              <p className="aiuto-sotto num">{testoBase(f)}</p>
              <p className="aiuto-sotto">
                X-Ray stima su SPX; in ES i prezzi si spostano della base, come sul chart di ATAS.
              </p>
            </div>
          </Popover.Dropdown>
        </Popover>
      </div>
      <ActionIcon variant="default" size="md" className="solo-telefono filtri-telefono" aria-label="Filtri e spiegazioni" onClick={() => setCassetto(true)}>
        <IconAdjustments size={16} stroke={1.8} />
      </ActionIcon>
      {cassetto && (
        <Drawer opened onClose={() => setCassetto(false)} position="bottom" size="auto" title="Filtri" classNames={{ content: 'cassetto' }}>
          <div className="cassetto-filtri">
            <label className="ct-campo verticale">
              <span className="campo-nome">Giorno</span>
              <Giorno f={f} id="giorno-tel" attiva={filtri.giorno} />
            </label>
            <div className="ct-campo verticale">
              <span className="campo-nome">Prezzi</span>
              <SceltaPrezzi f={f} attiva={filtri.prezzi} />
              <span className="aiuto-sotto num">{testoBase(f)}</span>
            </div>
            <div className="ct-campo verticale">
              <span className="campo-nome">Spiegazioni</span>
              <SceltaSpiegazioni className="spiegazioni-tel" />
            </div>
            {riserva && (
              <div className="ct-campo verticale">
                <span className="campo-nome">La riserva</span>
                <p className="aiuto-riserva">{riserva}</p>
              </div>
            )}
          </div>
        </Drawer>
      )}
    </div>
  );
}
