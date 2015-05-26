namespace YorkshireDigital.MeetupApi.Clients
{
    using RestSharp;

    public interface IMeetupClient
    {
        IGroupsClient Groups { get; set; }
        IEventsClient Events { get; set; }
        IProfileClient Profile { get; set; }
    }

    public class MeetupClient : IMeetupClient
    {
        public MeetupClient(string apiKey)
        {
            Groups = new GroupsClient(apiKey);
            Events = new EventsClient(apiKey);
            Profile = new ProfileClient(apiKey);
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
