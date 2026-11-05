using System.Text.Json;

namespace XrayLive.Server;

/// <summary>
/// Le letture dal vivo scritte dall'agente (2 novembre 2026): un file JSON per lettura in
/// quant/dati/letture/&lt;giorno NY&gt;/&lt;HHMM&gt;.json, scritto da scripts/scrivi_lettura.py quando
/// l'utente la chiede. Il server non le calcola e non le cambia: le elenca e le restituisce com'e'
/// (la forma: quant/metodo/come-si-leggono-le-opzioni-dal-vivo.md, "La pagina Lettura").
/// </summary>
internal sealed class Letture
{
    private readonly string _dir;
    private readonly Func<string, double?>? _prezzo;

    /// <param name="prezzo">il prezzo di adesso di una radice (NQ, ES), per la riga "prezzo adesso" della pagina</param>
    public Letture(string dir, Func<string, double?>? prezzo = null)
    {
        _dir = dir;
        _prezzo = prezzo;
    }

    /// <summary>I giorni con almeno una lettura, dal piu' recente.</summary>
    public List<string> Giorni() =>
        Directory.Exists(_dir)
            ? Directory.GetDirectories(_dir).Select(Path.GetFileName).OfType<string>()
                .Where(g => Directory.EnumerateFiles(Path.Combine(_dir, g), "*.json").Any())
                .OrderByDescending(g => g, StringComparer.Ordinal).ToList()
            : [];

    /// <summary>Le letture di un giorno in ordine di tempo (senza giorno: l'ultimo che ne ha). Un file
    /// illeggibile non ferma le altre: compare con il suo errore.</summary>
    public object Del(string? giorno)
    {
        var giorni = Giorni();
        string? g = string.IsNullOrEmpty(giorno) ? giorni.FirstOrDefault() : giorno;
        var letture = new List<object>();
        if (g is not null && Directory.Exists(Path.Combine(_dir, g)))
        {
            foreach (string f in Directory.GetFiles(Path.Combine(_dir, g), "*.json").OrderBy(x => x, StringComparer.Ordinal))
            {
                try
                {
                    using var doc = JsonDocument.Parse(File.ReadAllText(f));
                    letture.Add(new { file = Path.GetFileName(f), lettura = doc.RootElement.Clone() });
                }
                catch (Exception e) when (e is IOException or JsonException)
                {
                    letture.Add(new { file = Path.GetFileName(f), errore = e.Message });
                }
            }
        }
        return new { giorno = g, giorni, letture, prezzoAdesso = PrezzoAdesso(letture) };
    }

    /// <summary>Il prezzo di adesso dello strumento dell'ultima lettura (NQ dal contratto NQ, ES da ES), dal bridge.</summary>
    private object? PrezzoAdesso(List<object> letture)
    {
        if (_prezzo is null || letture.Count == 0) return null;
        var ultima = System.Text.Json.JsonSerializer.SerializeToElement(letture[^1]);
        if (!ultima.TryGetProperty("lettura", out var l) || !l.TryGetProperty("strumento", out var s) || s.ValueKind != JsonValueKind.String)
            return null;
        string strumento = s.GetString() ?? "";
        string radice = strumento.Length >= 2 ? strumento[..2].ToUpperInvariant() : strumento;
        if (radice is not ("NQ" or "ES")) return null;
        return _prezzo(radice) is { } p ? new { strumento, prezzo = p, quando = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds() } : null;
    }
}
