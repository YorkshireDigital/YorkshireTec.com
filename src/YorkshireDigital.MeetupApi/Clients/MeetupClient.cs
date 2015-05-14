namespace YorkshireDigital.MeetupApi.Clients
{
    using RestSharp;

    public interface IMeetupClient
    {
        IGroupsClient Groups { get; set; }

        //List<Event> GetEvents(string meetupName);
    }

    public class MeetupClient : IMeetupClient
    {
        public MeetupClient(string apiKey)
        {
            Groups = new GroupsClient(apiKey);
            Events = new EventsClient(apiKey);
        }

        public MeetupClient(IRestClient restClient)
        {
            Groups = new GroupsClient(restClient);
            Events = new EventsClient(restClient);
        }

        public IGroupsClient Groups { get; set; }
        public IEventsClient Events { get; set; }

        //public List<Event> GetEvents(string meetupName)
        //{
        //    var request = new RestRequest("events", Method.GET);
        //    request.AddParameter("group_urlname", meetupName);
        //    request.AddParameter("key", apiKey);
            
        //    var response = client.Execute(request);
        //    var json = response.Content;

        //    var content = JsonConvert.DeserializeObject<ApiResponse<>>(json);

        //    return content.Results;
        //}
    }
}
