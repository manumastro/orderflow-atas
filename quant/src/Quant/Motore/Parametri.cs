using System.Globalization;
using System.Security.Cryptography;
using System.Text;

namespace Quant.Motore;

/// <summary>
/// I parametri di una prova. Ogni lettura registra il valore usato, anche quando e' il default:
/// cosi' il rapporto e il registro elencano tutti i parametri, non solo quelli passati a mano,
/// e l'impronta identifica davvero la variante provata.
/// </summary>
public sealed class Parametri
{
    private readonly SortedDictionary<string, string> _dati = new(StringComparer.Ordinal);
    private readonly SortedDictionary<string, string> _usati = new(StringComparer.Ordinal);

    public Parametri() { }

    public Parametri(IEnumerable<KeyValuePair<string, string>> coppie)
    {
        foreach (var (k, v) in coppie) _dati[k] = v;
    }

    public static Parametri Da(IEnumerable<string> testi)
    {
        var p = new Parametri();
        foreach (var t in testi)
        {
            var i = t.IndexOf('=');
            if (i <= 0) throw new ArgumentException($"parametro '{t}': atteso chiave=valore");
            p._dati[t[..i].Trim()] = t[(i + 1)..].Trim();
        }
        return p;
    }

    public Parametri Con(string chiave, object valore)
    {
        var p = new Parametri(_dati);
        p._dati[chiave] = Convert.ToString(valore, CultureInfo.InvariantCulture)!;
        return p;
    }

    public int Intero(string chiave, int predefinito) =>
        int.Parse(Leggi(chiave, predefinito.ToString(CultureInfo.InvariantCulture)), CultureInfo.InvariantCulture);

    public double Reale(string chiave, double predefinito) =>
        double.Parse(Leggi(chiave, predefinito.ToString("R", CultureInfo.InvariantCulture)), CultureInfo.InvariantCulture);

    public bool Booleano(string chiave, bool predefinito) => Leggi(chiave, predefinito ? "si" : "no").ToLowerInvariant() switch
    {
        "si" or "sì" or "true" or "1" or "yes" => true,
        "no" or "false" or "0" => false,
        var v => throw new ArgumentException($"{chiave}={v}: atteso si/no"),
    };

    public string Testo(string chiave, string predefinito) => Leggi(chiave, predefinito);

    /// <summary>Un orario di New York, "hh:mm", in minuti dalla mezzanotte.</summary>
    public int Orario(string chiave, string predefinito) => Quant.Dati.Calendario.Minuto(Leggi(chiave, predefinito));

    private string Leggi(string chiave, string predefinito)
    {
        var v = _dati.TryGetValue(chiave, out var x) ? x : predefinito;
        _usati[chiave] = v;
        return v;
    }

    /// <summary>I parametri passati ma mai letti: un refuso nel nome, quasi sempre.</summary>
    public IEnumerable<string> NonUsati => _dati.Keys.Where(k => !_usati.ContainsKey(k));

    public IReadOnlyDictionary<string, string> Usati => _usati;

    public string Impronta(string strategia, int versione)
    {
        var testo = $"{strategia}|v{versione}|" + string.Join(";", _usati.Select(kv => $"{kv.Key}={kv.Value}"));
        return Convert.ToHexStringLower(SHA256.HashData(Encoding.UTF8.GetBytes(testo)))[..10];
    }

    public override string ToString() => string.Join(" ", _usati.Select(kv => $"{kv.Key}={kv.Value}"));
}
