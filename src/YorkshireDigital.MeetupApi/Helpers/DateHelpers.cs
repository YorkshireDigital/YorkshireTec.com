namespace YorkshireDigital.MeetupApi.Helpers
{
    using System;

    public class DateHelpers
    {
        public static DateTime MeetupTimeStampToDateTime(double unixTimeStamp)
        {
            // Timestamp is milliseconds past epoch
            var epoch = new DateTime(1970, 1, 1, 0, 0, 0, 0, System.DateTimeKind.Utc);
            epoch = epoch.AddMilliseconds(unixTimeStamp).ToLocalTime();
            return epoch;
        }
    }
}
