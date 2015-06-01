namespace YorkshireDigital.MeetupApi.Requests
{
    using System.ComponentModel;

    public class ProfileDeleteRequest : BaseRequest
    {
        public ProfileDeleteRequest()
            : base("profile/:gid/:mid")
        {
        }

        [Description("gid")]
        public string GroupId { get; set; }
        [Description("mid")]
        public string MemberId { get; set; }
    }
}
