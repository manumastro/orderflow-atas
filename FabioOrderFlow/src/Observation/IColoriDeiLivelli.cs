using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Drawing;
using ATAS.Indicators;
using OFT.Rendering.Context;
using OFT.Rendering.Tools;

namespace FabioOrderFlow.Observation;

/// <summary>
/// I colori e gli stili dei livelli, con due regole fisse invece di un colore scelto riga per riga.
///
/// <para><b>Perche'.</b> Fino al 23 settembre 2026 il colore lo scriveva il file delle regole, livello
/// per livello, e ogni giornata ne aggiungeva uno: cassa, notte ed Europa finivano in tre azzurri
/// diversi, e con dodici righe sul chart non si capiva a quale seduta appartenesse un bordo. Un colore
/// deve dire una cosa sola.</para>
///
/// <list type="bullet">
/// <item><b>il COLORE dice la seduta</b>: cassa di oggi, cassa di ieri, notte, Europa, valore di piu'
/// giorni, COT. Un bordo e il suo POC hanno lo stesso colore, e la banda del valore anche;</item>
/// <item><b>lo STILE dice il ruolo</b>: POC pieno e spesso, VAH e VAL tratteggiati, massimo e minimo
/// punteggiati, nodi e mensole a tratto e punto.</item>
/// </list>
///
/// <para>La seduta si legge dal nome della regola ("POC cash", "VAL notte", "VAH cash ieri"). Il colore
/// scritto nel file vale solo per i livelli che non appartengono a nessuna seduta nota.</para>
/// </summary>
public sealed partial class DataBridge
{
    [Display(Name = "Colors by session", GroupName = "Levels",
        Description = "Il colore dice la seduta, lo stile dice il ruolo. Spento: valgono i colori del file.")]
    public bool ColoriPerSeduta { get; set; } = true;

    [Display(Name = "Show legend", GroupName = "Levels")]
    public bool ShowLegend { get; set; } = true;

    private enum Seduta { Oggi, Ieri, Notte, Europa, Cot, Altro }

    private static readonly Dictionary<Seduta, (string Hex, string Nome)> Palette = new()
    {
        [Seduta.Oggi] = ("#4FC3F7", "cassa oggi"),
        [Seduta.Ieri] = ("#FFA726", "cassa ieri"),
        [Seduta.Notte] = ("#9575CD", "notte"),
        [Seduta.Europa] = ("#D4E157", "Europa"),
        [Seduta.Cot] = ("#FFD54F", "COT"),
    };

    /// <summary>Colore del valore di piu' giorni e dei vuoti: neutri, perche' non sono una seduta.</summary>
    private static readonly Color ColoreStorico = Color.FromArgb(255, 207, 216, 220);

    private static Seduta SedutaDi(string? nome)
    {
        var n = (nome ?? string.Empty).ToLowerInvariant();
        if (n.Contains("cot", StringComparison.Ordinal)) { return Seduta.Cot; }
        if (n.Contains("ieri", StringComparison.Ordinal)) { return Seduta.Ieri; }
        if (n.Contains("cash", StringComparison.Ordinal) || n.Contains("cassa", StringComparison.Ordinal)) { return Seduta.Oggi; }
        if (n.Contains("notte", StringComparison.Ordinal) || n.Contains("asia", StringComparison.Ordinal)) { return Seduta.Notte; }
        if (n.Contains("europ", StringComparison.Ordinal)) { return Seduta.Europa; }
        return Seduta.Altro;
    }

    /// <summary>Colore, stile e spessore di un livello: seduta dal nome, ruolo dal tipo.</summary>
    private (string Colore, string Stile, int Spessore) Aspetto(BridgeRule regola, string tipo)
    {
        var seduta = SedutaDi(regola.Nome);
        if (!ColoriPerSeduta || seduta == Seduta.Altro || !Palette.TryGetValue(seduta, out var p))
        {
            return (regola.Color ?? "#7A8FA6", regola.Style ?? "solid", regola.Width);
        }

        return tipo switch
        {
            "poc" => (p.Hex, "solid", 3),
            "vah" or "val" => (p.Hex, "dash", 2),
            "massimo" or "minimo" => (p.Hex, "dot", 1),
            _ => (p.Hex, "dashdot", 1),
        };
    }

    /// <summary>Il colore della banda del valore: quello della sua seduta.</summary>
    private Color ColoreBanda(string? area)
    {
        var seduta = SedutaDi(area);
        return ColoriPerSeduta && Palette.TryGetValue(seduta, out var p)
            ? ParseColor(p.Hex)
            : Color.FromArgb(255, 79, 195, 247);
    }

    /// <summary>
    /// La legenda, in basso a sinistra: senza, i colori sono una regola che conosce solo chi l'ha scritta.
    /// </summary>
    private void DisegnaLegenda(RenderContext context, Rectangle area)
    {
        if (!ShowLegend || !ColoriPerSeduta)
        {
            return;
        }

        var font = new RenderFont("Arial", Math.Max(7, LevelFontSize - 2));
        var voci = new List<(string Testo, Color Colore)>();
        foreach (var s in new[] { Seduta.Oggi, Seduta.Ieri, Seduta.Notte, Seduta.Europa, Seduta.Cot })
        {
            voci.Add((Palette[s].Nome, ParseColor(Palette[s].Hex)));
        }

        voci.Add(("valore 5 gg / vuoti", ColoreStorico));
        voci.Add(("muro sotto", MuroVerde));
        voci.Add(("muro sopra", MuroRosso));

        var alto = context.MeasureString("Hg", font).Height;
        var x = area.Left + 8;
        var y = area.Bottom - alto - 6;
        var larghezza = 0;
        foreach (var (testo, _) in voci)
        {
            larghezza += 14 + context.MeasureString(testo, font).Width + 12;
        }

        context.FillRectangle(Color.FromArgb(185, 12, 12, 16), new Rectangle(x - 5, y - 3, larghezza + 6, alto + 6));
        foreach (var (testo, colore) in voci)
        {
            context.FillRectangle(colore, new Rectangle(x, y + alto / 2 - 2, 10, 4));
            x += 14;
            context.DrawString(testo, font, colore, x, y);
            x += context.MeasureString(testo, font).Width + 12;
        }

        var ruoli = "POC pieno · VAH/VAL tratteggio · max/min punti";
        context.DrawString(ruoli, font, Color.FromArgb(200, 170, 170, 170), area.Left + 8, y - alto - 4);
    }
}
