namespace YorkshireDigital.MeetupApi.Responses
{
    using System.Collections.Generic;

    public class MeetupResponse
    {
        public List<MeetupEventResponse> Results { get; set; }
        public MeetupMeta Meta { get; set; }
    }
}
