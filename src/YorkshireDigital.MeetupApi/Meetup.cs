namespace YorkshireDigital.MeetupApi
{
    using System.Collections.Generic;
    using Newtonsoft.Json;
    using RestSharp;
    using YorkshireDigital.MeetupApi.Responses;

    public interface IMeetup
    {
        List<MeetupEventResponse> GetEvent(string meetupName);
    }

    public class Meetup : IMeetup
    {
        private const string ApiRoot = @"http://api.meetup.com/2/";
        private readonly string apiKey;
        private readonly RestClient client;

        public Meetup(string apiKey)
        {
            this.apiKey = apiKey;
            client = new RestClient(ApiRoot);
        }

        public List<MeetupEventResponse> GetEvent(string meetupName)
        {
            var request = new RestRequest("events", Method.GET);
            request.AddParameter("group_urlname", meetupName);
            request.AddParameter("key", apiKey);

            var response = client.Execute(request);
            var json = response.Content;

            var content = JsonConvert.DeserializeObject<MeetupResponse>(json);

            return content.Results;
        }
    }
}
