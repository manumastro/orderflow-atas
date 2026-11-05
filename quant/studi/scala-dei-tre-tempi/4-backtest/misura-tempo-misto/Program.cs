// K3 della scala dei tre tempi (criteri sigillati del 5 novembre 2026): il range della cassa 9:30-16:00 ET
// nei giorni con i tre tempi allineati (V1: mese, settimana, giorno; memoria candela) contro i giorni misti.
// Una giornata = un'osservazione; Welch sul logaritmo del range, una coda. Si carica solo il dentro campione:
// le barre dopo il 31 dicembre 2024 non si leggono nemmeno.
//
//   dotnet run -c Release --project quant/studi/scala-dei-tre-tempi/4-backtest/misura-tempo-misto

var serie = Archivio.Serie("NQ", "m1", null, new DateTime(2025, 1, 1, 0, 0, 0, DateTimeKind.Utc));
var fine = new DateOnly(2024, 12, 31);
Console.WriteLine(serie);
var punto = serie.Strumento.InPrezzo(1);

var allineati = new List<(double Range, double Relativo)>();
var misti = new List<(double Range, double Relativo)>();
var precedenti = new List<double>();   // i range di cassa delle giornate prima, per il relativo
var storico = new List<RiassuntoGiorno>();
for (var d = 0; d < serie.Giorni.Length; d++)
{
    var g = serie.Giorni[d];
    if (g.Data > fine) break;
    if (d > 0) storico.Add(serie.Riassunti[d - 1]);
    int hi = int.MinValue, lo = int.MaxValue, n = 0;
    for (var i = g.Primo; i <= g.Ultimo; i++)
    {
        var m = serie.MinutoEt[i];
        if (m < Calendario.AperturaCassa || m >= Calendario.ChiusuraCassa) continue;
        hi = Math.Max(hi, serie.Barre[i].Massimo);
        lo = Math.Min(lo, serie.Barre[i].Minimo);
        n++;
    }
    if (n < 300) continue;   // una cassa intera ha 390 minuti: le mezze giornate e i buchi non si contano
    var range = (hi - lo) * punto;
    var verso = LaScala.Allineati(storico, g.Data, "msg", memoriaScalino: false);
    if (verso is not null && precedenti.Count >= 20)
    {
        var rel = range / precedenti.TakeLast(20).Average();
        (verso == 0 ? misti : allineati).Add((range, rel));
    }
    precedenti.Add(range);
}

static (double Media, double Var) Mv(IReadOnlyList<double> x)
{
    var m = x.Average();
    return (m, x.Sum(v => (v - m) * (v - m)) / (x.Count - 1));
}
static double Mediana(IEnumerable<double> x) { var s = x.Order().ToArray(); return s.Length % 2 == 1 ? s[s.Length / 2] : (s[s.Length / 2 - 1] + s[s.Length / 2]) / 2; }

var la = allineati.Select(x => Math.Log(x.Range)).ToList();
var lm = misti.Select(x => Math.Log(x.Range)).ToList();
var (ma, va) = Mv(la);
var (mm, vm) = Mv(lm);
var se = Math.Sqrt(va / la.Count + vm / lm.Count);
var t = (mm - ma) / se;   // negativo se i misti sono piu' stretti
var df = Math.Pow(va / la.Count + vm / lm.Count, 2) / (Math.Pow(va / la.Count, 2) / (la.Count - 1) + Math.Pow(vm / lm.Count, 2) / (lm.Count - 1));
var p = Normale.Cdf(t);   // una coda; con questi gradi di liberta' la t e' una normale
Console.WriteLine($"giornate: allineate {allineati.Count}, miste {misti.Count}");
Console.WriteLine($"range di cassa, mediana: allineate {Mediana(allineati.Select(x => x.Range)):0.0} punti, miste {Mediana(misti.Select(x => x.Range)):0.0} punti");
Console.WriteLine($"range relativo alla media delle 20 casse prima, mediana: allineate {Mediana(allineati.Select(x => x.Relativo)):0.000}, miste {Mediana(misti.Select(x => x.Relativo)):0.000}");
Console.WriteLine($"Welch sul log del range (misti meno allineati): t = {t:0.00}, gradi di liberta' {df:0}, p a una coda = {p:0.0000}");
var ra = allineati.Select(x => Math.Log(x.Relativo)).ToList();
var rm = misti.Select(x => Math.Log(x.Relativo)).ToList();
var (mra, vra) = Mv(ra);
var (mrm, vrm) = Mv(rm);
var tr = (mrm - mra) / Math.Sqrt(vra / ra.Count + vrm / rm.Count);
Console.WriteLine($"seconda lettura, senza soglia: Welch sul log del relativo t = {tr:0.00}");
