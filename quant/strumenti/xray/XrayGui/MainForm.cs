using System.Diagnostics;
using System.Net.Http;
using System.Text.Json;

namespace XrayGui;

/// <summary>
/// La finestra di gestione di X-Ray: il lettore e il server sono vivi? Le famiglie arrivano?
/// Avvia, ferma, compatta, apre la pagina. L'analisi non sta qui: sta nella pagina locale
/// (XrayWeb, servita da XrayLive su http://127.0.0.1:8790/), che si apre dal pulsante in alto.
/// Tre pagine: Stato, Famiglie, Lettore.
/// </summary>
internal sealed class MainForm : Form
{
    private const string Pagina = "http://127.0.0.1:8790/";

    private static readonly Dictionary<string, string> Indicatori = new()
    {
        ["levels"] = "Options Key Levels",
        ["observed"] = "Strike Profile: gamma/charm osservate",
        ["forecast"] = "Strike Profile: previsione",
        ["nextday"] = "Strike Profile: giorno dopo",
        ["gex"] = "GEX Heatmap / Strike Profile GEX",
        ["dex"] = "Strike Profile DEX",
        ["vex"] = "Strike Profile VEX",
        ["pressure"] = "Hedge Pressure",
    };

    private static readonly HttpClient Http = new() { Timeout = TimeSpan.FromSeconds(3) };

    private string _root;
    private readonly System.Windows.Forms.Timer _timer = new() { Interval = 5000 };
    private readonly Dictionary<string, Panel> _pagine = new();
    private readonly Dictionary<string, Button> _voci = new();
    private string _pagina = "Stato";

    private readonly Label _statoPunto = Tema.Etichetta("●", Tema.Titolo, Tema.Spento);
    private readonly Label _statoTesto = Tema.Etichetta("", Tema.Base, Tema.Spento);
    private readonly RichTextBox _stato = Testo();
    private readonly DataGridView _famiglie = Griglia();
    private readonly TextBox _log = Tema.Scuro(new TextBox
    {
        Dock = DockStyle.Fill, Multiline = true, ReadOnly = true, ScrollBars = ScrollBars.Vertical,
        BackColor = Tema.Sfondo, ForeColor = Tema.Spento, Font = Tema.Mono, BorderStyle = BorderStyle.None,
    });

    // l'ultima risposta del server della pagina (null = non risponde)
    private JsonDocument? _server;
    private bool _chiedendo;

    public MainForm()
    {
        Text = "X-Ray · gestione";
        Width = 1100;
        Height = 760;
        MinimumSize = new Size(860, 560);
        BackColor = Tema.Sfondo;
        ForeColor = Tema.Testo;
        Font = Tema.Base;
        _root = Config.Cartella();

        Costruisci();
        _timer.Tick += (_, _) => Aggiorna();
        _timer.Start();
        Shown += (_, _) => Aggiorna();
    }

    // ================================================================ costruzione

    private void Costruisci()
    {
        var testa = new Panel { Dock = DockStyle.Top, Height = 60, BackColor = Tema.Pannello, Padding = new Padding(14, 0, 14, 0) };
        var sinistra = new FlowLayoutPanel { Dock = DockStyle.Left, AutoSize = true, WrapContents = false, Padding = new Padding(0, 10, 0, 0) };
        sinistra.Controls.Add(Tema.Etichetta("X-Ray", Tema.Grande));
        sinistra.Controls.Add(Tema.Etichetta("  gestione   ", Tema.Base, Tema.Spento));
        _statoPunto.Margin = new Padding(8, 10, 2, 0);
        _statoTesto.Margin = new Padding(0, 13, 0, 0);
        sinistra.Controls.Add(_statoPunto);
        sinistra.Controls.Add(_statoTesto);
        var destra = new FlowLayoutPanel { Dock = DockStyle.Right, AutoSize = true, WrapContents = false, Padding = new Padding(0, 15, 0, 0) };
        destra.Controls.Add(Tema.Bottone("Apri la pagina", (_, _) => ApriPagina(), primario: true));
        destra.Controls.Add(Tema.Bottone("Avvia", (_, _) => Avvia()));
        destra.Controls.Add(Tema.Bottone("Ferma", (_, _) => Script("-Stop")));
        testa.Controls.Add(destra);
        testa.Controls.Add(sinistra);

        var nav = new FlowLayoutPanel { Dock = DockStyle.Left, Width = 160, FlowDirection = FlowDirection.TopDown, BackColor = Tema.Sfondo, Padding = new Padding(10, 14, 10, 0) };
        var contenuto = new Panel { Dock = DockStyle.Fill, BackColor = Tema.Sfondo, Padding = new Padding(4, 10, 12, 12) };
        foreach (var (nome, pagina) in new[] { ("Stato", PaginaStato()), ("Famiglie", PaginaFamiglie()), ("Lettore", PaginaLettore()) })
        {
            pagina.Dock = DockStyle.Fill;
            pagina.Visible = nome == _pagina;
            contenuto.Controls.Add(pagina);
            _pagine[nome] = pagina;
            var voce = new Button
            {
                Text = nome, Width = 138, Height = 36, FlatStyle = FlatStyle.Flat, TextAlign = ContentAlignment.MiddleLeft,
                Font = Tema.Base, ForeColor = Tema.Testo, BackColor = Tema.Sfondo, Margin = new Padding(0, 0, 0, 4),
                Padding = new Padding(10, 0, 0, 0), Cursor = Cursors.Hand,
            };
            voce.FlatAppearance.BorderSize = 0;
            voce.FlatAppearance.MouseOverBackColor = Tema.Pannello;
            voce.Click += (_, _) => Mostra(nome);
            _voci[nome] = voce;
            nav.Controls.Add(voce);
        }

        Controls.Add(contenuto);
        Controls.Add(nav);
        Controls.Add(testa);
        Mostra(_pagina);
    }

    private void Mostra(string nome)
    {
        _pagina = nome;
        foreach (var (n, p) in _pagine) p.Visible = n == nome;
        foreach (var (n, v) in _voci)
        {
            v.BackColor = n == nome ? Tema.PannelloChiaro : Tema.Sfondo;
            v.Font = n == nome ? Tema.Titolo : Tema.Base;
        }
        Aggiorna();
    }

    private Panel PaginaStato()
    {
        var pagina = new Panel { BackColor = Tema.Pannello, Padding = new Padding(14) };
        pagina.Controls.Add(_stato);
        return pagina;
    }

    private Panel PaginaFamiglie()
    {
        var pagina = new Panel();
        _famiglie.Columns.Add("famiglia", "famiglia");
        _famiglie.Columns.Add("ricevuta", "ultimo chunk");
        _famiglie.Columns.Add("istante", "istante dentro (New York)");
        _famiglie.Columns.Add("giornate", "giornate");
        _famiglie.Columns.Add("indicatore", "indicatore da tenere aperto su ES");
        _famiglie.Columns[0].FillWeight = 90;
        _famiglie.Columns[1].FillWeight = 50;
        _famiglie.Columns[2].FillWeight = 70;
        _famiglie.Columns[3].FillWeight = 40;
        _famiglie.Columns[4].FillWeight = 120;
        var nota = Tema.Etichetta("Una famiglia arriva solo mentre il suo indicatore X-Ray e' aperto su un chart ES. " +
                                  "Verde: meno di 15 minuti; giallo: meno di 3 ore; grigio: ferma.", Tema.Piccolo, Tema.Spento);
        nota.Dock = DockStyle.Top;
        nota.Padding = new Padding(0, 0, 0, 8);
        pagina.Controls.Add(_famiglie);
        pagina.Controls.Add(nota);
        return pagina;
    }

    private Panel PaginaLettore()
    {
        var pagina = new Panel();
        var barra = new FlowLayoutPanel
        {
            Dock = DockStyle.Top, AutoSize = true, AutoSizeMode = AutoSizeMode.GrowAndShrink, WrapContents = true,
            BackColor = Tema.Sfondo, Padding = new Padding(0, 2, 0, 8),
        };
        barra.Controls.Add(Tema.Bottone("Avvia", (_, _) => Avvia(), primario: true));
        barra.Controls.Add(Tema.Bottone("Ferma", (_, _) => Script("-Stop")));
        barra.Controls.Add(Tema.Bottone("Registra il task", (_, _) => Script("-Install")));
        barra.Controls.Add(Tema.Bottone("Togli il task", (_, _) => Script("-Remove")));
        barra.Controls.Add(Tema.Bottone("Stato del task", (_, _) => Script("-Status")));
        barra.Controls.Add(Tema.Bottone("Compatta archivio", (_, _) => Compatta()));
        barra.Controls.Add(Tema.Bottone("Cartella dati...", (_, _) => ScegliCartella()));
        barra.Controls.Add(Tema.Bottone("Apri cartella", (_, _) => ApriCartella()));
        pagina.Controls.Add(_log);
        pagina.Controls.Add(barra);
        return pagina;
    }

    // ================================================================ aggiornamento

    private void Aggiorna()
    {
        if (!IsHandleCreated) return;
        Stato stato;
        try { stato = Dati.LeggiStato(_root); }
        catch (Exception ex) when (ex is IOException or UnauthorizedAccessException) { return; }
        Intestazione(stato);
        ChiediServer();
        try
        {
            switch (_pagina)
            {
                case "Stato": AggiornaStato(stato); break;
                case "Famiglie": AggiornaFamiglie(); break;
            }
        }
        catch (Exception ex)
        {
            Log($"aggiornamento: {ex.GetType().Name} {ex.Message}");
        }
    }

    /// <summary>Chiede /api/stato al server della pagina, senza bloccare la finestra.</summary>
    private async void ChiediServer()
    {
        if (_chiedendo) return;
        _chiedendo = true;
        try
        {
            string testo = await Http.GetStringAsync(Pagina + "api/stato");
            _server?.Dispose();
            _server = JsonDocument.Parse(testo);
        }
        catch (Exception ex) when (ex is HttpRequestException or TaskCanceledException or JsonException)
        {
            _server?.Dispose();
            _server = null;
        }
        finally
        {
            _chiedendo = false;
        }
    }

    private void Intestazione(Stato s)
    {
        string server = _server is null ? "pagina spenta" : "pagina accesa";
        if (!s.Esiste)
        {
            _statoPunto.ForeColor = Tema.Spento;
            _statoTesto.Text = $"nessun lettore ha mai scritto in {_root}  ·  {server}";
            return;
        }
        _statoPunto.ForeColor = s.Vivo ? Tema.Verde : Tema.Rosso;
        string chunk = s.UltimoChunkSecondiFa is { } fa ? $"chunk {Dati.Eta(fa)} fa" : "nessun chunk nuovo";
        _statoTesto.Text = (s.Vivo ? "lettore vivo" : $"lettore FERMO da {Dati.Eta(s.SecondiFa)}") +
                           $"  ·  {chunk}  ·  {server}" + (s.Errori > 0 ? $"  ·  {s.Errori} errori" : "");
        _statoTesto.ForeColor = s.Vivo ? Tema.Testo : Tema.Rosso;
    }

    private void AggiornaStato(Stato s)
    {
        var t = new Scrittore(_stato);
        t.Titolo("Il lettore");
        t.Riga("stato", !s.Esiste ? "mai avviato su questa cartella" : s.Vivo ? $"vivo (pid {s.Pid})" : $"fermo da {Dati.Eta(s.SecondiFa)}",
            s.Vivo ? Tema.Verde : Tema.Rosso);
        t.Riga("cartella dati", _root, Tema.Testo);
        t.Riga("chunk letti", $"{s.Chunk:N0}   errori {s.Errori}", s.Errori > 0 ? Tema.Rosso : Tema.Testo);
        if (s.UltimoErrore is not null) t.Riga("ultimo errore", s.UltimoErrore, Tema.Rosso);
        t.Riga("ultimo chunk", s.UltimoChunk + (s.UltimoChunkSecondiFa is { } f ? $"   ({Dati.Eta(f)} fa)" : ""), Tema.Testo);
        t.Riga("memoria", $"{Dati.N(s.RamMB, 0)} MB", Tema.Testo);
        t.Spazio();

        t.Titolo("La pagina");
        if (_server is null)
        {
            t.Riga("server", $"non risponde su {Pagina}", Tema.Rosso);
            t.Nota("Il server gira dentro il lettore (XrayLive --watch --serve): parte con Avvia (all'accesso non parte niente).");
        }
        else
        {
            var r = _server.RootElement;
            var srv = r.GetProperty("server");
            t.Riga("server", $"acceso su {Pagina}", Tema.Verde);
            t.Riga("da", Testo(srv, "avviato"), Tema.Testo);
            t.Riga("bridge ATAS", Testo(srv, "bridge") + (srv.TryGetProperty("bridgeErrore", out var be) && be.ValueKind == JsonValueKind.String
                ? $"   ({be.GetString()})" : "   (ES letto: prezzi in ES disponibili)"),
                srv.TryGetProperty("bridgeErrore", out var be2) && be2.ValueKind == JsonValueKind.String ? Tema.Giallo : Tema.Testo);
            t.Nota("Apri la pagina: Adesso, Per strike, Heat map, Nel tempo, Deriva, Livelli, Previsione.");
        }
        t.Spazio();

        t.Titolo("L'archivio");
        t.Riga("spazio", $"{Dati.N(s.ArchivioMB, 0)} MB in {s.Giornate} giornate", Tema.Testo);
        t.Riga("da questo avvio", $"{s.Scritte:N0} colonne scritte, {s.Doppie:N0} doppie saltate, {s.Revisioni:N0} revisioni", Tema.Testo);
        t.Nota("Una colonna si scrive una volta; le giornate chiuse si comprimono (.jsonl.gz).");
        t.Fine();
    }

    private void AggiornaFamiglie()
    {
        var ricevute = Dati.Ricevute(_root);
        string latest = Path.Combine(_root, "latest");
        var nomi = Directory.Exists(latest)
            ? Directory.GetFiles(latest, "*.json").Select(Path.GetFileNameWithoutExtension).OfType<string>().OrderBy(n => n).ToList()
            : new List<string>();
        _famiglie.Rows.Clear();
        foreach (var nome in nomi)
        {
            var col = Dati.Ultima(Dati.Latest(_root, nome));
            long ts = Dati.Lungo(col?["ts"]);
            double fa = ricevute.TryGetValue(nome, out var quando) ? (DateTime.UtcNow - quando).TotalSeconds : double.NaN;
            string dentro = ts > 0 ? Dati.OraNY(ts) + (ts > DateTimeOffset.UtcNow.ToUnixTimeMilliseconds() ? "  (futuro)" : "") : "-";
            int r = _famiglie.Rows.Add(nome, double.IsFinite(fa) ? Dati.Eta(fa) + " fa" : "?", dentro,
                Dati.Giorni(_root, nome).Count, Indicatori.GetValueOrDefault(nome.Split("__")[0], "?"));
            _famiglie.Rows[r].Cells[1].Style.ForeColor = !double.IsFinite(fa) ? Tema.Spento
                : fa < 900 ? Tema.Verde : fa < 10800 ? Tema.Giallo : Tema.Spento;
        }
    }

    /// <summary>Una PNG per pagina in `cartella` (XrayGui --foto).</summary>
    internal void Fotografa(string cartella)
    {
        Directory.CreateDirectory(cartella);
        foreach (var nome in _pagine.Keys.ToList())
        {
            Mostra(nome);
            for (int i = 0; i < 40; i++) { Application.DoEvents(); Thread.Sleep(25); }
            Aggiorna();
            for (int i = 0; i < 20; i++) { Application.DoEvents(); Thread.Sleep(25); }
            using var bmp = new Bitmap(Width, Height);
            DrawToBitmap(bmp, new Rectangle(0, 0, Width, Height));
            bmp.Save(Path.Combine(cartella, nome.Replace(' ', '-') + ".png"));
        }
    }

    // ================================================================ comandi

    private void ApriPagina()
    {
        Process.Start(new ProcessStartInfo(Pagina) { UseShellExecute = true });
        if (_server is null) Log($"la pagina non risponde ancora su {Pagina}: avvia il lettore (Avvia)");
    }

    private void Script(string argomento, Action? poi = null)
    {
        string script = Path.Combine(Config.Strumento(), "scripts", "xray_live_task.ps1");
        Log($"xray_live_task.ps1 {argomento}");
        var p = Avvia("powershell", "-NoProfile", "-ExecutionPolicy", "Bypass", "-File", script, argomento, "-OutDir", _root);
        p.Exited += (_, _) => BeginInvoke(() => { Aggiorna(); poi?.Invoke(); });
    }

    private void Avvia()
    {
        // con il task registrato lo si avvia (all'accesso non parte); senza, il lettore parte da qui
        var registrato = Avvia("powershell", "-NoProfile", "-Command",
            "if (Get-ScheduledTask -TaskName XrayLive -ErrorAction SilentlyContinue) { exit 0 } else { exit 1 }");
        registrato.Exited += (_, _) => BeginInvoke(() =>
        {
            if (registrato.ExitCode == 0)
            {
                Script("-Start");
                return;
            }
            string dll = Config.Lettore();
            if (!File.Exists(dll))
            {
                Log($"manca {dll}: compila con  dotnet build quant/strumenti/xray/XrayLive -c Release");
                return;
            }
            Avvia("dotnet", dll, "--watch", "--serve", "--interval", "15", "--out", _root);
            Log("lettore e pagina avviati da questa finestra (senza task: si fermano con Ferma o al riavvio)");
        });
    }

    private void Compatta()
    {
        string dll = Config.Lettore();
        if (MessageBox.Show(this, "Fermo il lettore, riscrivo l'archivio senza doppioni e lo riavvio.\n" +
                                  "L'archivio di adesso resta in series.vecchia finche' non lo cancelli.\n\nProcedo?",
                "Compatta archivio", MessageBoxButtons.YesNo, MessageBoxIcon.Question) != DialogResult.Yes) return;
        Script("-Stop", () =>
        {
            var p = Avvia("dotnet", dll, "--compatta", "--out", _root);
            p.Exited += (_, _) => BeginInvoke(() =>
            {
                Log($"compattazione finita (codice {p.ExitCode})");
                Avvia();
            });
        });
    }

    private void ScegliCartella()
    {
        using var dialog = new FolderBrowserDialog { SelectedPath = _root };
        if (dialog.ShowDialog(this) != DialogResult.OK) return;
        _root = dialog.SelectedPath;
        Config.Salva(_root);
        Aggiorna();
    }

    private void ApriCartella()
    {
        if (Directory.Exists(_root))
            Process.Start(new ProcessStartInfo("explorer.exe", $"\"{_root}\"") { UseShellExecute = true });
    }

    private Process Avvia(string file, params string[] args)
    {
        var psi = new ProcessStartInfo(file)
        {
            UseShellExecute = false,
            CreateNoWindow = true,
            RedirectStandardOutput = true,
            RedirectStandardError = true,
            WorkingDirectory = Config.Strumento(),
        };
        foreach (var arg in args) psi.ArgumentList.Add(arg);
        var process = new Process { StartInfo = psi, EnableRaisingEvents = true };
        process.OutputDataReceived += (_, e) => { if (!string.IsNullOrWhiteSpace(e.Data)) Log(e.Data); };
        process.ErrorDataReceived += (_, e) => { if (!string.IsNullOrWhiteSpace(e.Data)) Log(e.Data); };
        process.Start();
        process.BeginOutputReadLine();
        process.BeginErrorReadLine();
        return process;
    }

    private void Log(string line)
    {
        if (IsDisposed) return;
        if (InvokeRequired)
        {
            try { BeginInvoke(() => Log(line)); } catch (InvalidOperationException) { }
            return;
        }
        _log.AppendText($"{DateTime.Now:HH:mm:ss}  {line}{Environment.NewLine}");
    }

    // ================================================================ pezzi

    private static string Testo(JsonElement e, string nome) =>
        e.TryGetProperty(nome, out var v) && v.ValueKind == JsonValueKind.String ? v.GetString() ?? "" : "";

    private static RichTextBox Testo() => Tema.Scuro(new RichTextBox
    {
        Dock = DockStyle.Fill, ReadOnly = true, BorderStyle = BorderStyle.None, BackColor = Tema.Pannello,
        ForeColor = Tema.Testo, Font = Tema.Base, DetectUrls = false, ScrollBars = RichTextBoxScrollBars.Vertical,
    });

    private static DataGridView Griglia()
    {
        var g = Tema.Scuro(new DataGridView
        {
            Dock = DockStyle.Fill, ReadOnly = true, AllowUserToAddRows = false, AllowUserToResizeRows = false,
            RowHeadersVisible = false, SelectionMode = DataGridViewSelectionMode.FullRowSelect, MultiSelect = false,
            AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill, BackgroundColor = Tema.Pannello,
            BorderStyle = BorderStyle.None, GridColor = Tema.Bordo, EnableHeadersVisualStyles = false,
            CellBorderStyle = DataGridViewCellBorderStyle.SingleHorizontal,
            ColumnHeadersBorderStyle = DataGridViewHeaderBorderStyle.None, ColumnHeadersHeight = 32,
        });
        g.ColumnHeadersDefaultCellStyle.SelectionBackColor = Tema.Sfondo;
        g.ColumnHeadersDefaultCellStyle.SelectionForeColor = Tema.Spento;
        g.RowTemplate.Height = 28;
        g.DefaultCellStyle.BackColor = Tema.Pannello;
        g.DefaultCellStyle.ForeColor = Tema.Testo;
        g.DefaultCellStyle.SelectionBackColor = Tema.PannelloChiaro;
        g.DefaultCellStyle.SelectionForeColor = Tema.Testo;
        g.DefaultCellStyle.Font = Tema.Base;
        g.ColumnHeadersDefaultCellStyle.BackColor = Tema.Sfondo;
        g.ColumnHeadersDefaultCellStyle.ForeColor = Tema.Spento;
        g.ColumnHeadersDefaultCellStyle.Font = Tema.Base;
        return g;
    }

    /// <summary>Scrive il testo colorato di una RichTextBox senza farla sfarfallare.</summary>
    private sealed class Scrittore
    {
        private readonly RichTextBox _box;
        private readonly RichTextBox _buffer = new();

        public Scrittore(RichTextBox box)
        {
            _box = box;
            _buffer.Font = box.Font;
        }

        public void Titolo(string s) => Scrivi(s + "\n", Tema.Titolo, Tema.Testo);
        public void Nota(string s) => Scrivi(s + "\n", Tema.Piccolo, Tema.Spento);
        public void Spazio() => Scrivi("\n", Tema.Piccolo, Tema.Spento);

        public void Riga(string etichetta, string valore, Color colore)
        {
            Scrivi(etichetta.PadRight(16), Tema.Mono, Tema.Spento);
            Scrivi(valore + "\n", Tema.Mono, colore);
        }

        private void Scrivi(string s, Font f, Color c)
        {
            _buffer.SelectionStart = _buffer.TextLength;
            _buffer.SelectionFont = f;
            _buffer.SelectionColor = c;
            _buffer.AppendText(s);
        }

        public void Fine()
        {
            if (_box.Rtf != _buffer.Rtf) _box.Rtf = _buffer.Rtf;
            _buffer.Dispose();
        }
    }
}
