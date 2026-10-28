using System.Globalization;

namespace FabioOrderFlow.Observation;

/// <summary>
/// <b>Le sedute, in un posto solo.</b> Ogni orario di sessione si scrive nel fuso del suo mercato e
/// si converte in UTC <b>per la data della seduta</b>: la cassa di New York e' 09:30-16:00 di New
/// York, il giorno CME comincia alle 17:00 di Chicago (18:00 di New York), la London alle 08:00 di
/// Londra. Mai un UTC fisso.
///
/// <para><b>Perche'.</b> Fino al 27 ottobre 2026 la cassa era scritta 13:30Z-20:00Z, che e' giusto
/// solo con l'ora legale americana. Dal 2 novembre 2026 (New York torna all'ora solare il 1) la
/// cassa e' 14:30Z-21:00Z, e con l'orario fisso valore, POC, apertura e chiusura si sarebbero
/// misurati un'ora prima. Londra cambia ora in un'altra data (25 ottobre): fra il 25/10 e il 1/11
/// lo scarto Italia-New York e' di 5 ore invece di 6. La regola e il perche':
/// <c>docs/research/metodo/gli-orari-si-scrivono-nel-fuso-del-mercato.md</c>. La gemella in
/// Python e' <c>FabioOrderFlow/tools/sedute.py</c>: le due devono dire la stessa cosa.</para>
///
/// <para><b>La sintassi di un orario</b> (proprieta' dell'indicatore e finestre delle regole):
/// <c>09:30NY</c> (anche <c>ET</c>) New York, <c>08:00LON</c> Londra, <c>17:00CT</c> Chicago,
/// <c>13:30Z</c> UTC fisso, un ISO completo vale cosi' com'e'. Un orario si appoggia al
/// <b>giorno</b> passato, e la conversione usa le regole dell'ora legale di quel giorno.</para>
/// </summary>
public static class LeSedute
{
    public static readonly TimeZoneInfo NewYork = Fuso("America/New_York", "Eastern Standard Time");
    public static readonly TimeZoneInfo Londra = Fuso("Europe/London", "GMT Standard Time");
    public static readonly TimeZoneInfo Chicago = Fuso("America/Chicago", "Central Standard Time");

    /// <summary>La cassa degli indici (NQ, ES) e dell'oro: 09:30-16:00 di New York.</summary>
    public const string AperturaCassa = "09:30NY";
    public const string ChiusuraCassa = "16:00NY";

    /// <summary>Il pit NYMEX del crude: 09:00-14:30 di New York.</summary>
    public const string AperturaPitCrude = "09:00NY";
    public const string ChiusuraPitCrude = "14:30NY";

    /// <summary>La London si misura dalle 08:00 di Londra all'apertura di New York.</summary>
    public const string AperturaLondra = "08:00LON";

    /// <summary>Il giorno CME comincia alle 17:00 di Chicago, cioe' alle 18:00 di New York.</summary>
    public const string InizioGiorno = "18:00NY";

    private static TimeZoneInfo Fuso(string iana, string windows)
    {
        foreach (var id in new[] { iana, windows })
        {
            if (TimeZoneInfo.TryFindSystemTimeZoneById(id, out var fuso))
            {
                return fuso;
            }
        }

        throw new TimeZoneNotFoundException($"fuso {iana} ({windows}) non trovato sul sistema");
    }

    /// <summary>
    /// I valori che fino al 27 ottobre 2026 erano i default delle proprieta' (13:30Z, 20:00Z,
    /// 07:00Z, 13:00Z, 18:30Z). ATAS salva le proprieta' nel workspace, quindi un chart vecchio li
    /// riporta: non sono una scelta, sono il default di allora, e si leggono come la seduta
    /// dinamica di cui erano la versione estiva.
    /// </summary>
    private static readonly Dictionary<string, string> Ereditati = new(StringComparer.OrdinalIgnoreCase)
    {
        ["13:30Z"] = AperturaCassa,
        ["20:00Z"] = ChiusuraCassa,
        ["07:00Z"] = AperturaLondra,
        ["13:00Z"] = AperturaPitCrude,
        ["18:30Z"] = ChiusuraPitCrude,
    };

    /// <summary>Il valore di una proprieta' di orario: vuoto o ereditato diventa il default dinamico.</summary>
    public static string Orario(string? scritto, string predefinito)
    {
        if (string.IsNullOrWhiteSpace(scritto))
        {
            return predefinito;
        }

        var t = scritto.Trim();
        return Ereditati.TryGetValue(t, out var dinamico) ? dinamico : t;
    }

    /// <summary>
    /// Un momento in UTC. Un orario (<c>09:30NY</c>, <c>08:00LON</c>, <c>17:00CT</c>,
    /// <c>13:30Z</c>, <c>13:30</c> = UTC) si appoggia alla data di <paramref name="giorno"/>;
    /// un ISO completo vale cosi' com'e'.
    /// </summary>
    public static DateTime Momento(string grezzo, DateTime giorno)
    {
        var testo = grezzo.Trim();
        if (testo.Length <= 9 && testo.Contains(':', StringComparison.Ordinal))
        {
            var (ore, fuso) = Separa(testo);
            var parti = ore.Split(':');
            var h = int.Parse(parti[0], CultureInfo.InvariantCulture);
            var m = parti.Length > 1 ? int.Parse(parti[1], CultureInfo.InvariantCulture) : 0;
            var locale = DateTime.SpecifyKind(giorno.Date.AddHours(h).AddMinutes(m), DateTimeKind.Unspecified);
            if (fuso is null)
            {
                return DateTime.SpecifyKind(locale, DateTimeKind.Utc);
            }

            if (fuso.IsInvalidTime(locale))
            {
                locale = locale.AddHours(1); // l'ora che non esiste nella notte del cambio
            }

            return TimeZoneInfo.ConvertTimeToUtc(locale, fuso);
        }

        return DateTime.Parse(testo, CultureInfo.InvariantCulture,
            DateTimeStyles.AdjustToUniversal | DateTimeStyles.AssumeUniversal);
    }

    /// <summary>Vero per un orario del giorno (<c>09:30NY</c>, <c>13:30Z</c>), falso per un ISO completo.</summary>
    public static bool EOrario(string grezzo)
    {
        var t = grezzo.Trim();
        return t.Length <= 9 && t.Contains(':', StringComparison.Ordinal);
    }

    private static (string Ore, TimeZoneInfo? Fuso) Separa(string testo)
    {
        var t = testo.ToUpperInvariant();
        if (t.EndsWith("LON", StringComparison.Ordinal)) { return (t[..^3], Londra); }
        if (t.EndsWith("NY", StringComparison.Ordinal)) { return (t[..^2], NewYork); }
        if (t.EndsWith("ET", StringComparison.Ordinal)) { return (t[..^2], NewYork); }
        if (t.EndsWith("CT", StringComparison.Ordinal)) { return (t[..^2], Chicago); }
        if (t.EndsWith('Z')) { return (t[..^1], null); }
        return (t, null);
    }

    /// <summary>
    /// Il giorno di mercato di un istante UTC: dalle 18:00 di New York (17:00 di Chicago) la notte
    /// appartiene alla seduta dopo, come la conta il CME. In ora legale sono le 22:00Z, in ora
    /// solare le 23:00Z.
    /// </summary>
    public static DateTime GiornoDiMercato(DateTime utc)
    {
        var ny = TimeZoneInfo.ConvertTimeFromUtc(DateTime.SpecifyKind(utc, DateTimeKind.Utc), NewYork);
        return ny.Hour >= 18 ? ny.Date.AddDays(1) : ny.Date;
    }

    /// <summary>L'inizio (UTC) del giorno di mercato <paramref name="giorno"/>: le 18:00 di New York del giorno prima.</summary>
    public static DateTime InizioDelGiorno(DateTime giorno) => Momento(InizioGiorno, giorno.Date.AddDays(-1));

    /// <summary>La cassa di New York di una data, in UTC.</summary>
    public static (DateTime Da, DateTime A) Cassa(DateTime giorno)
        => (Momento(AperturaCassa, giorno), Momento(ChiusuraCassa, giorno));

    /// <summary><c>14:30Z-21:00Z</c>: l'orario vero di una finestra in un giorno, per le etichette.</summary>
    public static string Etichetta(string da, string a, DateTime giorno)
        => $"{Momento(da, giorno):HH:mm}Z-{Momento(a, giorno):HH:mm}Z";
}
