namespace YorkshireDigital.MeetupApi.Requests
{
    using System.ComponentModel;
    using YorkshireDigital.MeetupApi.Requests.Enum;

    public class EventsRequest : BaseRequest
    {
        public EventsRequest()
            : base("events")
        {
        }

        [Description("event_id")]
        public string EventId { get; set; }
        [Description("group_domain")]
        public string GroupDomain { get; set; }
        [Description("group_id")]
        public int GroupId { get; set; }
        [Description("group_urlname")]
        public string GroupUrlName { get; set; }
        [Description("status")]
        public EventStatus Status { get; set; }
    }
}
