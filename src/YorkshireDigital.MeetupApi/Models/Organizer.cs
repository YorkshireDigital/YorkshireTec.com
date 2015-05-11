namespace YorkshireDigital.MeetupApi.Models
{
    using Newtonsoft.Json;

    public class Organizer
    {
        [JsonProperty(PropertyName = "member_id")]
        public int MemberId { get; set; }
        public string Name { get; set; }
    }
}