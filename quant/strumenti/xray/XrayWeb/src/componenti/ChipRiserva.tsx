// La riserva, sempre visibile in forma breve (B.6): una chip con l'icona e poche parole ("stima
// OptionsDepth · non provato"); un clic apre la riserva intera. Una per gruppo nella barra del contesto,
// e una propria nell'intestazione dei riquadri che ne hanno una ("segno supposto").

import type { ReactNode } from 'react';
import { Popover, UnstyledButton } from '@mantine/core';
import { IconAlertCircle } from '@tabler/icons-react';

function Chip({ testo, intera, classe }: { testo: string; intera?: ReactNode; classe: string }) {
  if (!intera) {
    return (
      <span className={classe}>
        <IconAlertCircle size={13} stroke={1.8} aria-hidden />
        {testo}
      </span>
    );
  }
  return (
    <Popover width={360} position="bottom-end" shadow="md" withinPortal>
      <Popover.Target>
        <UnstyledButton className={classe} aria-label={`Riserva: ${testo}`}>
          <IconAlertCircle size={13} stroke={1.8} aria-hidden />
          {testo}
        </UnstyledButton>
      </Popover.Target>
      <Popover.Dropdown className="aiuto-pannello">
        <div className="aiuto-testo">
          <strong className="aiuto-titolo">La riserva</strong>
          <p className="aiuto-riserva">{intera}</p>
        </div>
      </Popover.Dropdown>
    </Popover>
  );
}

/** La chip della riserva del gruppo, nella barra del contesto. */
export function ChipRiserva({ breve, intera }: { breve: string; intera: ReactNode }) {
  return <Chip testo={breve} intera={intera} classe="chip-riserva" />;
}

/** La chip della riserva propria di un riquadro ("segno supposto"). */
export function ChipRiservaPropria({ testo, intera }: { testo: string; intera?: ReactNode }) {
  return <Chip testo={testo} intera={intera} classe="chip-riserva propria" />;
}
