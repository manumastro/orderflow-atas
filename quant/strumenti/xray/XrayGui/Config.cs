namespace XrayGui;

/// <summary>Dove stanno lo strumento (scripts, XrayLive) e la cartella dati scelta.</summary>
internal static class Config
{
    /// <summary>quant/strumenti/xray: la cartella che ha scripts/ e XrayLive/, risalendo dall'eseguibile.</summary>
    public static string Strumento()
    {
        var dir = new DirectoryInfo(AppContext.BaseDirectory);
        while (dir is not null)
        {
            if (File.Exists(Path.Combine(dir.FullName, "scripts", "xray_live_task.ps1"))
                && Directory.Exists(Path.Combine(dir.FullName, "XrayLive"))) return dir.FullName;
            dir = dir.Parent;
        }
        return AppContext.BaseDirectory;
    }

    public static string Lettore() =>
        Path.Combine(Strumento(), "XrayLive", "bin", "Release", "net10.0", "XrayLive.dll");

    /// <summary>La cartella dati: quella salvata, se no quant/dati/xray accanto allo strumento.</summary>
    public static string Cartella()
    {
        try
        {
            if (File.Exists(Percorso()) && File.ReadAllText(Percorso()).Trim() is { Length: > 0 } salvata)
                return salvata;
        }
        catch
        {
            // una configurazione illeggibile vale come assente
        }
        string accanto = Path.GetFullPath(Path.Combine(Strumento(), "..", "..", "dati", "xray"));
        return Directory.Exists(accanto) ? accanto
            : Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.UserProfile), "orderflow-atas", "quant", "dati", "xray");
    }

    public static void Salva(string cartella)
    {
        try
        {
            Directory.CreateDirectory(Path.GetDirectoryName(Percorso())!);
            File.WriteAllText(Percorso(), cartella);
        }
        catch
        {
            // non salvarla non deve fermare la finestra
        }
    }

    private static string Percorso() => Path.Combine(
        Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "XrayGui", "outdir.txt");
}
