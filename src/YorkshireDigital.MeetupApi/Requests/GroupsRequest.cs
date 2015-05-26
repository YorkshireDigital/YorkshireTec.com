namespace YorkshireDigital.MeetupApi.Requests
{
    using System.ComponentModel;

    public class GroupsRequest : BaseRequest
    {
        public GroupsRequest() : base("groups")
        {
            
        }

        [Description("group_urlname")]
        public string GroupUrlName { get; set; }

    }
}
