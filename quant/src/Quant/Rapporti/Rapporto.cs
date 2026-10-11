using System.Globalization;
using System.Text;
using Quant.Motore;
using Quant.Statistiche;
using Quant.Validazione;

namespace Quant.Rapporti;

/// <summary>
/// Il rapporto di una validazione: un file Markdown con i grafici in SVG accanto, in
/// <c>quant/risultati/&lt;strategia&gt;/</c>. Si scrive per una persona e per un agente: ogni
/// numero porta la sua convenzione, e i risultati negativi si scrivono come quelli positivi.
/// </summary>
public static class Rapporto
{
    private static readonly CultureInfo It = CultureInfo.GetCultureInfo("it-IT");

    public static string Scrivi(EsitoValidazione v, string? cartella = null)
    {
        var dentro = v.Dentro.Risultato;
        cartella ??= Path.Combine(Dati.Percorsi.Risultati, v.Strategia);
        Directory.CreateDirectory(cartella);
        var nome = $"{DateTime.Now:yyyy-MM-dd-HHmm}-{dentro.Impronta}{(v.Fuori is null ? "-dentro" : "-fuori")}";
        var md = Path.Combine(cartella, nome + ".md");
        var svgCurva = nome + "-curva.svg";
        var svgSpaghetti = nome + "-montecarlo.svg";

        File.WriteAllText(Path.Combine(cartella, svgCurva), GraficoCurve(v));
        File.WriteAllText(Path.Combine(cartella, svgSpaghetti), GraficoSpaghetti(v.Ricampionamento));

        var s = new StringBuilder();
        s.AppendLine($"# {v.Strategia}: {(v.Fuori is null ? "validazione dentro campione" : v.Superata ? "supera i cancelli" : "non supera i cancelli")}");
        s.AppendLine();
        s.AppendLine($"Generato il {DateTime.Now:yyyy-MM-dd HH:mm} da `quant valida`. Impronta `{dentro.Impronta}`, versione {dentro.Versione}. " +
                     "Superare i cancelli **non approva** una regola operativa: dice che la prova non l'ha smentita.");
        s.AppendLine();
        s.AppendLine("## Le Regole");
        s.AppendLine();
        s.AppendLine(v.Regole);
        s.AppendLine();
        if (v.Idea is not null) s.AppendLine($"L'idea, il paper e il perche': `{v.Idea}`.");
        s.AppendLine();
        s.AppendLine($"- **Parametri**: `{dentro.Parametri}`");
        s.AppendLine($"- **Dimensione**: {dentro.Dimensione}");
        s.AppendLine($"- **Esecuzione**: {dentro.Esecuzione}");
        s.AppendLine($"- **Dati**: {dentro.Serie.Descrizione}{(dentro.Serie.Rettificata ? ", rettificata all'indietro ai rollover" : "")}");
        s.AppendLine($"- **Divisione**: dentro fino al {v.Divisione.FineDentro:yyyy-MM-dd} compreso, fissata il {v.Divisione.FissataIl:yyyy-MM-dd} ({v.Divisione.Quota:P0} di {v.Divisione.GiorniAllora} giornate)");
        s.AppendLine();

        s.AppendLine("## I Cancelli");
        s.AppendLine();
        s.AppendLine("| | condizione | valore | |");
        s.AppendLine("|---|---|---|---|");
        foreach (var c in v.Cancelli) s.AppendLine($"| {c.Codice} | {c.Descrizione} | {c.Valore} | {(c.Passato ? "passa" : "**non passa**")} |");
        if (v.Fuori is null) s.AppendLine("\nIl fuori campione non e' stato guardato: si guarda una volta, a regole congelate, con `--fuori`.");
        s.AppendLine();

        s.AppendLine("## I Numeri");
        s.AppendLine();
        Tabella(s, v.Dentro, v.Fuori);
        s.AppendLine();
        s.AppendLine($"Sharpe probabilistico dentro (sopra zero): {v.SharpeProbabilistico:0.000}. " +
                     $"Sharpe deflazionato per {v.VariantiProvate} variant{(v.VariantiProvate == 1 ? "e" : "i")} nel registro: {v.SharpeDeflazionato:0.000}.");
        s.AppendLine();
        s.AppendLine($"![curva]({svgCurva})");
        s.AppendLine();

        s.AppendLine("## Il Modello Nullo");
        s.AppendLine();
        s.AppendLine("Ogni operazione rifatta togliendo una cosa sola, con le stesse uscite, contratti e costi.");
        s.AppendLine();
        s.AppendLine("| campione | nullo | netto reale | nullo p5 / p50 / p95 | vantaggio per operazione | p |");
        s.AppendLine("|---|---|---:|---:|---:|---:|");
        foreach (var c in new[] { v.Dentro, v.Fuori }.OfType<Campione>())
        foreach (var n in new[] { c.NulloTempo, c.NulloVerso })
            s.AppendLine($"| {c.Nome} | {n.Metodo} | {D(n.NettoReale)} | {D(n.NullaP05)} / {D(n.NullaP50)} / {D(n.NullaP95)} | {D(n.VantaggioPerOperazione)} | {n.P:0.000} |");
        s.AppendLine();

        s.AppendLine("## Il Monte Carlo");
        s.AppendLine();
        s.AppendLine($"Sulle {v.Ricampionamento.Lunghezza} operazioni {(v.Fuori is null ? "dentro campione" : "dentro e fuori")}, {v.Ricampionamento.Simulazioni:N0} simulazioni.");
        s.AppendLine();
        s.AppendLine("| metodo | finale p5 / p50 / p95 | prob. perdita | drawdown p50 / p95 / p99 | serie di perdite p50 / p95 |");
        s.AppendLine("|---|---:|---:|---:|---:|");
        foreach (var mc in new[] { v.Rimescolamento, v.Ricampionamento, v.RicampionamentoGiorni })
            s.AppendLine($"| {mc.Metodo} | {D(mc.FinaleP05)} / {D(mc.FinaleP50)} / {D(mc.FinaleP95)} | {mc.ProbPerdita:P1} | " +
                         $"{D(mc.DrawdownP50)} / {D(mc.DrawdownP95)} / {D(mc.DrawdownP99)} | {mc.SerieDiPerditeP50:0} / {mc.SerieDiPerditeP95:0} |");
        s.AppendLine();
        s.AppendLine($"Il drawdown della sequenza vera e' {D(v.Rimescolamento.DrawdownOriginale)}; il {v.Rimescolamento.QuotaPeggioriDellOriginale:P0} dei rimescolamenti fa peggio. " +
                     "Se quella quota e' piccola, l'ordine vero delle operazioni e' stato fortunato.");
        s.AppendLine();
        s.AppendLine($"![monte carlo]({svgSpaghetti})");
        s.AppendLine();
        s.AppendLine("### I Metri Per Il Vivo");
        s.AppendLine();
        s.AppendLine("Il drawdown entro le prime N operazioni, dal ricampionamento. Superare il p95 dal vivo e' un segnale d'allarme; il p99, un motivo per fermarsi.");
        s.AppendLine();
        s.AppendLine("| operazioni | p50 | p95 | p99 |");
        s.AppendLine("|---:|---:|---:|---:|");
        foreach (var x in v.Ricampionamento.Soglie) s.AppendLine($"| {x.Operazioni} | {D(x.DrawdownP50)} | {D(x.DrawdownP95)} | {D(x.DrawdownP99)} |");
        s.AppendLine();

        s.AppendLine("## Le Uscite");
        s.AppendLine();
        s.AppendLine("| campione | " + string.Join(" | ", Enum.GetNames<MotivoUscita>()) + " |");
        s.AppendLine("|---|" + string.Concat(Enum.GetNames<MotivoUscita>().Select(_ => "---:|")));
        foreach (var c in new[] { v.Dentro, v.Fuori }.OfType<Campione>())
            s.AppendLine($"| {c.Nome} | " + string.Join(" | ", Enum.GetValues<MotivoUscita>().Select(m => c.Risultato.Operazioni.Count(o => o.MotivoUscita == m))) + " |");
        s.AppendLine();
        if (dentro.Saltati > 0 || v.Fuori?.Risultato.Saltati > 0)
            s.AppendLine($"Ingressi saltati (dimensione zero o eseguito oltre lo stop): dentro {dentro.Saltati}, fuori {v.Fuori?.Risultato.Saltati ?? 0}.");

        File.WriteAllText(md, s.ToString());
        return md;
    }

    /// <summary>La tabella delle statistiche, dentro e fuori affiancati.</summary>
    public static void Tabella(StringBuilder s, Campione dentro, Campione? fuori)
    {
        var cols = new List<(string, Risultato, Metriche)> { (dentro.Nome, dentro.Risultato, dentro.Metriche) };
        if (fuori is not null) cols.Add((fuori.Nome, fuori.Risultato, fuori.Metriche));
        s.AppendLine("| | " + string.Join(" | ", cols.Select(c => c.Item1)) + " |");
        s.AppendLine("|---|" + string.Concat(cols.Select(_ => "---:|")));
        void Riga(string nome, Func<Risultato, Metriche, string> f) => s.AppendLine($"| {nome} | " + string.Join(" | ", cols.Select(c => f(c.Item2, c.Item3))) + " |");
        Riga("periodo", (r, _) => $"{r.Da:yyyy-MM-dd} -> {r.A:yyyy-MM-dd}");
        Riga("giornate (con operazioni)", (_, m) => $"{m.Giorni} ({m.GiorniConOperazioni})");
        Riga("operazioni", (_, m) => $"{m.Operazioni}");
        Riga("netto", (_, m) => D(m.Netto));
        Riga("lordo / costi", (_, m) => $"{D(m.Lordo)} / {D(m.Costi)}");
        Riga("vincenti", (_, m) => $"{m.PercVincenti:P1}");
        Riga("media vincente / perdente", (_, m) => $"{D(m.MediaVincente)} / {D(m.MediaPerdente)}");
        Riga("profit factor", (_, m) => F(m.ProfitFactor));
        Riga("aspettativa per operazione", (_, m) => D(m.Aspettativa));
        Riga("tick per contratto, media", (_, m) => F(m.TickMediPerContratto));
        Riga("R medio", (_, m) => F(m.RMedio));
        Riga("drawdown massimo (giornate)", (_, m) => $"{D(m.MaxDrawdown)} ({m.DurataMaxDrawdownGiorni})");
        Riga("serie massima di perdite", (_, m) => $"{m.MassimaSerieDiPerdite}");
        Riga("Sharpe annuo (giornaliero x radice di 252)", (_, m) => F(m.SharpeAnnuo));
        Riga("Sortino annuo", (_, m) => F(m.SortinoAnnuo));
        Riga("**t per giornata** (il test)", (_, m) => $"**{F(m.TGiornaliero)}**");
        Riga("t per operazione (ingenuo, non si usa)", (_, m) => F(m.TIngenuo));
        Riga("asimmetria / curtosi giornaliere", (_, m) => $"{F(m.Asimmetria)} / {F(m.Curtosi)}");
        Riga("durata del calcolo", (r, _) => $"{r.Durata.TotalMilliseconds:N0} ms su {r.Barre:N0} barre");
    }

    public static string D(double x) => double.IsFinite(x) ? x.ToString("#,0", It) + " $" : "n/d";
    public static string F(double x) => double.IsFinite(x) ? x.ToString("0.00", It) : "n/d";

    private static string GraficoCurve(EsitoValidazione v)
    {
        var serie = new List<(string Nome, double[] Valori, string Colore)>();
        var giorni = v.Dentro.Risultato.NettoGiorno.Concat(v.Fuori?.Risultato.NettoGiorno ?? []).ToArray();
        serie.Add(("netto cumulato", Cumula(giorni), "#2563eb"));
        var confine = v.Dentro.Risultato.NettoGiorno.Length;
        return Svg.Linee("Netto cumulato per giornata" + (v.Fuori is null ? "" : " (la riga verticale separa dentro e fuori)"),
            serie, v.Fuori is null ? null : confine);
    }

    private static string GraficoSpaghetti(EsitoMonteCarlo mc)
    {
        var serie = mc.Percorsi.Where(p => p is not null).Select(p => ("", p, "#94a3b8")).ToList();
        return Svg.Linee($"Monte Carlo, {mc.Metodo}: {serie.Count} percorsi", serie, null, opaco: true);
    }

    private static double[] Cumula(double[] x)
    {
        var r = new double[x.Length];
        double c = 0;
        for (var i = 0; i < x.Length; i++) r[i] = c += x[i];
        return r;
    }
}

/// <summary>Grafici a linee in SVG, scritti a mano: nessuna dipendenza, leggibili ovunque.</summary>
internal static class Svg
{
    public static string Linee(string titolo, IReadOnlyList<(string Nome, double[] Valori, string Colore)> serie, int? verticale, bool opaco = false)
    {
        const int w = 900, h = 360, ml = 70, mr = 20, mt = 36, mb = 30;
        var tutti = serie.SelectMany(s => s.Valori).Append(0).ToArray();
        var min = tutti.Min();
        var max = tutti.Max();
        if (max - min < 1e-9) max = min + 1;
        var n = Math.Max(2, serie.Max(s => s.Valori.Length));
        double X(int i) => ml + (w - ml - mr) * i / (double)(n - 1);
        double Y(double v) => mt + (h - mt - mb) * (1 - (v - min) / (max - min));
        var inv = CultureInfo.InvariantCulture;
        var s = new StringBuilder();
        s.Append($"<svg xmlns=\"http://www.w3.org/2000/svg\" viewBox=\"0 0 {w} {h}\" font-family=\"sans-serif\" font-size=\"12\">");
        s.Append($"<rect width=\"{w}\" height=\"{h}\" fill=\"#ffffff\"/>");
        s.Append($"<text x=\"{ml}\" y=\"20\" font-size=\"14\" fill=\"#111827\">{System.Security.SecurityElement.Escape(titolo)}</text>");
        foreach (var t in new[] { min, (min + max) / 2, max, 0.0 }.Distinct())
        {
            var y = Y(t).ToString("0.0", inv);
            s.Append($"<line x1=\"{ml}\" x2=\"{w - mr}\" y1=\"{y}\" y2=\"{y}\" stroke=\"{(t == 0 ? "#6b7280" : "#e5e7eb")}\"/>");
            s.Append($"<text x=\"{ml - 6}\" y=\"{y}\" text-anchor=\"end\" dominant-baseline=\"middle\" fill=\"#374151\">{t.ToString("#,0", inv)}</text>");
        }
        if (verticale is { } vx)
        {
            var x = X(vx).ToString("0.0", inv);
            s.Append($"<line x1=\"{x}\" x2=\"{x}\" y1=\"{mt}\" y2=\"{h - mb}\" stroke=\"#dc2626\" stroke-dasharray=\"4 3\"/>");
        }
        foreach (var (_, valori, colore) in serie)
        {
            if (valori.Length == 0) continue;
            var passo = Math.Max(1, valori.Length / 1500);
            s.Append($"<polyline fill=\"none\" stroke=\"{colore}\" stroke-width=\"{(opaco ? "0.8" : "1.6")}\"{(opaco ? " stroke-opacity=\"0.5\"" : "")} points=\"");
            for (var i = 0; i < valori.Length; i += passo)
                s.Append(X(i).ToString("0.0", inv)).Append(',').Append(Y(valori[i]).ToString("0.0", inv)).Append(' ');
            s.Append(X(valori.Length - 1).ToString("0.0", inv)).Append(',').Append(Y(valori[^1]).ToString("0.0", inv));
            s.Append("\"/>");
        }
        s.Append("</svg>");
        return s.ToString();
    }
}
