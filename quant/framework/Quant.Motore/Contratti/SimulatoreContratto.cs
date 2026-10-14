namespace Quant.Motore.Contratti;

public enum FineFase { Target, Pavimento, LimiteRigido, Scadenza, Orizzonte, MassimoPrelievi }

/// <summary>Un ciclo del compratore: tentativi fino a passare tutte le fasi, poi il conto finanziato fino alla fine.</summary>
public struct Ciclo
{
    public int Tentativi;
    /// <summary>Tentativi in cui si e' passata ciascuna fase di valutazione, e quelli in cui ci si e' arrivati.</summary>
    public int[] Passate;
    public int[] Arrivate;
    public int SessioniValutazione;
    public bool Finanziato;
    public int SessioniFinanziate;
    public int NumeroPrelievi;
    public double Prelevato;
    /// <summary>Quanto il compratore ha pagato e incassato in soldi veri: e' cio' che conta (Youngblood §3).</summary>
    public double Costo;
    public double Incassato;
    public FineFase FineFinanziata;
    public double Netto => Incassato - Costo;
}

/// <summary>
/// Il simulatore di contratto: applica le regole di un <see cref="Contratto"/> alle sessioni di una
/// sorgente e restituisce i soldi veri del compratore. Ogni ciclo e' indipendente, con il suo flusso
/// del generatore: il risultato non dipende dal numero di thread.
/// </summary>
public static class SimulatoreContratto
{
    public sealed record Opzioni
    {
        /// <summary>Oltre questi tentativi il ciclo si chiude senza conto finanziato (il compratore smette).</summary>
        public int MassimoTentativi { get; init; } = 1_000;
        /// <summary>Sessioni massime nella fase finanziata.</summary>
        public int OrizzonteFinanziato { get; init; } = 1_000;
        /// <summary>Dopo questi prelievi il conto simulato finisce (per esempio la chiamata al vivo). Null: nessun limite.</summary>
        public int? MassimoPrelievi { get; init; }
        /// <summary>Simula solo la valutazione: niente fase finanziata.</summary>
        public bool SoloValutazione { get; init; }
    }

    public static Ciclo[] Esegui(Contratto contratto, ISorgenteSessioni sorgente, int cicli, ulong seme, Opzioni? opzioni = null)
    {
        opzioni ??= new Opzioni();
        var esiti = new Ciclo[cicli];
        Parallel.For(0, cicli,
            () => (new double[sorgente.MassimoPassi], new double[sorgente.MassimoPassi]),
            (i, _, buf) =>
            {
                var rng = new Caso(seme, (ulong)i);
                esiti[i] = UnCiclo(contratto, sorgente.Apri(), ref rng, buf.Item1, buf.Item2, opzioni);
                return buf;
            },
            _ => { });
        return esiti;
    }

    public static Ciclo UnCiclo(Contratto c, ICursoreSessioni cursore, ref Caso rng, double[] chiusura, double[] minimo, Opzioni o)
    {
        var fasi = c.Valutazione.Length;
        var ciclo = new Ciclo { Passate = new int[fasi], Arrivate = new int[fasi], Costo = c.Prezzi.Ingresso };
        while (true)
        {
            ciclo.Tentativi++;
            if (ciclo.Tentativi > 1) ciclo.Costo += c.Prezzi.Reset;
            var passato = true;
            for (var f = 0; f < fasi; f++)
            {
                ciclo.Arrivate[f]++;
                var (fine, sessioni) = Valutazione(c.Valutazione[f], cursore, ref rng, chiusura, minimo);
                ciclo.SessioniValutazione += sessioni;
                if (fine != FineFase.Target) { passato = false; break; }
                ciclo.Passate[f]++;
            }
            if (passato) break;
            if (ciclo.Tentativi >= o.MassimoTentativi) return ciclo;
        }

        ciclo.Finanziato = true;
        ciclo.Costo += c.Prezzi.Attivazione;
        if (o.SoloValutazione) return ciclo;
        Finanziata(c, ref ciclo, cursore, ref rng, chiusura, minimo, o);
        return ciclo;
    }

    /// <summary>Una fase di valutazione: fino al target, alla violazione o alla scadenza.</summary>
    public static (FineFase Fine, int Sessioni) Valutazione(Fase f, ICursoreSessioni cursore, ref Caso rng, double[] chiusura, double[] minimo)
    {
        var conto = new Conto(f);
        var target = f.Target ?? throw new InvalidOperationException($"la fase {f.Nome} non ha un target");
        for (var s = 1; ; s++)
        {
            var n = cursore.Prossima(ref rng, chiusura, minimo);
            var esito = conto.Sessione(chiusura.AsSpan(0, n), minimo.AsSpan(0, n), target);
            if (esito is { } fine) return (fine, s);
            if (f.Scadenza is { } sc && s >= sc) return (FineFase.Scadenza, s);
        }
    }

    private static void Finanziata(Contratto c, ref Ciclo ciclo, ICursoreSessioni cursore, ref Caso rng, double[] chiusura, double[] minimo, Opzioni o)
    {
        var f = c.Finanziata;
        var p = f.Prelievi ?? throw new InvalidOperationException("la fase finanziata non ha regole di prelievo");
        var conto = new Conto(f);
        double basePeriodo = 0, migliore = 0;
        var giorniBuoni = 0;
        for (var s = 1; s <= o.OrizzonteFinanziato; s++)
        {
            var n = cursore.Prossima(ref rng, chiusura, minimo);
            var prima = conto.Saldo;
            var esito = conto.Sessione(chiusura.AsSpan(0, n), minimo.AsSpan(0, n), double.PositiveInfinity);
            ciclo.SessioniFinanziate = s;
            if (esito is { } fine) { ciclo.FineFinanziata = fine; return; }

            var giorno = conto.Saldo - prima;
            if (giorno > p.ProfittoGiorno) giorniBuoni++;
            if (giorno > migliore) migliore = giorno;

            double prelievo = 0;
            if (p.Intervallo is { } k)
            {
                if (s % k == 0 && conto.Saldo > 0)
                {
                    prelievo = conto.Saldo;
                    conto.Azzera();
                }
            }
            else
            {
                var periodo = conto.Saldo - basePeriodo;
                var consistente = p.Consistenza is not { } q || (periodo > 0 && migliore <= q * periodo);
                if (conto.Saldo >= p.SaldoMinimo && giorniBuoni >= p.GiorniMinimi && consistente)
                {
                    if (p.BloccaPavimento) conto.BloccaPavimento();
                    var tetto = p.Massimi.Length == 0 ? double.PositiveInfinity : p.Massimi[Math.Min(ciclo.NumeroPrelievi, p.Massimi.Length - 1)];
                    var possibile = Math.Min(tetto, conto.Saldo - Math.Max(conto.Pavimento, 0));
                    if (possibile >= p.Minimo)
                    {
                        prelievo = possibile;
                        conto.Preleva(prelievo);
                        basePeriodo = conto.Saldo;
                        migliore = 0;
                        giorniBuoni = 0;
                    }
                }
            }

            if (prelievo > 0)
            {
                if (ciclo.NumeroPrelievi == 0 && c.Prezzi.RimborsoAlPrimoPrelievo) ciclo.Incassato += c.Prezzi.Ingresso;
                ciclo.NumeroPrelievi++;
                ciclo.Prelevato += prelievo;
                ciclo.Incassato += p.Quota * prelievo;
                if (o.MassimoPrelievi is { } mp && ciclo.NumeroPrelievi >= mp) { ciclo.FineFinanziata = FineFase.MassimoPrelievi; return; }
            }
        }
        ciclo.FineFinanziata = FineFase.Orizzonte;
    }

    /// <summary>
    /// Il conto dentro una fase: saldo, massimo e pavimento (TF eq. 2), limite giornaliero.
    /// L'ordine dentro un passo: limite giornaliero, poi violazione del pavimento, poi target
    /// (Lim §4.2: la regola giornaliera prima delle barriere cumulate).
    /// </summary>
    private sealed class Conto(Fase f)
    {
        public double Saldo { get; private set; }
        private double _massimo;
        public double Pavimento { get; private set; } = Math.Min(-f.Drawdown, f.Blocco ?? double.PositiveInfinity);
        private bool _bloccato;
        private double? _limite = f.LimiteGiornaliero;
        private bool _alzaDomani;

        public FineFase? Sessione(ReadOnlySpan<double> chiusura, ReadOnlySpan<double> minimo, double target)
        {
            if (_alzaDomani) { _limite = f.LimiteGiornalieroAlzato; _alzaDomani = false; }
            double pnl = 0;
            for (var k = 0; k < chiusura.Length; k++)
            {
                var c = chiusura[k];
                var m = minimo[k];
                var fermo = false;
                if (_limite is { } lim && m <= -lim)
                {
                    if (f.LimiteRigido) return FineFase.LimiteRigido;
                    // limite morbido: si esce al limite e si smette per la giornata. Prima pero' il
                    // percorso puo' aver toccato il pavimento, se questo era piu' vicino del limite.
                    m = -lim;
                    c = -lim;
                    fermo = true;
                }
                if (!f.ViolazioneAllaChiusura && Saldo + m <= Pavimento) return FineFase.Pavimento;
                if (Saldo + c >= target) return FineFase.Target;
                if (!f.PavimentoAllaChiusura || (f.PavimentoOgniPassi is { } ogni && (k + 1) % ogni == 0)) Aggiorna(Saldo + c);
                pnl = c;
                if (fermo) break;
            }
            Saldo += pnl;
            if (f.ViolazioneAllaChiusura && Saldo <= Pavimento) return FineFase.Pavimento;
            Aggiorna(Saldo);
            if (f.SogliaLimiteAlzato is { } soglia && f.LimiteGiornalieroAlzato is not null && Saldo >= soglia) _alzaDomani = true;
            return null;
        }

        private void Aggiorna(double valore)
        {
            if (_bloccato || valore <= _massimo) return;
            _massimo = valore;
            Pavimento = Math.Min(_massimo - f.Drawdown, f.Blocco ?? double.PositiveInfinity);
        }

        public void BloccaPavimento()
        {
            if (f.Blocco is { } b) Pavimento = Math.Max(Pavimento, b);
            _bloccato = true;
        }

        public void Preleva(double x) => Saldo -= x;

        /// <summary>Lim: il saldo torna al livello iniziale dopo ogni prelievo; le barriere sono statiche.</summary>
        public void Azzera() { Saldo = 0; _massimo = 0; }
    }
}
