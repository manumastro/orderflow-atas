using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using ATAS.Indicators;
using Utils.Common.Logging;

namespace FabioOrderFlow.Observation;

/// <summary>
/// Il motore dei livelli, dentro l'indicatore.
///
/// <para><b>Perche' sta qui e non in uno script.</b> Fino al 20 settembre 2026 i livelli li
/// calcolava <c>livelli_vivi.py</c>, un processo esterno che ogni trenta secondi ricalcolava e
/// ridepositava. Funzionava finche' girava. Quando moriva — ATAS riavviato, terminale chiuso,
/// eccezione — le righe restavano sul chart <b>identiche a prima</b>, con la tilde che continuava
/// a promettere che si muovevano. Un livello vivo fermo e' peggio di un livello scaduto, perche'
/// dichiara una freschezza che non ha.</para>
///
/// <para>La difesa non e' accorgersene: e' togliere di mezzo la cosa che puo' morire. Qui il
/// motore <b>e' l'indicatore</b>. Se il chart e' aperto i livelli sono di adesso; se il chart e'
/// chiuso non c'e' niente da ingannare. Non esiste lo stato intermedio che faceva danno.</para>
///
/// <para><b>Cosa resta all'analisi.</b> Quali regole mettere nel file, su quale finestra, e a cosa
/// serve arrivare a quel livello. Cioe' la strategia. Il prezzo lo trova la macchina: un POC, un
/// bordo del valore, un massimo o una mensola sono <i>misure</i>, e una misura non ha bisogno di
/// un agente che la rifaccia a mano.</para>
///
/// <para><b>I tre marcatori, e sono tre perche' le cose sono tre.</b> Fino a oggi la tilde
/// distingueva "vivo" da "tutto il resto", e finiva per mettere insieme due cose diverse: un
/// massimo della notte — che nasce da una misura su una finestra ormai chiusa — e una mensola
/// scritta a mano dall'analisi. La prima e' vera per costruzione, la seconda e' un'affermazione
/// di qualcuno.</para>
///
/// <code>
///     ~   misurato, finestra aperta    si muove a ogni barra
///     =   misurato, finestra chiusa    non si muove piu', ma nasce da una misura
///     *   dichiarato dall'analisi      e' un'affermazione, e puo' invecchiare
/// </code>
/// </summary>
public sealed partial class DataBridge
{
    /// <summary>I tipi che si calcolano da una finestra. Tutto il resto e' <c>fisso</c>.</summary>
    private static readonly HashSet<string> TipiMisurati = new(StringComparer.OrdinalIgnoreCase)
    {
        "poc", "vah", "val", "massimo", "minimo", "nodo_top", "nodo_base", "mensola", "tetto",
        "aggressione", "assorbimento",
    };

    /// <summary>
    /// Punti minimi fra un livello e la sua invalidazione. Sotto, lo stop sta dentro il rumore
    /// e non dietro una struttura. Tarato su NQ; si dichiara per scenario con
    /// <c>stacco_minimo</c>.
    /// </summary>
    private const decimal StaccoInvalidazione = 10m;

    /// <summary>
    /// Punti sotto i quali due livelli si sovrappongono sul chart e il secondo non si disegna.
    /// Due etichette a cinque punti di distanza su NQ diventano illeggibili entrambe: il chart
    /// perde due informazioni invece di guadagnarne una.
    /// </summary>
    private const decimal StaccoMinimoFraLivelli = 8m;

    private static readonly string RulesStorePath = Path.Combine(
        Environment.GetFolderPath(Environment.SpecialFolder.UserProfile),
        ".fabio-data-bridge-rules.json");

    private static readonly object RulesStoreSync = new();

    /// <summary>Le regole depositate dall'analisi. Sostituite per intero, mai modificate sul posto.</summary>
    private volatile BridgeRule[] _rules = Array.Empty<BridgeRule>();

    /// <summary>L'indice dell'ultima barra su cui le regole sono state risolte.</summary>
    private int _regoleAllaBarra = -1;

    /// <summary>L'ora di mercato di quella barra: e' quella che il pannello dichiara.</summary>
    private DateTime _regoleAllOra = DateTime.MinValue;

    /// <summary>
    /// Le regole che non hanno prodotto un livello, con il motivo. Non e' diagnostica: e'
    /// contesto. "POC cash: finestra ancora vuota" dice che la cash non e' aperta, e chi guarda
    /// il chart deve saperlo — altrimenti nota solo che una riga che si aspettava non c'e'.
    /// </summary>
    private volatile string[] _regoleSaltate = Array.Empty<string>();

    // ------------------------------------------------------------------ la regola

    /// <summary>
    /// Una regola: come si trova un prezzo, non quale prezzo e'.
    ///
    /// <para>La finestra si dichiara <b>per regola</b>, perche' finestre diverse misurano
    /// popolazioni diverse: il POC della cash e quello della notte sono due numeri, non due
    /// stime dello stesso numero.</para>
    /// </summary>
    private sealed record BridgeRule
    {
        /// <summary>Il nome con cui la regola viene citata da un bersaglio o da una invalidazione.</summary>
        public string? Nome { get; init; }

        /// <summary>poc | vah | val | massimo | minimo | nodo_top | nodo_base | mensola | aggressione | assorbimento | fisso</summary>
        public string? Tipo { get; init; }

        public BridgeWindow? Finestra { get; init; }

        /// <summary>Solo per <c>fisso</c>: il prezzo, scritto dall'analisi.</summary>
        public decimal? Price { get; init; }

        /// <summary>Modello: <c>{prezzo}</c>, <c>{pct}</c>, <c>{lotti}</c>, <c>{delta}</c>, <c>{tocchi}</c>.</summary>
        public string? Label { get; init; }

        public string? Color { get; init; }

        public string? Style { get; init; }

        public int Width { get; init; } = 1;

        public string? Note { get; init; }

        /// <summary>Conta per la strategia. Senza, lo spessore fa da convenzione.</summary>
        public bool? Chiave { get; init; }

        /// <summary>
        /// Lotti minimi nella finestra perche' la misura valga. Nei primi minuti di cash tutto
        /// il volume sta in una fascia sola e il POC direbbe "89% del volume" perche' non c'e'
        /// altro: non e' una misura prematura, e' una misura falsa.
        /// </summary>
        [JsonPropertyName("minimo_lotti")]
        public decimal MinimoLotti { get; init; }

        /// <summary>La griglia dei nodi, in punti. Default 25.</summary>
        public decimal Passo { get; init; } = 25m;

        /// <summary>La griglia su cui si cercano POC e bordi del valore, in punti. Default 1.</summary>
        public decimal Grana { get; init; } = 1m;

        /// <summary>
        /// Per <c>mensola</c>: quanto lontani possono stare due appoggi e contare come lo stesso.
        /// Per <c>aggressione</c> e <c>assorbimento</c>: la larghezza della fascia esaminata.
        /// </summary>
        public decimal Tolleranza { get; init; } = 2m;

        /// <summary>Per <c>mensola</c>: appoggi minimi perche' sia una mensola e non un minimo.</summary>
        [JsonPropertyName("appoggi_minimi")]
        public int AppoggiMinimi { get; init; } = 3;

        public BridgeCondition[]? Condizioni { get; init; }

        public BridgeRuleScenario? Scenario { get; init; }
    }

    /// <summary>
    /// La finestra di una regola. Un orario nudo (<c>13:30Z</c>) si appoggia al giorno di
    /// mercato; un ISO completo vale cosi' com'e'. Senza <c>A</c> la finestra arriva a adesso,
    /// e allora il livello si muove.
    /// </summary>
    private sealed record BridgeWindow
    {
        public string? Da { get; init; }

        public string? A { get; init; }
    }

    /// <summary>
    /// Lo scenario come lo scrive l'analisi: bersaglio e invalidazione <b>per nome di livello</b>,
    /// non per prezzo. Un bersaglio scritto come numero invecchia — il POC della cash si sposta
    /// a ogni barra — quindi si dichiara il nome e lo risolve la macchina, a ogni giro.
    /// </summary>
    private sealed record BridgeRuleScenario
    {
        public string? Direzione { get; init; }

        public string? Nome { get; init; }

        [JsonPropertyName("bersaglio_livello")]
        public string? BersaglioLivello { get; init; }

        [JsonPropertyName("invalida_livello")]
        public string? InvalidaLivello { get; init; }

        /// <summary>
        /// Il livello su cui si porta lo stop a pareggio. <b>E' il prezzo al quale l'analisi si
        /// smonta</b>, cioe' dove il lato opposto tornerebbe a vincere la battaglia — non una
        /// distanza e non "dopo 1R". <i>"Why I put the break even point at zero is point at 65?
        /// This is where the buyers got completely absorbed. So it's a level where you could
        /// expect to see sellers getting back in"</i> <c>[1 · 2:11:58]</c>.
        ///
        /// <para>Sta qui e non nella testa di chi legge perche' nel live Q1 la gestione <b>e'
        /// l'edge</b>, e il break even ne e' la regola singola piu' importante. Uno stop senza il
        /// suo break even e' meta' istruzione.</para>
        /// </summary>
        [JsonPropertyName("pareggio_livello")]
        public string? PareggioLivello { get; init; }

        [JsonPropertyName("stacco_minimo")]
        public decimal? StaccoMinimo { get; init; }
    }

    private sealed record RulesRequest
    {
        public BridgeRule[]? Rules { get; init; }
    }

    // ------------------------------------------------------------------ il profilo

    /// <summary>Il profilo di una finestra: tutto quello che serve a risolvere ogni tipo.</summary>
    private sealed class ProfiloFinestra
    {
        public decimal Poc;
        public decimal Val;
        public decimal Vah;
        public decimal Massimo;
        public decimal Minimo;
        public decimal NodoBase;
        public decimal NodoTop;
        public decimal Totale;
        public decimal Passo;
        public Dictionary<decimal, decimal> Fasce = new();
        public Dictionary<decimal, decimal> FasceDelta = new();

        /// <summary>Volume e delta per prezzo sulla griglia fine: servono ai tipi da tape.</summary>
        public Dictionary<decimal, decimal> Prezzi = new();
        public Dictionary<decimal, decimal> Delta = new();

        /// <summary>
        /// Quante volte ogni prezzo e' stato il MINIMO di una barra, e quante il MASSIMO.
        ///
        /// <para><b>Separati, e non e' un dettaglio.</b> Contandoli insieme si trova il prezzo
        /// piu' visitato, che dentro un range di mezz'ora e' il suo centro: al primo giro la
        /// regola ha risposto 29.172, il mezzo della mensola 29.142-29.195, invece dei minimi
        /// a 29.142. Un appoggio e' fatto di minimi ripetuti e un tetto di massimi ripetuti;
        /// la loro somma non e' niente.</para>
        /// </summary>
        public Dictionary<decimal, int> AppoggiBassi = new();

        public Dictionary<decimal, int> AppoggiAlti = new();

        /// <summary>
        /// Vero quando la finestra puo' ancora crescere, e quindi il livello si muove.
        ///
        /// <para><b>Non basta guardare se la finestra ha una fine dichiarata.</b> Una finestra
        /// 07:00Z-13:30Z e' chiusa sul calendario, ma alle 13:00Z non e' ancora finita: il suo
        /// POC si sposta a ogni barra fino alle 13:30. Marcarla <c>=</c> perche' "ha una fine"
        /// direbbe fermo di un livello che si muove — la stessa bugia che il motore dentro
        /// l'indicatore esiste per togliere. Visto al primo giro vero, 20 settembre.</para>
        /// </summary>
        public bool Aperta;
    }

    /// <summary>
    /// Il profilo della finestra, dalle barre che l'indicatore ha gia' in memoria.
    ///
    /// <para><b>Due griglie, e vanno tenute distinte.</b> <paramref name="grana"/> e' quella su
    /// cui si cercano POC e bordi del valore. Sul tick nudo non si cercano: il volume di una
    /// notte si spalma su millecinquecento prezzi da un quarto di punto, e il singolo tick piu'
    /// scambiato puo' finire in una zona che non e' affatto il cuore del volume. Il 14 settembre
    /// il POC sul tick dava 29.150 mentre la fascia piu' pesante della notte era 29.300-29.324,
    /// col 18,4% contro il 13,7%. <paramref name="passo"/> e' la griglia dei nodi.</para>
    ///
    /// <para><b>La barra in formazione non entra mai.</b> Una misura presa su una barra aperta
    /// cambia da sola fra un tick e il successivo, e il chart mostrerebbe un livello che balla
    /// senza che il mercato abbia fatto niente.</para>
    /// </summary>
    private ProfiloFinestra? Profilo(DateTime da, DateTime? a, decimal passo, decimal grana)
    {
        var ultimo = CurrentBar - 2; // la barra in formazione e' CurrentBar - 1: esclusa sempre
        if (ultimo < 0)
        {
            return null;
        }

        // La finestra e' aperta se non ha una fine, oppure se la fine e' ancora davanti
        // all'ultima barra chiusa: in entrambi i casi il prossimo minuto puo' spostare la misura.
        var ultimaChiusa = GetCandle(ultimo)?.Time ?? DateTime.MinValue;
        var p = new ProfiloFinestra { Passo = passo, Aperta = a is null || a.Value > ultimaChiusa };
        var fine = a ?? DateTime.MaxValue;
        decimal massimo = decimal.MinValue;
        decimal minimo = decimal.MaxValue;
        var barre = 0;

        for (var bar = 0; bar <= ultimo; bar++)
        {
            var candle = GetCandle(bar);
            if (candle is null || candle.Time < da)
            {
                continue;
            }

            if (candle.Time >= fine)
            {
                break;
            }

            if (candle.Volume <= 0)
            {
                continue;
            }

            barre++;
            massimo = Math.Max(massimo, candle.High);
            minimo = Math.Min(minimo, candle.Low);

            Conta(p.AppoggiAlti, Math.Floor(candle.High / grana) * grana);
            Conta(p.AppoggiBassi, Math.Floor(candle.Low / grana) * grana);

            foreach (var livello in candle.GetAllPriceLevels())
            {
                var prezzo = Math.Floor(livello.Price / grana) * grana;
                p.Prezzi[prezzo] = p.Prezzi.GetValueOrDefault(prezzo) + livello.Volume;
                p.Delta[prezzo] = p.Delta.GetValueOrDefault(prezzo) + (livello.Ask - livello.Bid);
            }
        }

        if (barre == 0 || p.Prezzi.Count == 0)
        {
            return null;
        }

        p.Massimo = massimo;
        p.Minimo = minimo;
        p.Totale = p.Prezzi.Values.Sum();

        // --- POC e bordi del valore: si parte dal prezzo piu' scambiato e si allarga verso il
        //     lato piu' pesante finche' dentro non c'e' il 70% del volume della finestra.
        var ordinati = p.Prezzi.Keys.OrderBy(x => x).ToList();
        p.Poc = p.Prezzi.OrderByDescending(kv => kv.Value).First().Key;
        var i = ordinati.IndexOf(p.Poc);
        int basso = i, alto = i;
        var dentro = p.Prezzi[p.Poc];
        while (dentro < 0.7m * p.Totale && (basso > 0 || alto < ordinati.Count - 1))
        {
            var giu = basso > 0 ? p.Prezzi[ordinati[basso - 1]] : -1m;
            var su = alto < ordinati.Count - 1 ? p.Prezzi[ordinati[alto + 1]] : -1m;
            if (su >= giu)
            {
                alto++;
                dentro += su;
            }
            else
            {
                basso--;
                dentro += giu;
            }
        }

        p.Val = ordinati[basso];
        p.Vah = ordinati[alto];

        foreach (var (prezzo, volume) in p.Prezzi)
        {
            var fascia = Math.Floor(prezzo / passo) * passo;
            p.Fasce[fascia] = p.Fasce.GetValueOrDefault(fascia) + volume;
            p.FasceDelta[fascia] = p.FasceDelta.GetValueOrDefault(fascia) + p.Delta.GetValueOrDefault(prezzo);
        }

        p.NodoBase = p.Fasce.OrderByDescending(kv => kv.Value).First().Key;
        p.NodoTop = p.NodoBase + passo - (InstrumentInfo?.TickSize ?? 0.25m);
        return p;
    }

    private static void Conta(Dictionary<decimal, int> dove, decimal chiave)
        => dove[chiave] = dove.GetValueOrDefault(chiave) + 1;

    // ------------------------------------------------------------------ la risoluzione

    /// <summary>
    /// Trasforma le regole in livelli e li mette sul chart. Si chiama a ogni barra nuova, e
    /// dopo ogni deposito di regole.
    /// </summary>
    private void RisolviRegole()
    {
        var regole = _rules;
        if (regole.Length == 0)
        {
            return;
        }

        var ultimo = CurrentBar - 1;
        if (ultimo < 1)
        {
            return;
        }

        var adesso = GetCandle(ultimo).Time;
        var giorno = adesso.Date;
        var cache = new Dictionary<string, ProfiloFinestra?>(StringComparer.Ordinal);
        var saltate = new List<string>();
        var risolti = new List<BridgeLevel>();
        var prezzoDi = new Dictionary<string, decimal>(StringComparer.OrdinalIgnoreCase);
        var scenariDi = new Dictionary<string, BridgeRuleScenario>(StringComparer.Ordinal);

        foreach (var regola in regole)
        {
            var livello = Risolvi(regola, giorno, cache, saltate);
            if (livello is null)
            {
                continue;
            }

            risolti.Add(livello);
            if (!string.IsNullOrWhiteSpace(regola.Nome))
            {
                prezzoDi[regola.Nome!] = livello.Price;
                if (regola.Scenario is not null)
                {
                    scenariDi[regola.Nome!] = regola.Scenario;
                }
            }
        }

        // --- secondo passaggio: i nomi diventano prezzi ---------------------------------------
        // Si fa DOPO aver risolto tutto, perche' uno scenario puo' puntare a un livello
        // dichiarato piu' sotto nel file.
        for (var k = 0; k < risolti.Count; k++)
        {
            var livello = risolti[k];
            if (livello.Nome is null || !scenariDi.TryGetValue(livello.Nome, out var sc))
            {
                continue;
            }

            decimal? bersaglio = null;
            decimal? invalida = null;

            if (!string.IsNullOrWhiteSpace(sc.BersaglioLivello))
            {
                if (prezzoDi.TryGetValue(sc.BersaglioLivello!, out var t))
                {
                    bersaglio = t;
                }
                else
                {
                    // Un bersaglio che non si risolve non si inventa: si toglie e si dichiara.
                    saltate.Add($"{livello.Nome}: bersaglio «{sc.BersaglioLivello}» non risolto");
                }
            }

            if (!string.IsNullOrWhiteSpace(sc.InvalidaLivello)
                && prezzoDi.TryGetValue(sc.InvalidaLivello!, out var inv))
            {
                // UNA INVALIDAZIONE TROPPO VICINA NON E' UNA INVALIDAZIONE. Uno stop a un tick
                // dall'ingresso lo prende il rumore, non il mercato, e sul pannello quel numero
                // e' peggio di un campo vuoto perche' sembra una misura.
                var stacco = sc.StaccoMinimo ?? StaccoInvalidazione;
                if (Math.Abs(inv - livello.Price) >= stacco)
                {
                    invalida = inv;
                }
                else
                {
                    saltate.Add($"{livello.Nome}: invalidazione a "
                                + $"{Math.Abs(inv - livello.Price).ToString("0.##", Italiano)} punti, "
                                + $"sotto i {stacco.ToString("0.##", Italiano)} richiesti");
                }
            }

            // IL PAREGGIO SI RISOLVE COME GLI ALTRI, MA NON HA LO STACCO MINIMO. Il break even
            // di Fabio e' deliberatamente vicino - "now you understand why my break even point
            // was so close" [3 · 15:39] - perche' non e' uno stop: se viene toccato non si perde
            // niente, si restituisce il tentativo. Applicargli la difesa dell'invalidazione lo
            // cancellerebbe proprio quando e' fatto bene.
            decimal? pareggio = null;
            if (!string.IsNullOrWhiteSpace(sc.PareggioLivello))
            {
                if (prezzoDi.TryGetValue(sc.PareggioLivello!, out var pg))
                {
                    pareggio = pg;
                }
                else
                {
                    saltate.Add($"{livello.Nome}: pareggio «{sc.PareggioLivello}» non risolto");
                }
            }

            risolti[k] = livello with
            {
                Scenario = new BridgeScenario
                {
                    Direzione = sc.Direzione,
                    Nome = sc.Nome,
                    Bersaglio = bersaglio,
                    Invalida = invalida,
                    Pareggio = pareggio,
                },
            };
        }

        // --- si sfoltisce: due etichette sovrapposte sono due informazioni perse ---------------
        // Si tiene il primo in ordine di dichiarazione, perche' l'ordine nel file e' la priorita'
        // scelta dall'analisi, e si dice cosa e' caduto: un livello che sparisce senza dirlo e'
        // peggio di uno di troppo.
        var tenuti = new List<BridgeLevel>();
        foreach (var livello in risolti)
        {
            var vicino = tenuti.FirstOrDefault(t => Math.Abs(t.Price - livello.Price) < StaccoMinimoFraLivelli);
            if (vicino is not null)
            {
                // Il motivo deve dire CHI ha vinto e di quanto: "coincide con 29.172" obbligava
                // chi legge il pannello a cercarsi a mano quale livello fosse quel prezzo.
                saltate.Add(
                    $"{livello.Nome} {Prezzo(livello.Price)}: a "
                    + $"{Prezzo(Math.Abs(vicino.Price - livello.Price))} punti da «{vicino.Nome}», "
                    + $"che e' dichiarato prima (stacco minimo {StaccoMinimoFraLivelli})");
                continue;
            }

            tenuti.Add(livello);
        }

        _levels = tenuti.OrderByDescending(l => l.Price).ToArray();
        _levelsUpdatedUtc = DateTime.UtcNow;
        _regoleAllaBarra = ultimo;
        _regoleAllOra = adesso;
        _regoleSaltate = saltate.ToArray();
        PersistLevels();
    }

    /// <summary>Una regola diventa un livello, oppure niente e un motivo.</summary>
    private BridgeLevel? Risolvi(
        BridgeRule regola, DateTime giorno,
        Dictionary<string, ProfiloFinestra?> cache, List<string> saltate)
    {
        var nome = regola.Nome ?? regola.Tipo ?? "senza nome";
        var tipo = (regola.Tipo ?? "fisso").ToLowerInvariant();
        decimal prezzo;
        string pct = string.Empty, lotti = string.Empty, delta = string.Empty, tocchi = string.Empty;
        char marcatore;

        if (tipo == "fisso")
        {
            if (regola.Price is not { } dichiarato || dichiarato <= 0)
            {
                saltate.Add($"{nome}: regola «fisso» senza prezzo");
                return null;
            }

            prezzo = dichiarato;
            marcatore = '*';
        }
        else
        {
            if (!TipiMisurati.Contains(tipo))
            {
                saltate.Add($"{nome}: tipo «{tipo}» sconosciuto");
                return null;
            }

            if (regola.Finestra?.Da is null)
            {
                saltate.Add($"{nome}: manca la finestra");
                return null;
            }

            var da = Momento(regola.Finestra.Da!, giorno);
            var a = regola.Finestra.A is null ? (DateTime?)null : Momento(regola.Finestra.A!, giorno);
            // Una finestra che finisce prima di cominciare attraversa la mezzanotte: e' il caso
            // normale della notte, dichiarata come 22:00Z -> 07:00Z. Senza questo l'inizio
            // cadrebbe stasera invece che ieri sera, e la finestra sarebbe vuota.
            if (a is not null && a <= da)
            {
                da = da.AddDays(-1);
            }

            var chiave = $"{da:O}|{a:O}|{regola.Passo}|{regola.Grana}";
            if (!cache.TryGetValue(chiave, out var profilo))
            {
                profilo = Profilo(da, a, regola.Passo, regola.Grana);
                cache[chiave] = profilo;
            }

            if (profilo is null)
            {
                saltate.Add($"{nome}: finestra ancora vuota");
                return null;
            }

            if (profilo.Totale < regola.MinimoLotti)
            {
                saltate.Add($"{nome}: {Lotti(profilo.Totale)} lotti su "
                            + $"{Lotti(regola.MinimoLotti)} richiesti, ancora presto");
                return null;
            }

            var trovato = Trova(tipo, regola, profilo, out var appoggi);
            if (trovato is null)
            {
                saltate.Add($"{nome}: «{tipo}» non trovato nella finestra");
                return null;
            }

            prezzo = trovato.Value;
            marcatore = profilo.Aperta ? '~' : '=';

            // Il peso che l'etichetta puo' onestamente rivendicare e' quello della fascia in cui
            // il prezzo cade, non del singolo tick.
            var fascia = Math.Floor(prezzo / profilo.Passo) * profilo.Passo;
            var volumeFascia = profilo.Fasce.GetValueOrDefault(fascia);
            var deltaFascia = profilo.FasceDelta.GetValueOrDefault(fascia);
            pct = profilo.Totale > 0
                ? (100m * volumeFascia / profilo.Totale).ToString("0.0", Italiano) + "%"
                : string.Empty;
            lotti = Lotti(volumeFascia);
            delta = Segnato(deltaFascia);
            tocchi = appoggi.ToString(CultureInfo.InvariantCulture);
        }

        var tick = InstrumentInfo?.TickSize ?? 0.25m;
        prezzo = Math.Round(prezzo / tick, MidpointRounding.AwayFromZero) * tick;

        var etichetta = (regola.Label ?? nome)
            .Replace("{prezzo}", Prezzo(prezzo), StringComparison.Ordinal)
            .Replace("{pct}", pct, StringComparison.Ordinal)
            .Replace("{lotti}", lotti, StringComparison.Ordinal)
            .Replace("{delta}", delta, StringComparison.Ordinal)
            .Replace("{tocchi}", tocchi, StringComparison.Ordinal);

        // Il marcatore va in coda al NOME, prima del separatore, non in fondo alla riga: in fondo
        // finirebbe dopo la frase che dice a cosa serve il livello, dove nessuno lo cerca.
        var taglio = etichetta.IndexOf(" · ", StringComparison.Ordinal);
        etichetta = taglio < 0
            ? $"{etichetta} {marcatore}"
            : $"{etichetta[..taglio]} {marcatore}{etichetta[taglio..]}";

        // L'AREA SERVE A DIPINGERE LA BANDA DEL VALORE, e da una lista di prezzi non si deduce:
        // per sapere che due bordi appartengono alla stessa area bisogna che lo dica qualcuno.
        // Lo dice il nome della regola, che e' gia' scritto cosi': "VAL Europa", "POC cash".
        var area = string.Empty;
        if (tipo is "poc" or "vah" or "val")
        {
            area = (regola.Nome ?? string.Empty)
                .Replace("VAL", string.Empty, StringComparison.Ordinal)
                .Replace("VAH", string.Empty, StringComparison.Ordinal)
                .Replace("POC", string.Empty, StringComparison.Ordinal)
                .Trim();
        }

        return new BridgeLevel
        {
            Price = prezzo,
            Label = etichetta,
            Color = regola.Color ?? "#7A8FA6",
            Style = regola.Style ?? "solid",
            Width = regola.Width,
            Note = regola.Note,
            Role = tipo,
            Area = area,
            Key = regola.Chiave ?? regola.Width >= 2,
            Conditions = regola.Condizioni,
            Nome = regola.Nome,
            Marcatore = marcatore.ToString(),
        };
    }

    /// <summary>Il prezzo che il tipo indica dentro il profilo.</summary>
    private decimal? Trova(string tipo, BridgeRule regola, ProfiloFinestra p, out int appoggi)
    {
        appoggi = 0;
        switch (tipo)
        {
            case "poc": return p.Poc;
            case "val": return p.Val;
            case "vah": return p.Vah;
            case "massimo": return p.Massimo;
            case "minimo": return p.Minimo;
            case "nodo_base": return p.NodoBase;
            case "nodo_top": return p.NodoTop;

            // LA MENSOLA (minimi ripetuti) E IL TETTO (massimi ripetuti).
            //
            // Nel live e' una delle due cose che vale la pena marcare, perche' nasce da ordini
            // eseguiti: se sei minimi cadono sullo stesso prezzo, li' sotto c'e' qualcuno che
            // compra, e non e' un'opinione. Finora la scriveva l'analisi a mano — "difesa sei
            // volte 12:36-13:02" — e il 20 settembre quella frase e' rimasta sul chart con un
            // conteggio sbagliato E un esito che non era ancora successo. Contarli e' lavoro da
            // macchina.
            case "mensola":
            case "tetto":
            {
                var candidati = (tipo == "mensola" ? p.AppoggiBassi : p.AppoggiAlti)
                    .Where(kv => kv.Value >= regola.AppoggiMinimi)
                    .OrderByDescending(kv => kv.Value)
                    .ThenByDescending(kv => p.Prezzi.GetValueOrDefault(kv.Key))
                    .ToList();
                if (candidati.Count == 0)
                {
                    return null;
                }

                appoggi = candidati[0].Value;
                return candidati[0].Key;
            }

            // LA MASSIMA AGGRESSIONE: il prezzo col delta piu' grande in valore assoluto.
            // Chi ha preso il mercato, e dove. E' il primo dei due livelli che il live dice di
            // marcare: *"it's not necessary to mark intermediate level that are useless for us"*.
            case "aggressione":
            {
                if (p.Delta.Count == 0)
                {
                    return null;
                }

                return p.Delta.OrderByDescending(kv => Math.Abs(kv.Value)).First().Key;
            }

            // IL MASSIMO ASSORBIMENTO: molto scambiato, delta quasi nullo. Sforzo alto, risultato
            // nullo — e' la coppia con cui il metodo legge l'assorbimento. Si cerca fra i prezzi
            // piu' scambiati quello dove il delta ha pesato di meno rispetto al volume: tanti
            // contratti, nessun vincitore, cioe' il lato passivo che ha retto.
            case "assorbimento":
            {
                var soglia = p.Prezzi.Values.DefaultIfEmpty(0m).Max() * 0.35m;
                var forti = p.Prezzi.Where(kv => kv.Value >= soglia).ToList();
                if (forti.Count == 0)
                {
                    return null;
                }

                return forti
                    .OrderBy(kv => Math.Abs(p.Delta.GetValueOrDefault(kv.Key)) / Math.Max(kv.Value, 1m))
                    .ThenByDescending(kv => kv.Value)
                    .First().Key;
            }

            default: return null;
        }
    }

    /// <summary>
    /// Un momento della finestra. <c>13:30Z</c> e' un orario del giorno di mercato;
    /// un ISO completo vale cosi' com'e'.
    /// </summary>
    private static DateTime Momento(string grezzo, DateTime giorno)
    {
        var testo = grezzo.Trim();
        if (testo.Length <= 6 && testo.Contains(':', StringComparison.Ordinal))
        {
            var parti = testo.TrimEnd('Z').Split(':');
            return giorno.Date
                .AddHours(int.Parse(parti[0], CultureInfo.InvariantCulture))
                .AddMinutes(parti.Length > 1 ? int.Parse(parti[1], CultureInfo.InvariantCulture) : 0);
        }

        return DateTime.Parse(testo, CultureInfo.InvariantCulture,
            DateTimeStyles.AdjustToUniversal | DateTimeStyles.AssumeUniversal);
    }

    // ------------------------------------------------------------------ l'endpoint

    private async Task<object> RulesAsync(HttpListenerContext context)
    {
        var method = context.Request.HttpMethod.ToUpperInvariant();

        if (method is "DELETE")
        {
            _rules = Array.Empty<BridgeRule>();
            _regoleSaltate = Array.Empty<string>();
            _regoleAllaBarra = -1;
            PersistRules();
            _levels = Array.Empty<BridgeLevel>();
            _levelsUpdatedUtc = DateTime.UtcNow;
            PersistLevels();
            Repaint();
            return RulesPayload();
        }

        if (method is "POST" or "PUT")
        {
            using var reader = new StreamReader(context.Request.InputStream, Encoding.UTF8);
            var body = await reader.ReadToEndAsync().ConfigureAwait(false);
            if (string.IsNullOrWhiteSpace(body))
            {
                throw new BridgeException(400, "empty body: send a JSON array of rules, or {\"rules\": [...]}");
            }

            BridgeRule[]? parsed;
            try
            {
                var trimmed = body.TrimStart();
                parsed = trimmed.StartsWith('[')
                    ? JsonSerializer.Deserialize<BridgeRule[]>(body, JsonOptions)
                    : JsonSerializer.Deserialize<RulesRequest>(body, JsonOptions)?.Rules;
            }
            catch (JsonException exception)
            {
                throw new BridgeException(400, $"malformed JSON: {exception.Message}");
            }

            if (parsed is null)
            {
                throw new BridgeException(400, "no rules in the request");
            }

            _rules = parsed;
            PersistRules();
            RisolviRegole();
            Repaint();
            return RulesPayload();
        }

        return RulesPayload();
    }

    private object RulesPayload() => new
    {
        schema = Schema,
        chart = _id,
        instrument = InstrumentInfo?.Instrument,
        count = _rules.Length,
        resolvedAtBar = _regoleAllaBarra,
        resolvedAtMarketTime = _regoleAllOra == DateTime.MinValue ? null : Iso(_regoleAllOra),
        levels = _levels.Length,
        skipped = _regoleSaltate,
        rules = _rules,
    };

    private void RestoreRules()
    {
        var instrument = InstrumentInfo?.Instrument;
        if (string.IsNullOrWhiteSpace(instrument))
        {
            return;
        }

        try
        {
            var store = ReadRulesStore();
            if (store.TryGetValue(instrument, out var saved) && saved.Length > 0)
            {
                _rules = saved;
            }
        }
        catch (Exception exception)
        {
            this.LogError("FofDataBridge could not restore the saved rules.", exception);
        }
    }

    private static Dictionary<string, BridgeRule[]> ReadRulesStore()
    {
        if (!File.Exists(RulesStorePath))
        {
            return new Dictionary<string, BridgeRule[]>(StringComparer.OrdinalIgnoreCase);
        }

        var json = File.ReadAllText(RulesStorePath);
        return JsonSerializer.Deserialize<Dictionary<string, BridgeRule[]>>(json, JsonOptions)
               ?? new Dictionary<string, BridgeRule[]>(StringComparer.OrdinalIgnoreCase);
    }

    private void PersistRules()
    {
        var instrument = InstrumentInfo?.Instrument;
        if (string.IsNullOrWhiteSpace(instrument))
        {
            return;
        }

        try
        {
            lock (RulesStoreSync)
            {
                var store = ReadRulesStore();
                if (_rules.Length == 0)
                {
                    store.Remove(instrument);
                }
                else
                {
                    store[instrument] = _rules;
                }

                File.WriteAllText(RulesStorePath, JsonSerializer.Serialize(store, JsonOptions));
            }
        }
        catch (Exception exception)
        {
            this.LogError("FofDataBridge could not save the rules.", exception);
        }
    }
}
