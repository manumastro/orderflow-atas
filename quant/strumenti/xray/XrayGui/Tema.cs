using System.Runtime.InteropServices;

namespace XrayGui;

/// <summary>Colori e controlli della dashboard: scuro, piatto, leggibile.</summary>
internal static class Tema
{
    public static readonly Color Sfondo = Color.FromArgb(22, 24, 29);
    public static readonly Color Pannello = Color.FromArgb(31, 35, 42);
    public static readonly Color PannelloChiaro = Color.FromArgb(42, 47, 56);
    public static readonly Color Bordo = Color.FromArgb(55, 61, 72);
    public static readonly Color Testo = Color.FromArgb(230, 232, 236);
    public static readonly Color Spento = Color.FromArgb(150, 158, 170);
    public static readonly Color Verde = Color.FromArgb(63, 185, 80);
    public static readonly Color Rosso = Color.FromArgb(248, 81, 73);
    public static readonly Color Blu = Color.FromArgb(88, 166, 255);
    public static readonly Color Arancio = Color.FromArgb(240, 136, 62);
    public static readonly Color Giallo = Color.FromArgb(227, 179, 65);
    public static readonly Color Viola = Color.FromArgb(188, 140, 255);

    public static readonly Font Base = new("Segoe UI", 9.5f);
    public static readonly Font Piccolo = new("Segoe UI", 8.5f);
    public static readonly Font Titolo = new("Segoe UI Semibold", 11f);
    public static readonly Font Grande = new("Segoe UI Semibold", 18f);
    public static readonly Font Mono = new("Consolas", 9.5f);

    public static Button Bottone(string testo, EventHandler click, bool primario = false)
    {
        var b = new Button
        {
            Text = testo,
            AutoSize = true,
            FlatStyle = FlatStyle.Flat,
            BackColor = primario ? Color.FromArgb(35, 134, 54) : PannelloChiaro,
            ForeColor = Testo,
            Font = Base,
            Padding = new Padding(8, 3, 8, 3),
            Margin = new Padding(0, 0, 6, 0),
            Cursor = Cursors.Hand,
        };
        b.FlatAppearance.BorderColor = Bordo;
        b.FlatAppearance.MouseOverBackColor = Bordo;
        b.Click += click;
        return b;
    }

    public static Label Etichetta(string testo, Font? font = null, Color? colore = null) => new()
    {
        Text = testo,
        AutoSize = true,
        Font = font ?? Base,
        ForeColor = colore ?? Testo,
        BackColor = Color.Transparent,
        Margin = new Padding(0, 2, 0, 2),
    };

    public static ComboBox Scelta(int larghezza)
    {
        return Scuro(new ComboBox
        {
            DropDownStyle = ComboBoxStyle.DropDownList,
            Width = larghezza,
            FlatStyle = FlatStyle.Flat,
            BackColor = PannelloChiaro,
            ForeColor = Testo,
            Font = Base,
            Margin = new Padding(0, 2, 10, 0),
        }, "DarkMode_CFD");
    }

    public static Color Segno(double v) => v > 0 ? Verde : v < 0 ? Rosso : Spento;

    [DllImport("uxtheme.dll", CharSet = CharSet.Unicode)]
    private static extern int SetWindowTheme(IntPtr hwnd, string? appName, string? idList);

    /// <summary>Barre di scorrimento e tendine scure (il tema scuro di Windows 10/11).</summary>
    public static T Scuro<T>(T c, string tema = "DarkMode_Explorer") where T : Control
    {
        c.HandleCreated += (_, _) => SetWindowTheme(c.Handle, tema, null);
        return c;
    }
}
