namespace Quant.Validazione;

/// <summary>Un campione provato: il risultato, le sue statistiche e i due modelli nulli.</summary>
public sealed record Campione(string Nome, Risultato Risultato, Metriche Metriche, EsitoNullo NulloTempo, EsitoNullo NulloVerso);

/// <summary>Un cancello: una condizione con la sua soglia dichiarata, e se e' passata.</summary>
public sealed record Cancello(string Codice, string Descrizione, string Valore, bool Passato);

public sealed record EsitoValidazione
{
    public required string Strategia { get; init; }
    public required string Regole { get; init; }
    public required string? Idea { get; init; }
    public required Divisione Divisione { get; init; }
    public required Campione Dentro { get; init; }
    public Campione? Fuori { get; init; }
    public required EsitoMonteCarlo Rimescolamento { get; init; }
    public required EsitoMonteCarlo Ricampionamento { get; init; }
    public required EsitoMonteCarlo RicampionamentoGiorni { get; init; }
    public required int VariantiProvate { get; init; }
    public required double SharpeDeflazionato { get; init; }
    public required double SharpeProbabilistico { get; init; }
    public required List<string> FuoriGiaVisto { get; init; }
    public required List<Cancello> Cancelli { get; init; }
    public bool Superata => Fuori is not null && Cancelli.All(c => c.Passato);
}

/// <summary>
/// La validazione del metodo, con le aggiunte che la fase 40R ha reso obbligatorie.
///
/// Il metodo del transcript: si sviluppa dentro campione, si congela, si applica fuori campione,
/// poi il Monte Carlo. Le aggiunte: il modello nullo, le statistiche per giornata, lo Sharpe
/// deflazionato per il numero di varianti, e un fuori campione che si consuma una volta sola.
/// </summary>
public static class Validatore
{
    public sealed record Opzioni
    {
        public bool Fuori { get; init; }
        public int Repliche { get; init; } = 1000;
        public int Simulazioni { get; init; } = 10_000;
        public ulong Seme { get; init; } = 20261011;
        public bool Annota { get; init; } = true;
    }

    // Le soglie dei cancelli. Si cambiano qui e nel documento del metodo, insieme.
    public const double PNullo = 0.05;
    public const double SogliaSharpeDeflazionato = 0.95;
    public const int MinimoOperazioniFuori = 30;
    public const double QuotaSharpeFuori = 0.5;

    public static EsitoValidazione Esegui(Serie serie, Func<Strategia> fabbrica, Parametri parametri, Esecuzione esecuzione, Opzioni opzioni)
    {
        var divisione = Registro.Divisione(serie);
        var ultimoDentro = serie.GiornoDa(divisione.FineDentro.AddDays(1)) - 1;
        if (ultimoDentro < 0) throw new InvalidOperationException($"nessuna giornata dentro campione (fine {divisione.FineDentro})");

        var dentro = Prova(serie, fabbrica, parametri, esecuzione, 0, ultimoDentro, "dentro", opzioni);
        Campione? fuori = null;
        var impronta = dentro.Risultato.Impronta;
        var giaVisto = Registro.FuoriGiaVisto(dentro.Risultato.Strategia, serie.Strumento.Simbolo, impronta);
        if (opzioni.Fuori)
        {
            if (ultimoDentro + 1 >= serie.Giorni.Length) throw new InvalidOperationException("nessuna giornata fuori campione nei dati");
            fuori = Prova(serie, fabbrica, parametri, esecuzione, ultimoDentro + 1, serie.Giorni.Length - 1, "fuori", opzioni);
        }

        // Il Monte Carlo sulle operazioni di tutto cio' che e' stato provato
        var ops = dentro.Risultato.Operazioni.Concat(fuori?.Risultato.Operazioni ?? []).ToList();
        var netti = ops.Select(o => o.Netto).ToArray();
        var giorni = dentro.Risultato.NettoGiorno.Concat(fuori?.Risultato.NettoGiorno ?? []).ToArray();

        var varianti = Registro.VariantiDentro(dentro.Risultato.Strategia, serie.Strumento.Simbolo);
        var prove = Math.Max(1, varianti.Count);
        double? varianza = null;
        if (varianti.Count >= 2)
        {
            var dev = Metriche.MediaDev([.. varianti.Select(v => v.SharpeGiornaliero)]).Dev;
            varianza = dev * dev;
        }
        var m = dentro.Metriche;
        var dsr = SharpeDeflazionato.Calcola(m.SharpeGiornaliero, m.Giorni, m.Asimmetria, m.Curtosi, prove, varianza);
        var psr = SharpeDeflazionato.Probabilistico(m.SharpeGiornaliero, m.Giorni, m.Asimmetria, m.Curtosi);

        var sceglieVerso = ops.Select(o => o.Lato).Distinct().Count() > 1;
        var cancelli = new List<Cancello>
        {
            new("D1", $"dentro: batte lo stare nel mercato a caso, stesso verso e stessa esposizione (p <= {PNullo})", $"p = {dentro.NulloTempo.P:0.000}", dentro.NulloTempo.P <= PNullo),
            // il verso e' un cancello solo se la strategia lo sceglie: su una strategia a verso unico misura il beta
            sceglieVerso
                ? new("D2", $"dentro: il verso scelto batte la moneta (p <= {PNullo})", $"p = {dentro.NulloVerso.P:0.000}", dentro.NulloVerso.P <= PNullo)
                : new("D2", "dentro: il verso (verso unico: e' il beta del campione, non si conta)", $"p = {dentro.NulloVerso.P:0.000}", true),
            new("D3", $"dentro: Sharpe deflazionato per {prove} variant{(prove == 1 ? "e" : "i")} >= {SogliaSharpeDeflazionato}", $"{dsr:0.000}", dsr >= SogliaSharpeDeflazionato),
        };
        if (fuori is not null)
        {
            var f = fuori.Metriche;
            cancelli.Add(new("F1", $"fuori: almeno {MinimoOperazioniFuori} operazioni", $"{f.Operazioni}", f.Operazioni >= MinimoOperazioniFuori));
            cancelli.Add(new("F2", "fuori: netto positivo", $"{f.Netto:N0} $", f.Netto > 0));
            cancelli.Add(new("F3", $"fuori: Sharpe almeno {QuotaSharpeFuori:P0} di quello dentro", $"{f.SharpeAnnuo:0.00} contro {m.SharpeAnnuo:0.00}",
                m.SharpeAnnuo > 0 && f.SharpeAnnuo >= QuotaSharpeFuori * m.SharpeAnnuo));
            cancelli.Add(new("F4", "fuori: mai guardato da un'altra variante", giaVisto.Count == 0 ? "intatto" : $"gia' visto da {string.Join(", ", giaVisto)}", giaVisto.Count == 0));
        }

        return new EsitoValidazione
        {
            Strategia = dentro.Risultato.Strategia,
            Regole = fabbrica().Regole,
            Idea = fabbrica().Idea,
            Divisione = divisione,
            Dentro = dentro,
            Fuori = fuori,
            Rimescolamento = MonteCarlo.Rimescola(netti, opzioni.Simulazioni, opzioni.Seme),
            Ricampionamento = MonteCarlo.Ricampiona(netti, opzioni.Simulazioni, opzioni.Seme + 1),
            RicampionamentoGiorni = MonteCarlo.RicampionaGiorni(giorni, opzioni.Simulazioni, opzioni.Seme + 2),
            VariantiProvate = prove,
            SharpeDeflazionato = dsr,
            SharpeProbabilistico = psr,
            FuoriGiaVisto = giaVisto,
            Cancelli = cancelli,
        };
    }

    public static Campione Prova(Serie serie, Func<Strategia> fabbrica, Parametri parametri, Esecuzione esecuzione,
                                 int da, int a, string nome, Opzioni opzioni)
    {
        var strategia = fabbrica();
        var r = Backtest.Esegui(serie, strategia, parametri, esecuzione, da, a);
        var m = Metriche.Da(r);
        var nt = ModelloNullo.Tempo(r, strategia, opzioni.Repliche, opzioni.Seme);
        var nv = ModelloNullo.Verso(r, strategia, opzioni.Repliche, opzioni.Seme + 3);
        if (opzioni.Annota) Registro.Annota(DaRisultato(r, m, nome));
        return new Campione(nome, r, m, nt, nv);
    }

    public static Prova DaRisultato(Risultato r, Metriche m, string campione) => new()
    {
        Quando = DateTime.UtcNow, Strategia = r.Strategia, Versione = r.Versione, Impronta = r.Impronta,
        Parametri = r.Parametri.ToString(), Simbolo = r.Serie.Strumento.Simbolo, Campione = campione,
        Da = r.Da, A = r.A, Operazioni = m.Operazioni, Netto = Math.Round(m.Netto, 2),
        SharpeGiornaliero = Math.Round(m.SharpeGiornaliero, 5), Esecuzione = r.Esecuzione.ToString(),
    };
}
