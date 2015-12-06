namespace YorkshireDigital.MeetupApi.Clients
{
    using Newtonsoft.Json;
    using RestSharp;
    using System;
    using System.Linq;
    using System.Threading;
    using YorkshireDigital.MeetupApi.Models;
    using YorkshireDigital.MeetupApi.Requests;

    public class BaseClient
    {
        private const string ApiRoot = @"http://api.meetup.com/2/";
        internal readonly string ApiKey;
        internal readonly string MemberId;
        internal readonly IRestClient Client;

        private decimal RateLimit = 30;
        private decimal RateLimitRemaining = 30;
        private decimal RateLimitReset = 30;

        public BaseClient(string apiKey, string memberId)
        {
            ApiKey = apiKey;
            MemberId = memberId;
            Client = new RestClient(ApiRoot);
        }

        public BaseClient(IRestClient restClient)
        {
            Client = restClient;
        }

        protected ApiResponse<T> Execute<T>(RestRequest request)
        {
            if (RateLimitRemaining < RateLimit / 2)
            {
                decimal secondsPerReqUntilReset = RateLimitReset / RateLimitRemaining;
                Thread.Sleep(Convert.ToInt32(secondsPerReqUntilReset * 1000));
            }

            var response = Client.Execute(request);

            RateLimit = decimal.Parse(response.Headers.Single(x => x.Name == "X-RateLimit-Limit").Value.ToString());
            RateLimitRemaining = decimal.Parse(response.Headers.Single(x => x.Name == "X-RateLimit-Remaining").Value.ToString());
            RateLimitReset = decimal.Parse(response.Headers.Single(x => x.Name == "X-RateLimit-Reset").Value.ToString());

            var json = response.Content;

            var content = JsonConvert.DeserializeObject<ApiResponse<T>>(json);

            return content;
        }

        public ApiResponse<T> Get<T>(BaseRequest request)
        {
            var restRequest = request.ToRestRequest(Method.GET, ApiKey);

            return Execute<T>(restRequest);
        }
    }
}
