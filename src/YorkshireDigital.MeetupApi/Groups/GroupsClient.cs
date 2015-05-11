namespace YorkshireDigital.MeetupApi.Groups
{
    using System.Collections.Generic;
    using RestSharp;
    using YorkshireDigital.MeetupApi.Groups.Requests;
    using YorkshireDigital.MeetupApi.Helpers;
    using YorkshireDigital.MeetupApi.Models;

    public class GroupsClient : BaseClient
    {
        #region ctor
        public GroupsClient(string apiKey) : base(apiKey)
        {
        }

        public GroupsClient(IRestClient restClient) : base(restClient)
        {
        }
        #endregion

        public ApiResponse<List<Group>> Get(GroupsRequest request)
        {
            var restRequest = new RestRequest("groups", Method.GET);

            if (!string.IsNullOrEmpty(request.GroupUrlName))
            {
                restRequest.AddParameter(request.GetDescriptionValue("GroupUrlName"), request.GroupUrlName);
            }

            restRequest.AddParameter("key", ApiKey);

            return Execute<List<Group>>(restRequest);
        }
    }
}
