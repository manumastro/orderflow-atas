using System.Diagnostics;
using System.Globalization;
using System.Text;

CultureInfo.DefaultThreadCurrentCulture = CultureInfo.GetCultureInfo("it-IT");
CultureInfo.CurrentCulture = CultureInfo.GetCultureInfo("it-IT");
Console.OutputEncoding = Encoding.UTF8;

const string Aiuto = """
quant: il framework quantitativo, dall'idea alla validazione.

  quant dati                         cosa c'e' in archivio
  quant chart                        i chart aperti in ATAS, dal bridge
  quant scarica --chart ID [--simbolo NQ]
                                     scarica le barre caricate su quel chart di ATAS
  quant rollover --chart ID [--simbolo NQ] [--tipo m1] [--misurato NQU6=294.5]
                                     date dei cambi da ATAS, scarto misurato, dal salto o dal carry: SIMBOLO/rollover.json
  quant taglia --simbolo NQ --tipo m1 --prima AAAA-MM-GGTHH:MMZ
                                     toglie dall'archivio i record prima di quell'istante
  quant opzioni --chart ID [--simbolo ES] [--scadenze 3] [--strike 40]
                                     registra la catena delle opzioni (ATAS non ne ha la storia)
  quant sintetico [--giorni 500] [--seme 42] [--deriva-notte PUNTI] [--deriva-cassa PUNTI]
                                     genera il mercato finto SINT (per validare il framework)
  quant divisione [--simbolo NQ] [--fissa] [--quota 0.8] [--fine AAAA-MM-GG]
                                     mostra o fissa, una volta sola, il confine dentro/fuori campione
  quant strategie                    le strategie e le loro regole
  quant prova STRATEGIA [opzioni]    backtest dentro campione, nel registro
  quant valida STRATEGIA [opzioni] [--fuori]
                                     validazione completa con rapporto; --fuori consuma il fuori campione
  quant contratto verifica           i criteri sigillati del simulatore di contratto (C1-C4)
  quant contratto NOME [--sorgente browniano|seduta|youngblood] [opzioni del contratto]
                                     simula un contratto di prop firm (vedi quant/studi/contratto-prop/)

opzioni del contratto:
  --d 850 --deriva 0 --controlli 78  sessioni browniane: deviazione e deriva per sessione ($), controlli
  --contratti 1 --valore-punto 2 --costo 2.82
                                     sessioni storiche: contratti (MNQ di default), costo per contratto
  --prezzo 145 --reset 95 --cicli 100000 --seme 1 --orizzonte 1000 --max-prelievi N --solo-valutazione
  --senza-limite                     toglie il limite giornaliero (per misurarne l'effetto)
  --politica NOME | --politiche      come opera il compratore in ogni fase (quant/studi/politica-per-fase/);
                                     --politiche: una riga per ogni politica del catalogo

opzioni comuni:
  --simbolo NQ|SINT   --tipo m1   --p chiave=valore (ripetibile)
  --commissione 2.5   --slittamento 1   --repliche 1000   --simulazioni 10000
""";

try
{
    if (args.Length == 0 || args[0] is "-h" or "--help" or "aiuto")
    {
        Console.WriteLine(Aiuto);
        return 0;
    }
    var o = Opzioni.Leggi(args.Skip(1).ToArray());
    switch (args[0])
    {
        case "dati":
            Console.WriteLine($"archivio: {Archivio.Radice}");
            foreach (var r in Archivio.Inventario()) Console.WriteLine("  " + r);
            return 0;

        case "chart":
        {
            using var ponte = new PonteAtas(o.Valore("bridge"));
            foreach (var (id, s, p) in await ponte.Chart()) Console.WriteLine($"  {id}  {s,-8} {p}");
            return 0;
        }

        case "scarica":
        {
            var chart = o.Valore("chart") ?? throw new ArgumentException("--chart ID: vedi `quant chart`");
            var simbolo = o.Valore("simbolo") ?? "NQ";
            using var ponte = new PonteAtas(o.Valore("bridge"));
            var t = Stopwatch.StartNew();
            var (barre, strumento, periodo) = await ponte.Barre(chart, Strumento.Da(simbolo), avanzamento: Console.WriteLine);
            var tipo = o.Valore("tipo") ?? periodo.Split(' ')[0].ToLowerInvariant();
            var n = Archivio.Scrivi<Barra>(simbolo, tipo, barre, b => b.Inizio);
            Console.WriteLine($"{n:N0} barre di {strumento} {periodo} in {simbolo}/{tipo}, " +
                              $"{Tempo.Utc(barre[0].Inizio):yyyy-MM-dd} -> {Tempo.Utc(barre[^1].Inizio):yyyy-MM-dd}, {t.ElapsedMilliseconds:N0} ms");
            return 0;
        }

        case "rollover":
        {
            // Le date dei cambi le da' ATAS (/rollovers sul chart del continuo); lo scarto lo calcola
            // ScartiDiRollover: misurato, salto (se la pausa e' breve) o carry (se c'e' il fine settimana).
            var chart = o.Valore("chart") ?? throw new ArgumentException("--chart ID del chart del continuo: vedi `quant chart`");
            var simbolo = o.Valore("simbolo") ?? "NQ";
            var tipo = o.Valore("tipo") ?? "m1";
            var strumento = Strumento.Da(simbolo);
            // --misurato NQU6=294.5: lo scarto del rollover che parte da NQU6, misurato sui minuti appaiati
            var misurati = o.Tutti("misurato").Select(x => x.Split('='))
                .ToDictionary(x => x[0], x => double.Parse(x[1], CultureInfo.InvariantCulture), StringComparer.OrdinalIgnoreCase);
            var barre = Archivio.Leggi<Barra>(simbolo, tipo, tempo: b => b.Inizio);
            if (barre.Length == 0) throw new InvalidOperationException($"archivio vuoto per {simbolo}/{tipo}");
            using var ponte = new PonteAtas(o.Valore("bridge"));
            using var doc = await ponte.Chiedi("/rollovers", new Dictionary<string, string?>
            {
                ["chart"] = chart,
                ["from"] = Tempo.Utc(barre[0].Inizio).AddDays(-120).ToString("yyyy-MM-dd"),
                ["to"] = Tempo.Utc(barre[^1].Inizio).AddDays(120).ToString("yyyy-MM-dd"),
            });
            var date = doc.RootElement.GetProperty("rollovers").EnumerateArray()
                .Select(r => (Codice: r.GetProperty("code").GetString()!, Tempo: Tempo.Ms(DateTime.Parse(r.GetProperty("date").GetString()!, CultureInfo.InvariantCulture, DateTimeStyles.AdjustToUniversal))))
                .OrderBy(r => r.Tempo).ToList();
            var cambi = date.Zip(date.Skip(1), (a, b) => new ScartiDiRollover.Cambio(a.Tempo, a.Codice, b.Codice, b.Tempo)).ToList();
            var rollover = ScartiDiRollover.Calcola(barre, strumento, cambi, misurati);
            foreach (var r in rollover)
                Console.WriteLine($"  {Tempo.Utc(r.Tempo):yyyy-MM-dd ddd HH:mm}Z  {r.Da} -> {r.A}  {strumento.InPrezzo(r.Scarto),9:+0.00;-0.00} punti  {r.Metodo}");
            Archivio.ScriviRollover(simbolo, rollover);
            Console.WriteLine($"{rollover.Count} rollover in {simbolo}/rollover.json: " +
                              string.Join(", ", rollover.GroupBy(r => r.Metodo).Select(g => $"{g.Count()} {g.Key}")));
            return 0;
        }

        case "taglia":
        {
            // Toglie dall'archivio i record prima di un istante: per le barre di un contratto singolo
            // scaricate quando non era ancora il principale (poco scambiato, con i buchi).
            var simbolo = o.Valore("simbolo") ?? throw new ArgumentException("--simbolo");
            var tipo = o.Valore("tipo") ?? "m1";
            var prima = DateTime.Parse(o.Valore("prima") ?? throw new ArgumentException("--prima AAAA-MM-GGTHH:MMZ"), CultureInfo.InvariantCulture, DateTimeStyles.AdjustToUniversal);
            var soglia = Tempo.Ms(prima);
            var tolti = 0;
            foreach (var f in Directory.GetFiles(Archivio.Cartella(simbolo, tipo), "*.bin"))
            {
                var mese = Path.GetFileNameWithoutExtension(f);
                var tutti = Archivio.Leggi<Barra>(simbolo, tipo, DateTime.ParseExact(mese + "-01", "yyyy-MM-dd", CultureInfo.InvariantCulture, DateTimeStyles.AdjustToUniversal | DateTimeStyles.AssumeUniversal),
                    DateTime.ParseExact(mese + "-01", "yyyy-MM-dd", CultureInfo.InvariantCulture, DateTimeStyles.AdjustToUniversal | DateTimeStyles.AssumeUniversal).AddMonths(1), b => b.Inizio);
                var restano = tutti.Where(b => b.Inizio >= soglia).ToArray();
                tolti += tutti.Length - restano.Length;
                if (restano.Length == 0) File.Delete(f); else Archivio.ScriviMese<Barra>(simbolo, tipo, mese, restano);
            }
            Console.WriteLine($"{tolti:N0} barre tolte da {simbolo}/{tipo}, prima del {prima:yyyy-MM-dd HH:mm}Z");
            return 0;
        }

        case "opzioni":
        {
            var chart = o.Valore("chart") ?? throw new ArgumentException("--chart ID di un chart del contratto (per esempio ESZ6 M1): vedi `quant chart`");
            var simbolo = o.Valore("simbolo") ?? "ES";
            using var ponte = new PonteAtas(o.Valore("bridge"));
            var t = Stopwatch.StartNew();
            var file = await ponte.RegistraOpzioni(chart, simbolo, o.Intero("scadenze", 3), o.Intero("strike", 40), Console.WriteLine);
            Console.WriteLine($"{file.Count} serie registrate in {Path.GetDirectoryName(file.FirstOrDefault() ?? "")}, {t.ElapsedMilliseconds:N0} ms");
            return 0;
        }

        case "sintetico":
        {
            var opz = new Sintetico.Opzioni
            {
                Giorni = o.Intero("giorni", 500), Seme = (ulong)o.Intero("seme", 42),
                DerivaNotte = o.Reale("deriva-notte", 0), DerivaCassa = o.Reale("deriva-cassa", 0),
            };
            var t = Stopwatch.StartNew();
            var barre = Sintetico.Genera(opz);
            var cartella = Archivio.Cartella("SINT", "m1");
            if (Directory.Exists(cartella)) Directory.Delete(cartella, true);
            Archivio.Scrivi<Barra>("SINT", "m1", barre, b => b.Inizio);
            Console.WriteLine($"SINT m1: {barre.Length:N0} barre, {opz.Giorni} giornate, deriva notte {opz.DerivaNotte} e cassa {opz.DerivaCassa} punti, {t.ElapsedMilliseconds:N0} ms");
            Console.WriteLine("La divisione dentro/fuori di SINT si fissa con `quant divisione --simbolo SINT --fissa`; una volta fissata non si sposta.");
            return 0;
        }

        case "divisione":
        {
            var simbolo = o.Valore("simbolo") ?? "NQ";
            var serie = Archivio.Serie(simbolo, o.Valore("tipo") ?? "m1");
            Console.WriteLine(serie);
            var d = Registro.LeggiDivisione(serie.Strumento.Simbolo);
            if (d is null && o.Flag("fissa"))
                d = Registro.Fissa(serie, o.Reale("quota", 0.8), o.Valore("fine") is { } f ? DateOnly.Parse(f, CultureInfo.InvariantCulture) : null);
            if (d is null) Console.WriteLine($"non fissata. Con --fissa: dentro fino al {serie.Giorni[(int)Math.Floor(serie.Giorni.Length * 0.8) - 1].Data:yyyy-MM-dd} (80% di {serie.Giorni.Length} giornate).");
            else
            {
                var fuori = serie.Giorni.Length - serie.GiornoDa(d.FineDentro.AddDays(1));
                Console.WriteLine($"dentro fino al {d.FineDentro:yyyy-MM-dd} compreso, fissata il {d.FissataIl:yyyy-MM-dd}; oggi {serie.Giorni.Length - fuori} giornate dentro e {fuori} fuori.");
            }
            return 0;
        }

        case "strategie":
            foreach (var nome in Catalogo.Nomi)
            {
                var s = Catalogo.Fabbrica(nome)();
                s.Prepara(new Parametri());
                Console.WriteLine($"{nome} (v{s.Versione})\n  {s.Regole}\n  parametri: {new Parametri().ToString()}\n  idea: {s.Idea}\n");
            }
            return 0;

        case "prova":
        {
            var (serie, fabbrica, p, e) = Prepara(o);
            var divisione = Registro.Divisione(serie);
            var ultimo = serie.GiornoDa(divisione.FineDentro.AddDays(1)) - 1;
            var r = Backtest.Esegui(serie, fabbrica(), p, e, 0, ultimo);
            var m = Metriche.Da(r);
            Registro.Annota(Validatore.DaRisultato(r, m, "dentro"));
            var sb = new StringBuilder();
            Rapporto.Tabella(sb, new Campione("dentro", r, m, Vuoto(), Vuoto()), null);
            Console.WriteLine($"{r.Strategia} {r.Impronta}  {r.Parametri}\n{r.Dimensione}; {r.Esecuzione}\n");
            Console.WriteLine(sb.ToString());
            Console.WriteLine($"Dentro campione fino al {divisione.FineDentro:yyyy-MM-dd}. Varianti provate finora: " +
                              $"{Registro.VariantiDentro(r.Strategia, serie.Strumento.Simbolo).Count}.");
            return 0;
        }

        case "valida":
        {
            var (serie, fabbrica, p, e) = Prepara(o);
            var opz = new Validatore.Opzioni
            {
                Fuori = o.Flag("fuori"), Repliche = o.Intero("repliche", 1000), Simulazioni = o.Intero("simulazioni", 10_000),
            };
            var t = Stopwatch.StartNew();
            var v = Validatore.Esegui(serie, fabbrica, p, e, opz);
            var file = Rapporto.Scrivi(v);
            foreach (var c in v.Cancelli) Console.WriteLine($"  {c.Codice}  {(c.Passato ? "passa    " : "NON PASSA")}  {c.Descrizione}: {c.Valore}");
            Console.WriteLine();
            Console.WriteLine(v.Fuori is null ? "Validazione dentro campione: il fuori non e' stato guardato."
                : v.Superata ? "Supera i cancelli. Non e' un'approvazione: la prova non l'ha smentita." : "NON supera i cancelli.");
            Console.WriteLine($"rapporto: {Path.GetRelativePath(Directory.GetCurrentDirectory(), file)}  ({t.ElapsedMilliseconds:N0} ms)");
            return 0;
        }

        case "contratto":
        {
            var nome = o.Posizionale ?? throw new ArgumentException($"manca il contratto: verifica, oppure {string.Join(", ", CatalogoContratti.Nomi)}");
            var t = Stopwatch.StartNew();
            if (nome == "verifica")
            {
                var righe = VerificaContratto.Esegui();
                foreach (var r in righe)
                    Console.WriteLine($"  {r.Criterio}  {(r.Passa ? "passa    " : "NON PASSA")}  {r.Caso,-52} atteso {r.Atteso,9:0.0000}  misurato {r.Misurato,9:0.0000}  scarto {r.Scarto,8:0.0000}");
                Console.WriteLine(righe.All(r => r.Passa) ? "Tutti i criteri passano: il simulatore non e' smentito." : "ALMENO UN CRITERIO NON PASSA.");
                Console.WriteLine($"({t.ElapsedMilliseconds:N0} ms)");
                return 0;
            }
            var contratto = CatalogoContratti.Da(nome).ConPrezzi(o.Valore("prezzo") is null ? null : o.Reale("prezzo", 0), o.Valore("reset") is null ? null : o.Reale("reset", 0));
            if (o.Flag("senza-limite"))
                contratto = contratto with
                {
                    Nome = contratto.Nome + " (senza limite giornaliero)",
                    Valutazione = [.. contratto.Valutazione.Select(f => f with { LimiteGiornaliero = null, LimiteGiornalieroAlzato = null })],
                    Finanziata = contratto.Finanziata with { LimiteGiornaliero = null, LimiteGiornalieroAlzato = null },
                };
            ISorgenteSessioni sorgente;
            int? sessioniStoriche = null;
            var tipo = o.Valore("sorgente") ?? "browniano";
            if (tipo == "browniano")
                sorgente = new SessioniBrowniane(o.Reale("d", 850), o.Reale("deriva", 0), o.Intero("controlli", 78));
            else
            {
                var serie = Archivio.Serie(o.Valore("simbolo") ?? "NQ", o.Valore("tipo") ?? "m1");
                var vp = o.Reale("valore-punto", Strumento.MNQ.ValorePunto);
                var sessioni = tipo switch
                {
                    "seduta" => SessioniDaSerie.Seduta(serie, vp),
                    "youngblood" => SessioniDaSerie.Youngblood(serie, vp, o.Intero("distanza", 100)),
                    _ => throw new ArgumentException($"--sorgente {tipo}: attesa browniano, seduta o youngblood"),
                };
                sorgente = new SessioniStoriche(sessioni, o.Intero("contratti", 1), o.Reale("costo", 2.82), o.Intero("blocco", 5));
                sessioniStoriche = sessioni.Count;
            }
            var opz = new SimulatoreContratto.Opzioni
            {
                OrizzonteFinanziato = o.Intero("orizzonte", 1000),
                MassimoPrelievi = o.Valore("max-prelievi") is null ? null : o.Intero("max-prelievi", 0),
                SoloValutazione = o.Flag("solo-valutazione"),
                Politica = o.Valore("politica") is { } np ? CatalogoContratti.Politica(np) : null,
            };
            if (o.Flag("politiche"))
            {
                Console.WriteLine($"{contratto.Nome}; sessioni: {sorgente.Descrizione}");
                Console.WriteLine($"orizzonte finanziato {opz.OrizzonteFinanziato} sessioni{(opz.MassimoPrelievi is { } m ? $", al massimo {m} prelievi" : "")}");
                Console.WriteLine($"{"politica",-9} {"P",6} {"prelievo",8} {"netto",7} {"SE",4} {"mediana",7} {"perdita",7}  condotta");
                foreach (var pol in CatalogoContratti.Politiche)
                {
                    var rp = Riepilogo.Da(SimulatoreContratto.Esegui(contratto, sorgente, o.Intero("cicli", 100_000), (ulong)o.Intero("seme", 1), opz with { Politica = pol }), contratto.Prezzi);
                    Console.WriteLine($"{pol.Nome,-9} {rp.Passaggio,6:0.000} {rp.ProbabilitaPrelievo,8:P1} {rp.NettoMedio,7:N0} {rp.ErroreNetto,4:N0} {rp.NettoP50,7:N0} {rp.ProbabilitaPerdita,7:P1}  valutazione {pol.Valutazione}; finanziato {pol.Finanziata}");
                }
                Console.WriteLine($"({t.ElapsedMilliseconds:N0} ms)");
                return 0;
            }
            var cicli = SimulatoreContratto.Esegui(contratto, sorgente, o.Intero("cicli", 100_000), (ulong)o.Intero("seme", 1), opz);
            var riep = Riepilogo.Da(cicli, contratto.Prezzi);
            Console.WriteLine($"{contratto.Nome}: {contratto.Fonte}");
            Console.WriteLine($"prezzi: ingresso {contratto.Prezzi.Ingresso:N0} $, reset {contratto.Prezzi.Reset:N0} $, attivazione {contratto.Prezzi.Attivazione:N0} $");
            Console.WriteLine($"sessioni: {sorgente.Descrizione}");
            Console.WriteLine($"orizzonte finanziato {opz.OrizzonteFinanziato} sessioni{(opz.MassimoPrelievi is { } mp ? $", al massimo {mp} prelievi" : "")}");
            if (opz.Politica is { } po) Console.WriteLine($"politica {po}");
            Console.WriteLine();
            foreach (var r in riep.Righe(sessioniStoriche)) Console.WriteLine("  " + r);
            Console.WriteLine();
            Console.WriteLine($"({t.ElapsedMilliseconds:N0} ms)");
            return 0;
        }

        default:
            Console.Error.WriteLine($"comando sconosciuto: {args[0]}\n\n{Aiuto}");
            return 2;
    }
}
catch (Exception ex) when (ex is ArgumentException or InvalidOperationException or HttpRequestException or InvalidDataException or DirectoryNotFoundException)
{
    Console.Error.WriteLine("errore: " + ex.Message);
    return 1;
}

static (Serie, Func<Strategia>, Parametri, Esecuzione) Prepara(Opzioni o)
{
    var nome = o.Posizionale ?? throw new ArgumentException("manca la strategia: vedi `quant strategie`");
    var fabbrica = Catalogo.Fabbrica(nome);
    var simbolo = o.Valore("simbolo") ?? "NQ";
    var serie = Archivio.Serie(simbolo, o.Valore("tipo") ?? "m1");
    if (serie.Barre.Length == 0) throw new InvalidOperationException($"archivio vuoto per {simbolo}: `quant scarica` o `quant sintetico`");
    Console.WriteLine(serie);
    var e = Esecuzione.Standard with
    {
        CommissioneLato = o.Reale("commissione", Esecuzione.Standard.CommissioneLato),
        SlittamentoTick = o.Intero("slittamento", Esecuzione.Standard.SlittamentoTick),
    };
    return (serie, fabbrica, Parametri.Da(o.Tutti("p")), e);
}

static EsitoNullo Vuoto() => new() { Metodo = "", Repliche = 0, NettoReale = 0, P = double.NaN };

/// <summary>Le opzioni della riga di comando: --chiave valore, --flag, e un argomento posizionale.</summary>
internal sealed class Opzioni
{
    private readonly List<(string Chiave, string? Valore)> _coppie = [];
    public string? Posizionale { get; private set; }

    public static Opzioni Leggi(string[] a)
    {
        var o = new Opzioni();
        for (var i = 0; i < a.Length; i++)
        {
            if (a[i].StartsWith("--"))
            {
                var k = a[i][2..];
                var v = i + 1 < a.Length && !a[i + 1].StartsWith("--") ? a[++i] : null;
                o._coppie.Add((k, v));
            }
            else if (o.Posizionale is null) o.Posizionale = a[i];
            else throw new ArgumentException($"argomento in piu': {a[i]}");
        }
        return o;
    }

    public string? Valore(string k) => _coppie.LastOrDefault(c => c.Chiave == k).Valore;
    public IEnumerable<string> Tutti(string k) => _coppie.Where(c => c.Chiave == k && c.Valore is not null).Select(c => c.Valore!);
    public bool Flag(string k) => _coppie.Any(c => c.Chiave == k);
    public int Intero(string k, int d) => Valore(k) is { } v ? int.Parse(v, CultureInfo.InvariantCulture) : d;
    public double Reale(string k, double d) => Valore(k) is { } v ? double.Parse(v, CultureInfo.InvariantCulture) : d;
}
