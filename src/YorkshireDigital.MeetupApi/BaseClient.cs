namespace YorkshireDigital.MeetupApi
{
    using Newtonsoft.Json;
    using RestSharp;
    using YorkshireDigital.MeetupApi.Models;

    public class BaseClient
    {
        private const string ApiRoot = @"http://api.meetup.com/2/";
        internal readonly string ApiKey;
        internal readonly IRestClient Client;

        public BaseClient(string apiKey)
        {
            ApiKey = apiKey;
            Client = new RestClient(ApiRoot);
        }

        public BaseClient(IRestClient restClient)
        {
            Client = restClient;
        }

        protected ApiResponse<T> Execute<T>(RestRequest request)
        {
            request.AddParameter("key", ApiKey);

            var response = Client.Execute(request);
            var json = response.Content;

            var content = JsonConvert.DeserializeObject<ApiResponse<T>>(json);

            return content;
        }
    }
}
