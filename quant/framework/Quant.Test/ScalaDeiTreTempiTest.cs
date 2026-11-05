using Quant.Studi;

namespace Quant.Test;

/// <summary>La scala del video del 5 novembre 2026 (quant/studi/scala-dei-tre-tempi/): casi scritti a mano.</summary>
public class ScalaDeiTreTempiTest
{
    private static LaScala.Candela C(int o, int h, int l, int c) => new(o, h, l, c);

    [Fact]
    public void Lo_scalino_chiede_la_chiusura_oltre_il_massimo_precedente()
    {
        var prima = C(100, 110, 90, 105);
        Assert.Equal(1, LaScala.Scalino(prima, C(105, 120, 95, 111)));   // massimo, minimo e chiusura sopra 110
        Assert.Equal(0, LaScala.Scalino(prima, C(105, 120, 95, 109)));   // il settembre del video: chiude sotto il massimo
        Assert.Equal(0, LaScala.Scalino(prima, C(105, 120, 90, 115)));   // minimo uguale: non piu' alto
        Assert.Equal(-1, LaScala.Scalino(prima, C(95, 105, 80, 89)));
        Assert.Equal(0, LaScala.Scalino(prima, C(95, 105, 80, 91)));     // l'8 ottobre: due tick sopra il minimo, pausa
        Assert.Equal(0, LaScala.Scalino(prima, C(100, 108, 92, 100)));   // candela interna
    }

    private static RiassuntoGiorno G(DateOnly d, int o, int h, int l, int c) => new(d, o, h, l, c, 1);

    [Fact]
    public void La_settimana_aperta_non_si_giudica()
    {
        // tre settimane: 2-6 marzo, 9-13 marzo, e il lunedi' 16 marzo (aperta, non deve contare)
        var giorni = new List<RiassuntoGiorno>();
        var d = new DateOnly(2026, 3, 2);
        for (var i = 0; i < 5; i++) giorni.Add(G(d.AddDays(i), 100, 110, 90, 105));
        for (var i = 0; i < 5; i++) giorni.Add(G(d.AddDays(7 + i), 105, 120 + i, 95, 121 + i));
        giorni.Add(G(new DateOnly(2026, 3, 16), 50, 60, 40, 45));
        var s = LaScala.Candele(giorni, 's', new DateOnly(2026, 3, 17));
        Assert.Equal(2, s.Count);
        Assert.Equal(C(105, 124, 95, 125), s[1]);
        Assert.Equal(1, LaScala.Scalino(s[0], s[1]));
        // il lunedi' 16 e' chiuso ma la sua settimana no: oggi e' martedi' 17
        Assert.Equal(2, LaScala.Candele(giorni, 's', new DateOnly(2026, 3, 17)).Count);
        Assert.Equal(3, LaScala.Candele(giorni, 's', new DateOnly(2026, 3, 23)).Count);
    }

    [Fact]
    public void La_memoria_dello_scalino_supera_le_pause()
    {
        var c = new List<LaScala.Candela> { C(0, 10, 0, 5), C(5, 20, 5, 21), C(21, 30, 10, 31), C(31, 32, 20, 25), C(25, 31, 21, 26) };
        Assert.Equal(0, LaScala.Stato(c, memoriaScalino: false));   // l'ultima e' una pausa
        Assert.Equal(1, LaScala.Stato(c, memoriaScalino: true));    // l'ultimo scalino era su
        Assert.Null(LaScala.Stato(c.Take(2).ToList(), false));      // la prima candela non conta: servono tre
    }
}
