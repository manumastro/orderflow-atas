namespace XrayGui;

internal static class Program
{
    /// <summary>XrayGui, oppure XrayGui --foto CARTELLA: una PNG per pagina, senza mostrare la finestra
    /// (per controllare la grafica senza fotografare il desktop).</summary>
    [STAThread]
    private static void Main(string[] args)
    {
        ApplicationConfiguration.Initialize();
        if (args.Length == 2 && args[0] == "--foto")
        {
            using var form = new MainForm { StartPosition = FormStartPosition.Manual, Location = new Point(-4000, -4000), ShowInTaskbar = false };
            form.Show();
            form.Fotografa(args[1]);
            return;
        }
        Application.Run(new MainForm());
    }
}
