using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using ATAS.DataFeedsCore;
using System.Threading;
using System.Threading.Tasks;
using ATAS.Indicators;
using Utils.Common.Logging;

namespace FabioOrderFlow.Observation;

/// <summary>
/// Il regime, la velocita' del tape e i big trades: le tre cose che nel live Q1 vengono
/// <b>prima</b> del setup e che fino al 20 settembre 2026 il chart non diceva affatto.
///
/// <para><b>Perche' stanno nell'indicatore e non nella testa dell'agente.</b> Fabio, a chi gli
/// chiede da dove cominciare, riduce tutta la sequenza a due cose: <i>"I would start from
/// sensitive level of the market like we are doing together, and regime"</i> <c>[5 · 1:18:06]</c>.
/// I livelli li calcola gia' il motore delle regole. Il regime no: era dichiarato a mano nel file
/// della giornata, e da li' restava fermo finche' qualcuno non lo riscriveva — cioe' esattamente
/// il difetto del livello vivo fermo, spostato su una parola invece che su un prezzo.</para>
///
/// <para><b>Sono misure di questo repository, non del corso.</b> Il live riconosce il regime a
/// occhio e lo dice a parole. Qui serve un numero, e allora ogni soglia e' dichiarata accanto al
/// numero che produce, nel pannello e in questo file. Le righe del live che le giustificano sono
/// citate una per una; dove la misura aggiunge qualcosa che nel live non c'e', lo dice.</para>
/// </summary>
public sealed partial class DataBridge
{
    // ------------------------------------------------------------------ le impostazioni

    [Display(Name = "Finestra del regime", GroupName = "Regime",
        Description = "Su quante barre chiuse si misura il regime. Su M1, 30 e' mezz'ora.")]
    [Range(10, 600)]
    public int FinestraRegime { get; set; } = 30;

    [Display(Name = "Candele coperte", GroupName = "Regime",
        Description = "Quante candele precedenti una sola deve coprire perche' la giornata sia " +
                      "direzionale. Cinque e' la riga del live [6 · 1:19:04].")]
    [Range(2, 20)]
    public int CandeleCoperte { get; set; } = 5;

    [Display(Name = "Finestra della velocita'", GroupName = "Regime",
        Description = "Su quante barre chiuse si costruisce la distribuzione del volume contro " +
                      "cui si misura la velocita'. E' un PROXY della speed of tape, non la speed.")]
    [Range(20, 600)]
    public int FinestraVelocita { get; set; } = 60;

    [Display(Name = "Soglia big trade", GroupName = "Regime",
        Description = "Lotti da cui un ordine aggressivo e' un big trade. Taratura di Fabio: " +
                      "60 su NQ in cash, 20-30 in premarket e Londra [5 · 6:04].")]
    [Range(1, 10000)]
    public int SogliaBigTrade { get; set; } = 60;

    /// <summary>
    /// Efficienza da cui in su la finestra e' direzionale. E' il rapporto fra quanto il prezzo si
    /// e' spostato NETTO e quanta strada ha fatto in totale: un'ora che sale di cento punti
    /// facendone duecento e' direzionale, una che ne fa duecento per tornare al punto di partenza
    /// non lo e', e i due casi hanno lo stesso range.
    /// </summary>
    private const decimal EfficienzaDirezionale = 0.35m;

    /// <summary>Efficienza sotto la quale la finestra e' choppy, qualunque cosa faccia il range.</summary>
    private const decimal EfficienzaChoppy = 0.15m;

    /// <summary>Rotture rientrate da cui in su la finestra e' choppy. *"They go from one side of the auction to the other side"* <c>[6 · 54:03]</c>.</summary>
    private const int RientriChoppy = 3;

    /// <summary>Percentile del volume da cui in su la velocita' e' alta, e sotto il quale e' morta.</summary>
    private const int PercentileAlto = 70;

    private const int PercentileMorto = 30;

    /// <summary>Per quanti minuti si tiene un big trade nel registro del tape.</summary>
    private const int MinutiDiTape = 90;

    // ------------------------------------------------------------------ il regime

    /// <summary>Cosa dice il regime, con i numeri che lo dicono. Mai solo l'etichetta.</summary>
    private sealed record LetturaDelRegime
    {
        public string Nome { get; init; } = "non misurabile";

        /// <summary>La frase con i numeri: e' quella che finisce nel pannello sotto il nome.</summary>
        public string Perche { get; init; } = string.Empty;

        /// <summary>Cosa impone alla size. E' il motivo per cui il regime sta in cima e non in fondo.</summary>
        public string Size { get; init; } = string.Empty;

        public decimal Efficienza { get; init; }

        public int Rientri { get; init; }

        public bool Copertura { get; init; }

        public bool Misurabile { get; init; }
    }

    /// <summary>
    /// Il regime delle ultime <see cref="FinestraRegime"/> barre chiuse.
    ///
    /// <para>Tre misure, e ciascuna risponde a una riga del live.</para>
    /// <code>
    ///   copertura    una candela copre piu' di CandeleCoperte candele del range precedente
    ///                "when you are directional, one candle cover more than five candle of the
    ///                 previous range"                                      [6 · 1:19:04]
    ///   efficienza   spostamento netto diviso strada percorsa. MISURA NOSTRA: nel live la
    ///                distinzione fra "va da qualche parte" e "balla" e' a occhio
    ///   rientri      rotture degli estremi recenti che rientrano entro tre barre
    ///                "they go from one side of the auction to the other side"   [6 · 54:03]
    /// </code>
    ///
    /// <para><b>Le tre non si sommano, si ordinano.</b> Una candela che copre cinque candele
    /// dentro una finestra che torna al punto di partenza non e' una giornata direzionale: e' uno
    /// strappo dentro il chop, ed e' il caso in cui Fabio dice di aspettarsi il chop proprio dopo
    /// l'esplosione — <i>"usually what you see is profit release and then you start like this for
    /// hours and hours"</i> <c>[4 · 1:05]</c>. Per questo la copertura da sola non basta: serve
    /// anche l'efficienza.</para>
    /// </summary>
    private LetturaDelRegime LeggiIlRegime(int ultimo)
    {
        var da = ultimo - FinestraRegime + 1;
        if (ultimo < 1 || da < CandeleCoperte)
        {
            return new LetturaDelRegime
            {
                Nome = "non misurabile",
                Perche = $"servono {FinestraRegime + CandeleCoperte} barre chiuse, ce ne sono {ultimo + 1}",
            };
        }

        decimal strada = 0m;
        decimal alto = decimal.MinValue;
        decimal basso = decimal.MaxValue;
        var copertura = false;
        var rientri = 0;

        for (var bar = da; bar <= ultimo; bar++)
        {
            var c = GetCandle(bar);
            if (c is null)
            {
                continue;
            }

            strada += c.High - c.Low;
            alto = Math.Max(alto, c.High);
            basso = Math.Min(basso, c.Low);

            // La copertura: questa candela contro il range delle CandeleCoperte precedenti.
            decimal altoPrima = decimal.MinValue;
            decimal bassoPrima = decimal.MaxValue;
            for (var p = bar - CandeleCoperte; p < bar; p++)
            {
                var q = GetCandle(p);
                if (q is null)
                {
                    continue;
                }
                altoPrima = Math.Max(altoPrima, q.High);
                bassoPrima = Math.Min(bassoPrima, q.Low);
            }

            if (altoPrima > decimal.MinValue && c.High - c.Low > altoPrima - bassoPrima)
            {
                copertura = true;
            }

            // La rottura rientrata: chiude oltre l'estremo delle CandeleCoperte precedenti, e
            // entro tre barre chiude di nuovo dentro. E' il "da una parte all'altra dell'asta".
            if (altoPrima > decimal.MinValue && bar + 3 <= ultimo)
            {
                var sopra = c.Close > altoPrima;
                var sotto = c.Close < bassoPrima;
                if (sopra || sotto)
                {
                    for (var d = bar + 1; d <= Math.Min(ultimo, bar + 3); d++)
                    {
                        var r = GetCandle(d);
                        if (r is null)
                        {
                            continue;
                        }
                        if ((sopra && r.Close < altoPrima) || (sotto && r.Close > bassoPrima))
                        {
                            rientri++;
                            break;
                        }
                    }
                }
            }
        }

        var primo = GetCandle(da);
        var netto = primo is null ? 0m : Math.Abs(GetCandle(ultimo).Close - primo.Open);
        var efficienza = strada > 0 ? netto / strada : 0m;

        // L'ORDINE CONTA: choppy ha la precedenza, perche' e' il regime in cui l'errore costa di
        // piu'. Una giornata tossica presa per direzionale fa aprire con size piena su rotture
        // che rientrano, ed e' il modo documentato di restituire una giornata alle commissioni
        // [4 · 1:22:54]. Il contrario - una direzionale presa per choppy - fa solo perdere un
        // treno, e un treno perso non toglie soldi dal conto.
        string nome, size;
        if (efficienza < EfficienzaChoppy || rientri >= RientriChoppy)
        {
            nome = "CHOPPY";
            size = "size ridotta, si rubano 1.000-2.000, due stop e si chiude";
        }
        else if (copertura && efficienza >= EfficienzaDirezionale)
        {
            nome = "DIREZIONALE";
            size = "trend following: si fa il fade del ritracciamento nel verso del giorno, size piena";
        }
        else
        {
            nome = "BALANCE";
            size = "mean reverting sui bordi verso il POC, meta' size";
        }

        return new LetturaDelRegime
        {
            Nome = nome,
            Perche = $"efficienza {efficienza:0.00} (netto {Prezzo(netto)} su strada {Prezzo(strada)}), "
                     + $"{rientri} rotture rientrate, copertura {(copertura ? "si" : "no")}",
            Size = size,
            Efficienza = efficienza,
            Rientri = rientri,
            Copertura = copertura,
            Misurabile = true,
        };
    }

    // ------------------------------------------------------------------ la velocita'

    /// <summary>
    /// Il <b>proxy</b> della speed of tape, e la parola proxy non e' una cautela: e' il nome
    /// giusto della cosa.
    ///
    /// <para>La speed of tape e' <i>"how fast order are being inputs"</i> <c>[3 · 1:08:41]</c> —
    /// il ritmo con cui gli ordini entrano. Il bridge non ha quel dato. Quello che c'e' e' il
    /// volume della barra, e quanto sta in alto nella distribuzione delle ultime
    /// <see cref="FinestraVelocita"/>. Sono due cose diverse: mille lotti in dieci secondi e
    /// mille lotti in sessanta danno lo stesso volume e velocita' opposte.</para>
    ///
    /// <para>Serve comunque, e serve soprattutto <b>a non entrare</b>: <i>"It's keeping you out of
    /// useless moment in the market. You are not participating in moments where you could have
    /// only taken stop-loss"</i> <c>[1 · 57:47]</c>. Un veto su un proxy resta un veto onesto; un
    /// permesso su un proxy no, ed e' per questo che entra nei veti e non nei prerequisiti.</para>
    /// </summary>
    private (int Percentile, decimal Volume, bool Misurabile) VelocitaProxy(int ultimo)
    {
        var da = ultimo - FinestraVelocita + 1;
        if (ultimo < 5 || da < 0)
        {
            return (0, 0m, false);
        }

        var viva = GetCandle(ultimo);
        if (viva is null || viva.Volume <= 0)
        {
            return (0, 0m, false);
        }

        var sotto = 0;
        var conta = 0;
        for (var bar = da; bar < ultimo; bar++)
        {
            var c = GetCandle(bar);
            if (c is null || c.Volume <= 0)
            {
                continue;
            }
            conta++;
            if (c.Volume < viva.Volume)
            {
                sotto++;
            }
        }

        return conta < 10
            ? (0, viva.Volume, false)
            : ((int)Math.Round(sotto * 100.0 / conta), viva.Volume, true);
    }

    // ------------------------------------------------------------------ i big trades

    /// <summary>Un ordine aggressivo sopra la soglia, come il tape lo ha consegnato.</summary>
    private sealed class BigTrade
    {
        public DateTime Time { get; set; }

        public decimal Price { get; set; }

        public decimal Volume { get; set; }

        public bool Compra { get; set; }

        /// <summary>
        /// L'oggetto del tape da cui viene, quando viene dal vivo. Serve a ritrovare la sua
        /// posizione dopo un riordino: senza, un trade ancora in crescita perde il suo indice e
        /// al prossimo aggiornamento si duplica.
        /// </summary>
        public CumulativeTrade? Sorgente { get; set; }
    }

    private readonly object _tapeSync = new();

    private readonly List<BigTrade> _bigTrades = new();

    /// <summary>
    /// L'indice di un trade gia' visto. Il tape consegna lo stesso trade piu' volte mentre
    /// cresce (<c>OnUpdateCumulativeTrade</c>): senza questo, un solo ordine da 300 lotti
    /// comparirebbe come sei ordini distinti e il muro sembrerebbe sei volte piu' spesso.
    /// </summary>
    private readonly Dictionary<CumulativeTrade, int> _tapeVisti = new();

    /// <summary>
    /// Se il tape ha mai parlato. <b>Zero big trades e nessun dato sono due cose diverse</b>, e
    /// il pannello deve poterle distinguere: "libro sottile" e' un'informazione, "non mi arriva
    /// il tape" e' un guasto, e un pannello che li stampa uguali mente.
    /// </summary>
    private volatile bool _tapeHaParlato;

    /// <summary>
    /// Da quando il registro del tape e' completo. <b>E non e' un dettaglio: e' il difetto che
    /// questa misura ha per costruzione.</b>
    ///
    /// <para><c>OnCumulativeTrade</c> comincia a parlare quando l'indicatore si carica, quindi
    /// appena dopo un deploy il registro e' vuoto mentre il mercato ha appena scambiato. Il
    /// 20 settembre 2026, subito dopo una ricarica, il pannello avrebbe scritto "nessun ordine da
    /// 60+ lotti" su una finestra che ne conteneva tre - uno da 75 in acquisto e due in vendita
    /// da 90 e 61. Sarebbe stata una bugia con l'aria di una misura.</para>
    ///
    /// <para>Due difese, e servono entrambe: il registro si <b>semina</b> all'avvio con una
    /// richiesta storica, e ogni risposta dichiara <b>da quando</b> copre. Se la finestra chiesta
    /// comincia prima, la risposta non e' un numero: e' "non lo so per tutta la finestra".</para>
    /// </summary>
    private DateTime? _tapeDa;

    /// <summary>Perche' la semina si tenta una volta sola, riuscita o no.</summary>
    private int _tapeSeminato;

    protected override void OnCumulativeTrade(CumulativeTrade trade) => RegistraBigTrade(trade);

    protected override void OnUpdateCumulativeTrade(CumulativeTrade trade) => RegistraBigTrade(trade);

    private void RegistraBigTrade(CumulativeTrade trade)
    {
        if (trade is null)
        {
            return;
        }

        _tapeHaParlato = true;
        lock (_tapeSync)
        {
            // Il primo trade vivo e' il confine della copertura, a meno che la semina non abbia
            // gia' portato indietro il registro piu' lontano di cosi'.
            if (_tapeDa is null || trade.Time < _tapeDa.Value)
            {
                _tapeDa = trade.Time;
            }
        }

        if (trade.Volume < SogliaBig)
        {
            // Puo' esserci gia' stato sopra soglia e poi... no: un cumulative trade solo cresce.
            // Se e' sotto soglia adesso, sotto soglia era anche prima, e non c'e' niente da
            // togliere.
            return;
        }

        lock (_tapeSync)
        {
            if (_tapeVisti.TryGetValue(trade, out var i) && i < _bigTrades.Count)
            {
                _bigTrades[i].Volume = trade.Volume;
                _bigTrades[i].Price = trade.Lastprice;
                return;
            }

            _tapeVisti[trade] = _bigTrades.Count;
            _bigTrades.Add(new BigTrade
            {
                Time = trade.Time,
                Price = trade.Lastprice,
                Volume = trade.Volume,
                Compra = trade.Direction.ToString().StartsWith("Buy", StringComparison.OrdinalIgnoreCase),
                Sorgente = trade,
            });

            PotaIlTape();
        }
    }

    /// <summary>Toglie i big trades piu' vecchi di <see cref="MinutiDiTape"/>. Va chiamata sotto lock.</summary>
    private void PotaIlTape()
    {
        if (_bigTrades.Count < 2000)
        {
            return;
        }

        var limite = _bigTrades[^1].Time.AddMinutes(-MinutiDiTape);
        var taglio = 0;
        while (taglio < _bigTrades.Count && _bigTrades[taglio].Time < limite)
        {
            taglio++;
        }

        if (taglio == 0)
        {
            return;
        }

        _bigTrades.RemoveRange(0, taglio);
        RicostruisciLaMappa();
    }

    /// <summary>
    /// Rifa' l'indice dei trade vivi dalla lista, dopo un riordino o una potatura.
    ///
    /// <para>Svuotare la mappa sarebbe piu' semplice e sarebbe sbagliato: un trade ancora in
    /// crescita tornerebbe a essere trattato come nuovo al primo aggiornamento, e comparirebbe
    /// due volte. E' successo il 20 settembre 2026 - il registro contava cinque big trade dove
    /// ce n'erano quattro - e si e' visto solo confrontando con la richiesta storica.</para>
    /// </summary>
    private void RicostruisciLaMappa()
    {
        _tapeVisti.Clear();
        for (var i = 0; i < _bigTrades.Count; i++)
        {
            if (_bigTrades[i].Sorgente is { } sorgente)
            {
                _tapeVisti[sorgente] = i;
            }
        }
    }

    /// <summary>
    /// I big trades dentro una fascia di prezzo, da un certo istante in poi.
    ///
    /// <para>E' la meta' mancante della coppia sforzo/risultato: la footprint dice quanti lotti
    /// sono passati a quel prezzo, questa dice se erano <b>ordini grossi</b>. Mille lotti in
    /// ordini da due non sono un muro; mille lotti in sei ordini da centosessanta lo sono, ed e'
    /// la differenza che il live guarda sempre — <i>"look how many absorption contract you have
    /// here on this horizontal level: 70, 75, 141, 33"</i> <c>[6 · 1:01:24]</c>.</para>
    /// </summary>
    private (int Quanti, decimal Netto, decimal Totale, bool Copre) BigTradesAlLivello(
        decimal prezzo, decimal fascia, DateTime da, DateTime a)
    {
        lock (_tapeSync)
        {
            var quanti = 0;
            decimal netto = 0m;
            decimal totale = 0m;
            foreach (var t in _bigTrades)
            {
                if (t.Time < da || t.Time > a || t.Price < prezzo - fascia || t.Price > prezzo + fascia)
                {
                    continue;
                }
                quanti++;
                totale += t.Volume;
                netto += t.Compra ? t.Volume : -t.Volume;
            }

            return (quanti, netto, totale, _tapeDa is not null && _tapeDa.Value <= da);
        }
    }

    /// <summary>Quanti big trades in tutta la finestra, per dire se il libro e' vivo o sottile.</summary>
    private (int Quanti, decimal Netto, bool Copre) BigTradesNellaFinestra(DateTime da, DateTime a)
    {
        lock (_tapeSync)
        {
            var quanti = 0;
            decimal netto = 0m;
            foreach (var t in _bigTrades)
            {
                if (t.Time < da || t.Time > a)
                {
                    continue;
                }
                quanti++;
                netto += t.Compra ? t.Volume : -t.Volume;
            }

            return (quanti, netto, _tapeDa is not null && _tapeDa.Value <= da);
        }
    }

    /// <summary>
    /// Semina il registro del tape con i big trades gia' scambiati, una volta sola.
    ///
    /// <para>Senza questa, ogni deploy azzera i big trades e il pannello impiega un'ora a tornare
    /// utile. Con ATAS X che ricarica l'indicatore a ogni deploy, succederebbe piu' volte al
    /// giorno, e sempre nel momento in cui si e' appena finito di lavorare sul chart.</para>
    ///
    /// <para>Fallisce in silenzio di proposito: se la richiesta storica non torna, il registro
    /// resta scoperto e <see cref="_tapeDa"/> lo dichiara. Una semina fallita che finge di essere
    /// riuscita sarebbe peggio di nessuna semina.</para>
    /// </summary>
    private async Task SeminaIlTapeAsync(DateTime fine)
    {
        if (Interlocked.Exchange(ref _tapeSeminato, 1) != 0)
        {
            return;
        }

        try
        {
            var inizio = fine.AddMinutes(-MinutiDiTape);
            var richiesta = new CumulativeTradesRequest(
                inizio, fine, CumulativeTradesMode.Filter, SogliaBig, 0);
            var trades = await RequestCumulativeAsync(richiesta, CancellationToken.None)
                .ConfigureAwait(false);

            lock (_tapeSync)
            {
                foreach (var t in trades)
                {
                    if (t.Volume < SogliaBig || t.Time < inizio || t.Time > fine)
                    {
                        continue;
                    }

                    // Il tape vivo puo' aver gia' consegnato questo stesso trade mentre la
                    // richiesta storica era in volo, e sarebbe un oggetto diverso con lo stesso
                    // contenuto: si confronta cio' che si vede, non l'identita'.
                    var gia = false;
                    foreach (var v in _bigTrades)
                    {
                        if (v.Time == t.Time && v.Price == t.Lastprice && v.Volume == t.Volume)
                        {
                            gia = true;
                            break;
                        }
                    }
                    if (gia)
                    {
                        continue;
                    }

                    _bigTrades.Add(new BigTrade
                    {
                        Time = t.Time,
                        Price = t.Lastprice,
                        Volume = t.Volume,
                        Compra = t.Direction.ToString().StartsWith("Buy", StringComparison.OrdinalIgnoreCase),
                    });
                }

                // I semi sono piu' vecchi di cio' che e' arrivato dal vivo nel frattempo, e
                // PotaIlTape si fida dell'ordine: si riordina invece di sperare.
                _bigTrades.Sort((a, b) => a.Time.CompareTo(b.Time));
                RicostruisciLaMappa();
                _tapeDa = _tapeDa is null || inizio < _tapeDa.Value ? inizio : _tapeDa;
                _tapeHaParlato = true;
            }
        }
        catch (Exception errore)
        {
            this.LogWarn($"semina del tape non riuscita: {errore.Message}");
        }
    }

    /// <summary>
    /// La finestra su cui si contano i big trades: le ultime <see cref="PanelLookback"/> barre
    /// CHIUSE, estremi inclusi.
    ///
    /// <para>Il limite superiore non e' pignoleria. Senza, il conteggio includeva anche gli
    /// ordini arrivati nella barra ancora in formazione, e la riga diceva "in 10 barre" mentre ne
    /// contava undici — un numero che non tornava con nessuna verifica fatta sui dati storici, e
    /// che infatti il 20 settembre 2026 non e' tornato.</para>
    /// </summary>
    private (DateTime Da, DateTime A) FinestraDelTape(int ultimo)
    {
        var da = GetCandle(Math.Max(0, ultimo - PanelLookback + 1))?.Time ?? DateTime.MinValue;

        // MAI OLTRE L'ULTIMA BARRA. GetCandle(CurrentBar) non restituisce null: va fuori
        // intervallo e tira un'eccezione. Il pannello chiama questa funzione con
        // ultimo = CurrentBar - 1, quindi ultimo + 1 era precisamente la barra che non esiste -
        // e l'eccezione, partendo dentro OnRender, non si e' fermata alla misura: ha portato giu'
        // il disegno di TUTTO, pannello e livelli insieme. Il 20 settembre 2026 il chart e'
        // rimasto nudo mentre il motore risolveva regolarmente dodici livelli.
        var a = ultimo + 1 <= CurrentBar - 1
            ? GetCandle(ultimo + 1).Time.AddTicks(-1)
            : DateTime.MaxValue; // l'ultima barra e' quella in formazione: la finestra arriva a adesso
        return (da, a);
    }

    /// <summary>Da che ora il registro del tape copre, nell'orologio di chi guarda.</summary>
    private string EtaDelTape()
        => _tapeDa is null ? "mai" : OraLocale(_tapeDa.Value).ToString("HH:mm");

    // ------------------------------------------------------------------ i veti

    /// <summary>
    /// I motivi per <b>non</b> prendere il setup che il pannello sta mostrando.
    ///
    /// <para><b>Perche' i veti hanno un blocco loro.</b> Il pannello mostrava i prerequisiti di
    /// uno scenario — le condizioni spuntate — e mai le ragioni contrarie. Ma nel live i veti non
    /// sono il complemento dei prerequisiti: sono una lista a parte, dichiarata, e ne basta uno.
    /// <i>"We cannot force setups. Only when it's there"</i> <c>[4 · 43:22]</c>.</para>
    ///
    /// <para>Qui ci sono i quattro misurabili con cio' che il bridge ha. Gli altri due del live —
    /// "troppo vicini al muro" e la posizione nella curva del composito — chiedono un dato che
    /// questo chart non ha, e <b>non si simulano</b>: un veto inventato e' peggio di un veto
    /// mancante, perche' fa saltare setup buoni con l'aria di una misura.</para>
    /// </summary>
    private List<string> Veti(
        int ultimo, decimal prezzo, LetturaDelRegime regime, BridgeLevel? gioco, bool dentroValore)
    {
        var veti = new List<string>();

        // 1. NO MAN'S LAND. "We are in the middle. This is not where I want to engage. I want to
        //    have a clean path" [4 · 17:37]. Il centro del range della finestra del regime, il
        //    terzo di mezzo, e solo quando nessun livello e' in gioco: se un livello c'e', il
        //    prezzo non e' in mezzo al nulla, e' su qualcosa.
        if (gioco is null)
        {
            decimal alto = decimal.MinValue, basso = decimal.MaxValue;
            for (var bar = Math.Max(0, ultimo - FinestraRegime + 1); bar <= ultimo; bar++)
            {
                var c = GetCandle(bar);
                if (c is null)
                {
                    continue;
                }
                alto = Math.Max(alto, c.High);
                basso = Math.Min(basso, c.Low);
            }

            if (alto > basso)
            {
                var quota = (prezzo - basso) / (alto - basso);
                if (quota > 0.33m && quota < 0.67m)
                {
                    veti.Add($"in mezzo al range delle ultime {FinestraRegime} barre "
                             + $"({quota * 100m:0}% fra {Prezzo(basso)} e {Prezzo(alto)}): niente livello, niente strada pulita");
                }
            }
        }

        // 2. NIENTE SPEED, NIENTE BIG TRADES. "I usually don't like to take trades when there is
        //    no support from the aggressive order market participants" [2 · 44:17].
        var (percentile, _, velocitaOk) = VelocitaProxy(ultimo);
        var (daQuando, aQuando) = FinestraDelTape(ultimo);
        var (quantiBig, _, copreBig) = BigTradesNellaFinestra(daQuando, aQuando);

        if (!_tapeHaParlato || !copreBig)
        {
            veti.Add("il registro del tape non copre tutta la finestra: i big trades non si "
                     + "possono ne' vedere ne' escludere");
        }
        else if (velocitaOk && percentile <= PercentileMorto && quantiBig == 0)
        {
            veti.Add($"nessun big trade da {SogliaBig} lotti in {PanelLookback} barre e velocita' "
                     + $"al {percentile}° percentile: libro sottile, e non e' un posto dove entrare");
        }

        // 3. IL REGIME. Choppy non vieta di operare, vieta la size piena e il terzo tentativo:
        //    "If I reach a maximum of three stop loss... usually I stop for the day" [2 · 1:10:39].
        if (regime.Nome == "CHOPPY")
        {
            veti.Add("regime choppy: size ridotta, niente bersagli lontani, due stop e si chiude");
        }

        // 4. CONTROTREND RISPETTO AL BIAS. "I would not consider a short here for only one
        //    reason: the auction is still long" [2 · 1:56:36]. Il bias del giorno lo si legge dal
        //    delta di seduta, che e' il conto che il pannello gia' stampa: e' la scelta piu'
        //    onesta, perche' usa un numero che chi guarda sta gia' vedendo.
        var verso = gioco?.Scenario?.Direzione?.ToUpperInvariant();
        if (verso is "LONG" or "SHORT")
        {
            var apertura = BarraDellApertura(ultimo);
            if (apertura >= 0 && apertura < ultimo)
            {
                var (dCash, vCash) = DeltaDiBarre(apertura, ultimo);
                if (vCash > 0)
                {
                    var quota = dCash * 100m / vCash;
                    if (verso == "SHORT" && quota >= 2m)
                    {
                        veti.Add($"lo scenario e' SHORT ma l'asta di seduta e' lunga (delta {Segnato(dCash)}, {quota:0.0}%)");
                    }
                    else if (verso == "LONG" && quota <= -2m)
                    {
                        veti.Add($"lo scenario e' LONG ma l'asta di seduta e' corta (delta {Segnato(dCash)}, {quota:0.0}%)");
                    }
                }
            }

            // 5. IL MODELLO CONTRO LA POSIZIONE. "Prendere un setup momentum dentro la balance, o
            //    un mean reverting fuori dal valore, e' lo stesso errore con due nomi."
            var nome = gioco?.Scenario?.Nome ?? string.Empty;
            var meanReverting = nome.IndexOf("mean revert", StringComparison.OrdinalIgnoreCase) >= 0
                                || nome.IndexOf("fade del bordo", StringComparison.OrdinalIgnoreCase) >= 0;
            if (meanReverting && !dentroValore)
            {
                veti.Add("scenario di mean reverting con il prezzo FUORI dal valore: e' il modello sbagliato");
            }
        }

        return veti;
    }

    // ------------------------------------------------------------------ l'endpoint

    /// <summary>
    /// Il regime e la velocita' anche fuori dal chart.
    ///
    /// <para>Serve al giro d'orizzonte. Il pannello lo vede chi guarda lo schermo; l'agente
    /// legge dal bridge, e se il regime vivesse solo nel pannello si troverebbe a dedurlo di
    /// nuovo a mano a ogni messaggio — cioe' la cosa che questo lavoro doveva togliere di
    /// mezzo.</para>
    ///
    /// <para>Ogni numero esce con la sua soglia accanto, perche' una classificazione senza la
    /// regola che l'ha prodotta non e' verificabile e non si puo' contestare.</para>
    /// </summary>
    private object Regime()
    {
        var ultimo = CurrentBar - 2; // la barra in formazione non entra in nessuna misura
        if (ultimo < 1)
        {
            throw new BridgeException(409, "non ci sono abbastanza barre chiuse");
        }

        var r = LeggiIlRegime(ultimo);
        var (percentile, volumeBarra, velocitaOk) = VelocitaProxy(ultimo);
        var (daQuando, aQuando) = FinestraDelTape(ultimo);
        var (quantiBig, nettoBig, copreBig) = BigTradesNellaFinestra(daQuando, aQuando);

        return new
        {
            schema = Schema,
            instrument = InstrumentInfo?.Instrument,
            marketTimeUtc = Iso(GetCandle(ultimo).Time),
            regime = new
            {
                nome = r.Nome,
                perche = r.Perche,
                size = r.Size,
                misurabile = r.Misurabile,
                efficienza = Math.Round(r.Efficienza, 3),
                rientri = r.Rientri,
                copertura = r.Copertura,
                finestraBarre = FinestraRegime,
                soglie = new
                {
                    direzionale = EfficienzaDirezionale,
                    choppy = EfficienzaChoppy,
                    rientriChoppy = RientriChoppy,
                    candeleCoperte = CandeleCoperte,
                },
            },
            velocita = new
            {
                // IL NOME DEL CAMPO DICE CHE E' UN PROXY. Un campo chiamato "speed" verrebbe
                // citato come speed of tape entro una settimana, e la distinzione andrebbe persa
                // proprio nel punto in cui e' importante.
                proxy = "volume della barra contro la distribuzione delle ultime N barre",
                nonE = "la speed of tape, che e' il ritmo di immissione degli ordini e non c'e' nel bridge",
                misurabile = velocitaOk,
                percentile,
                volumeBarra,
                finestraBarre = FinestraVelocita,
                soglie = new { alta = PercentileAlto, morta = PercentileMorto },
            },
            bigTrades = new
            {
                // Zero e "nessun dato" sono due cose diverse, e chi legge deve poterle separare.
                tapeHaParlato = _tapeHaParlato,
                copreTuttaLaFinestra = copreBig,
                registroDaUtc = _tapeDa is null ? null : Iso(_tapeDa.Value),
                soglia = SogliaBig,
                finestraBarre = PanelLookback,
                quanti = quantiBig,
                netto = nettoBig,
            },
        };
    }
}
