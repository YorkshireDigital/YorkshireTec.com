namespace YorkshireDigital.MeetupApi.Groups
{
    using System.Collections.Generic;
    using RestSharp;
    using YorkshireDigital.MeetupApi.Groups.Requests;
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
            var restRequest = request.ToRestRequest();

            return Execute<List<Group>>(restRequest);
        }
    }
}
