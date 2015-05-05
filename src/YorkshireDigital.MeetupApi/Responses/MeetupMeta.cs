namespace YorkshireDigital.MeetupApi.Responses
{
    using System;
    using Newtonsoft.Json;
    using YorkshireDigital.MeetupApi.Helpers;

    public class MeetupMeta
    {
        public string Next { get; set; }
        public string Method { get; set; }
        [JsonProperty(PropertyName = "total_count")]
        public int TotalCount { get; set; }
        public string Link { get; set; }
        public int Count { get; set; }
        public string Description { get; set; }
        public string Lon { get; set; }
        public string Title { get; set; }
        public string Url { get; set; }
        public string Id { get; set; }
        public double Updated { get; set; }
        public string Lat { get; set; }

        public DateTime UpdatedDate
        {
            get
            {
                return DateHelpers.MeetupTimeStampToDateTime(Updated);
            }
        }
    }
}
