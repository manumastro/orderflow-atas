namespace Quant.Test;

public class DatiTest
{
    [Fact]
    public void LOraLegaleAmericanaSpostaLApertura()
    {
        // 9:30 ET e' 14:30Z d'inverno e 13:30Z d'estate; il cambio USA del 2026 e' l'8 marzo
        Assert.Equal(new DateTime(2026, 3, 6, 14, 30, 0), Tempo.Utc(Calendario.Utc(new DateOnly(2026, 3, 6), Calendario.AperturaCassa)));
        Assert.Equal(new DateTime(2026, 3, 9, 13, 30, 0), Tempo.Utc(Calendario.Utc(new DateOnly(2026, 3, 9), Calendario.AperturaCassa)));
        Assert.Equal(new DateTime(2026, 11, 2, 14, 30, 0), Tempo.Utc(Calendario.Utc(new DateOnly(2026, 11, 2), Calendario.AperturaCassa)));
    }

    [Fact]
    public void LaGiornataDiBorsaCominciaAlleDiciottoDelGiornoPrima()
    {
        var et = new DateTime(2026, 3, 3, 19, 0, 0); // martedi' 19:00 ET
        Assert.Equal(new DateOnly(2026, 3, 4), Calendario.GiornataDi(et));
        Assert.Equal(new DateOnly(2026, 3, 3), Calendario.GiornataDi(new DateTime(2026, 3, 3, 16, 59, 0)));
    }

    [Fact]
    public void IlConvertitoreConCacheDaLoStessoRisultato()
    {
        var conv = new Calendario.Convertitore();
        var t = Tempo.Ms(new DateTime(2026, 3, 1));
        for (var k = 0; k < 20_000; k++)
        {
            var ms = t + k * 7 * Tempo.Minuto;
            Assert.Equal(Calendario.Et(ms), conv.Et(ms));
        }
    }

    [Fact]
    public void LArchivioRileggeCioCheScrive()
    {
        var cartella = Path.Combine(Path.GetTempPath(), "quant-test-" + Guid.NewGuid().ToString("N"));
        Environment.SetEnvironmentVariable("QUANT_DATI", cartella);
        try
        {
            var barre = Sintetico.Genera(new Sintetico.Opzioni { Giorni = 30, Seme = 3 });
            Archivio.Scrivi<Barra>("SINT", "m1", barre, b => b.Inizio);
            // riscrivere una parte non duplica
            Archivio.Scrivi<Barra>("SINT", "m1", barre.AsSpan(100, 500), b => b.Inizio);
            var lette = Archivio.Leggi<Barra>("SINT", "m1");
            Assert.Equal(barre, lette);
            var serie = Archivio.Serie("SINT");
            Assert.Equal(30, serie.Giorni.Length);
            Assert.All(serie.Giorni, g => Assert.Equal(1380, g.Barre));
        }
        finally
        {
            Environment.SetEnvironmentVariable("QUANT_DATI", null);
            if (Directory.Exists(cartella)) Directory.Delete(cartella, true);
        }
    }

    [Fact]
    public void LaRettificaAllIndietroToglieIlSaltoDelRollover()
    {
        var barre = Sintetico.Genera(new Sintetico.Opzioni { Giorni = 4, Seme = 5 });
        var t = barre[2000].Inizio;
        var prima = barre[1999].Chiusura;
        var roll = new Rollover(t, "NQH6", "NQM6", 1200);
        var serie = new Serie(Strumento.NQ, (Barra[])barre.Clone(), "test", [roll]);
        Assert.Equal(prima + 1200, serie.Barre[1999].Chiusura);
        Assert.Equal(barre[2000].Chiusura, serie.Barre[2000].Chiusura);
    }

    private static Barra B(DateTime t, double apertura, double chiusura) => new()
    {
        Inizio = Tempo.Ms(t), Fine = Tempo.Ms(t.AddSeconds(59)),
        Apertura = Strumento.NQ.InTick(apertura), Massimo = Strumento.NQ.InTick(Math.Max(apertura, chiusura)),
        Minimo = Strumento.NQ.InTick(Math.Min(apertura, chiusura)), Chiusura = Strumento.NQ.InTick(chiusura), Volume = 100,
    };

    [Fact]
    public void LoScartoDelRolloverViene_dal_salto_dal_carry_o_dalla_misura()
    {
        var u = DateTimeKind.Utc;
        // tre rollover a 91 giorni l'uno dall'altro: giovedi' con un'ora di pausa, venerdi' con il fine
        // settimana in mezzo, giovedi' con un'ora di pausa
        var t1 = new DateTime(2019, 3, 7, 22, 0, 0, u);
        var t2 = t1.AddDays(91);
        var t3 = t2.AddDays(91);
        var t4 = t3.AddDays(91);
        var barre = new[]
        {
            B(t1.AddMinutes(-61), 10_000, 10_000), B(t1, 10_020, 10_021),                 // salto +20
            B(t2.AddMinutes(-61), 10_000, 10_000), B(t2.AddHours(48), 9_900, 9_901),     // fine settimana: -100 inutilizzabile
            B(t3.AddMinutes(-61), 10_000, 10_000), B(t3, 10_040, 10_041),                 // salto +40
        };
        var cambi = new[]
        {
            new ScartiDiRollover.Cambio(Tempo.Ms(t1), "NQH9", "NQM9", Tempo.Ms(t2)),
            new ScartiDiRollover.Cambio(Tempo.Ms(t2), "NQM9", "NQU9", Tempo.Ms(t3)),
            new ScartiDiRollover.Cambio(Tempo.Ms(t3), "NQU9", "NQZ9", Tempo.Ms(t4)),
        };
        var r = ScartiDiRollover.Calcola(barre, Strumento.NQ, cambi);
        Assert.Equal(("salto", 20.0), (r[0].Metodo, Strumento.NQ.InPrezzo(r[0].Scarto)));
        Assert.Equal(("salto", 40.0), (r[2].Metodo, Strumento.NQ.InPrezzo(r[2].Scarto)));
        // a meta' strada il tasso e' la media dei due, e il prezzo e' lo stesso: lo scarto e' circa 30
        Assert.Equal("carry", r[1].Metodo);
        Assert.Equal(30.0, Strumento.NQ.InPrezzo(r[1].Scarto), 0);

        // una misura data da fuori vince sul salto
        var m = ScartiDiRollover.Calcola(barre, Strumento.NQ, cambi, new Dictionary<string, double> { ["NQH9"] = 25.5 });
        Assert.Equal(("misurato", 25.5), (m[0].Metodo, Strumento.NQ.InPrezzo(m[0].Scarto)));
    }
}
