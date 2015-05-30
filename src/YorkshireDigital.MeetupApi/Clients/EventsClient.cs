namespace YorkshireDigital.MeetupApi.Clients
{
    using System.Collections.Generic;
    using RestSharp;
    using YorkshireDigital.MeetupApi.Models;
    using YorkshireDigital.MeetupApi.Requests;

    public interface IEventsClient
    {
        ApiResponse<List<Event>> Get(EventsRequest request);
    }

    public class EventsClient : BaseClient, IEventsClient
    {
        #region ctor
        public EventsClient(string apiKey, string memberId)
            : base(apiKey, memberId)
        {
        }

        public EventsClient(IRestClient restClient) : base(restClient)
        {
        }
        #endregion

        public ApiResponse<List<Event>> Get(EventsRequest request)
        {
            return Get<List<Event>>(request);
        }
    }
}
