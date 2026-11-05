// Le prove dei numeri all'italiana e delle ore: le ore si provano in un fuso qualunque (Tokyo,
// vite.config.ts) e a cavallo dei cambi d'ora, che in America e in Europa cadono in date diverse.

import { describe, expect, it } from 'vitest';
import {
  auto,
  cifreDi,
  distanza,
  doppiaOra,
  eta,
  etaLunga,
  etichettaGiorno,
  giornoCorto,
  giornoEOra,
  nome,
  num,
  oraDoppiaCorta,
  oraIT,
  oraNY,
  oraSecondi,
  percento,
  segno,
  sig,
} from './formato';

describe('i numeri', () => {
  it('scrive all’italiana: punto per le migliaia, virgola per i decimali', () => {
    expect(num(7624.3, 2)).toBe('7.624,30');
    expect(num(1234567, 0)).toBe('1.234.567');
    expect(num(0.5)).toBe('0,5');
  });

  it('mette il trattino quando il numero manca o non è finito', () => {
    expect(num(null)).toBe('–');
    expect(num(undefined)).toBe('–');
    expect(num(Number.NaN)).toBe('–');
    expect(segno(Number.POSITIVE_INFINITY)).toBe('–');
  });

  it('scrive il segno: più, il meno tipografico, niente sullo zero', () => {
    expect(segno(11.9)).toBe('+11,9');
    expect(segno(-73.3, 0)).toBe('−73');
    expect(segno(0)).toBe('0,0');
  });

  it('sceglie le cifre secondo la grandezza', () => {
    expect(cifreDi(250)).toBe(0);
    expect(cifreDi(-25)).toBe(1);
    expect(cifreDi(2.5)).toBe(2);
    expect(cifreDi(0.25)).toBe(3);
    expect(auto(-653.2)).toBe('−653');
    expect(auto(2.674)).toBe('+2,67');
    expect(auto(0.0312)).toBe('+0,031');
  });

  it('scrive le quote in percentuale', () => {
    expect(percento(0.64)).toBe('64%');
    expect(percento(null)).toBe('–');
  });
});

describe('i numeri nuovi della pagina in stile QuantData (5 novembre 2026)', () => {
  it('la distanza dal prezzo ha la freccia, e lo zero non ne ha', () => {
    expect(distanza(29)).toBe('↑29');
    expect(distanza(-101.4)).toBe('↓101');
    expect(distanza(1234)).toBe('↑1.234');
    expect(distanza(0.2)).toBe('0');
    expect(distanza(-2.25, 2)).toBe('↓2,25');
    expect(distanza(null)).toBe('–');
  });

  it('due cifre significative, col segno: le greche di OptionsDepth', () => {
    expect(sig(14.3)).toBe('+14');
    expect(sig(-0.2734)).toBe('−0,27');
    expect(sig(1234)).toBe('+1.200');
    expect(sig(9.96)).toBe('+10');
    expect(sig(-0.0049)).toBe('−0,0049');
    expect(sig(0)).toBe('0');
    expect(sig(1234, 3)).toBe('+1.230');
    expect(sig(undefined)).toBe('–');
  });

  it('l’età lunga passa ai giorni oltre le 48 ore', () => {
    expect(etaLunga(42)).toBe('42 s');
    expect(etaLunga(7 * 60)).toBe('7 min');
    expect(etaLunga(3 * 3600)).toBe('3 h');
    expect(etaLunga(2 * 86400 + 19 * 3600)).toBe('2 giorni e 19 h');
    expect(etaLunga(3 * 86400)).toBe('3 giorni');
    expect(etaLunga(null)).toBe('–');
  });
});

describe('le ore', () => {
  it('l’orologio della testata: l’ora di New York con i secondi', () => {
    expect(oraSecondi(Date.UTC(2026, 10, 5, 11, 42, 5))).toBe('06:42:05');
  });

  it('le due ore corte e il giorno con le ore', () => {
    expect(oraDoppiaCorta(Date.UTC(2026, 10, 5, 15, 30))).toBe('10:30 NY · 16:30 IT');
    expect(giornoEOra(Date.UTC(2026, 10, 2, 16, 8))).toBe('lun 02/11 · 11:08 New York (17:08 Italia)');
    expect(giornoEOra(null)).toBe('–');
  });

  it('la cassa apre alle 09:30 di New York, in inverno (2 novembre: New York e Roma all’ora solare)', () => {
    const apertura = Date.UTC(2026, 10, 2, 14, 30);
    expect(oraNY(apertura)).toBe('09:30');
    expect(oraIT(apertura)).toBe('15:30');
  });

  it('il 30 ottobre New York è ancora all’ora legale e Roma no: sei ore fra le due, non cinque', () => {
    const apertura = Date.UTC(2026, 9, 30, 13, 30);
    expect(oraNY(apertura)).toBe('09:30');
    expect(oraIT(apertura)).toBe('14:30');
  });

  it('scrive le due ore insieme', () => {
    expect(doppiaOra(Date.UTC(2026, 10, 2, 21, 0))).toBe('16:00 New York · 22:00 Italia');
    expect(oraNY(null)).toBe('–');
  });

  it('scrive i giorni della seduta', () => {
    expect(etichettaGiorno('2026-11-02')).toBe('lun 02/11/2026');
    expect(giornoCorto('2026-11-27')).toBe('27/11');
  });

  it('scrive l’età di un dato in parole', () => {
    expect(eta(42)).toBe('42 s');
    expect(eta(300)).toBe('5 min');
    expect(eta(7200)).toBe('2 h');
    expect(eta(null)).toBe('–');
  });
});

describe('i nomi', () => {
  it('traduce scadenze e gruppi, e lascia com’è quello che non conosce', () => {
    expect(nome('0dte')).toBe('0DTE (scade oggi)');
    expect(nome('nonprocust')).toBe('Retail');
    expect(nome('ignoto')).toBe('ignoto');
  });
});
