namespace Quant.Studi;

/// <summary>
/// VWAP: long sopra, short sotto, si esce quando il prezzo la riattraversa. Secondo esempio del
/// transcript (Zarattini e Aziz, "Volume Weighted Average Price (VWAP): The Holy Grail for Day
/// Trading Systems", su QQQ a un minuto). Il perche': gli algoritmi VWAP delle istituzioni comprano
/// o vendono in proporzione al volume, e amplificano i movimenti quando il prezzo cambia lato.
///
/// Con barre M1 la VWAP si stima dal prezzo tipico (H+L+C)/3 per il volume della barra; con il
/// footprint o i tick sara' esatta.
/// </summary>
public sealed class VwapTendenza : Strategia
{
    public override string Nome => "vwap-tendenza";
    public override string? Idea => "quant/studi/vwap-tendenza/1-idea.md";

    private int _inizio, _fine, _attesa;
    private bool _soloLong;
    private double _somma, _volume;

    public override string Regole =>
        $"VWAP ancorata alle {Calendario.Orario(_inizio)} ET. Ingresso: chiusura M1 sopra la VWAP long, sotto short" +
        $"{(_soloLong ? " (solo long)" : "")}, a mercato alla barra dopo, dopo {_attesa} minuti di attesa. " +
        $"Uscita: chiusura dall'altra parte della VWAP (e si gira), oppure alle {Calendario.Orario(_fine)} ET. Nessuno stop. " +
        "Dimensione: dai parametri (in sviluppo un contratto).";

    public override (int Da, int A) FinestraIngresso => (_inizio + _attesa, _fine);

    public override void Prepara(Parametri p)
    {
        _inizio = p.Orario("inizio", "09:30");
        _fine = p.Orario("fine", "16:00");
        _attesa = p.Intero("attesa", 0);
        _soloLong = p.Booleano("soloLong", false);
    }

    public override void NuovaGiornata(Contesto c)
    {
        _somma = 0;
        _volume = 0;
    }

    public override void SuBarra(Contesto c)
    {
        var m = c.MinutoEt;
        if (m < _inizio || m >= _fine) return;
        var b = c.Barra;
        _somma += (b.Massimo + b.Minimo + b.Chiusura) / 3.0 * b.Volume;
        _volume += b.Volume;
        if (m < _inizio + _attesa || _volume <= 0 || m >= _fine - 1) return;

        var vwap = _somma / _volume;
        Lato? voluto = b.Chiusura > vwap ? Lato.Long : b.Chiusura < vwap ? Lato.Short : c.Posizione?.Lato;
        if (_soloLong && voluto == Lato.Short) voluto = null;
        var attuale = c.Posizione?.Lato;
        if (voluto == attuale) return;
        if (attuale is not null) c.Esci();
        if (voluto is { } lato)
            c.Invia(new Ordine { Lato = lato, UscitaAlle = c.OggiAlle(_fine), Motivo = lato == Lato.Long ? "sopra la VWAP" : "sotto la VWAP" });
    }
}
