// ECharts con i soli moduli che la pagina usa: barre, linee, punti, serie su misura (mappe e
// profili a rettangoli), griglia, tooltip, linee di riferimento, segni sui punti (i minuti anomali),
// aree (il velo dello 0DTE in scadenza), zoom, cursore condiviso. Canvas:
// le prove contano i pixel disegnati.

import * as echarts from 'echarts/core';
import { BarChart, CustomChart, LineChart, ScatterChart } from 'echarts/charts';
import {
  AxisPointerComponent,
  DataZoomInsideComponent,
  DataZoomSliderComponent,
  GridComponent,
  MarkAreaComponent,
  MarkLineComponent,
  MarkPointComponent,
  TooltipComponent,
} from 'echarts/components';
import { CanvasRenderer } from 'echarts/renderers';
import type { ComposeOption, EChartsType } from 'echarts/core';
import type { BarSeriesOption, CustomSeriesOption, LineSeriesOption, ScatterSeriesOption } from 'echarts/charts';
import type {
  DataZoomComponentOption,
  GridComponentOption,
  MarkAreaComponentOption,
  MarkLineComponentOption,
  MarkPointComponentOption,
  TooltipComponentOption,
} from 'echarts/components';

echarts.use([
  BarChart,
  CustomChart,
  LineChart,
  ScatterChart,
  AxisPointerComponent,
  DataZoomInsideComponent,
  DataZoomSliderComponent,
  GridComponent,
  MarkAreaComponent,
  MarkLineComponent,
  MarkPointComponent,
  TooltipComponent,
  CanvasRenderer,
]);

/** Le opzioni di un grafico, con i soli tipi dei moduli caricati. */
export type Opzioni = ComposeOption<
  | BarSeriesOption
  | CustomSeriesOption
  | LineSeriesOption
  | ScatterSeriesOption
  | DataZoomComponentOption
  | GridComponentOption
  | MarkAreaComponentOption
  | MarkLineComponentOption
  | MarkPointComponentOption
  | TooltipComponentOption
>;

export type Istanza = EChartsType;

export function crea(contenitore: HTMLElement): Istanza {
  return echarts.init(contenitore, null, { renderer: 'canvas' });
}

/** Lega cursore e zoom dei grafici di un gruppo (la Deriva). */
export function collega(gruppo: string): void {
  echarts.connect(gruppo);
}
