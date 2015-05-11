namespace YorkshireDigital.MeetupApi
{
    using RestSharp;
    using YorkshireDigital.MeetupApi.Groups;

    public interface IMeetup
    {
        //List<Event> GetEvents(string meetupName);
    }

    public class MeetupClient : IMeetup
    {
        public MeetupClient(string apiKey)
        {
            Groups = new GroupsClient(apiKey);
        }

        public MeetupClient(IRestClient restClient)
        {
            Groups = new GroupsClient(restClient);
        }

        public GroupsClient Groups { get; set; }

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
