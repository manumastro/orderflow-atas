namespace Quant.Test;

/// <summary>La logica del simulatore di contratto su sessioni costruite a mano.</summary>
public class SimulatoreContrattoTest
{
    /// <summary>Restituisce sessioni gia' scritte, in ordine. Ogni sessione: coppie (chiusura, minimo) per passo.</summary>
    private sealed class Fisse(params (double C, double M)[][] sessioni) : ISorgenteSessioni, ICursoreSessioni
    {
        private int _i;
        public string Descrizione => "fisse";
        public int MassimoPassi => sessioni.Max(s => s.Length);
        public ICursoreSessioni Apri() => this;

        public int Prossima(ref Caso rng, Span<double> chiusura, Span<double> minimo)
        {
            var s = sessioni[_i++ % sessioni.Length];
            for (var k = 0; k < s.Length; k++) { chiusura[k] = s[k].C; minimo[k] = s[k].M; }
            return s.Length;
        }
    }

    private static (FineFase, int) Gioca(Fase f, Fisse s)
    {
        var rng = new Caso(1);
        var c = s.Apri();
        return SimulatoreContratto.Valutazione(f, c, ref rng, new double[s.MassimoPassi], new double[s.MassimoPassi]);
    }

    private static readonly Fase Base = new() { Nome = "prova", Target = 3000, Drawdown = 2000 };

    [Fact]
    public void Il_target_si_passa_alla_chiusura_del_passo()
    {
        var (fine, sessioni) = Gioca(Base, new Fisse([(1000, 1000)], [(2000, 900), (2100, 1900)]));
        Assert.Equal(FineFase.Target, fine);
        Assert.Equal(2, sessioni);
    }

    [Fact]
    public void La_violazione_in_tempo_reale_usa_il_minimo_del_passo()
    {
        // il minimo tocca -2000 e poi la sessione chiude in utile: il conto e' perso lo stesso
        var (fine, _) = Gioca(Base, new Fisse([(-500, -2000), (500, -500)]));
        Assert.Equal(FineFase.Pavimento, fine);
    }

    [Fact]
    public void La_violazione_alla_chiusura_ignora_il_minimo_intraday()
    {
        var f = Base with { ViolazioneAllaChiusura = true };
        var (fine, _) = Gioca(f, new Fisse([(-500, -2500), (3000, -500)]));
        Assert.Equal(FineFase.Target, fine);
    }

    [Fact]
    public void Il_pavimento_alla_chiusura_sale_solo_con_il_saldo_di_fine_sessione()
    {
        // sessione 1: picco intraday +1500, chiude a +200. Pavimento EOD: -1800. Intraday sarebbe -500.
        // sessione 2: scende a -1700: con il pavimento EOD sopravvive, con quello intraday no.
        var sessioni = new Fisse([(1500, 0), (200, 200)], [(-1900, -1900), (-1000, -1900)], [(4000, 4000)]);
        Assert.Equal(FineFase.Target, Gioca(Base, sessioni).Item1);
        var intraday = Base with { PavimentoAllaChiusura = false };
        Assert.Equal(FineFase.Pavimento, Gioca(intraday, new Fisse([(1500, 0), (200, 200)], [(-1900, -1900), (-1000, -1900)], [(3000, 3000)])).Item1);
    }

    [Fact]
    public void Il_blocco_ferma_il_pavimento()
    {
        // saldo EOD 2500: senza blocco il pavimento sale a 500, con il blocco a +100 resta a 100
        var f = Base with { Target = 5000, Blocco = 100 };
        var (fine, _) = Gioca(f, new Fisse([(2500, 2500)], [(-2350, -2350)], [(5000, 5000)]));
        Assert.Equal(FineFase.Target, fine); // 2500 - 2350 = 150 > 100
        var senza = Base with { Target = 5000 };
        Assert.Equal(FineFase.Pavimento, Gioca(senza, new Fisse([(2500, 2500)], [(-2350, -2350)], [(5000, 5000)])).Item1);
    }

    [Fact]
    public void Il_limite_giornaliero_morbido_ferma_la_giornata_al_limite()
    {
        var f = Base with { LimiteGiornaliero = 1250 };
        // la sessione 1 scende a -1300: si esce a -1250 e il resto della sessione non conta
        var (fine, sessioni) = Gioca(f, new Fisse([(-1300, -1300), (2000, 2000)], [(3000, 3000)], [(1300, 1300)]));
        Assert.Equal(FineFase.Target, fine);
        Assert.Equal(3, sessioni); // -1250, +3000 = 1750, +1300 = 3050
    }

    [Fact]
    public void Il_limite_giornaliero_rigido_perde_il_conto()
    {
        var f = Base with { LimiteGiornaliero = 1250, LimiteRigido = true };
        Assert.Equal(FineFase.LimiteRigido, Gioca(f, new Fisse([(-1300, -1300)])).Item1);
    }

    [Fact]
    public void Con_il_pavimento_piu_vicino_del_limite_si_perde_il_conto()
    {
        // saldo -1000 dopo la prima sessione: pavimento a 1000 di distanza, limite a 1250
        var f = Base with { LimiteGiornaliero = 1250 };
        Assert.Equal(FineFase.Pavimento, Gioca(f, new Fisse([(-1000, -1000)], [(-1300, -1300)])).Item1);
    }

    [Fact]
    public void La_scadenza()
    {
        var f = Base with { Scadenza = 3 };
        Assert.Equal((FineFase.Scadenza, 3), Gioca(f, new Fisse([(10, 10)])));
    }

    [Fact]
    public void I_prelievi_di_Tradeify_aspettano_la_consistenza_e_bloccano_il_pavimento()
    {
        var c = TradeifyGrowth50K.Regole(false);
        // valutazione: +3000 in un giorno. Finanziato: un giorno da +1500 e cinque da +400.
        // Dopo il quinto da 400 il saldo e' 3500, i giorni sopra 150 $ sono 6, ma il migliore e'
        // 1500 > 35% di 3500 = 1225: niente prelievo. Al sesto da 400 (3900): 1500 > 1365, no.
        // Al settimo (4300): 1500 <= 1505, si': prelievo di 1500 (tetto del primo).
        var giorni = new List<(double, double)[]> { new[] { (3000.0, 3000.0) }, new[] { (1500.0, 1500.0) } };
        for (var i = 0; i < 7; i++) giorni.Add([(400.0, 400.0)]);
        // poi due giornate pessime: il limite giornaliero (salito a 2000 $ sopra i 3000 $ di saldo)
        // ferma la prima a -2000 (saldo 800); la seconda tocca il pavimento bloccato a +100
        giorni.Add([(-50_000.0, -50_000.0)]);
        giorni.Add([(-50_000.0, -50_000.0)]);
        var s = new Fisse([.. giorni]);
        var rng = new Caso(1);
        var ciclo = SimulatoreContratto.UnCiclo(c, s.Apri(), ref rng, new double[1], new double[1], new SimulatoreContratto.Opzioni());
        Assert.True(ciclo.Finanziato);
        Assert.Equal(1, ciclo.NumeroPrelievi);
        Assert.Equal(1500, ciclo.Prelevato);
        Assert.Equal(0.9 * 1500, ciclo.Incassato);
        Assert.Equal(145, ciclo.Costo);
        Assert.Equal(FineFase.Pavimento, ciclo.FineFinanziata);
        Assert.Equal(10, ciclo.SessioniFinanziate);
    }

    [Fact]
    public void Lim_preleva_ogni_venti_sessioni_e_rimborsa_il_prezzo()
    {
        var c = LimClassico.Regole(true);
        // due passi in due giorni, poi 20 giorni da +100: prelievo di 2000 all'80% piu' il rimborso
        var giorni = new List<(double, double)[]> { new[] { (10_000.0, 10_000.0) }, new[] { (5_000.0, 5_000.0) } };
        for (var i = 0; i < 20; i++) giorni.Add([(100.0, 100.0)]);
        giorni.Add([(-6_000.0, -6_000.0)]);
        var s = new Fisse([.. giorni]);
        var rng = new Caso(1);
        var ciclo = SimulatoreContratto.UnCiclo(c, s.Apri(), ref rng, new double[1], new double[1], new SimulatoreContratto.Opzioni());
        Assert.Equal(1, ciclo.NumeroPrelievi);
        Assert.Equal(0.8 * 2000 + 500, ciclo.Incassato, 6);
        Assert.Equal(FineFase.LimiteRigido, ciclo.FineFinanziata);
    }
}
