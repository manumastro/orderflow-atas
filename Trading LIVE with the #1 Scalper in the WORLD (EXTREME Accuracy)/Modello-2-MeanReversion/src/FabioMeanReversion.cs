// ============================================================================
// FabioMeanReversion — Balance/Consolidation Zones (Historical Rectangles)
// ============================================================================
// Semplice rilevamento di zone di consolidamento/balance PASSATE dopo un leg
// espansivo. Solo rettangoli semi-trasparenti cyan. Niente sessioni, niente
// paintbars, niente valori di profile/POC/VAH/VAL.
//
// Filosofia (dal transcript di Fabio Valentino):
//   - impulso direzionale espansivo
//   - mercato smette di fare nuovi massimi/minimi → area compressa (balance)
//   - le zone sono strutturali: stesse strutture che si vedono su D1/M5/M1.
//   - niente parametri fissi esposti: la detection è relativa alla volatilità
//     locale e alla price action recente (full history).
//
// Log ultra-dettagliato per debug (tutta la history, un file al giorno):
//   %APPDATA%\ATAS\Logs\FabioBalanceZone_yyyy-MM-dd.log
// Interrogare in tempo reale con Modello-2-MeanReversion\tail-balance-log.bat
// ============================================================================

using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Text.RegularExpressions;
using ATAS.Indicators;
using ATAS.Indicators.Drawing;

namespace FabioMeanReversion;

[DisplayName("Fabio Mean Reversion")]
public class FabioMeanReversion : Indicator
{
    #region === Parametri interni (non esposti all'utente) ===

    // Tick size per default NQ/ES; usato solo per evitare divisioni per zero.
    private decimal EffectiveTickSize => 0.25m;

    // Durata minima visualmente significativa per una zona di balance.
    // Non è un parametro utente: è una soglia di "visibilità" (es. 45 minuti).
    // Il numero di barre si adatta al timeframe automaticamente.
    private const int MinDurationMinutes = 45;

    #endregion

    #region === Stato ===

    private int _lastLoggedBar = -1;
    private int _lastProcessedBar = -1;
    private int _minZoneBars = 5;

    // Impulso (leg) che ha generato la zona aperta corrente
    private int _lastImpulseEnd = -1;
    private int _lastImpulseStart = -1;
    private decimal _lastImpulseAvgRange;

    // Zona aperta corrente (verrà disegnata quando si chiude)
    private int _openZoneStart = -1;
    private decimal _openZoneHigh = decimal.MinValue;
    private decimal _openZoneLow = decimal.MaxValue;

    // Confini della zona: fissati all'apertura, permettono piccole espansioni
    // del 10% ma chiudono la zona su un vero breakout.
    private decimal _zoneBoundaryHigh = decimal.MinValue;
    private decimal _zoneBoundaryLow = decimal.MaxValue;

    // Un file di log al giorno: tutti i dati sono conservati e facilmente
    // recuperabili con nome FabioBalanceZone_yyyy-MM-dd.log
    private static string LogDirectory => Path.Combine(
        Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData),
        "ATAS", "Logs");

    private static string LogFilePath => Path.Combine(LogDirectory,
        $"FabioBalanceZone_{DateTime.Now:yyyy-MM-dd}.log");

    private static void LogBal(string msg)
    {
        try
        {
            Directory.CreateDirectory(LogDirectory);
            File.AppendAllText(LogFilePath, $"[{DateTime.Now:HH:mm:ss}] {msg}\r\n");
        }
        catch { }
        System.Diagnostics.Debug.WriteLine(msg);
    }

    #endregion

    #region === Costruttore ===

    public FabioMeanReversion() : base(true)
    {
        DenyToChangePanel = true;
        DataSeries[0].IsHidden = true;

        LogBal("=== FabioMeanReversion BALANCE ZONE ONLY started ===");
    }

    #endregion

    #region === Calcolo principale ===

    protected override void OnCalculate(int bar, decimal value)
    {
        if (bar == 0)
        {
            Rectangles.Clear();
            ResetState();

            if (CurrentBar > 0)
            {
                try
                {
                    var first = GetCandle(0);
                    var last = GetCandle(CurrentBar - 1);
                    _minZoneBars = ComputeMinZoneBars();

                    LogBal($"=== RECALC START bars={CurrentBar} tf={ChartInfo?.TimeFrame ?? "?"} " +
                           $"minZoneBars={_minZoneBars} first={first.Time:yyyy-MM-dd HH:mm} " +
                           $"last={last.Time:yyyy-MM-dd HH:mm} file={Path.GetFileName(LogFilePath)} ===");
                }
                catch
                {
                    _minZoneBars = ComputeMinZoneBars();
                    LogBal($"=== RECALC START bars={CurrentBar} tf={ChartInfo?.TimeFrame ?? "?"} " +
                           $"minZoneBars={_minZoneBars} file={Path.GetFileName(LogFilePath)} ===");
                }
            }
            else
            {
                LogBal($"=== RECALC START no bars file={Path.GetFileName(LogFilePath)} ===");
            }
        }

        if (bar < 0)
            return;

        // Log dettagliato OHLC di questa barra (tutta la history, una volta per barra)
        LogBarDetails(bar);

        if (bar < _minZoneBars)
            return;

        // Evita di processare più volte la stessa barra in tempo reale.
        if (bar == _lastProcessedBar)
            return;

        int impulseEnd = FindLastImpulse(bar);
        if (impulseEnd < 1)
        {
            CloseOpenZoneIfAny(bar, "no significant impulse");
            _lastProcessedBar = bar;
            return;
        }

        int impulseStart = FindImpulseStart(impulseEnd);
        decimal legRange = CalculateRange(impulseStart, impulseEnd);
        decimal legAvgRange = RobustAvgRange(impulseStart, impulseEnd);

        // Se abbiamo già una zona aperta, ignora impulsi interni che NON rompono
        // i lati della zona (failed breakout: Fabio dice "l'era di consolidamento
        // è ancora la stessa"). Questo preserva la grande zona 10:45-15:25
        // anche se ha qualche barra espansiva interna.
        if (_openZoneStart >= 0 && impulseEnd > _openZoneStart)
        {
            var impCandle = GetCandle(impulseEnd);
            decimal impRange = impCandle.High - impCandle.Low;
            bool breaksHigh = impCandle.Close > _zoneBoundaryHigh && impRange > _lastImpulseAvgRange * 1.4m;
            bool breaksLow = impCandle.Close < _zoneBoundaryLow && impRange > _lastImpulseAvgRange * 1.4m;

            if (!breaksHigh && !breaksLow)
            {
                // Mantieni l'ancoraggio precedente e continua a estendere la zona.
                impulseEnd = _lastImpulseEnd;
                impulseStart = _lastImpulseStart;
                legRange = CalculateRange(impulseStart, impulseEnd);
                legAvgRange = _lastImpulseAvgRange;
            }
            else
            {
                // Nuovo impulso che rompe la zona attuale: chiudi la zona attuale.
                int closeAt = Math.Min(bar - 1, impulseStart - 1);
                if (closeAt >= _openZoneStart)
                {
                    AddZoneRectangle(_openZoneStart, closeAt, _openZoneLow, _openZoneHigh, "nuovo impulso rompe zona");
                    ResetOpenZone();
                }
                else
                {
                    ResetOpenZone();
                }

                _lastImpulseEnd = impulseEnd;
                _lastImpulseStart = impulseStart;
                _lastImpulseAvgRange = legAvgRange;
            }
        }
        else
        {
            _lastImpulseEnd = impulseEnd;
            _lastImpulseStart = impulseStart;
            _lastImpulseAvgRange = legAvgRange;
        }

        int compStart = impulseEnd + 1;
        if (compStart >= bar)
        {
            CloseOpenZoneIfAny(bar, "nessuna barra dopo l'impulso");
            _lastProcessedBar = bar;
            return;
        }

        // Se c'è una zona aperta e il prezzo la rompe con follow-through, chiudi l'era.
        if (_openZoneStart >= 0)
        {
            var cur = GetCandle(bar);
            decimal curRange = cur.High - cur.Low;
            decimal zoneRange = _zoneBoundaryHigh - _zoneBoundaryLow;
            decimal buffer = Math.Max(zoneRange * 0.15m, legAvgRange * 0.5m);
            bool breakUp = cur.Close > _zoneBoundaryHigh + buffer && curRange >= legAvgRange * 0.9m;
            bool breakDown = cur.Close < _zoneBoundaryLow - buffer && curRange >= legAvgRange * 0.9m;

            if (breakUp || breakDown)
            {
                int closeAt = bar - 1;
                if (closeAt >= _openZoneStart)
                {
                    AddZoneRectangle(_openZoneStart, closeAt, _openZoneLow, _openZoneHigh, "breakout chiude era");
                    ResetOpenZone();
                }
                else
                {
                    ResetOpenZone();
                }

                // Tratta la barra di breakout come inizio del nuovo leg
                _lastImpulseEnd = bar;
                _lastImpulseStart = bar;
                _lastImpulseAvgRange = curRange;

                _lastProcessedBar = bar;
                return;
            }
        }

        int compBars = bar - compStart + 1;
        GetHighLow(compStart, bar, out decimal compHigh, out decimal compLow);
        decimal compRange = compHigh - compLow;
        decimal compAvgRange = RobustAvgRange(compStart, bar);

        decimal rangeRatio = legRange > 0 ? compRange / legRange : 0;
        decimal avgRatio = legAvgRange > 0 ? compAvgRange / legAvgRange : 0;

        // Log numerico dei controlli su questa barra (tutta la history)
        LogBal($"[BAL]   -> LEGA endBar={impulseEnd} startBar={impulseStart} " +
               $"range={legRange:F2} avgRange={legAvgRange:F2} bars={impulseEnd - impulseStart + 1}");
        LogBal($"[BAL]   -> COMP start={compStart} bars={compBars} compRange={compRange:F2} " +
               $"compAvg={compAvgRange:F2} rangeRatio={rangeRatio:P0} avgRatio={avgRatio:P0}");

        bool minOk = compBars >= _minZoneBars;
        bool avgOk = avgRatio <= 0.65m;            // candelle compresse vs media del leg (più stretto)
        bool rangeOk = rangeRatio <= 2.0m;         // la zona può anche essere ampia, purché non in fuga

        if (!minOk || !avgOk || !rangeOk)
        {
            string why;
            if (!minOk)
                why = $"bars {compBars} < {_minZoneBars}";
            else if (!avgOk)
                why = $"avg ratio {avgRatio:P0} > 65%";
            else
                why = $"range ratio {rangeRatio:P0} > 200%";

            // Se la zona fugge oltre il 250% del leg, chiudila definitivamente;
            // altrimenti resta in attesa di un vero breakout.
            bool runaway = !rangeOk && rangeRatio > 2.5m;
            if (runaway || (!avgOk && avgRatio > 1.0m))
                CloseOpenZoneIfAny(bar, $"zona terminata ({why})");
            else
                LogBal($"[BAL] Bar={bar}: comp non ancora matura ({why}) start={compStart}");

            _lastProcessedBar = bar;
            return;
        }

        // La zona è valida. Aggiorna/estendi quella aperta.
        if (_openZoneStart < 0 || compStart != _openZoneStart)
        {
            if (_openZoneStart >= 0)
            {
                // Nuovo inizio di zona (replot su nuovo dealing range)
                AddZoneRectangle(_openZoneStart, bar - 1, _openZoneLow, _openZoneHigh, "replot su nuovo dealing range");
            }

            _openZoneStart = compStart;
            _openZoneHigh = compHigh;
            _openZoneLow = compLow;
            _zoneBoundaryHigh = compHigh;
            _zoneBoundaryLow = compLow;

            LogBal($"[BAL] *** ZONE OPEN *** bar={bar} start={compStart} end={bar} " +
                   $"high={compHigh:F2} low={compLow:F2} bars={compBars}");
        }
        else
        {
            _openZoneHigh = Math.Max(_openZoneHigh, compHigh);
            _openZoneLow = Math.Min(_openZoneLow, compLow);
            // I boundary restano fissi all'apertura: così il box non "insegue"
            // il prezzo per giorni, ma una chiusura vera li rompe nettamente.

            LogBal($"[BAL] *** ZONE EXTEND *** bar={bar} start={compStart} end={bar} " +
                   $"high={_openZoneHigh:F2} low={_openZoneLow:F2} bars={compBars}");
        }

        _lastProcessedBar = bar;
    }

    protected override void OnFinishRecalculate()
    {
        // Alla fine dei dati storici chiude l'ultima zona aperta (rendendola una
        // zona passata disegnata come box).
        if (_openZoneStart >= 0 && CurrentBar - 1 >= _openZoneStart)
        {
            AddZoneRectangle(_openZoneStart, CurrentBar - 1, _openZoneLow, _openZoneHigh, "fine dati storici");
        }

        ResetOpenZone();
        _lastProcessedBar = -1;
    }

    #endregion

    #region === Helpers detection ===

    // Ultimo impulso/leg espansivo rilevante prima della barra corrente.
    // "Rilevante" = range robusto rispetto alla volatilità locale degli ultimi ~50 barre
    // e corpo direzionale decente. Non ci si ferma al primo barra 1.5x la precedente.
    private int FindLastImpulse(int currentBar)
    {
        if (currentBar < 5)
            return -1;

        int lookbackStart = Math.Max(1, currentBar - Math.Min(currentBar, 50));
        decimal baseline = RobustAvgRange(lookbackStart, currentBar - 1);
        if (baseline < EffectiveTickSize)
            baseline = EffectiveTickSize;

        int maxStart = Math.Max(1, currentBar - Math.Min(currentBar, 30));
        decimal maxRecent = MaxRange(maxStart, currentBar - 1);
        decimal threshold = Math.Max(baseline * 2.5m, maxRecent * 0.65m);

        for (int i = currentBar - 1; i >= 1; i--)
        {
            var c = GetCandle(i);
            decimal range = c.High - c.Low;
            if (range <= EffectiveTickSize)
                continue;

            decimal body = Math.Abs(c.Close - c.Open);
            bool strongRange = range >= threshold;
            bool directionalBody = range > 0 && body / range >= 0.50m;

            if (strongRange && directionalBody)
                return i;
        }

        return -1;
    }

    // Trova l'inizio del leg che termina in impulseEnd, estendendosi a ritroso
    // tra barre consecutive che contribuiscono alla stessa direzione.
    private int FindImpulseStart(int impulseEnd)
    {
        if (impulseEnd <= 0)
            return 0;

        var c = GetCandle(impulseEnd);
        int dir = Math.Sign(c.Close - c.Open);
        if (dir == 0)
            dir = c.Close >= c.Low + (c.High - c.Low) / 2m ? 1 : -1;

        int start = impulseEnd;
        int maxLookback = Math.Max(0, impulseEnd - 12);

        for (int i = impulseEnd - 1; i >= maxLookback; i--)
        {
            var ic = GetCandle(i);
            decimal r = ic.High - ic.Low;
            if (r <= EffectiveTickSize)
                break;

            int idir = Math.Sign(ic.Close - ic.Open);
            decimal ibody = Math.Abs(ic.Close - ic.Open);
            bool contributes = (idir == dir && ibody >= r * 0.35m) || ibody >= r * 0.6m;

            if (contributes)
                start = i;
            else
                break;
        }

        return start;
    }

    // Range massimo del periodo
    private decimal CalculateRange(int start, int end)
    {
        if (start > end)
            return 0;

        GetHighLow(start, end, out decimal hi, out decimal lo);
        return hi - lo;
    }

    // Range massimo nel periodo (usato per filtrare impulsi veramente strutturali).
    private decimal MaxRange(int start, int end)
    {
        if (start > end)
            return 0;
        decimal max = 0;
        for (int i = start; i <= end; i++)
        {
            decimal r = GetCandle(i).High - GetCandle(i).Low;
            if (r > max) max = r;
        }
        return max;
    }

    // Media range robusta: esclude le 2 barre più ampie per non far saltare
    // la media a causa di outlier (fakeout/spike) all'interno della compressione.
    private decimal RobustAvgRange(int start, int end)
    {
        if (start > end)
            return 0;

        int n = end - start + 1;
        if (n <= 2)
        {
            decimal sum = 0;
            for (int i = start; i <= end; i++)
                sum += GetCandle(i).High - GetCandle(i).Low;
            return n == 0 ? 0 : sum / n;
        }

        decimal total = 0;
        decimal max1 = 0;
        decimal max2 = 0;

        for (int i = start; i <= end; i++)
        {
            decimal r = GetCandle(i).High - GetCandle(i).Low;
            total += r;

            if (r > max1)
            {
                max2 = max1;
                max1 = r;
            }
            else if (r > max2)
            {
                max2 = r;
            }
        }

        return (total - max1 - max2) / (n - 2);
    }

    private void GetHighLow(int start, int end, out decimal hi, out decimal lo)
    {
        hi = decimal.MinValue;
        lo = decimal.MaxValue;

        for (int i = start; i <= end; i++)
        {
            var c = GetCandle(i);
            if (c.High > hi) hi = c.High;
            if (c.Low < lo) lo = c.Low;
        }

        if (hi == decimal.MinValue) hi = 0;
        if (lo == decimal.MaxValue) lo = 0;
    }

    #endregion

    #region === Helpers grafici / stato ===

    private int ComputeMinZoneBars()
    {
        int tfMinutes = ParseTimeFrameMinutes(ChartInfo?.TimeFrame);
        int needed = (int)Math.Ceiling((decimal)MinDurationMinutes / tfMinutes);
        return Math.Max(needed, 5);
    }

    private int ParseTimeFrameMinutes(string? tf)
    {
        if (string.IsNullOrWhiteSpace(tf))
            return 5;

        string s = tf.Trim().ToLowerInvariant();

        if (s.Contains("day") || s.Contains("daily"))
            return 1440;
        if (s.Contains("week"))
            return 10080;
        if (s.Contains("hour") || s.Contains("hr") || s.Contains("h"))
        {
            var m = System.Text.RegularExpressions.Regex.Match(s, @"\d+(\.\d+)?");
            if (m.Success && decimal.TryParse(m.Value, System.Globalization.NumberStyles.Any,
                System.Globalization.CultureInfo.InvariantCulture, out decimal h))
                return (int)Math.Max(1, h * 60);
            return 60;
        }

        {
            var m = System.Text.RegularExpressions.Regex.Match(s, @"\d+(\.\d+)?");
            if (m.Success && decimal.TryParse(m.Value, System.Globalization.NumberStyles.Any,
                System.Globalization.CultureInfo.InvariantCulture, out decimal min))
                return (int)Math.Max(1, min);
        }

        return 5;
    }

    private void ResetState()
    {
        _lastLoggedBar = -1;
        _lastProcessedBar = -1;
        _lastImpulseEnd = -1;
        _lastImpulseStart = -1;
        _lastImpulseAvgRange = 0;
        ResetOpenZone();
    }

    private void ResetOpenZone()
    {
        _openZoneStart = -1;
        _openZoneHigh = decimal.MinValue;
        _openZoneLow = decimal.MaxValue;
        _zoneBoundaryHigh = decimal.MinValue;
        _zoneBoundaryLow = decimal.MaxValue;
    }

    private void LogBarDetails(int bar)
    {
        if (bar == _lastLoggedBar)
            return;

        var c = GetCandle(bar);
        decimal range = c.High - c.Low;
        decimal body = Math.Abs(c.Close - c.Open);
        string extra = c.Volume > 0 ? $" vol={c.Volume:F0}" : "";
        if (c.Delta != 0)
            extra += $" delta={c.Delta:F0}";

        LogBal($"[BAL] BAR bar={bar} {c.Time:yyyy-MM-dd HH:mm} O={c.Open:F2} H={c.High:F2} " +
               $"L={c.Low:F2} C={c.Close:F2} rng={range:F2} body={body:F2}" + extra);

        _lastLoggedBar = bar;
    }

    private void CloseOpenZoneIfAny(int currentBar, string reason)
    {
        if (_openZoneStart < 0)
            return;

        int closeBar = currentBar - 1;
        if (closeBar >= _openZoneStart)
        {
            AddZoneRectangle(_openZoneStart, closeBar, _openZoneLow, _openZoneHigh, reason);
        }
        else
        {
            LogBal($"[BAL]   -> open zone skipped (closeBar={closeBar} < start={_openZoneStart}) reason={reason}");
        }

        ResetOpenZone();
    }

    private void AddZoneRectangle(int firstBar, int secondBar, decimal lowPrice, decimal highPrice, string reason)
    {
        if (firstBar >= secondBar || firstBar < 0)
            return;

        LogBal($"[BAL] +++ RECT ADDED ({reason}) bars={firstBar}..{secondBar} " +
               $"low={lowPrice:F2} high={highPrice:F2}");

        // Non disporre Pen/Brush: DrawingRectangle può mantenerli in vita per il rendering del chart.
        var outlinePen = new System.Drawing.Pen(System.Drawing.Color.DarkCyan, 1);
        var fillBrush = new System.Drawing.SolidBrush(System.Drawing.Color.FromArgb(40, 0, 255, 255));

        var rect = new DrawingRectangle(firstBar, lowPrice, secondBar, highPrice, outlinePen, fillBrush)
        {
            ExtendLeft = false,
            ExtendRight = false
        };
        Rectangles.Add(rect);
    }

    #endregion
}
