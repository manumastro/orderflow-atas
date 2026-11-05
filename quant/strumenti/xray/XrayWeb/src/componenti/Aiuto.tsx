// La "i": la spiegazione intera di un riquadro o di una pagina, sempre a un clic. Sul desktop un
// popover di 360 px; sul telefono un cassetto dal basso. Dentro: in parole semplici, la nota tecnica,
// la riserva. Raggiungibile, non dominante (B.5).

import { useState, type ReactNode } from 'react';
import { ActionIcon, Drawer, Popover } from '@mantine/core';
import { useMediaQuery } from '@mantine/hooks';
import { IconInfoCircle } from '@tabler/icons-react';

interface Props {
  /** di cosa parla, per l'aria-label ("Spiegazione: Esposizione per strike") */
  titolo: string;
  semplice?: ReactNode;
  sotto?: ReactNode;
  riserva?: ReactNode;
  /** altro contenuto in fondo */
  children?: ReactNode;
  className?: string;
}

function Contenuto({ titolo, semplice, sotto, riserva, children }: Props) {
  return (
    <div className="aiuto-testo">
      <strong className="aiuto-titolo">{titolo}</strong>
      {semplice && <div className="aiuto-semplice">{semplice}</div>}
      {sotto && <p className="aiuto-sotto">{sotto}</p>}
      {riserva && <p className="aiuto-riserva">{riserva}</p>}
      {children}
    </div>
  );
}

export function Aiuto(props: Props) {
  const telefono = useMediaQuery('(max-width: 767px)') ?? false;
  const [aperto, setAperto] = useState(false);
  const bottone = (
    <ActionIcon
      variant="subtle"
      color="gray"
      size="sm"
      radius="sm"
      className={props.className ?? 'aiuto'}
      aria-label={`Spiegazione: ${props.titolo}`}
      aria-expanded={aperto}
      onClick={() => setAperto((a) => !a)}
    >
      <IconInfoCircle size={15} stroke={1.8} />
    </ActionIcon>
  );
  if (telefono) {
    return (
      <>
        {bottone}
        {aperto && (
          <Drawer opened onClose={() => setAperto(false)} position="bottom" size="auto" title={props.titolo} classNames={{ content: 'cassetto' }}>
            <Contenuto {...props} />
          </Drawer>
        )}
      </>
    );
  }
  return (
    <Popover opened={aperto} onChange={setAperto} width={360} position="bottom-start" shadow="md" withinPortal>
      <Popover.Target>{bottone}</Popover.Target>
      <Popover.Dropdown className="aiuto-pannello">
        <Contenuto {...props} />
      </Popover.Dropdown>
    </Popover>
  );
}
