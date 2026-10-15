using System.Globalization;
using ATAS.DataFeedsCore;
using ATAS.Indicators;

namespace FabioOrderFlow.Observation;

/// <summary>
/// La catena delle opzioni dello strumento del chart, dalla connessione di ATAS (14 ottobre 2026).
///
/// Gli indicatori opzioni di ATAS calcolano GEX, Zero Gamma e muri in campi offuscati, che non si
/// leggono in modo affidabile. Il provider da cui partono pero' e' aperto a ogni indicatore:
/// `OptionsDataProvider`. Qui lo si espone com'e', senza calcoli: serie in scadenza, opzioni con
/// strike e tipo, e per ognuna bid, ask, ultimo, open interest, volume e settlement. La GEX la
/// calcola il framework, con la convenzione dichiarata (quant/metodo/le-opzioni-in-atas.md).
///
///   /opzioni                         le serie disponibili (codice, scadenza, tipo)
///   /opzioni?expiration=2026-10-14   le opzioni di quella serie, con le quotazioni
///            &amp;strikes=40               solo i 40 strike piu' vicini al prezzo, per lato (default)
///            &amp;wait=20                  secondi massimi di attesa delle quotazioni (default)
///
/// Le sottoscrizioni restano aperte finche' vive l'indicatore, e la seconda richiesta e' immediata;
/// oltre il tetto di ATAS (512 per indicatore) si chiudono quelle che la richiesta non usa.
/// </summary>
public sealed partial class DataBridge
{
    private readonly Dictionary<string, IOptionQuoteSubscription> _opzioni = new();
    private readonly SemaphoreSlim _opzioniLock = new(1, 1);
    private const int TettoSottoscrizioni = 500;

    private async Task<object> OpzioniAsync(System.Collections.Specialized.NameValueCollection query, CancellationToken cancellation)
    {
        var provider = OptionsDataProvider;
        if (provider is null || !provider.IsAvailable)
        {
            throw new BridgeException(503, "OptionsDataProvider non disponibile: la connessione non da' le opzioni di questo strumento?");
        }

        var serie = (await provider.GetOptionSeriesAsync(cancellation).ConfigureAwait(false)).ToList();
        var scadenza = query["expiration"];
        if (string.IsNullOrEmpty(scadenza))
        {
            return new
            {
                schema = Schema,
                instrument = InstrumentInfo?.Instrument,
                count = serie.Count,
                series = serie.OrderBy(s => s.Expiration).Select(s => new
                {
                    code = s.Code,
                    exchange = s.Exchange,
                    underlying = s.UnderlyingCode,
                    expiration = Iso(s.Expiration),
                    type = s.Type.ToString(),
                }),
            };
        }

        var giorno = DateTime.ParseExact(scadenza, "yyyy-MM-dd", CultureInfo.InvariantCulture);
        var scelte = serie.Where(s => s.Expiration.Date == giorno.Date).ToList();
        if (scelte.Count == 0)
        {
            throw new BridgeException(404, $"nessuna serie in scadenza il {scadenza}: vedi /opzioni");
        }

        var perLato = Math.Clamp(OptionalInt(query, "strikes") ?? 40, 1, 500);
        var attesa = TimeSpan.FromSeconds(Math.Clamp(OptionalInt(query, "wait") ?? 20, 0, 120));
        var prezzo = GetCandle(CurrentBar - 1)?.Close ?? 0m;

        var opzioni = new List<Security>();
        foreach (var s in scelte)
        {
            opzioni.AddRange(await provider.GetOptionsAsync(s, cancellation).ConfigureAwait(false));
        }

        // i perLato strike sopra e sotto il prezzo
        var strike = opzioni.Where(o => o.StrikePrice is not null).Select(o => o.StrikePrice!.Value).Distinct().OrderBy(k => k).ToList();
        var sotto = strike.Where(k => k <= prezzo).TakeLast(perLato);
        var sopra = strike.Where(k => k > prezzo).Take(perLato);
        var tenuti = sotto.Concat(sopra).ToHashSet();
        var scelteOpzioni = opzioni.Where(o => o.StrikePrice is { } k && tenuti.Contains(k)).ToList();

        List<IOptionQuoteSubscription> sottoscrizioni;
        await _opzioniLock.WaitAsync(cancellation).ConfigureAwait(false);
        try
        {
            // ATAS concede 512 sottoscrizioni alle opzioni per indicatore, e una sottoscrizione mai
            // chiusa resta aperta finche' vive l'indicatore: il 15 ottobre 2026 la terza scadenza
            // chiesta ha avuto solo l'errore del limite. Se le nuove non ci stanno, si chiudono
            // quelle che questa richiesta non usa.
            var chiavi = scelteOpzioni.Select(o => o.SecurityId ?? o.Code).ToHashSet();
            if (_opzioni.Count + chiavi.Count(k => !_opzioni.ContainsKey(k)) > TettoSottoscrizioni)
            {
                foreach (var k in _opzioni.Keys.Where(k => !chiavi.Contains(k)).ToList())
                {
                    _opzioni[k].Dispose();
                    _opzioni.Remove(k);
                }
            }

            sottoscrizioni = scelteOpzioni.Select(o =>
            {
                var chiave = o.SecurityId ?? o.Code;
                if (!_opzioni.TryGetValue(chiave, out var sub))
                {
                    sub = provider.SubscribeToOption(o);
                    _opzioni[chiave] = sub;
                }

                return sub;
            }).ToList();
        }
        finally
        {
            _opzioniLock.Release();
        }

        // si aspettano le quotazioni: almeno l'open interest o un prezzo per ogni opzione
        var limite = DateTime.UtcNow + attesa;
        while (DateTime.UtcNow < limite && sottoscrizioni.Any(s => s.Summary is not { } x || (x.OpenInterest is null && x.BestBidPrice is null && x.LastTradePrice is null)))
        {
            await Task.Delay(250, cancellation).ConfigureAwait(false);
        }

        var righe = sottoscrizioni.Select(s =>
        {
            var o = s.Option;
            var q = s.Summary;
            return new
            {
                code = o.Code,
                strike = o.StrikePrice,
                type = o.OptionType?.ToString(),
                expiration = Iso(o.Expiration),
                bid = q?.BestBidPrice,
                bidVolume = q?.BestBidVolume,
                ask = q?.BestAskPrice,
                askVolume = q?.BestAskVolume,
                last = q?.LastTradePrice,
                openInterest = q?.OpenInterest,
                volume = q?.CurrentDayTotalVolume,
                settlement = q?.SettlementPrice,
                prevClose = q?.PrevDayClosePrice,
            };
        }).OrderBy(r => r.strike).ThenBy(r => r.type).ToList();

        return new
        {
            schema = Schema,
            instrument = InstrumentInfo?.Instrument,
            underlyingPrice = prezzo,
            at = Iso(DateTime.UtcNow),
            series = scelte.Select(s => s.Code),
            strikesPerSide = perLato,
            quoted = righe.Count(r => r.openInterest is not null || r.bid is not null || r.last is not null),
            count = righe.Count,
            options = righe,
        };
    }
}
