using System;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace FabioOrderFlow.Observation;

/// <summary>
/// Il ruolo del chart: un indicatore solo, che disegna cose diverse secondo il timeframe.
///
/// <para><b>Perche'.</b> Il 9 ottobre 2026 l'utente ha chiesto un workspace diviso in tab, ognuna
/// con una responsabilita': il composito su una, le sedute della settimana su un'altra, la seduta
/// di adesso sull'M1. Con un indicatore unico che disegnava tutto dappertutto, ogni chart mostrava
/// tutto, e non si capiva niente. Il ruolo lo decide il timeframe, cosi' la stessa DLL va bene su
/// ogni tab e non c'e' niente da configurare; l'impostazione <see cref="Ruolo"/> serve solo a
/// forzarlo.</para>
///
/// <code>
/// Daily e oltre         COMPOSITO   il profilo dei 90 giorni, premium / fair value / discount
/// da M15 a H4           SETTIMANA   una gaussiana per ogni seduta cash, e il framing
/// da M1 a M10           SEDUTA      la seduta di adesso, le due prima, il pannello, il tape
/// range, tick, secondi  ESECUZIONE  la seduta di adesso e le due prima, senza pannello
/// </code>
///
/// <para>Le fonti: M1 primario <c>[1 · 1:24:04]</c>, 40 range quando e' choppy <c>[2 · 1:16:00]</c>,
/// una gaussiana per seduta <c>[1 · 22:13]</c>, composito di 90 giorni <c>[4 · 14:18]</c>.</para>
/// </summary>
public sealed partial class DataBridge
{
    public enum RuoloDelChart
    {
        Auto,
        Composito,
        Settimana,
        Seduta,
        Esecuzione,
    }

    [Display(Name = "Role (Auto = from timeframe)", GroupName = "Bridge",
        Description = "Auto: Daily = composito 90 giorni, M15-H4 = sedute della settimana, " +
                      "M1-M10 = seduta e pannello, range/tick = esecuzione. Gli altri valori lo forzano.")]
    public RuoloDelChart Ruolo { get; set; } = RuoloDelChart.Auto;

    /// <summary>Il ruolo che vale adesso: quello forzato, o quello che dice il timeframe.</summary>
    private RuoloDelChart RuoloEffettivo
    {
        get
        {
            if (Ruolo != RuoloDelChart.Auto)
            {
                return Ruolo;
            }

            var tipo = Convert.ToString(ChartInfo?.ChartType) ?? string.Empty;
            var tf = (Convert.ToString(ChartInfo?.TimeFrame) ?? string.Empty).Trim();
            if (!string.Equals(tipo, "TimeFrame", StringComparison.OrdinalIgnoreCase))
            {
                // Range, tick, volume, secondi: il chart dell'esecuzione, non del contesto.
                return string.IsNullOrEmpty(tipo) ? RuoloDelChart.Seduta : RuoloDelChart.Esecuzione;
            }

            var minuti = MinutiDi(tf);
            return minuti switch
            {
                null => RuoloDelChart.Composito, // Daily, Weekly, Monthly
                >= 15 => RuoloDelChart.Settimana,
                _ => RuoloDelChart.Seduta,
            };
        }
    }

    /// <summary>I minuti di una barra da "M30" o "H1"; null per Daily e oltre.</summary>
    private static int? MinutiDi(string tf)
    {
        if (tf.Length < 2 || !int.TryParse(tf[1..], out var n))
        {
            return null;
        }

        return char.ToUpperInvariant(tf[0]) switch
        {
            'M' => n,
            'H' => n * 60,
            _ => null,
        };
    }

    /// <summary>Solo la seduta misura il tape, i muri e le regole: sugli altri chart sarebbe lavoro doppio.</summary>
    private bool SonoLaSeduta => RuoloEffettivo == RuoloDelChart.Seduta;

    /// <summary>
    /// <c>/ruolo</c>: il ruolo del chart e cio' che quel ruolo disegna, in numeri. Serve all'agente
    /// a leggere il composito e le sedute della settimana senza ricalcolarli, e a controllare che
    /// quello che si vede sul chart sia quello che crede.
    /// </summary>
    private object RuoloInJson()
    {
        var ruolo = RuoloEffettivo;
        object? dati = null;
        if (ruolo == RuoloDelChart.Composito && IlCompositoDelChart() is { } c)
        {
            var prezzo = GetCandle(CurrentBar - 1).Close;
            var alto = c.Vah + c.Grana;
            dati = new
            {
                dal = Iso(c.Dal),
                barre = c.Barre,
                grana = c.Grana,
                val = c.Val,
                vah = alto,
                poc = c.Poc + c.Grana / 2,
                prezzo,
                zona = prezzo > alto ? "premium" : prezzo < c.Val ? "discount" : "fair value",
                aggiustato = c.Scarto == 0m ? null : new
                {
                    scarto = c.Scarto,
                    finoA = c.FinoA.ToString("yyyy-MM-dd"),
                    val = c.ValAgg,
                    vah = c.VahAgg + c.Grana,
                    poc = c.PocAgg + c.Grana / 2,
                    zona = prezzo > c.VahAgg + c.Grana ? "premium" : prezzo < c.ValAgg ? "discount" : "fair value",
                },
                nota = "barre Daily: 24 ore; i prezzi grezzi sono quelli del continuo, l'aggiustato sposta i giorni prima del rollover",
            };
        }
        else if (ruolo == RuoloDelChart.Settimana)
        {
            dati = LeSeduteDellaSettimana().Select(s => new
            {
                giorno = s.Giorno.ToString("yyyy-MM-dd"),
                val = s.P.Val,
                vah = s.P.Vah,
                poc = s.P.Poc,
                massimo = s.P.Massimo,
                minimo = s.P.Minimo,
                inCorso = s.InCorso,
            });
        }
        else
        {
            dati = LeZone().Select(z => new { nome = z.Nome, sigla = z.Sigla, val = z.Val, vah = z.Vah, poc = z.Poc, inFormazione = z.Oggi });
        }

        return new { schema = Schema, chart = ChartDescriptor(), ruolo = ruolo.ToString(), impostato = Ruolo.ToString(), dati };
    }
}
