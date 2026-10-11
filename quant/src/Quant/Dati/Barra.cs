using System.Runtime.InteropServices;

namespace Quant.Dati;

/// <summary>
/// Una barra, come sta su disco: 44 byte, layout fisso, letta senza conversioni.
/// I prezzi sono in tick. Volume bid = aggressore venditore, volume ask = aggressore compratore
/// (la convenzione di ATAS).
/// </summary>
[StructLayout(LayoutKind.Sequential, Pack = 4)]
public struct Barra
{
    /// <summary>Apertura della barra, millisecondi Unix UTC.</summary>
    public long Inizio;
    /// <summary>Ultimo scambio della barra, millisecondi Unix UTC. Conta per le barre a range.</summary>
    public long Fine;
    public int Apertura;
    public int Massimo;
    public int Minimo;
    public int Chiusura;
    public int Volume;
    public int VolumeBid;
    public int VolumeAsk;

    public readonly int Delta => VolumeAsk - VolumeBid;

    public override readonly string ToString() =>
        $"{Tempo.Utc(Inizio):yyyy-MM-dd HH:mm}Z O{Apertura} H{Massimo} L{Minimo} C{Chiusura} V{Volume}";
}

/// <summary>
/// Uno scambio del tape, 20 byte. Lato: +1 compratore aggressore, -1 venditore, 0 ignoto.
/// </summary>
[StructLayout(LayoutKind.Sequential, Pack = 4)]
public struct Scambio
{
    public long Tempo;
    public int Prezzo;
    public int Volume;
    public int Lato;
}

public static class Tempo
{
    public static DateTime Utc(long ms) => DateTime.UnixEpoch.AddMilliseconds(ms);

    public static long Ms(DateTime utc) =>
        (long)(DateTime.SpecifyKind(utc, DateTimeKind.Utc) - DateTime.UnixEpoch).TotalMilliseconds;

    public const long Minuto = 60_000;
    public const long Ora = 3_600_000;
    public const long Giorno = 86_400_000;
}
