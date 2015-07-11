namespace YorkshireDigital.Web.Infrastructure
{
    using System;

    public static class Extensions
    {
        public static string DateSuffix(this DateTime date)
        {
            return (date.Day % 10 == 1 && date.Day != 11) ? "st"
                : (date.Day % 10 == 2 && date.Day != 12) ? "nd"
                : (date.Day % 10 == 3 && date.Day != 13) ? "rd"
                : "th";
        }
        
        public static string ToLyndensFancyFormat(this DateTime date)
        {
            var local = date.ToLocalTime();
            return string.Format("{0}{1} {2}<sup>{3}</sup> {4}", local.ToString("h:mm"), local.ToString("tt").ToLower(),
                local.ToString("dddd, d"), local.DateSuffix(), local.ToString("MMMM"));
        }
    }
}