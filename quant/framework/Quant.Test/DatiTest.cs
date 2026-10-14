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
}
