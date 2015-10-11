namespace YorkshireDigital.MeetupApi.Clients
{
    using RestSharp;
    using System.Configuration;

    public interface IMeetupClient
    {
        IGroupsClient Groups { get; set; }
        IEventsClient Events { get; set; }
        IProfileClient Profile { get; set; }
    }

    public class MeetupClient : IMeetupClient
    {
        public MeetupClient() : this(ConfigurationManager.AppSettings["Meetup_Bot_ApiKey"], ConfigurationManager.AppSettings["Meetup_Bot_MemberId"])
        {

        }

        public MeetupClient(string apiKey, string memberId)
        {
            Groups = new GroupsClient(apiKey, memberId);
            Events = new EventsClient(apiKey, memberId);
            Profile = new ProfileClient(apiKey, memberId);
        }

        public MeetupClient(IRestClient restClient)
        {
            Groups = new GroupsClient(restClient);
            Events = new EventsClient(restClient);
            Profile = new ProfileClient(restClient);
        }

        public IGroupsClient Groups { get; set; }
        public IEventsClient Events { get; set; }
        public IProfileClient Profile { get; set; }
    }
}
