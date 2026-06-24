using System;

namespace FabioTrendFollowing;

internal static class MarketTimeZones
{
    private const string ItalyTimeZoneId = "W. Europe Standard Time";
    private const string LondonTimeZoneId = "GMT Standard Time";
    private const string NewYorkTimeZoneId = "Eastern Standard Time";

    private static readonly Lazy<TimeZoneInfo> ItalyTimeZone = new(CreateItalyTimeZone);
    private static readonly Lazy<TimeZoneInfo> LondonTimeZone = new(() => ResolveRequiredTimeZone(LondonTimeZoneId));
    private static readonly Lazy<TimeZoneInfo> NewYorkTimeZone = new(() => ResolveRequiredTimeZone(NewYorkTimeZoneId));

    internal static TimeZoneInfo Italy => ItalyTimeZone.Value;
    internal static TimeZoneInfo London => LondonTimeZone.Value;
    internal static TimeZoneInfo NewYork => NewYorkTimeZone.Value;

    internal static DateTime ToItaly(DateTime utcTime) => TimeZoneInfo.ConvertTimeFromUtc(utcTime, Italy);
    internal static DateTime ToLondon(DateTime utcTime) => TimeZoneInfo.ConvertTimeFromUtc(utcTime, London);
    internal static DateTime ToNewYork(DateTime utcTime) => TimeZoneInfo.ConvertTimeFromUtc(utcTime, NewYork);

    internal static string FormatUtcLondonItaly(DateTime utcTime)
    {
        var italyTime = ToItaly(utcTime);
        var londonTime = ToLondon(utcTime);
        return $"Italy={italyTime:yyyy-MM-dd HH:mm}, London={londonTime:yyyy-MM-dd HH:mm}, UTC={utcTime:yyyy-MM-dd HH:mm:ss}";
    }

    private static TimeZoneInfo CreateItalyTimeZone()
    {
        try
        {
            return TimeZoneInfo.FindSystemTimeZoneById(ItalyTimeZoneId);
        }
        catch
        {
            return TimeZoneInfo.Local;
        }
    }

    private static TimeZoneInfo ResolveRequiredTimeZone(string timeZoneId)
    {
        try
        {
            return TimeZoneInfo.FindSystemTimeZoneById(timeZoneId);
        }
        catch (TimeZoneNotFoundException ex)
        {
            throw new InvalidOperationException($"Timezone not found: {timeZoneId}", ex);
        }
    }
}
