namespace XrayLive.Server;

/// <summary>
/// Le giornate gia' lette, per famiglia. Una richiesta prende la giornata e la porta al passo con
/// i file (solo le righe nuove); le giornate non usate da 10 minuti si liberano, perche' la
/// previsione di un giorno pesa decine di MB (il 2 novembre 2026, dopo aver sfogliato molte
/// giornate, il processo era arrivato a 828 MB).
/// </summary>
internal sealed class Magazzino
{
    private readonly string _root;
    private readonly Dictionary<string, GiornataLetta> _giornate = new(StringComparer.OrdinalIgnoreCase);
    private readonly object _lock = new();
    private DateTime _pulizia = DateTime.UtcNow;

    public Magazzino(string root) => _root = root;

    public string Root => _root;

    public IReadOnlyList<ColStrike> Strike(string famiglia, string giorno) =>
        Prendi(famiglia, giorno, Lettura.Strike, c => c.Ts);

    public IReadOnlyList<ColPiano> Piano(string famiglia, string giorno) =>
        Prendi(famiglia, giorno, Lettura.Piano, c => c.Ts);

    public IReadOnlyList<ColPressione> Pressione(string giorno) =>
        Prendi("pressure__0dte", giorno, Lettura.Pressione, c => c.Ts);

    public IReadOnlyList<ColLivelli> Livelli(string giorno) =>
        Prendi("levels__gamma", giorno, Lettura.Livelli, c => c.Ts);

    private IReadOnlyList<T> Prendi<T>(string famiglia, string giorno,
        Func<System.Text.Json.JsonElement, System.Text.Json.JsonElement, (string, T)?> leggi, Func<T, long> ts)
    {
        string percorso = Lettura.Percorso(_root, famiglia, giorno);
        Giornata<T> g;
        lock (_lock)
        {
            if (!_giornate.TryGetValue(percorso, out var o))
                _giornate[percorso] = o = new Giornata<T>();
            g = (Giornata<T>)o;
            Pulisci();
        }
        lock (g)
        {
            Lettura.Aggiorna(g, percorso, leggi, ts);
            return g.Colonne.ToArray();
        }
    }

    private void Pulisci()
    {
        if (DateTime.UtcNow - _pulizia < TimeSpan.FromMinutes(1)) return;
        _pulizia = DateTime.UtcNow;
        int prima = _giornate.Count;
        foreach (var (k, o) in _giornate.ToList())
        {
            if (DateTime.UtcNow - o.Usata > TimeSpan.FromMinutes(10)) _giornate.Remove(k);
        }
        // le previsioni pesano: se ne tengono al massimo quattro giornate, le piu' usate di recente
        var pesanti = _giornate.Where(kv => kv.Key.Contains("forecast__") || kv.Key.Contains("nextday__"))
            .OrderByDescending(kv => kv.Value.Usata).Skip(4).Select(kv => kv.Key).ToList();
        foreach (var k in pesanti) _giornate.Remove(k);
        if (_giornate.Count < prima) GC.Collect(2, GCCollectionMode.Optimized, blocking: false, compacting: true);
    }
}
