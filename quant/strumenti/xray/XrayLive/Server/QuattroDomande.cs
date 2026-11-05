using System.Text.Json.Nodes;

namespace XrayLive.Server;

/// <summary>
/// Le opzioni in quattro domande (5 novembre 2026): il regime, il meteo, i muri sul profilo, il grilletto. Dal video sul
/// bias dalle opzioni (docs/research/fonti-video/2026-11-05-il-bias-dalle-opzioni.txt, [V2 · 0:20-0:47]); come si
/// leggono e quanto sono provate: docs/research/metodo/il-bias-del-giorno.md e
/// quant/metodo/come-si-leggono-le-opzioni-dal-vivo.md.
///
/// UNA REGOLA SOLA IN UN POSTO SOLO: la logica era in scripts/xray_query.py (<c>quattro_domande</c>) ed è stata
/// portata qui; xray_query.py ora legge /api/quattro-domande e traduce la risposta nel dizionario di prima, così il
/// giro (le_quattro_tab.py), lo studio (studio/raccogli.py) e la pagina leggono la stessa cosa. Le frasi sono
/// quelle di Python, parola per parola. Il codice mette in fila i numeri; dire cosa significano per la giornata
/// resta alla lettura, senza segnali.
///
/// TUTTI i prezzi della risposta sono nel future della radice (ES o NQ), come in Python: ES = SPX + la base ES-SPX.
/// </summary>
internal sealed class QuattroDomande
{
    /// <summary>Vicino = un muro a non più di tanti punti da un bordo del valore (convenzione, non provata).</summary>
    public static readonly Dictionary<string, double> SogliaMuro = new() { ["NQ"] = 25.0, ["ES"] = 6.0 };

    public const string EsitoD =
        "non provato: su 42 sedute di ES con X-Ray (08/09-04/11) il regime 0DTE delle 9:35 NON ha allargato " +
        "la giornata (0,55 contro 0,74 del movimento atteso, p 0,24, verso opposto al video); opzioni-gamma-es, ipotesi D";

    public const string Provato = "dice quanto, non dove; da rileggere in giornata; " + EsitoD;

    public const string Grilletto =
        "l'order flow (tab 3 e 4): al muro con freno si cerca l'assorbimento e il ritorno al POC, con " +
        "acceleratore l'accettazione oltre; qui nessun segnale";

    public const string Fonte = "docs/research/metodo/il-bias-del-giorno.md, [V2]";

    private readonly Misure _misure;
    private readonly Volatilita.MisureVol _vol;
    private readonly Bordi _bordi;

    public QuattroDomande(Misure misure, Volatilita.MisureVol vol, Bordi bordi)
    {
        _misure = misure;
        _vol = vol;
        _bordi = bordi;
    }

    /// <summary>"freno" se il valore è positivo o zero, "acceleratore" se negativo: il verso di Python.</summary>
    public static string? Verso(double? x) => x is not { } v ? null : v >= 0 ? "freno" : "acceleratore";

    /// <summary>La lettura del regime: "segnali opposti" se al prezzo e 0DTE ci sono e differiscono, altrimenti quello
    /// dei due che c'è. Restituisce (lettura, frase); le frasi sono quelle di Python.</summary>
    public static (string? lettura, string? frase) Lettura(string? alPrezzo, string? zeroDte)
    {
        if (alPrezzo is not null && zeroDte is not null && alPrezzo != zeroDte)
            return ("segnali opposti",
                $"segnali opposti: {alPrezzo} su tutte le scadenze, {zeroDte} sullo 0DTE; il regime non è netto, e lo 0DTE " +
                "pesa di più man mano che si avvicina la chiusura");
        string? uno = alPrezzo ?? zeroDte;
        return uno switch
        {
            "freno" => ("freno", "freno: per il video la giornata resta più stretta e i livelli tendono a tenere " +
                                 "(al bordo si guarda l'assorbimento)"),
            "acceleratore" => ("acceleratore", "acceleratore: per il video la giornata si allarga e i livelli tendono a rompersi " +
                                               "(al bordo si guarda se il prezzo accetta oltre)"),
            _ => (null, null),
        };
    }

    /// <summary>Ogni coppia (muro, bordo) entro la soglia, nell'ordine dei muri e poi dei bordi, come Python.</summary>
    public static List<object> Vicini(List<(string nome, double prezzo)> muri, List<Bordo> bordi, double soglia) =>
        (from m in muri
         from b in bordi
         where Math.Abs(m.prezzo - b.Prezzo) <= soglia
         select (object)new { muro = m.nome, prezzo = m.prezzo, bordo = b.Nome, prezzoBordo = b.Prezzo, distanza = Math.Round(m.prezzo - b.Prezzo, 2) })
        .ToList();

    public object Di(string radice, string? giorno, long? ts)
    {
        var errori = new List<string>();
        var muri = new List<(string nome, double prezzo)>();
        double? prezzo;
        double? baseEsSpx = null;
        object regime;
        string indice;
        bool vivo;
        (List<Bordo> bordi, string? nota) bordi;

        if (radice == "ES")
        {
            var giorni = _misure.Giorni();
            string? g = string.IsNullOrEmpty(giorno) ? giorni.FirstOrDefault() : giorno;
            vivo = ts is null && (g is null || g == giorni.FirstOrDefault());
            bordi = _bordi.Di("ES", vivo ? null : g);
            indice = "SPX";
            if (g is null)
            {
                errori.Add("nessuna giornata nell'archivio di X-Ray");
                prezzo = null;
                regime = RegimeVuoto("ES", "stima OptionsDepth su SPX");
            }
            else
            {
                var f = _misure.FotografiaAl(g, ts);
                baseEsSpx = f.Base.Valore;
                double? gamma = f.Gamma;
                var nette = f.Gex.ToDictionary(x => x.scadenza,
                    x => x.c is null ? (double?)null : Misure.R(x.c.Call.Sum() + x.c.Put.Sum(), 3));
                double? zero = nette.GetValueOrDefault("0dte");
                double? tutte = nette.Values.Any(v => v is not null) ? Misure.R(nette.Values.Sum(v => v ?? 0), 3) : null;
                double? flipVicino = null;
                if (baseEsSpx is { } b)
                {
                    prezzo = Math.Round(f.Rif.Prezzo + b, 2);
                    var flip = f.Zeri.Select(z => Math.Round(z + b, 2)).OrderBy(z => Math.Abs(z - prezzo.Value)).ToList();
                    flipVicino = flip.Count > 0 ? flip[0] : null;
                    if (f.Positivi.Count > 0) muri.Add(("freno più forte", Math.Round(f.Positivi[0].Strike + b, 2)));
                    if (f.Negativi.Count > 0) muri.Add(("acceleratore più forte", Math.Round(f.Negativi[0].Strike + b, 2)));
                    foreach (var z in flip) muri.Add(("flip", z));
                }
                else
                {
                    // senza la base i prezzi di ES non si fanno, e Python allora non dava niente: lo si dice
                    prezzo = null;
                    gamma = null;
                    errori.Add("manca la base ES-SPX (il bridge non risponde?): niente prezzi in ES");
                }
                string? alPrezzo = Verso(gamma), zeroDte = Verso(zero);
                var (lettura, frase) = Lettura(alPrezzo, zeroDte);
                regime = new
                {
                    radice = "ES",
                    @base = "stima OptionsDepth su SPX",
                    fonte = "X-Ray (OptionsDepth): gamma osservata al prezzo e GEX dei market maker per scadenza",
                    alPrezzo, zeroDte, tutte = Verso(tutte),
                    valori = new { alPrezzo = gamma, zeroDte = zero, tutte },
                    lettura, frase, provato = Provato,
                    gexPerScadenza = nette,
                    flipPiuVicino = flipVicino,
                    zero = (object?)null,
                };
            }
        }
        else
        {
            // NQ: l'ultimo punto della GEX viva della catena CME (gex-NQ.jsonl), come xray_query.gex_viva; solo dal vivo
            vivo = true;
            indice = "NDX";
            bordi = _bordi.Di("NQ");
            if (!string.IsNullOrEmpty(giorno) || ts is not null) errori.Add("su NQ c'è solo l'ultimo dato: giorno e istante si ignorano");
            var punti = _vol.PuntiGexViva("NQ");
            var u = punti.Count > 0 ? punti[^1] : null;
            double? D(string k) => u?[k] is JsonValue v && v.TryGetValue<double>(out var x) ? x : null;
            prezzo = D("future");
            if (u is null) errori.Add("GEX viva di NQ assente (gex-NQ.jsonl)");
            foreach (var (nome, k) in new[]
                     {
                         ("call wall", "callWall"), ("put wall", "putWall"), ("call wall 0DTE", "callWall0"),
                         ("put wall 0DTE", "putWall0"), ("zero gamma", "zero"), ("zero gamma 0DTE", "zero0"),
                     })
                if (D(k) is { } p) muri.Add((nome, Math.Round(p, 2)));
            string? alPrezzo = Verso(D("totale")), zeroDte = Verso(D("totale0"));
            var (lettura, frase) = Lettura(alPrezzo, zeroDte);
            regime = new
            {
                radice = "NQ",
                @base = "convenzione A, supposta",
                fonte = "GEX viva della catena CME di NQ, convenzione A (supposta: dealer lunghi call, corti put)",
                alPrezzo, zeroDte, tutte = alPrezzo,
                valori = new { alPrezzo = D("totale"), zeroDte = D("totale0"), tutte = D("totale") },
                lettura, frase, provato = Provato,
                gexPerScadenza = (object?)null,
                flipPiuVicino = (double?)null,
                zero = new { tutte = D("zero"), zeroDte = D("zero0") },
                quando = u?["ts"] is JsonValue t && t.TryGetValue<long>(out var tl) ? tl : (long?)null,
            };
        }

        // il meteo: solo dal vivo (la volatilità non ha storia per istante)
        object? meteo = null;
        if (vivo)
        {
            var ib = _vol.InBreve().Indici.FirstOrDefault(i => i.Indice == indice);
            if (ib is not null)
                meteo = new
                {
                    indice = ib.Indice, nomeIndice = ib.NomeIndice, fonteLivello = ib.FonteLivello, quando = ib.QuandoLivello,
                    livello = ib.Livello, fascia = ib.Fascia, movimentoOggi = ib.MovimentoOggi, fasciaPrezzi = ib.FasciaPrezzi,
                    movimentoGiorno = ib.MovimentoGiorno, realizzata20 = ib.Realizzata20,
                    implicitaMenoRealizzata = ib.ImplicitaMenoRealizzata, implicitaTesto = ib.ImplicitaTesto,
                    pendenza = ib.Pendenza, struttura = ib.Struttura, skew = ib.Skew, skewTesto = ib.SkewTesto,
                };
            else errori.Add("la volatilità non risponde");
        }
        else errori.Add("il meteo c'è solo dal vivo");

        double soglia = SogliaMuro[radice];
        return new
        {
            radice, giorno = radice == "ES" ? (string.IsNullOrEmpty(giorno) ? _misure.Giorni().FirstOrDefault() : giorno) : null, ts,
            unita = radice, baseEsSpx, prezzo, fonte = Fonte,
            regime, meteo,
            muri = new
            {
                sogliaPunti = soglia,
                bordi = bordi.bordi.Select(b => new { nome = b.Nome, tipo = b.Tipo, seduta = b.Seduta, prezzo = b.Prezzo }).ToList(),
                muri = muri.Select(m => new { muro = m.nome, prezzo = m.prezzo, dalPrezzo = prezzo is { } p ? Math.Round(m.prezzo - p, 2) : (double?)null })
                    .ToList(),
                vicini = Vicini(muri, bordi.bordi, soglia),
                nota = bordi.nota,
            },
            grilletto = Grilletto,
            errori,
        };
    }

    private static object RegimeVuoto(string radice, string @base) => new
    {
        radice, @base, fonte = "", alPrezzo = (string?)null, zeroDte = (string?)null, tutte = (string?)null,
        valori = new { alPrezzo = (double?)null, zeroDte = (double?)null, tutte = (double?)null },
        lettura = (string?)null, frase = (string?)null, provato = Provato, gexPerScadenza = (object?)null,
        flipPiuVicino = (double?)null, zero = (object?)null,
    };
}
