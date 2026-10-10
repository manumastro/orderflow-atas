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
/// Il ruolo COMPOSITO, sul chart Daily: il profilo degli ultimi 90 giorni, e dove sta il prezzo
/// rispetto al suo blocco di volume.
///
/// <para><b>La fonte.</b> <i>"The composite is the last three months, 90 days"</i> (<c>fabio_4.txt</c>,
/// 14:18). <i>"This block till this peak here is where we have the maximum concentration of volume
/// for this distribution. Everything above this line it's premium. Everything inside this line it's
/// fair value. Everything below this line it's discount"</i> (<c>fabio_5.txt</c>, 55:05). Il blocco qui
/// e' l'area di valore al 70%, la stessa regola delle sedute.</para>
///
/// <para><b>Perche' lo disegna l'indicatore.</b> Il profilo di ATAS che stava sul chart fino al 9 ottobre
/// 2026 era fisso, dal 24 aprile: cinque mesi e mezzo, non 90 giorni, e non si spostava. Questo si
/// ricalcola da solo e parte sempre dalla novantesima barra indietro, segnata sul chart.</para>
///
/// <para><b>Cosa non e'.</b> Le barre Daily contengono tutte le 24 ore, globex compreso, e sul continuo
/// non sono aggiustate al rollover: sono i prezzi grezzi, come il profilo di ATAS. Si dichiara in testa
/// al riquadro.</para>
/// </summary>
public sealed partial class DataBridge
{
    [Display(Name = "Composite days", GroupName = "Composito",
        Description = "Quante barre Daily entrano nel composito. Fabio: 90 (fabio_4.txt 14:18).")]
    [Range(5, 400)]
    public int CompositoGiorni { get; set; } = 90;

    /// <summary>
    /// Il composito com'e' sul chart (grezzo) e, quando c'e' l'M1 dello stesso contratto, aggiustato
    /// al rollover: i giorni fino a <see cref="FinoA"/> spostati di <see cref="Scarto"/> punti. Il
    /// profilo si disegna grezzo perche' deve combaciare con le candele; il giudizio premium /
    /// discount per il contratto che si opera si legge su quello aggiustato.
    /// </summary>
    private sealed record Composito(
        SortedDictionary<decimal, decimal> Fasce, decimal Grana, decimal Poc, decimal Val, decimal Vah,
        int PrimaBarra, DateTime Dal, int Barre,
        decimal Scarto = 0m, DateTime FinoA = default, decimal PocAgg = 0m, decimal ValAgg = 0m, decimal VahAgg = 0m);

    private Composito? _composito90;
    private DateTime _composito90Quando = DateTime.MinValue;
    private int _composito90Barra = -1;

    /// <summary>
    /// Il composito, ricalcolato a ogni barra nuova e al massimo ogni trenta secondi: sul Daily la
    /// barra di oggi non si chiude mai durante la giornata, ma cresce.
    /// </summary>
    private Composito? IlCompositoDelChart()
    {
        if (_composito90 is not null && CurrentBar - 1 == _composito90Barra
            && (DateTime.UtcNow - _composito90Quando).TotalSeconds < 30)
        {
            return _composito90;
        }

        _composito90Quando = DateTime.UtcNow;
        _composito90Barra = CurrentBar - 1;
        try
        {
            var grana = InPunti(5m);
            var fasce = new SortedDictionary<decimal, decimal>();
            var aggiustate = new SortedDictionary<decimal, decimal>();
            var (scarto, finoA) = LoScartoVistoDalGiornaliero();
            int primo = -1, barre = 0;
            for (var bar = CurrentBar - 1; bar >= 0 && barre < CompositoGiorni; bar--)
            {
                var c = GetCandle(bar);
                if (c is null || c.Volume <= 0)
                {
                    continue;
                }

                barre++;
                primo = bar;
                var sposta = scarto != 0m && GiornoDiMercato(c.Time) <= finoA ? scarto : 0m;
                foreach (var l in c.GetAllPriceLevels())
                {
                    var p = Math.Floor(l.Price / grana) * grana;
                    fasce[p] = fasce.GetValueOrDefault(p) + l.Volume;
                    var q = Math.Floor((l.Price + sposta) / grana) * grana;
                    aggiustate[q] = aggiustate.GetValueOrDefault(q) + l.Volume;
                }
            }

            if (fasce.Count == 0)
            {
                return _composito90 = null;
            }

            var (poc, val, vah) = IlValore(fasce);
            var (pocA, valA, vahA) = IlValore(aggiustate);
            _composito90 = new Composito(fasce, grana, poc, val, vah, primo, GetCandle(primo).Time, barre,
                scarto, finoA, pocA, valA, vahA);
        }
        catch (Exception errore)
        {
            this.LogError("composito non calcolato", errore);
        }

        return _composito90;
    }

    /// <summary>
    /// Lo scarto del rollover misurato dall'M1 dello stesso contratto, se e' aperto: e' l'M1 che sa i
    /// prezzi del contratto che si opera. Senza M1 lo scarto e' zero, e il riquadro lo dice.
    /// </summary>
    private (decimal Scarto, DateTime FinoA) LoScartoVistoDalGiornaliero()
    {
        var mia = Radice(InstrumentInfo?.Instrument);
        DataBridge? seduta;
        lock (HubSync)
        {
            seduta = Instances.Values.FirstOrDefault(i => !ReferenceEquals(i, this) && i.SonoLaSeduta
                && Radice(i.InstrumentInfo?.Instrument) == mia);
        }

        (decimal Scarto, DateTime FinoA) misurato = (0m, DateTime.MinValue);
        try
        {
            if (seduta is not null)
            {
                misurato = seduta.LoScartoDelRollover(this);
            }
        }
        catch (Exception)
        {
            // Si ricade sull'ultimo scarto ricordato.
        }

        // LO SCARTO SI RICORDA (10 ottobre 2026): l'M1 tiene circa quattro settimane, e un mese dopo il
        // rollover non ha piu' nessun giorno in comune col continuo prima del cambio. Lo scarto non si
        // misura piu', ma non e' cambiato: vale quello misurato l'ultima volta, fino al rollover dopo.
        var ricordi = LeggiGliScarti();
        if (misurato.Scarto != 0m)
        {
            ricordi[mia] = new ScartoRicordato(misurato.Scarto, misurato.FinoA);
            ScriviGliScarti(ricordi);
            return misurato;
        }

        return ricordi.TryGetValue(mia, out var r) ? (r.Scarto, r.FinoA) : misurato;
    }

    private sealed record ScartoRicordato(decimal Scarto, DateTime FinoA);

    private static readonly string ScartiPath = System.IO.Path.Combine(
        Environment.GetFolderPath(Environment.SpecialFolder.UserProfile), ".fabio-data-bridge-rollover.json");

    private static Dictionary<string, ScartoRicordato> LeggiGliScarti()
    {
        try
        {
            return System.IO.File.Exists(ScartiPath)
                ? System.Text.Json.JsonSerializer.Deserialize<Dictionary<string, ScartoRicordato>>(System.IO.File.ReadAllText(ScartiPath))
                  ?? new Dictionary<string, ScartoRicordato>()
                : new Dictionary<string, ScartoRicordato>();
        }
        catch (Exception)
        {
            return new Dictionary<string, ScartoRicordato>();
        }
    }

    private static void ScriviGliScarti(Dictionary<string, ScartoRicordato> scarti)
    {
        try
        {
            System.IO.File.WriteAllText(ScartiPath, System.Text.Json.JsonSerializer.Serialize(scarti));
        }
        catch (Exception)
        {
            // Senza file si rimisura la prossima volta: niente di peggio.
        }
    }

    /// <summary>
    /// POC e area di valore al 70% di un profilo gia' raggruppato: dal POC si allarga verso la fascia
    /// vicina piu' pesante, a parita' verso l'alto. E' la regola di <c>Profilo</c> e del giro.
    /// </summary>
    private static (decimal Poc, decimal Val, decimal Vah) IlValore(SortedDictionary<decimal, decimal> fasce)
    {
        var prezzi = fasce.Keys.ToList();
        var totale = fasce.Values.Sum();
        var poc = fasce.OrderByDescending(kv => kv.Value).First().Key;
        int basso = prezzi.IndexOf(poc), alto = basso;
        var dentro = fasce[poc];
        while (dentro < 0.7m * totale && (basso > 0 || alto < prezzi.Count - 1))
        {
            var giu = basso > 0 ? fasce[prezzi[basso - 1]] : -1m;
            var su = alto < prezzi.Count - 1 ? fasce[prezzi[alto + 1]] : -1m;
            if (su >= giu)
            {
                alto++;
                dentro += su;
            }
            else
            {
                basso--;
                dentro += giu;
            }
        }

        return (poc, prezzi[basso], prezzi[alto]);
    }

    /// <summary>Due righe: il blocco, e dove sta il prezzo rispetto a lui.</summary>
    private static IEnumerable<(string Testo, Color Colore)> DoveStaIlPrezzo(
        string quale, decimal val, decimal vah, decimal poc, decimal prezzo)
    {
        var arancio = Color.FromArgb(255, 247, 150, 70);
        var (zona, colore, cosa) = prezzo > vah
            ? ("PREMIUM", Color.FromArgb(255, 239, 83, 80), $"{Prezzo(prezzo - vah)} sopra il blocco: comprare dall'alto non ha senso")
            : prezzo < val
                ? ("DISCOUNT", Color.FromArgb(255, 102, 187, 106), $"{Prezzo(val - prezzo)} sotto il blocco: vendere dal basso non ha senso")
                : ("FAIR VALUE", Color.FromArgb(255, 207, 216, 220), "dentro il blocco di volume");
        yield return ($"{quale}:  blocco {Prezzo(val)} - {Prezzo(vah)}   POC {Prezzo(poc)}", arancio);
        yield return ($"      prezzo {Prezzo(prezzo)}  {zona}, {cosa}", colore);
    }

    /// <summary>
    /// Il profilo a destra, le tre righe del valore dall'inizio della finestra, il segno dove la
    /// finestra comincia, e in alto a sinistra dove sta il prezzo: premium, fair value o discount.
    /// </summary>
    private void DisegnaIlComposito(RenderContext context, Rectangle area, int dataRight)
    {
        var comp = IlCompositoDelChart();
        if (comp is null || ChartInfo is null)
        {
            return;
        }

        var font = new RenderFont("Arial", Math.Max(7, LevelFontSize - 1));
        var arancio = Color.FromArgb(255, 247, 150, 70);
        var oro = Color.FromArgb(255, 255, 213, 79);

        // IL PROFILO, sul quarto destro dell'area dei dati.
        var larghezza = Math.Max(80, (dataRight - area.Left) / 4);
        var massimo = comp.Fasce.Values.Max();
        foreach (var (prezzo, volume) in comp.Fasce)
        {
            var yAlto = ChartInfo.GetYByPrice(prezzo + comp.Grana, false);
            var yBasso = ChartInfo.GetYByPrice(prezzo, false);
            if (yBasso < area.Top || yAlto > area.Bottom)
            {
                continue;
            }

            var lungo = (int)(volume / massimo * larghezza);
            var dentro = prezzo >= comp.Val && prezzo <= comp.Vah;
            var colore = prezzo == comp.Poc ? Color.FromArgb(210, oro)
                : Color.FromArgb(dentro ? 120 : 45, arancio);
            context.FillRectangle(colore,
                new Rectangle(dataRight - lungo, yAlto, Math.Max(1, lungo), Math.Max(1, yBasso - yAlto - 1)));
        }

        // DA DOVE PARTE: la novantesima barra. Senza questo segno non si vede su quali giorni e' fatto.
        var xInizio = area.Left;
        try
        {
            xInizio = Math.Max(area.Left, ChartInfo.GetXByBar(comp.PrimaBarra, true));
            if (xInizio > area.Left)
            {
                context.DrawLine(new RenderPen(Color.FromArgb(140, arancio), 1, DashStyle.Dot),
                    xInizio, area.Top, xInizio, area.Bottom);
                context.DrawString($"inizio {comp.Barre}G  {comp.Dal:dd/MM}", font, arancio, xInizio + 4, area.Top + 4);
            }
        }
        catch (Exception)
        {
            // Senza la X della barra le righe partono da sinistra.
        }

        // LE TRE RIGHE DEL VALORE, con la scritta a sinistra del profilo.
        var righe = new[]
        {
            (P: comp.Vah + comp.Grana, S: "VAH", Colore: Color.FromArgb(200, arancio), Spessore: 1, Tratto: true),
            (P: comp.Poc + comp.Grana / 2, S: "POC", Colore: Color.FromArgb(230, oro), Spessore: 2, Tratto: false),
            (P: comp.Val, S: "VAL", Colore: Color.FromArgb(200, arancio), Spessore: 1, Tratto: true),
        };
        foreach (var (p, sigla, colore, spessore, tratto) in righe)
        {
            var y = ChartInfo.GetYByPrice(p, false);
            if (y < area.Top || y > area.Bottom)
            {
                continue;
            }

            context.DrawLine(new RenderPen(colore, spessore, tratto ? DashStyle.Dash : DashStyle.Solid), xInizio, y, dataRight, y);
            var testo = $"{comp.Barre}G {sigla} {Prezzo(p)}";
            var sz = context.MeasureString(testo, font);
            var x = dataRight - larghezza - sz.Width - 8;
            context.FillRectangle(Color.FromArgb(170, 0, 0, 0), new Rectangle(x - 2, y - sz.Height - 1, sz.Width + 6, sz.Height));
            context.DrawString(testo, font, colore, x, y - sz.Height - 1);
        }

        // DOVE STA IL PREZZO, in alto a sinistra: sul composito grezzo, e su quello aggiustato al
        // rollover quando l'M1 c'e'. Se i due dicono cose diverse, vale l'aggiustato.
        var prezzoOra = GetCandle(CurrentBar - 1).Close;
        var testa = new List<(string Testo, Color Colore)>
        {
            ($"COMPOSITO {comp.Barre} GIORNI dal {comp.Dal:dd/MM}  ·  barre Daily, 24 ore  ·  fasce da {Prezzo(comp.Grana)}", arancio),
        };
        testa.AddRange(DoveStaIlPrezzo("grezzo, come le candele", comp.Val, comp.Vah + comp.Grana, comp.Poc + comp.Grana / 2, prezzoOra));
        if (comp.Scarto != 0m)
        {
            testa.AddRange(DoveStaIlPrezzo(
                $"aggiustato al rollover (+{Prezzo(comp.Scarto)} fino al {comp.FinoA:dd/MM}), per il contratto",
                comp.ValAgg, comp.VahAgg + comp.Grana, comp.PocAgg + comp.Grana / 2, prezzoOra));
        }
        else
        {
            testa.Add(("aggiustato al rollover: manca l'M1 dello stesso contratto, non si puo' misurare", Color.FromArgb(255, 255, 183, 77)));
        }
        // Sotto le righe che ATAS scrive in alto a sinistra (strumento, O/H/L/C) quando il mouse e' sul chart.
        var yTesto = area.Top + 96;
        var largo = testa.Max(t => context.MeasureString(t.Testo, font).Width);
        var altoRiga = context.MeasureString("Hg", font).Height + 2;
        context.FillRectangle(Color.FromArgb(190, 12, 12, 16),
            new Rectangle(area.Left + 4, yTesto - 4, largo + 12, altoRiga * testa.Count + 6));
        foreach (var (testo, colore) in testa)
        {
            context.DrawString(testo, font, colore, area.Left + 10, yTesto);
            yTesto += altoRiga;
        }
    }
}
