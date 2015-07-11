namespace YorkshireDigital.MeetupApi.Models
{
    using Newtonsoft.Json;

    public class Profile
    {
        [JsonProperty(PropertyName = "member_id")]
        public int MemberId { get; set; }
        [JsonProperty(PropertyName = "other_services")]
        public object OtherServices { get; set; }
        [JsonProperty(PropertyName = "profile_url")]
        public string ProfileUrl { get; set; }
        public long Created { get; set; }
        public string Name { get; set; }
        public long Visited { get; set; }
        public Photo Photo { get; set; }
        [JsonProperty(PropertyName = "photo_url")]
        public string PhotoUrl { get; set; }
        public long Updated { get; set; }
        public string Status { get; set; }
        public Group Group { get; set; }
    }
}
