namespace YorkshireDigital.MeetupApi.Helpers
{
    using System;
    using System.ComponentModel;

    public static class DateHelpers
    {
        public static DateTime MeetupTimeStampToDateTime(double unixTimeStamp)
        {
            // Timestamp is milliseconds past epoch
            var epoch = new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);
            epoch = epoch.AddMilliseconds(unixTimeStamp);
            return epoch;
        }
        public static double DateTimeToMeetupTimeStamp(DateTime date)
        {
            var timespan = (date - new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc));

            var meetupTimestamp = timespan.TotalMilliseconds;

            return meetupTimestamp;
        }

        public static string GetDescriptionValue<T>(this T model, string member)
        {
            var type = typeof(T);
            var memInfo = type.GetMember(member);
            var attributes = memInfo[0].GetCustomAttributes(typeof(DescriptionAttribute), false);
            return ((DescriptionAttribute)attributes[0]).Description;
        }
    }
}
