namespace Quant.Dati;

/// <summary>
/// L'orologio di New York. Ogni regola del metodo e' scritta in ora di New York (9:30, 16:00),
/// mentre i dati sono in UTC: qui si converte, con l'ora legale americana, che non cambia nelle
/// stesse date di quella europea.
///
/// La giornata di borsa del future comincia alle 18:00 ET del giorno prima (riapertura Globex):
/// una barra delle 19:00 ET di martedi' appartiene alla giornata di mercoledi'.
/// </summary>
public static class Calendario
{
    public static readonly TimeZoneInfo NewYork = TimeZoneInfo.FindSystemTimeZoneById("America/New_York");

    public const int AperturaCassa = 9 * 60 + 30;
    public const int ChiusuraCassa = 16 * 60;
    public const int RiaperturaGlobex = 18 * 60;

    /// <summary>Ora di New York per un istante UTC, con una cache per ora: lo scarto cambia due volte l'anno.</summary>
    public sealed class Convertitore
    {
        private long _ora = long.MinValue;
        private long _scarto;

        public DateTime Et(long ms)
        {
            var ora = Math.DivRem(ms, Tempo.Ora, out var resto);
            if (resto < 0) ora--;
            if (ora != _ora)
            {
                _ora = ora;
                var utc = Tempo.Utc(ora * Tempo.Ora);
                _scarto = (long)NewYork.GetUtcOffset(utc).TotalMilliseconds;
            }
            return Tempo.Utc(ms + _scarto);
        }
    }

    public static DateTime Et(long ms) => TimeZoneInfo.ConvertTimeFromUtc(Tempo.Utc(ms), NewYork);

    /// <summary>La giornata di borsa a cui appartiene un'ora di New York.</summary>
    public static DateOnly GiornataDi(DateTime et)
    {
        var data = DateOnly.FromDateTime(et);
        return et.Hour * 60 + et.Minute >= RiaperturaGlobex ? data.AddDays(1) : data;
    }

    /// <summary>L'istante UTC di un'ora di New York in una data di calendario.</summary>
    public static long Utc(DateOnly data, int minutoEt)
    {
        var locale = data.ToDateTime(new TimeOnly(minutoEt / 60, minutoEt % 60), DateTimeKind.Unspecified);
        if (NewYork.IsInvalidTime(locale)) locale = locale.AddHours(1);
        return Tempo.Ms(TimeZoneInfo.ConvertTimeToUtc(locale, NewYork));
    }

    public static string Orario(int minutoEt) => $"{minutoEt / 60:00}:{minutoEt % 60:00}";

    public static int Minuto(string hhmm)
    {
        var parti = hhmm.Split(':');
        return int.Parse(parti[0]) * 60 + int.Parse(parti[1]);
    }
}
