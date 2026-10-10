using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Threading.Tasks;

namespace FabioOrderFlow.Observation;

/// <summary>
/// L'interfaccia di ATAS letta da dentro: le finestre, le tab, e la tab da mostrare.
///
/// <para><b>Perche' da dentro.</b> Il 9 ottobre 2026 l'agente doveva controllare cosa e' disegnato su
/// ognuna delle quattro tab del workspace, e da fuori non poteva: i clic simulati non arrivano ad ATAS
/// (che gira con privilegi diversi), e computer use da terminale su Windows non esiste. L'indicatore
/// invece gira nel processo di ATAS, sul suo stesso albero di controlli Avalonia: puo' elencare le
/// tab e selezionarne una sul thread dell'interfaccia. Lo screenshot lo fa poi lo script, ritagliato
/// sulla sola finestra di ATAS.</para>
///
/// <para><b>Tutto per reflection</b>, senza riferimenti di compilazione ad Avalonia: la versione e'
/// quella che ATAS carica, e cambia con gli aggiornamenti.</para>
/// </summary>
public sealed partial class DataBridge
{
    private static Type? TipoCaricato(string nome) => AppDomain.CurrentDomain.GetAssemblies()
        .Select(a =>
        {
            try
            {
                return a.GetType(nome, false);
            }
            catch (Exception)
            {
                return null;
            }
        })
        .FirstOrDefault(t => t is not null);

    /// <summary>Esegue sul thread dell'interfaccia di Avalonia e aspetta il risultato.</summary>
    private static Task<T> SulThreadDellInterfaccia<T>(Func<T> lavoro)
    {
        var tcs = new TaskCompletionSource<T>(TaskCreationOptions.RunContinuationsAsynchronously);
        var dispatcher = TipoCaricato("Avalonia.Threading.Dispatcher")
                         ?? throw new BridgeException(500, "Avalonia.Threading.Dispatcher non trovato");
        var ui = dispatcher.GetProperty("UIThread", BindingFlags.Public | BindingFlags.Static)!.GetValue(null);
        Action azione = () =>
        {
            try
            {
                tcs.SetResult(lavoro());
            }
            catch (Exception errore)
            {
                tcs.SetException(errore);
            }
        };
        var post = dispatcher.GetMethods(BindingFlags.Public | BindingFlags.Instance)
            .First(m => m.Name == "Post" && m.GetParameters().Length >= 1 && m.GetParameters()[0].ParameterType == typeof(Action));
        var argomenti = post.GetParameters().Select((p, i) => i == 0 ? azione : p.IsOptional ? Type.Missing
            : p.ParameterType.IsValueType ? Activator.CreateInstance(p.ParameterType) : null).ToArray();
        post.Invoke(ui, BindingFlags.OptionalParamBinding, null, argomenti, null);
        return tcs.Task;
    }

    private static IEnumerable<object> LeFinestre()
    {
        var app = TipoCaricato("Avalonia.Application")?.GetProperty("Current")?.GetValue(null);
        var vita = app?.GetType().GetProperty("ApplicationLifetime")?.GetValue(app);
        return vita?.GetType().GetProperty("Windows")?.GetValue(vita) is IEnumerable finestre
            ? finestre.Cast<object>().ToList()
            : new List<object>();
    }

    private static IEnumerable<object> IFigliVisivi(object nodo)
    {
        var estensioni = TipoCaricato("Avalonia.VisualTree.VisualExtensions");
        var metodo = estensioni?.GetMethods(BindingFlags.Public | BindingFlags.Static)
            .FirstOrDefault(m => m.Name == "GetVisualChildren" && m.GetParameters().Length == 1);
        if (metodo is null || !metodo.GetParameters()[0].ParameterType.IsInstanceOfType(nodo))
        {
            return Array.Empty<object>();
        }

        return metodo.Invoke(null, new[] { nodo }) is IEnumerable figli ? figli.Cast<object>() : Array.Empty<object>();
    }

    /// <summary>Le proprieta' di testo e di stato che servono a riconoscere un controllo.</summary>
    private static Dictionary<string, object?> IlControllo(object nodo)
    {
        var t = nodo.GetType();
        var d = new Dictionary<string, object?> { ["tipo"] = t.FullName };
        foreach (var nome in new[] { "Name", "Header", "Title", "Text", "Content", "IsSelected", "SelectedIndex", "IsVisible", "Bounds", "DataContext" })
        {
            var p = t.GetProperty(nome, BindingFlags.Public | BindingFlags.Instance);
            if (p is null || p.GetIndexParameters().Length > 0)
            {
                continue;
            }

            try
            {
                var v = p.GetValue(nodo);
                if (v is null)
                {
                    continue;
                }

                d[nome] = v is string or bool or int ? v : nome == "DataContext" ? v.GetType().FullName + " | " + v : v.ToString();
            }
            catch (Exception)
            {
                // Una proprieta' che non si legge non e' un motivo per fermare il giro.
            }
        }

        return d;
    }

    private static class Win
    {
        [DllImport("user32.dll")] public static extern IntPtr GetForegroundWindow();
        [DllImport("user32.dll")] public static extern bool SetForegroundWindow(IntPtr h);
        [DllImport("user32.dll")] public static extern bool ShowWindow(IntPtr h, int cmd);
        [DllImport("user32.dll")] public static extern bool IsIconic(IntPtr h);
        [DllImport("user32.dll")] public static extern bool BringWindowToTop(IntPtr h);
        [DllImport("user32.dll")] public static extern void keybd_event(byte vk, byte scan, uint flags, UIntPtr extra);

        /// <summary>
        /// Windows nega il primo piano a chi non ha l'ultimo input: un ALT premuto e rilasciato dal
        /// processo stesso glielo concede. E' la tecnica documentata dai limiti di SetForegroundWindow.
        /// </summary>
        public static void Davanti(IntPtr h)
        {
            if (h == IntPtr.Zero)
            {
                return;
            }

            if (IsIconic(h))
            {
                ShowWindow(h, 9); // SW_RESTORE
            }

            keybd_event(0x12, 0, 0, UIntPtr.Zero);
            keybd_event(0x12, 0, 2, UIntPtr.Zero);
            BringWindowToTop(h);
            SetForegroundWindow(h);
        }
    }

    /// <summary>Porta la finestra di ATAS davanti e restituisce quella che c'era prima.</summary>
    private static long PortaDavanti(object finestra)
    {
        var prima = Win.GetForegroundWindow();
        var gestore = finestra.GetType().GetMethod("TryGetPlatformHandle", Type.EmptyTypes)?.Invoke(finestra, null);
        var h = gestore?.GetType().GetProperty("Handle")?.GetValue(gestore) is IntPtr p ? p : IntPtr.Zero;
        Win.Davanti(h);
        finestra.GetType().GetMethod("Activate", Type.EmptyTypes)?.Invoke(finestra, null);
        return prima == h ? 0 : prima.ToInt64();
    }

    /// <summary>
    /// Seleziona una tab — per numero da 1, o per l'inizio del titolo — e restituisce dove sta la
    /// finestra sullo schermo, in pixel veri, perche' lo script ritagli lo screenshot su ATAS e
    /// su nient'altro. Con <paramref name="davanti"/> la finestra viene portata in primo piano.
    /// </summary>
    private static object MostraLaTab(string quale, bool davanti)
    {
        var finestra = LeFinestre().FirstOrDefault(f => f.GetType().Name == "MainWindow")
                       ?? throw new BridgeException(500, "finestra principale di ATAS non trovata");
        var vm = finestra.GetType().GetProperty("DataContext")!.GetValue(finestra)!;
        var tabs = ((IEnumerable)vm.GetType().GetProperty("Tabs")!.GetValue(vm)!).Cast<object>().ToList();
        string Titolo(object t) => t.GetType().GetProperty("Header")?.GetValue(t) as string ?? string.Empty;

        var scelta = int.TryParse(quale, out var n) && n >= 1 && n <= tabs.Count
            ? tabs[n - 1]
            : tabs.FirstOrDefault(t => Titolo(t).StartsWith(quale, StringComparison.OrdinalIgnoreCase));
        if (scelta is null)
        {
            throw new BridgeException(404, $"nessuna tab '{quale}': ci sono {string.Join(" | ", tabs.Select(Titolo))}");
        }

        var comando = scelta.GetType().GetProperty("ActivateCommand")?.GetValue(scelta) as System.Windows.Input.ICommand;
        if (comando is not null)
        {
            comando.Execute(null);
        }
        else
        {
            vm.GetType().GetProperty("SelectedTab")!.SetValue(vm, scelta);
        }

        long prima = 0;
        if (davanti)
        {
            prima = PortaDavanti(finestra);
        }

        // Position e' in pixel dello schermo; Bounds in pixel logici, da moltiplicare per la scala.
        var posizione = finestra.GetType().GetProperty("Position")?.GetValue(finestra);
        int Leggi(object? o, string nome) => Convert.ToInt32(o?.GetType().GetProperty(nome)?.GetValue(o) ?? 0);
        var scala = Convert.ToDouble(finestra.GetType().GetProperty("RenderScaling")?.GetValue(finestra) ?? 1.0);
        var bordi = finestra.GetType().GetProperty("Bounds")?.GetValue(finestra);
        double Misura(string nome) => Convert.ToDouble(bordi?.GetType().GetProperty(nome)?.GetValue(bordi) ?? 0.0);
        var stato = finestra.GetType().GetProperty("WindowState")?.GetValue(finestra)?.ToString();

        return new
        {
            schema = Schema,
            selezionata = Titolo(scelta),
            primaInPrimoPiano = prima,
            tabs = tabs.Select(Titolo),
            finestra = new
            {
                x = Leggi(posizione, "X"),
                y = Leggi(posizione, "Y"),
                larghezza = (int)Math.Round(Misura("Width") * scala),
                altezza = (int)Math.Round(Misura("Height") * scala),
                scala,
                stato,
            },
        };
    }

    /// <summary>
    /// Un tipo caricato in ATAS, per nome intero o semplice: i suoi membri e, se e' un'interfaccia o
    /// una classe base, chi lo implementa. Serve a trovare le API che la documentazione non dice.
    /// </summary>
    private static object DescriviIlTipo(string nome)
    {
        var tipi = AppDomain.CurrentDomain.GetAssemblies().SelectMany(a =>
        {
            try
            {
                return a.GetTypes();
            }
            catch (ReflectionTypeLoadException e)
            {
                return e.Types.Where(t => t is not null).Cast<Type>().ToArray();
            }
            catch (Exception)
            {
                return Array.Empty<Type>();
            }
        }).ToList();
        var trovati = tipi.Where(t => t.FullName == nome || t.Name == nome).ToList();
        return new
        {
            schema = Schema,
            tipi = trovati.Select(t => new
            {
                nome = t.FullName,
                assembly = t.Assembly.GetName().Name,
                interfaccia = t.IsInterface,
                basi = t.BaseType?.FullName,
                membri = t.GetMembers(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Static | BindingFlags.DeclaredOnly)
                    .Select(m => m switch
                    {
                        MethodInfo mi => $"{(mi.IsStatic ? "static " : "")}{mi.ReturnType.Name} {mi.Name}({string.Join(", ", mi.GetParameters().Select(x => x.ParameterType.Name + " " + x.Name))})",
                        PropertyInfo pi => $"prop {pi.PropertyType.Name} {pi.Name}",
                        FieldInfo fi => $"field {fi.FieldType.Name} {fi.Name}",
                        _ => $"{m.MemberType} {m.Name}",
                    }).Where(x => !x.Contains("<", StringComparison.Ordinal)).Distinct().Take(150),
                implementatori = tipi.Where(x => x != t && t.IsAssignableFrom(x)).Select(x => x.FullName).Take(40),
            }),
        };
    }

    /// <summary>
    /// Le proprieta' del view model della finestra principale e di quelli che contiene, fino al
    /// percorso <paramref name="percorso"/> ("" = il principale, "Tabs" = una sua proprieta').
    /// Serve a trovare come ATAS tiene la tab selezionata.
    /// </summary>
    private static object IViewModel(string percorso)
    {
        object? o = LeFinestre().FirstOrDefault(f => f.GetType().Name == "MainWindow");
        o = o?.GetType().GetProperty("DataContext")?.GetValue(o);
        foreach (var passo in percorso.Split('.', StringSplitOptions.RemoveEmptyEntries))
        {
            if (o is null)
            {
                break;
            }

            if (int.TryParse(passo, out var indice) && o is IEnumerable lista)
            {
                o = lista.Cast<object>().ElementAtOrDefault(indice);
                continue;
            }

            o = o.GetType().GetProperty(passo, BindingFlags.Public | BindingFlags.Instance)?.GetValue(o);
        }

        if (o is null)
        {
            return new { errore = "niente a questo percorso" };
        }

        var proprieta = new List<object>();
        foreach (var p in o.GetType().GetProperties(BindingFlags.Public | BindingFlags.Instance).Where(p => p.GetIndexParameters().Length == 0))
        {
            string valore;
            try
            {
                var v = p.GetValue(o);
                valore = v is null ? "null"
                    : v is string or bool or int or double or decimal or Enum ? v.ToString()!
                    : v is IEnumerable e ? $"[{e.Cast<object>().Count()}] {v.GetType().Name}"
                    : v.GetType().FullName!;
            }
            catch (Exception errore)
            {
                valore = "errore: " + errore.GetType().Name;
            }

            proprieta.Add(new { nome = p.Name, tipo = p.PropertyType.Name, scrivibile = p.CanWrite && p.SetMethod?.IsPublic == true, valore });
        }

        var metodi = o.GetType().GetMethods(BindingFlags.Public | BindingFlags.Instance | BindingFlags.DeclaredOnly)
            .Where(m => !m.IsSpecialName).Select(m => $"{m.Name}({string.Join(", ", m.GetParameters().Select(x => x.ParameterType.Name))})").Distinct().ToList();
        return new { tipo = o.GetType().FullName, proprieta, metodi };
    }

    /// <summary>
    /// <c>/ui</c>: le finestre di ATAS e, con <c>filtro</c>, i controlli il cui tipo o testo lo
    /// contiene, col percorso dei tipi che li contengono. <c>profondita</c> limita la discesa.
    /// </summary>
    private async Task<object> InterfacciaAsync(System.Collections.Specialized.NameValueCollection query)
    {
        if (query["ridai"] is { } ridai && long.TryParse(ridai, out var hwnd))
        {
            // Rimette in primo piano la finestra che c'era prima delle foto: e' ATAS a farlo, che in
            // quel momento e' davanti e quindi puo'.
            return await SulThreadDellInterfaccia(() =>
            {
                Win.Davanti(new IntPtr(hwnd));
                return (object)new { schema = Schema, ridata = hwnd };
            }).ConfigureAwait(false);
        }

        if (query["tab"] is { } tab)
        {
            var davanti = query["davanti"] is "1" or "true";
            return await SulThreadDellInterfaccia(() => MostraLaTab(tab, davanti)).ConfigureAwait(false);
        }

        if (query["descrivi"] is { } nomeTipo)
        {
            return DescriviIlTipo(nomeTipo);
        }

        if (query["vm"] is { } vm)
        {
            return await SulThreadDellInterfaccia(() => (object)new { schema = Schema, oggetti = IViewModel(vm) }).ConfigureAwait(false);
        }

        var filtro = query["filtro"] ?? "Tab";
        var profondita = int.TryParse(query["profondita"], out var p) ? p : 40;
        var massimo = int.TryParse(query["massimo"], out var m) ? m : 300;

        return await SulThreadDellInterfaccia(() =>
        {
            var finestre = new List<object>();
            foreach (var f in LeFinestre())
            {
                var trovati = new List<object>();
                var visti = 0;
                void Scendi(object nodo, int livello, string percorso)
                {
                    if (livello > profondita || trovati.Count >= massimo)
                    {
                        return;
                    }

                    visti++;
                    var c = IlControllo(nodo);
                    var testo = string.Join(" ", c.Values.Select(v => v?.ToString()));
                    if (testo.Contains(filtro, StringComparison.OrdinalIgnoreCase))
                    {
                        c["livello"] = livello;
                        c["percorso"] = percorso;
                        trovati.Add(c);
                    }

                    var nomeCorto = nodo.GetType().Name;
                    foreach (var figlio in IFigliVisivi(nodo))
                    {
                        Scendi(figlio, livello + 1, livello < 12 ? percorso + "/" + nomeCorto : percorso);
                    }
                }

                Scendi(f, 0, string.Empty);
                finestre.Add(new { finestra = IlControllo(f), controlliVisti = visti, trovati });
            }

            return (object)new { schema = Schema, filtro, finestre };
        }).ConfigureAwait(false);
    }
}
