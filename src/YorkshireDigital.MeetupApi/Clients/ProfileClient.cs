namespace YorkshireDigital.MeetupApi.Clients
{
    using System.Collections.Generic;
    using Newtonsoft.Json;
    using RestSharp;
    using YorkshireDigital.MeetupApi.Models;
    using YorkshireDigital.MeetupApi.Requests;

    public interface IProfileClient
    {
        Profile Create(ProfileCreateRequest profileCreateRequest);
        bool Delete(ProfileDeleteRequest profileDeleteRequest);
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

        public bool Delete(ProfileDeleteRequest profileDeleteRequest)
        {
            var restRequest = profileDeleteRequest.ToRestRequest(Method.DELETE, ApiKey);

            var response = Client.Execute(restRequest);
            var json = response.Content;

            var content = JsonConvert.DeserializeObject<Dictionary<string, string>>(json);

            return content.ContainsKey("message") && content["message"] == "member is removed from group";
        }
    }
}
