// ============================================================================
// FabioMeanReversion — Volume-Profile Balance Detection (phase 1: log only)
// ============================================================================
// Output: NESSUN rettangolo. Solo log di candidature/zone per validare la logica.
//
// Logica:
//   1. Finestra mobile minima calcolata dal timeframe (fase 1, poi dinamica).
//   2. Se la finestra corrente ha un profilo compatto (Value Area stretta,
//      POC centrale, delta bilanciato) si apre una candidata.
//   3. La candidata viene estesa barra per barra finché il profilo resta
//      bilanciato: la finestra è dinamica (da seed minimo fino al breakout).
//   4. Quando il profilo non è più bilanciato, la candidata viene chiusa e
//      loggata come potenziale zona se ha durata sufficiente.
// ============================================================================

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using ATAS.Indicators;

namespace FabioMeanReversion;

[DisplayName("Fabio Mean Reversion")]
public class FabioMeanReversion : Indicator
{
    #region === Soglie interne (non esposte, da tarare coi log) ===

    // Rapporto VA / range totale. Valori bassi = volume concentrato.
    private const decimal MaxConcentration = 0.55m;

    // Posizione POC all'interno del range (0 = sul minimo, 1 = sul massimo).
    private const decimal MinPocPosition = 0.30m;
    private const decimal MaxPocPosition = 0.70m;

    // |delta cumulato| / volume totale nella finestra. Basso = assenza di spinta direzionale.
    private const decimal MaxDeltaRatio = 0.50m;

    // Durata minima di una candidata per essere loggata come zona (minuti di chart).
    private const int MinDurationMinutes = 30;

    #endregion

    #region === Logger ===

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

    #region === Stato ===

    private int _lookbackBars = 12;        // seed minimo per aprire una candidata
    private int _minCandidateBars = 6;     // durata minima in barre per loggare una zona

    // Profilo della candidata aperta (se presente)
    private int _candidateStart = -1;
    private decimal _candidateHigh = decimal.MinValue;
    private decimal _candidateLow = decimal.MaxValue;
    private readonly Dictionary<decimal, decimal> _profile = new();
    private decimal _profileTotalVolume;
    private decimal _profileTotalDelta;

    #endregion

    #region === Costruttore ===

    public FabioMeanReversion() : base(true)
    {
        DenyToChangePanel = true;
        DataSeries[0].IsHidden = true;
        LogBal("=== FabioMeanReversion VOLUME-PROFILE PHASE 1 (log only) loaded ===");
    }

    #endregion

    #region === Calcolo principale ===

    protected override void OnCalculate(int bar, decimal value)
    {
        if (bar == 0)
        {
            Rectangles.Clear();
            ResetCandidate();
            _lookbackBars = ComputeLookbackBars();
            _minCandidateBars = Math.Max(5, (int)Math.Ceiling((decimal)MinDurationMinutes / Math.Max(1, ParseTimeFrameMinutes(ChartInfo?.TimeFrame))));

            LogBal($"=== RECALC START bars={CurrentBar} tf={ChartInfo?.TimeFrame ?? "?"} " +
                   $"lookback={_lookbackBars} minCandidateBars={_minCandidateBars} ===");
        }

        if (bar < 0)
            return;

        if (_candidateStart < 0)
        {
            // Cerca di aprire una candidata sulla finestra mobile minima.
            int start = bar - _lookbackBars + 1;
            if (start < 0)
                return;

            if (TryStartCandidate(start, bar, out decimal high, out decimal low, out var metrics))
            {
                _candidateStart = start;
                _candidateHigh = high;
                _candidateLow = low;
                LogBal($"[CANDIDATE START] bar={bar} {GetCandle(bar).Time:yyyy-MM-dd HH:mm} " +
                       $"startBar={start} high={high:F2} low={low:F2} " +
                       $"conc={metrics.Concentration:P2} pocPos={metrics.PocPosition:P2} deltaRatio={metrics.DeltaRatio:P2}");
            }
        }
        else
        {
            // Candidata già aperta: estendi con la barra corrente e valuta.
            decimal prevHigh = _candidateHigh;
            decimal prevLow = _candidateLow;
            AddBarToProfile(bar);
            UpdateCandidateHighLow(bar);

            var metrics = EvaluateProfile();
            if (!metrics.IsBalanced)
            {
                // Chiude al bordo precedente: la barra corrente ha rotto il balance.
                _candidateHigh = prevHigh;
                _candidateLow = prevLow;
                CloseCandidate(bar - 1, $"imbalance conc={metrics.Concentration:P2} " +
                    $"pocPos={metrics.PocPosition:P2} deltaRatio={metrics.DeltaRatio:P2}");
                return;
            }

            LogBal($"[CANDIDATE EXTEND] bar={bar} {GetCandle(bar).Time:yyyy-MM-dd HH:mm} " +
                   $"startBar={_candidateStart} high={_candidateHigh:F2} low={_candidateLow:F2} " +
                   $"bars={bar - _candidateStart + 1} " +
                   $"conc={metrics.Concentration:P2} pocPos={metrics.PocPosition:P2} deltaRatio={metrics.DeltaRatio:P2}");
        }
    }

    protected override void OnFinishRecalculate()
    {
        if (_candidateStart >= 0 && CurrentBar - 1 >= _candidateStart)
            CloseCandidate(CurrentBar - 1, "fine dati storici");
    }

    #endregion

    #region === Logica volume-profile ===

    // Prova ad aprire una candidata sulla finestra [start..end].
    private bool TryStartCandidate(int start, int end, out decimal high, out decimal low, out ProfileMetrics metrics)
    {
        high = decimal.MinValue;
        low = decimal.MaxValue;
        metrics = default;

        ResetCandidate();

        for (int i = start; i <= end; i++)
        {
            AddBarToProfile(i);
            var c = GetCandle(i);
            if (c.High > high) high = c.High;
            if (c.Low < low) low = c.Low;
        }

        if (_profile.Count == 0 || _profileTotalVolume <= 0)
            return false;

        _candidateHigh = high;
        _candidateLow = low;
        metrics = EvaluateProfile();

        if (!metrics.IsBalanced)
        {
            ResetCandidate();
            return false;
        }

        return true;
    }

    private void AddBarToProfile(int bar)
    {
        var c = GetCandle(bar);
        foreach (var lvl in c.GetAllPriceLevels())
        {
            if (lvl.Volume <= 0)
                continue;

            _profile.TryGetValue(lvl.Price, out var existing);
            _profile[lvl.Price] = existing + lvl.Volume;
            _profileTotalVolume += lvl.Volume;
        }

        _profileTotalDelta += c.Delta;
    }

    private void UpdateCandidateHighLow(int bar)
    {
        var c = GetCandle(bar);
        if (c.High > _candidateHigh) _candidateHigh = c.High;
        if (c.Low < _candidateLow) _candidateLow = c.Low;
    }

    private ProfileMetrics EvaluateProfile()
    {
        var result = new ProfileMetrics();
        decimal range = _candidateHigh - _candidateLow;

        if (_profile.Count == 0 || _profileTotalVolume <= 0 || range <= 0)
            return result;

        // POC
        decimal pocPrice = 0, maxVol = 0;
        foreach (var kv in _profile)
        {
            if (kv.Value > maxVol)
            {
                maxVol = kv.Value;
                pocPrice = kv.Key;
            }
        }

        // Value Area al 70%: accumula livelli dal più volume al meno volume.
        decimal accVol = 0;
        decimal vaMin = decimal.MaxValue;
        decimal vaMax = decimal.MinValue;
        foreach (var kv in _profile.OrderByDescending(x => x.Value))
        {
            accVol += kv.Value;
            if (kv.Key < vaMin) vaMin = kv.Key;
            if (kv.Key > vaMax) vaMax = kv.Key;
            if (accVol >= _profileTotalVolume * 0.70m)
                break;
        }

        result.Concentration = range > 0 ? (vaMax - vaMin) / range : 1m;
        result.PocPosition = range > 0 ? (pocPrice - _candidateLow) / range : 0.5m;
        result.DeltaRatio = _profileTotalVolume > 0 ? Math.Abs(_profileTotalDelta) / _profileTotalVolume : 1m;

        result.IsBalanced =
            result.Concentration <= MaxConcentration &&
            result.PocPosition >= MinPocPosition &&
            result.PocPosition <= MaxPocPosition &&
            result.DeltaRatio <= MaxDeltaRatio;

        return result;
    }

    private void CloseCandidate(int endBar, string reason)
    {
        if (_candidateStart < 0)
            return;

        int bars = endBar - _candidateStart + 1;
        string zoneTag = bars >= _minCandidateBars ? "[POTENTIAL ZONE]" : "[CANDIDATE DISCARDED]";

        LogBal($"{zoneTag} closed bar={endBar} {GetCandle(endBar).Time:yyyy-MM-dd HH:mm} " +
               $"startBar={_candidateStart} bars={bars} " +
               $"high={_candidateHigh:F2} low={_candidateLow:F2} reason={reason}");

        ResetCandidate();
    }

    private void ResetCandidate()
    {
        _candidateStart = -1;
        _candidateHigh = decimal.MinValue;
        _candidateLow = decimal.MaxValue;
        _profile.Clear();
        _profileTotalVolume = 0;
        _profileTotalDelta = 0;
    }

    #endregion

    #region === Timeframe helpers ===

    private int ComputeLookbackBars()
    {
        int tfMinutes = ParseTimeFrameMinutes(ChartInfo?.TimeFrame);
        // seed minimo: 30 minuti di chart (es. 6 barre M5, 30 barre M1)
        int seed = (int)Math.Ceiling(30m / Math.Max(1, tfMinutes));
        return Math.Max(seed, 5);
    }

    private static int ParseTimeFrameMinutes(string? tf)
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

    #endregion

    #region === Strutture ===

    private struct ProfileMetrics
    {
        public bool IsBalanced;
        public decimal Concentration;
        public decimal PocPosition;
        public decimal DeltaRatio;
    }

    #endregion
}
