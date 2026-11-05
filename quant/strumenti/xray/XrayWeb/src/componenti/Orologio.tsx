// L'orologio della testata: l'ora di adesso a New York e in Italia, ogni secondo (non l'istante scelto).
// Sul telefono solo "06:42 NY".

import { useEffect, useState } from 'react';
import { oraIT, oraNY, oraSecondi } from '../formato';

export function Orologio() {
  const [adesso, setAdesso] = useState(() => Date.now());
  useEffect(() => {
    const t = setInterval(() => setAdesso(Date.now()), 1000);
    return () => clearInterval(t);
  }, []);
  return (
    <span className="orologio num" aria-label="Ora di adesso">
      <span className="orologio-lungo">
        {oraSecondi(adesso)} New York · {oraIT(adesso)} Italia
      </span>
      <span className="orologio-corto">{oraNY(adesso)} NY</span>
    </span>
  );
}
