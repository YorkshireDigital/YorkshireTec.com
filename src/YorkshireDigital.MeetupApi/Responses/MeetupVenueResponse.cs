namespace YorkshireDigital.MeetupApi.Responses
{
    using Newtonsoft.Json;

    public class MeetupVenueResponse
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
