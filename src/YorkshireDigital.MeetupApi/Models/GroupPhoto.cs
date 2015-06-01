namespace YorkshireDigital.MeetupApi.Models
{
    using Newtonsoft.Json;

    public class GroupPhoto
    {
        [JsonProperty(PropertyName = "highres_link")]
        public string HighresLink { get; set; }
        [JsonProperty(PropertyName = "photo_id")]
        public int PhotoId { get; set; }
        [JsonProperty(PropertyName = "photo_link")]
        public string PhotoLink { get; set; }
        [JsonProperty(PropertyName = "thumb_link")]
        public string ThumbLink { get; set; }
    }
}