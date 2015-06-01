namespace YorkshireDigital.MeetupApi.Models
{
    using System;
    using System.Collections.Generic;
    using Newtonsoft.Json;
    using YorkshireDigital.MeetupApi.Helpers;

    public class Group
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
        [JsonProperty(PropertyName = "utc_offset")]
        public int UtcOffset { get; set; }
        public string Country { get; set; }
        public string Visibility { get; set; }
        public string City { get; set; }
        public string Timezone { get; set; }
        public List<Topic> Topics { get; set; }
        public string Link { get; set; }
        public double Rating { get; set; }
        public string Description { get; set; }
        public double Lon { get; set; }
        [JsonProperty(PropertyName = "group_photo")]
        public GroupPhoto GroupPhoto { get; set; }
        public Organizer Organizer { get; set; }
        public int Members { get; set; }
        public string State { get; set; }
        public Category Category { get; set; }
        public double Lat { get; set; }

        public DateTime CreatedDate
        {
            get
            {
                return DateHelpers.MeetupTimeStampToDateTime(Created);
            }
        }
    }
}
