namespace YorkshireDigital.MeetupApi.Clients
{
    using Newtonsoft.Json;
    using RestSharp;
    using YorkshireDigital.MeetupApi.Models;
    using YorkshireDigital.MeetupApi.Requests;

    public interface IProfileClient
    {
        Profile Create(ProfileCreateRequest profileCreateRequest);
    }

    public class ProfileClient : BaseClient, IProfileClient
    {
        public ProfileClient(string apiKey) : base(apiKey)
        {
        }

        public ProfileClient(IRestClient restClient) : base(restClient)
        {
        }

        public Profile Create(ProfileCreateRequest profileCreateRequest)
        {
            var restRequest = profileCreateRequest.ToRestRequest(Method.POST, ApiKey);


            var response = Client.Execute(restRequest);
            var json = response.Content;

            var content = JsonConvert.DeserializeObject<Profile>(json);

            return content;
        }
    }
}
