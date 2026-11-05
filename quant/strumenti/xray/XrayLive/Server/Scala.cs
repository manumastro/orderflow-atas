namespace XrayLive.Server;

/// <summary>Una voce della scala dei livelli: cosa c'è a quel prezzo, di quale tipo, da quale fonte.</summary>
internal sealed record VoceScala(
    string Cosa, string Tipo, string Fonte, string? Scadenza, double? Peso, double? SecondiFa, double Prezzo, string Quando,
    string? Seduta);

/// <summary>Una riga della scala: una voce, o più voci vicine fuse in una zona [Da, A].</summary>
internal sealed record RigaScala(
    double Prezzo, double Da, double A, List<object> Voci, double Distanza, string Quando, string? Seduta, bool Zona);

/// <summary>
/// La scala dei prezzi (5 novembre 2026, progetto della pagina in stile QuantData, E.3): i livelli di adesso e della
/// prossima seduta, ordinati dal più alto al più basso, con le voci vicine fuse in zone. La pagina la disegna così
/// com'è e ci mette solo la riga del prezzo.
///
/// La fusione: si ordinano le voci per prezzo; scorrendo, una voce entra nella zona corrente se dista dal suo
/// estremo alto non più di <c>soglia</c> punti e appartiene allo stesso momento (adesso o prossima seduta). Il
/// prezzo della zona è il centro fra i due estremi. Le soglie sono quelle dei muri delle quattro domande: 6 punti
/// per ES (anche in SPX), 25 per NQ. Una convenzione, non provata.
/// </summary>
internal static class Scala
{
    public const double SogliaEs = 6;
    public const double SogliaNq = 25;

    public static List<RigaScala> Fondi(IEnumerable<VoceScala> voci, double? riferimento, double soglia)
    {
        var righe = new List<RigaScala>();
        foreach (var gruppo in voci.Where(v => double.IsFinite(v.Prezzo)).GroupBy(v => v.Quando))
        {
            var ordinate = gruppo.OrderBy(v => v.Prezzo).ToList();
            var zona = new List<VoceScala>();
            foreach (var v in ordinate)
            {
                if (zona.Count > 0 && v.Prezzo - zona[^1].Prezzo > soglia)
                {
                    righe.Add(Riga(zona, riferimento));
                    zona = new List<VoceScala>();
                }
                zona.Add(v);
            }
            if (zona.Count > 0) righe.Add(Riga(zona, riferimento));
        }
        return righe.OrderByDescending(r => r.Prezzo).ThenBy(r => r.Quando == "adesso" ? 0 : 1).ToList();
    }

    private static RigaScala Riga(List<VoceScala> zona, double? riferimento)
    {
        double da = zona[0].Prezzo, a = zona[^1].Prezzo;
        double prezzo = Math.Round((da + a) / 2, 2);
        return new RigaScala(prezzo, Math.Round(da, 2), Math.Round(a, 2),
            zona.Select(v => (object)new { cosa = v.Cosa, tipo = v.Tipo, fonte = v.Fonte, scadenza = v.Scadenza, peso = v.Peso, secondiFa = v.SecondiFa })
                .ToList(),
            riferimento is { } r ? Math.Round(prezzo - r, 2) : 0, zona[0].Quando, zona[0].Seduta, zona.Count > 1);
    }

    /// <summary>Il nome breve di una scadenza di X-Ray, per le voci della scala ("muro freno 0DTE").</summary>
    public static string NomeScadenza(string s) => s switch
    {
        "0dte" => "0DTE",
        "next_expiry" => "prossima",
        "opex" => "opex",
        _ => s,
    };
}
