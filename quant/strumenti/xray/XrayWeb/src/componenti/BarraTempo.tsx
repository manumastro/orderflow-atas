// La barra del tempo (B.1): fissa in fondo, 44 px, solo nelle pagine che usano l'istante. ◀ ▶ spostano di
// un passo, con Maiusc di 15 minuti; il cursore ha una tacca per ogni ora di New York, e la cassa
// 09:30-16:00 e' una fascia piu' chiara con le due ore scritte (niente rosso: il rosso e' l'acceleratore).
// Al passaggio del mouse l'ora del punto; a destra l'ora scelta e "Torna al vivo" se non si e' al vivo.
// Sul telefono non c'e': la sostituiscono ◀ ▶ della barra del contesto.

import { useState, type MouseEvent } from 'react';
import { ActionIcon, Button, Slider } from '@mantine/core';
import { IconChevronLeft, IconChevronRight } from '@tabler/icons-react';
import { oraDoppiaCorta, oraNY } from '../formato';
import { indicePrima, useFiltri } from '../stato/contesto';
import { spostaIstante } from './ComandiTempo';

/** Le tacche: il primo istante di ogni ora di New York (indici nel cursore). */
export function tacche(istanti: readonly number[]): { value: number; label: string }[] {
  const out: { value: number; label: string }[] = [];
  let ora = '';
  istanti.forEach((t, i) => {
    const h = oraNY(t).slice(0, 2);
    if (h !== ora) {
      ora = h;
      if (i > 0) out.push({ value: i, label: h });
    }
  });
  return out;
}

export function BarraTempo() {
  const f = useFiltri();
  const n = f.istanti.length;
  const indice = f.ts == null ? n - 1 : indicePrima(f.istanti, f.ts);
  // mentre si trascina si mostra l'ora, e si chiede al server solo al rilascio
  const [trascina, setTrascina] = useState<number | null>(null);
  const mostrato = trascina ?? indice;
  const tMostrato = f.istanti[mostrato] ?? f.istante;
  const vai = (minuti: number) => {
    const j = spostaIstante(f.istanti, indice, minuti);
    if (j >= 0) f.scegliIstante(f.istanti[j] ?? null);
  };
  const passo = (e: MouseEvent) => (e.shiftKey ? 15 : 1);

  // la fascia della cassa, in percentuale del cursore
  const c = f.cassa;
  let fascia: { left: string; width: string } | null = null;
  if (c && n > 1) {
    const a = c.apertura <= (f.istanti[0] ?? 0) ? 0 : indicePrima(f.istanti, c.apertura);
    const b = c.chiusura >= (f.istanti[n - 1] ?? 0) ? n - 1 : indicePrima(f.istanti, c.chiusura);
    if (b > a) fascia = { left: `${(a / (n - 1)) * 100}%`, width: `${((b - a) / (n - 1)) * 100}%` };
  }

  return (
    <section className="barra-tempo" aria-label="Barra del tempo" data-passato={f.passato ? 'si' : undefined}>
      <ActionIcon variant="subtle" color="gray" size="md" aria-label="Indietro (Maiusc: 15 minuti)" disabled={indice <= 0} onClick={(e) => vai(-passo(e))}>
        <IconChevronLeft size={18} />
      </ActionIcon>
      <div className="tempo-pista">
        {fascia && (
          <div className="tempo-cassa" style={fascia} aria-hidden>
            <span className="tempo-cassa-ora inizio">09:30</span>
            <span className="tempo-cassa-nome">cassa</span>
            <span className="tempo-cassa-ora fine">16:00</span>
          </div>
        )}
        <Slider
          className="cursore"
          aria-label="Istante della giornata"
          min={0}
          max={Math.max(0, n - 1)}
          value={Math.max(0, mostrato)}
          disabled={n === 0}
          label={(v) => oraDoppiaCorta(f.istanti[v])}
          marks={tacche(f.istanti)}
          size="sm"
          color="gray"
          onChange={setTrascina}
          onChangeEnd={(v) => {
            setTrascina(null);
            f.scegliIstante(f.istanti[v] ?? null);
          }}
        />
      </div>
      <span className="tempo-scelto num">
        <span className="punto" data-passato={f.passato ? 'si' : undefined} aria-hidden /> {tMostrato ? oraDoppiaCorta(tMostrato) : '–'}
      </span>
      <ActionIcon variant="subtle" color="gray" size="md" aria-label="Avanti (Maiusc: 15 minuti)" disabled={indice >= n - 1} onClick={(e) => vai(passo(e))}>
        <IconChevronRight size={18} />
      </ActionIcon>
      {f.passato && (
        <Button size="xs" variant="default" onClick={() => f.scegliGiorno(f.giorni[0] ?? '')}>
          Torna al vivo
        </Button>
      )}
    </section>
  );
}
