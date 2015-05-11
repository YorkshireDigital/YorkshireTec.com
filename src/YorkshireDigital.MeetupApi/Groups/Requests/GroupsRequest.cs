namespace YorkshireDigital.MeetupApi.Groups.Requests
{
    using System.ComponentModel;
    using RestSharp;

    public class GroupsRequest : BaseRequest
    {
        public GroupsRequest() : base("groups", Method.GET)
        {
            
        }

        [Description("group_urlname")]
        public string GroupUrlName { get; set; }

    }
}
