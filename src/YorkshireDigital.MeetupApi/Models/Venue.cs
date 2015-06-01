namespace YorkshireDigital.MeetupApi.Models
{
    using Newtonsoft.Json;

    public class Venue
    {
        public string Country { get; set; }
        public string City { get; set; }
        [JsonProperty(PropertyName = "address_1")]
        public string Address1 { get; set; }
        public string Name { get; set; }
        public double Lon { get; set; }
        public int Id { get; set; }
        public double Lat { get; set; }
        public bool Repinned { get; set; }
    }
}
