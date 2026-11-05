// Il tema Mantine: scuro, Inter e Roboto Mono locali, le superfici blu notte desaturate (5 novembre
// 2026). Il colore primario e' l'accento: il segmento scelto e' la "pillola blu" di QuantData, ed e'
// l'unico posto dove compare (mai su un dato).

import { createTheme, type CSSVariablesResolver } from '@mantine/core';
import { C, FONT, FONT_MONO } from './colori';

export const tema = createTheme({
  fontFamily: FONT,
  fontFamilyMonospace: FONT_MONO,
  headings: { fontFamily: FONT, fontWeight: '600' },
  primaryColor: 'accento',
  primaryShade: { light: 6, dark: 7 },
  defaultRadius: 'sm',
  cursorType: 'pointer',
  colors: {
    accento: ['#e8eefb', '#c7d5f2', '#a3b9e8', '#7d9bdc', '#5a7fcf', '#3f66b8', '#2d5199', '#1f3d78', '#1b3566', '#142849'],
    // dark[0] il testo, dark[7] il riquadro, dark[9] la pagina
    dark: ['#eef1f5', '#b4bac6', '#9aa1ae', '#8a92a3', '#5d6577', '#2c3850', '#1f2a3d', '#111723', '#0d121c', '#0b0f17'],
  },
});

export const variabili: CSSVariablesResolver = () => ({
  variables: {},
  light: {},
  dark: {
    '--mantine-color-body': C.pagina,
    '--mantine-color-text': C.testo,
    '--mantine-color-dimmed': C.spento,
    '--mantine-color-default-border': C.bordoForte,
  },
});
