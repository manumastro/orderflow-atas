using System.Globalization;
using System.Text.Json.Nodes;

namespace XrayLive;

// La compattazione dell'archivio scritto prima del 30 ottobre 2026 (o di qualunque archivio):
// stesse regole del lettore (Archivio.cs), applicate ai file che ci sono gia'.
//
// Non tocca l'archivio finche' non ha finito: scrive series.nuova/, controlla che ogni giornata
// abbia le stesse chiavi di prima, poi rinomina series/ in series.vecchia/ e series.nuova/ in
// series/. La cartella vecchia la cancella chi ha verificato (la GUI, scheda Archivio, o a mano).
internal static class Compatta
{
    public static int Esegui(string root)
    {
        string serie = Path.Combine(root, "series");
        string nuova = Path.Combine(root, "series.nuova");
        string vecchia = Path.Combine(root, "series.vecchia");
        if (!Directory.Exists(serie))
        {
            Console.Error.WriteLine($"niente da compattare: manca {serie}");
            return 3;
        }
        if (Directory.Exists(vecchia))
        {
            Console.Error.WriteLine($"c'e' ancora {vecchia}: cancellala (dopo averla verificata) e riprova");
            return 2;
        }
        if (Directory.Exists(nuova)) Directory.Delete(nuova, recursive: true);

        var giorni = Directory.EnumerateFiles(serie, "*.jsonl*", SearchOption.AllDirectories)
            .Where(f => f.EndsWith(".jsonl") || f.EndsWith(".jsonl.gz"))
            .Select(f => f.EndsWith(".gz") ? f[..^3] : f)
            .Distinct(StringComparer.OrdinalIgnoreCase)
            .OrderBy(f => f, StringComparer.Ordinal)
            .ToList();

        long prima = DirBytes(serie);
        long righePrima = 0, colonnePrima = 0, scritte = 0;
        var nuovoArchivio = new Archivio(root);
        var oggi = DateTime.UtcNow.Date;
        int errori = 0;

        foreach (var giorno in giorni)
        {
            string relativo = Path.GetRelativePath(serie, giorno);
            string destinazione = Path.Combine(nuova, relativo);
            Directory.CreateDirectory(Path.GetDirectoryName(destinazione)!);

            var chiaviPrima = new HashSet<string>(StringComparer.Ordinal);
            long colonneGiorno = 0, scritteGiorno = 0;
            foreach (var riga in Archivio.Righe(giorno))
            {
                righePrima++;
                if (Archivio.Leggi(riga) is not { } r) continue;
                var (testa, colonne) = r;
                foreach (var nodo in colonne)
                {
                    if (nodo is not JsonObject col) continue;
                    Archivio.Arrotonda(col);
                    chiaviPrima.Add(Archivio.Chiave(testa, col));
                }
                colonneGiorno += colonne.Count;
                scritteGiorno += nuovoArchivio.Accoda(destinazione, testa, colonne);
            }
            colonnePrima += colonneGiorno;
            scritte += scritteGiorno;

            // la verifica: la giornata nuova ha esattamente le chiavi di quella vecchia
            var chiaviDopo = new HashSet<string>(StringComparer.Ordinal);
            foreach (var riga in Archivio.Righe(destinazione))
                if (Archivio.Leggi(riga) is { } r)
                    foreach (var nodo in r.colonne)
                        if (nodo is JsonObject col)
                            chiaviDopo.Add(Archivio.Chiave(r.testa, col));
            if (!chiaviPrima.SetEquals(chiaviDopo))
            {
                errori++;
                Console.Error.WriteLine($"DIVERSO {relativo}: {chiaviPrima.Count} chiavi prima, {chiaviDopo.Count} dopo");
            }

            // giornate chiuse compresse subito (la regola delle due ore vale per chi scrive dal vivo)
            if (File.Exists(destinazione)
                && DateTime.TryParseExact(Path.GetFileNameWithoutExtension(destinazione), "yyyy-MM-dd",
                    CultureInfo.InvariantCulture, DateTimeStyles.None, out var data)
                && data < oggi.AddDays(-1))
                Archivio.Comprimi(destinazione);

            Console.WriteLine($"{relativo,-60} {colonneGiorno,7} colonne -> {scritteGiorno,6}");
        }

        long dopo = DirBytes(nuova);
        Console.WriteLine();
        Console.WriteLine($"giornate {giorni.Count}, righe lette {righePrima:N0}, colonne {colonnePrima:N0} -> {scritte:N0} " +
                          $"(doppie {nuovoArchivio.Doppie:N0}, revisioni {nuovoArchivio.Revisioni:N0})");
        Console.WriteLine($"spazio {prima / 1048576.0:N0} MB -> {dopo / 1048576.0:N0} MB");

        if (errori > 0)
        {
            Console.Error.WriteLine($"{errori} giornate diverse: l'archivio NON e' stato toccato, il nuovo e' in {nuova}");
            return 1;
        }

        Directory.Move(serie, vecchia);
        Directory.Move(nuova, serie);
        CompattaLatest(Path.Combine(root, "latest"));
        Console.WriteLine($"fatto: l'archivio nuovo e' {serie}; il vecchio e' in {vecchia}, da cancellare dopo la verifica");
        return 0;
    }

    /// <summary>I file "latest" arrotondati e senza rientri.</summary>
    private static void CompattaLatest(string latest)
    {
        if (!Directory.Exists(latest)) return;
        foreach (var file in Directory.EnumerateFiles(latest, "*.json"))
        {
            try
            {
                if (JsonNode.Parse(File.ReadAllText(file)) is not JsonObject obj) continue;
                if (obj["columns"] is JsonArray cols)
                    foreach (var c in cols)
                        if (c is JsonObject o) Archivio.Arrotonda(o);
                if (obj["last"] is JsonObject last) Archivio.Arrotonda(last);
                File.WriteAllText(file, obj.ToJsonString());
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"{file}: {ex.Message}");
            }
        }
    }

    private static long DirBytes(string dir) => !Directory.Exists(dir) ? 0
        : Directory.EnumerateFiles(dir, "*", SearchOption.AllDirectories).Sum(f => new FileInfo(f).Length);
}
