namespace YorkshireDigital.MeetupApi.Clients
{
    using System.Collections.Generic;
    using RestSharp;
    using YorkshireDigital.MeetupApi.Models;
    using YorkshireDigital.MeetupApi.Requests;

    public interface IGroupsClient
    {
        ApiResponse<List<Group>> Get(GroupsRequest request);
    }

    public class GroupsClient : BaseClient, IGroupsClient
    {
        #region ctor
        public GroupsClient(string apiKey, string memberId) : base(apiKey, memberId)
        {
        }

        public GroupsClient(IRestClient restClient) : base(restClient)
        {
        }
        #endregion

        public ApiResponse<List<Group>> Get(GroupsRequest request)
        {
            return Get<List<Group>>(request);
        }
    }
}
