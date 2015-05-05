namespace YorkshireDigital.MeetupApi.Responses
{
    using System;
    using Newtonsoft.Json;
    using YorkshireDigital.MeetupApi.Helpers;

    public class MeetupGroupResponse
    {
        [JsonProperty(PropertyName = "join_mode")]
        public string JoinMode { get; set; }
        public double Created { get; set; }
        public string Name { get; set; }
        [JsonProperty(PropertyName = "group_lon")]
        public double GroupLon { get; set; }
        public int Id { get; set; }
        public string UrlName { get; set; }
        [JsonProperty(PropertyName = "group_lat")]
        public double GroupLat { get; set; }
        public string Who { get; set; }

        public DateTime CreatedDate
        {
            get
            {
                return DateHelpers.MeetupTimeStampToDateTime(Created);
            }
        }
    }
}
