using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using ATAS.Indicators;
using OFT.Rendering.Context;
using OFT.Rendering.Tools;
using Utils.Common.Logging;

namespace FabioOrderFlow.Observation;

/// <summary>
/// Il ruolo SETTIMANA, sul chart da M15 a H4: una gaussiana per ogni seduta cash, e il framing.
///
/// <para><b>La fonte.</b> <i>"If this is the Gaus distribution of the session of Monday, this is the
/// Gaus distribution of the session of Tuesday... the value is building up"</i> (<c>fabio_1.txt</c>,
/// 22:13); shift e' VAL piu' basso <b>e</b> VAH piu' basso (24:02), contano il POC della seduta e
/// quello della seduta che ha rotto (24:35). Solo la cassa: <i>"I use only the cash session"</i>
/// (1:14:20).</para>
///
/// <para><b>I colori sono quelli delle zone sull'M1</b>: PD (ieri), il giorno prima, la seduta di
/// framing quando il prezzo e' uscito dal valore delle ultime due, la seduta di oggi in corso. Le altre
/// sono grigie. Cosi' il tab della settimana dice <i>perche'</i> sull'M1 ci sono quelle scritte.</para>
///
/// <para><b>Su M30 e non su H1</b>, nel workspace: la cassa apre alle :30, e una barra H1 delle 13:00Z
/// mescolerebbe mezz'ora di premarket nella seduta. Le barre M30 cadono esattamente sull'apertura e
/// sulla chiusura.</para>
/// </summary>
public sealed partial class DataBridge
{
    [Display(Name = "Sessions", GroupName = "Settimana",
        Description = "Quante sedute cash disegnare sul chart della settimana, dalla piu' recente.")]
    [Range(1, 30)]
    public int SettimanaSedute { get; set; } = 10;

    private sealed record SedutaDellaSettimana(DateTime Giorno, int Da, int A, ProfiloFinestra P, bool InCorso);

    private SedutaDellaSettimana[] _settimana = Array.Empty<SedutaDellaSettimana>();
    private int _settimanaBarra = -1;
    private DateTime _settimanaQuando = DateTime.MinValue;

    private SedutaDellaSettimana[] LeSeduteDellaSettimana()
    {
        // Una volta per barra, e ogni dieci secondi finche' la seduta di oggi e' in corso: su M30
        // aspettare la chiusura della barra vorrebbe dire una gaussiana ferma per mezz'ora.
        var ultimo = CurrentBar - 1;
        if (ultimo < 1 || (ultimo == _settimanaBarra && (DateTime.UtcNow - _settimanaQuando).TotalSeconds < 10))
        {
            return _settimana;
        }

        _settimanaBarra = ultimo;
        _settimanaQuando = DateTime.UtcNow;
        var lista = new List<SedutaDellaSettimana>();
        try
        {
            var adesso = GetCandle(ultimo).Time;
            var oggi = GiornoDiMercato(adesso);
            for (var indietro = 0; indietro <= 45 && lista.Count < SettimanaSedute; indietro++)
            {
                var g = oggi.AddDays(-indietro);
                var da = Momento(Apertura, g);
                var a = Momento(Chiusura, g);
                if (adesso < da)
                {
                    continue;
                }

                var inCorso = adesso < a;
                var p = Profilo(da, a, InPunti(5m), InPunti(1m), conLaBarraAperta: inCorso);
                var (primo, ultimoDellaSeduta) = BarreFra(da, a);
                if (p is null || primo < 0)
                {
                    continue;
                }

                lista.Add(new SedutaDellaSettimana(g, primo, ultimoDellaSeduta, p, inCorso));
            }
        }
        catch (Exception errore)
        {
            this.LogError("sedute della settimana non calcolate", errore);
        }

        lista.Reverse();
        _settimana = lista.ToArray();
        return _settimana;
    }

    /// <summary>La prima e l'ultima barra che cadono in [da, a), cercando dalla fine.</summary>
    private (int Primo, int Ultimo) BarreFra(DateTime da, DateTime a)
    {
        int primo = -1, ultimo = -1;
        for (var bar = CurrentBar - 1; bar >= 0; bar--)
        {
            var t = GetCandle(bar)?.Time;
            if (t is null)
            {
                continue;
            }

            if (t < da)
            {
                break;
            }

            if (t < a)
            {
                primo = bar;
                ultimo = ultimo < 0 ? bar : ultimo;
            }
        }

        return (primo, ultimo);
    }

    private void DisegnaLaSettimana(RenderContext context, Rectangle area)
    {
        var sedute = LeSeduteDellaSettimana();
        if (sedute.Length == 0 || ChartInfo is null)
        {
            return;
        }

        // Le sedute che sull'M1 hanno un nome: PD, il giorno prima, il framing.
        var nomi = new Dictionary<DateTime, (string Sigla, Color Colore)>();
        foreach (var z in LeZone().Where(z => !z.Oggi && z.Giorno is not null))
        {
            nomi.TryAdd(z.Giorno!.Value.Date,
                (z.Nome.Contains("framing", StringComparison.Ordinal) ? $"{z.Sigla} framing" : z.Sigla, z.Colore));
        }

        var font = new RenderFont("Arial", Math.Max(7, LevelFontSize - 2));
        var altoRiga = context.MeasureString("Hg", font).Height;
        var grigio = Color.FromArgb(255, 144, 164, 174);
        var larghezzaBarra = 1;
        try
        {
            larghezzaBarra = Math.Max(1, (int)ChartInfo.PriceChartContainer.BarsWidth);
        }
        catch (Exception)
        {
            // Senza la larghezza della barra il profilo finisce all'inizio dell'ultima barra.
        }

        SedutaDellaSettimana? prima = null;
        foreach (var s in sedute)
        {
            var p = s.P;
            var (sigla, colore) = s.InCorso
                ? ("OGGI in corso", ParseColor(Palette[Seduta.Oggi].Hex))
                : nomi.TryGetValue(s.Giorno.Date, out var n) ? n : (string.Empty, grigio);

            // Lo spostamento del valore rispetto alla seduta prima, come nel giro d'orizzonte.
            var migra = string.Empty;
            if (prima is not null)
            {
                var sovrapposto = !(p.Val > prima.P.Vah || p.Vah < prima.P.Val);
                migra = $"valore {(p.Poc > prima.P.Poc ? "SU" : "GIU")}{(sovrapposto ? string.Empty : " e staccato")}";
            }

            prima = s;
            if (s.A < FirstVisibleBarNumber || s.Da > LastVisibleBarNumber)
            {
                continue;
            }

            int x0, x1;
            try
            {
                x0 = ChartInfo.GetXByBar(s.Da, true);
                x1 = ChartInfo.GetXByBar(s.A, true) + larghezzaBarra;
            }
            catch (Exception)
            {
                continue;
            }

            var largo = Math.Max(24, x1 - x0);

            // LA GAUSSIANA, sulle barre della seduta, fasce da 5 punti.
            var massimo = p.Fasce.Values.DefaultIfEmpty(0m).Max();
            foreach (var (fascia, volume) in p.Fasce)
            {
                if (massimo <= 0)
                {
                    break;
                }

                var yAlto = ChartInfo.GetYByPrice(fascia + p.Passo, false);
                var yBasso = ChartInfo.GetYByPrice(fascia, false);
                if (yBasso < area.Top || yAlto > area.Bottom)
                {
                    continue;
                }

                var dentro = fascia + p.Passo > p.Val && fascia <= p.Vah;
                var lungo = (int)(volume / massimo * largo);
                context.FillRectangle(Color.FromArgb(dentro ? 95 : 35, colore),
                    new Rectangle(x0, yAlto, Math.Max(1, lungo), Math.Max(1, yBasso - yAlto - 1)));
            }

            // VAH e VAL tratteggiati, POC pieno, sulla larghezza della seduta.
            foreach (var (prezzo, poc) in new[] { (p.Vah, false), (p.Poc, true), (p.Val, false) })
            {
                var y = ChartInfo.GetYByPrice(prezzo, false);
                if (y >= area.Top && y <= area.Bottom)
                {
                    context.DrawLine(new RenderPen(Color.FromArgb(poc ? 240 : 200, colore), poc ? 2 : 1,
                        poc ? DashStyle.Solid : DashStyle.Dash), x0, y, x0 + largo, y);
                }
            }

            // LA TESTA, sopra il massimo della seduta: giorno, nome, valore, spostamento.
            var testa = new List<string>
            {
                $"{Giorni[(int)s.Giorno.DayOfWeek][..3]} {s.Giorno:dd}{(sigla.Length > 0 ? "  " + sigla : string.Empty)}",
                $"VA {Prezzo(p.Val)}-{Prezzo(p.Vah)}",
                $"POC {Prezzo(p.Poc)}",
            };
            if (migra.Length > 0)
            {
                testa.Add(migra);
            }

            var yTesta = ChartInfo.GetYByPrice(p.Massimo, false) - altoRiga * testa.Count - 4;
            yTesta = Math.Max(area.Top + 2, yTesta);
            foreach (var riga in testa)
            {
                context.DrawString(riga, font, colore, x0 + 2, yTesta);
                yTesta += altoRiga;
            }
        }
    }
}
