using System;

namespace YorkshireDigital.Data.Helpers
{
    public static class DateTimeExtension
    {
        public static DateTime TruncateToSeconds(this DateTime source)
        {
            return new DateTime(source.Ticks - (source.Ticks % TimeSpan.TicksPerSecond), source.Kind);
        }
    }
}
