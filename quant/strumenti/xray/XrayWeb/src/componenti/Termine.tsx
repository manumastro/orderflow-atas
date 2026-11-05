// Un termine del glossario nel testo: sottolineato a puntini; al passaggio del mouse (o al tocco) apre
// la glossa in due righe, cosa significa per il prezzo e quanto e' provato. <Termine k="flip">flip</Termine>

import type { ReactNode } from 'react';
import { HoverCard } from '@mantine/core';
import { GLOSSARIO, type ChiaveGlossario } from '../glossario';

export function Termine({ k, children }: { k: ChiaveGlossario; children?: ReactNode }) {
  return (
    <HoverCard width={320} openDelay={150} position="top" shadow="md" withinPortal>
      <HoverCard.Target>
        <span className="termine" tabIndex={0} data-termine={k}>
          {children ?? k}
        </span>
      </HoverCard.Target>
      <HoverCard.Dropdown className="aiuto-pannello glossa">{GLOSSARIO[k]}</HoverCard.Dropdown>
    </HoverCard>
  );
}
