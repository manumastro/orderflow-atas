using Quant.Validazione;

namespace Quant.Dati;

/// <summary>
/// Un mercato finto con le misure di NQ: giornate Globex dalle 18:00 alle 17:00 ET, barre a un
/// minuto, volatilita' piu' alta all'apertura e alla chiusura della cassa.
///
/// Serve a validare il framework prima delle strategie. Su una passeggiata casuale nessuna
/// strategia deve superare i cancelli; con un vantaggio piantato apposta (una deriva nella notte)
/// la validazione deve trovarlo. Un framework che fallisce una delle due prove non e' pronto.
/// </summary>
public static class Sintetico
{
    public sealed record Opzioni
    {
        public int Giorni { get; init; } = 500;
        public DateOnly Inizio { get; init; } = new(2020, 1, 6);
        public double PrezzoIniziale { get; init; } = 15_000;
        /// <summary>Volatilita' giornaliera in punti (deviazione standard del movimento di una giornata).</summary>
        public double VolatilitaGiorno { get; init; } = 200;
        /// <summary>Deriva in punti per notte (16:00 -> 9:30 ET): il vantaggio piantato. Zero = nessun vantaggio.</summary>
        public double DerivaNotte { get; init; }
        /// <summary>Deriva in punti per seduta di cassa (9:30 -> 16:00 ET).</summary>
        public double DerivaCassa { get; init; }
        public ulong Seme { get; init; } = 42;
    }

    public static Barra[] Genera(Opzioni o)
    {
        var rng = new Caso(o.Seme);
        var barre = new List<Barra>(o.Giorni * 1380);
        var tick = Strumento.NQ.Tick;
        var prezzo = o.PrezzoIniziale;
        var data = o.Inizio;
        // peso della varianza per minuto: la notte pesa 0,35, la cassa 1, apertura e chiusura 3
        var pesi = new double[1440];
        for (var m = 0; m < 1440; m++)
            pesi[m] = m >= 570 && m < 600 ? 3.0 : m >= 930 && m < 960 ? 2.0 : m >= 570 && m < 960 ? 1.0 : 0.35;
        var minutiNotte = Enumerable.Range(0, 1440).Count(m => m >= 960 || m < 570);
        var sommaPesi = 0.0;
        for (var k = 0; k < 1380; k++) sommaPesi += pesi[(18 * 60 + k) % 1440];
        var sigmaBase = o.VolatilitaGiorno / Math.Sqrt(sommaPesi);

        for (var g = 0; g < o.Giorni; g++)
        {
            while (data.DayOfWeek is DayOfWeek.Saturday or DayOfWeek.Sunday) data = data.AddDays(1);
            var apertura = Calendario.Utc(data.AddDays(-1), 18 * 60);
            for (var k = 0; k < 1380; k++)
            {
                var inizio = apertura + k * Tempo.Minuto;
                var m = (18 * 60 + k) % 1440;
                if (m == 17 * 60) break; // pausa delle 17:00
                var sigma = sigmaBase * Math.Sqrt(pesi[m]);
                var deriva = m >= 570 && m < 960 ? o.DerivaCassa / 390.0 : o.DerivaNotte / (minutiNotte - 60);
                double a = prezzo, hi = prezzo, lo = prezzo;
                for (var s = 0; s < 8; s++)
                {
                    prezzo += deriva / 8 + sigma / Math.Sqrt(8) * rng.Normale();
                    hi = Math.Max(hi, prezzo);
                    lo = Math.Min(lo, prezzo);
                }
                var vol = (int)(200 * pesi[m] * (0.5 + rng.Uniforme())) + 1;
                var compra = (int)(vol * (0.4 + 0.2 * rng.Uniforme()));
                barre.Add(new Barra
                {
                    Inizio = inizio, Fine = inizio + Tempo.Minuto - 1,
                    Apertura = (int)Math.Round(a / tick), Massimo = (int)Math.Round(hi / tick),
                    Minimo = (int)Math.Round(lo / tick), Chiusura = (int)Math.Round(prezzo / tick),
                    Volume = vol, VolumeAsk = compra, VolumeBid = vol - compra,
                });
            }
            data = data.AddDays(1);
        }
        return [.. barre];
    }

    public static Serie Serie(Opzioni o) => new(Strumento.Da("SINT"), Genera(o), "SINT m1 sintetica");
}
