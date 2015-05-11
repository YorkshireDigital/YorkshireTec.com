namespace YorkshireDigital.MeetupApi.Models
{
    using System;
    using Newtonsoft.Json;
    using YorkshireDigital.MeetupApi.Helpers;

    public class Event
    {
        [JsonProperty(PropertyName = "utc_offset")]
        public int UtcOffset { get; set; }
        public Venue Venue { get; set; }
        [JsonProperty(PropertyName = "rsvp_limit")]
        public int RsvpLimit { get; set; }
        public int Headcount { get; set; }
        public string Visibility { get; set; }
        [JsonProperty(PropertyName = "waitlist_count")]
        public int WaitlistCount { get; set; }
        public double Created { get; set; }
        [JsonProperty(PropertyName = "maybe_rsvp_count")]
        public int MaybeRsvpCount { get; set; }
        public string Description { get; set; }
        [JsonProperty(PropertyName = "event_url")]
        public string EventUrl { get; set; }
        [JsonProperty(PropertyName = "yes_rsvp_count")]
        public int YesRsvpCount { get; set; }
        public string Name { get; set; }
        public string Id { get; set; }
        public double Time { get; set; }
        public double Updated { get; set; }
        public Group Group { get; set; }
        public string Status { get; set; }
        public int? Duration { get; set; }


        public DateTime CreatedDate
        {
            get
            {
                return DateHelpers.MeetupTimeStampToDateTime(Created);
            }
        }
        public DateTime StartDate
        {
            get
            {
                return DateHelpers.MeetupTimeStampToDateTime(Time);
            }
        }
        public DateTime UpdatedDate
        {
            get
            {
                return DateHelpers.MeetupTimeStampToDateTime(Updated);
            }
        }
    }
}
