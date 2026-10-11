using System.Text.Json;
using System.Text.Json.Serialization;
using Quant.Dati;

namespace Quant.Validazione;

/// <summary>Una riga del registro: una prova, con abbastanza dati da contarla e da ritrovarla.</summary>
public sealed record Prova
{
    public required DateTime Quando { get; init; }
    public required string Strategia { get; init; }
    public required int Versione { get; init; }
    public required string Impronta { get; init; }
    public required string Parametri { get; init; }
    public required string Simbolo { get; init; }
    public required string Campione { get; init; }
    public required DateOnly Da { get; init; }
    public required DateOnly A { get; init; }
    public required int Operazioni { get; init; }
    public required double Netto { get; init; }
    public required double SharpeGiornaliero { get; init; }
    public required string Esecuzione { get; init; }
}

/// <summary>
/// La divisione fra dentro e fuori campione, fissata una volta per simbolo e poi mai piu' spostata.
/// Se si ricalcolasse ogni volta come 80% dei dati, ogni download nuovo sposterebbe il confine e
/// pezzi di fuori campione finirebbero dentro senza che nessuno se ne accorga. I dati che arrivano
/// dopo la fissazione vanno tutti fuori campione: dati mai visti, cioe' il vivo che si accumula.
/// </summary>
public sealed record Divisione(string Simbolo, DateOnly FineDentro, DateTime FissataIl, int GiorniAllora, double Quota);

/// <summary>
/// Il registro delle prove: <c>quant/registro/prove.jsonl</c>, una riga per prova, in git.
/// Serve a due cose che la memoria non fa:
/// <list type="number">
/// <item>contare le varianti provate, che entrano nello Sharpe deflazionato;</item>
/// <item>sapere se il fuori campione e' gia' stato guardato con un'altra variante: da quel momento
/// non e' piu' fuori campione per quella strategia, e il rapporto lo scrive.</item>
/// </list>
/// </summary>
public static class Registro
{
    private static readonly JsonSerializerOptions Json = new()
    {
        WriteIndented = false,
        Converters = { new JsonStringEnumConverter() },
    };

    private static string FileProve => Path.Combine(Percorsi.Registro, "prove.jsonl");
    private static string FileDivisioni => Path.Combine(Percorsi.Registro, "divisioni.json");

    public static void Annota(Prova p)
    {
        Directory.CreateDirectory(Percorsi.Registro);
        File.AppendAllText(FileProve, JsonSerializer.Serialize(p, Json) + "\n");
    }

    public static List<Prova> Tutte()
    {
        if (!File.Exists(FileProve)) return [];
        return [.. File.ReadAllLines(FileProve).Where(l => l.Length > 0).Select(l => JsonSerializer.Deserialize<Prova>(l, Json)!)];
    }

    /// <summary>Le varianti distinte provate dentro campione per questa strategia e questo simbolo.</summary>
    public static List<Prova> VariantiDentro(string strategia, string simbolo) =>
        [.. Tutte().Where(p => p.Strategia == strategia && p.Simbolo == simbolo && p.Campione == "dentro")
                   .GroupBy(p => p.Impronta).Select(g => g.Last())];

    /// <summary>Le varianti, diverse da questa, che hanno gia' guardato il fuori campione.</summary>
    public static List<string> FuoriGiaVisto(string strategia, string simbolo, string impronta) =>
        [.. Tutte().Where(p => p.Strategia == strategia && p.Simbolo == simbolo && p.Campione == "fuori" && p.Impronta != impronta)
                   .Select(p => p.Impronta).Distinct()];

    public static Divisione? LeggiDivisione(string simbolo)
    {
        if (!File.Exists(FileDivisioni)) return null;
        var tutte = JsonSerializer.Deserialize<List<Divisione>>(File.ReadAllText(FileDivisioni), Json) ?? [];
        return tutte.FirstOrDefault(d => d.Simbolo == simbolo);
    }

    /// <summary>
    /// La divisione del simbolo. Non si fissa mai da sola: va fissata con <see cref="Fissa"/>
    /// (`quant divisione --fissa`) quando la storia disponibile e' quella su cui si lavorera'.
    /// Fissata su un mese di dati, congelerebbe il confine prima che arrivi la storia lunga.
    /// </summary>
    public static Divisione Divisione(Serie serie) =>
        LeggiDivisione(serie.Strumento.Simbolo)
        ?? throw new InvalidOperationException($"divisione dentro/fuori non fissata per {serie.Strumento.Simbolo}: " +
                                               $"`quant divisione --simbolo {serie.Strumento.Simbolo} --fissa` la fissa, una volta sola");

    public static Divisione Fissa(Serie serie, double quota = 0.8, DateOnly? fine = null)
    {
        var simbolo = serie.Strumento.Simbolo;
        if (LeggiDivisione(simbolo) is { } d)
            throw new InvalidOperationException($"divisione gia' fissata per {simbolo}: dentro fino al {d.FineDentro:yyyy-MM-dd}. Non si sposta.");
        if (serie.Giorni.Length < 10) throw new InvalidOperationException("troppo poche giornate per fissare la divisione");
        var k = (int)Math.Floor(serie.Giorni.Length * quota) - 1;
        var nuova = new Divisione(simbolo, fine ?? serie.Giorni[k].Data, DateTime.UtcNow, serie.Giorni.Length, quota);
        var tutte = File.Exists(FileDivisioni) ? JsonSerializer.Deserialize<List<Divisione>>(File.ReadAllText(FileDivisioni), Json) ?? [] : [];
        tutte.Add(nuova);
        Directory.CreateDirectory(Percorsi.Registro);
        File.WriteAllText(FileDivisioni, JsonSerializer.Serialize(tutte, new JsonSerializerOptions(Json) { WriteIndented = true }));
        return nuova;
    }
}
