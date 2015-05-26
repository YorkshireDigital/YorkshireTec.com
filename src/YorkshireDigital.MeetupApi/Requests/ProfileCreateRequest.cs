namespace YorkshireDigital.MeetupApi.Requests
{
    using System.Collections.Generic;
    using System.ComponentModel;

    public class ProfileCreateRequest : BaseRequest
    {
        public ProfileCreateRequest() : base("profile")
        {
        }

        [Description("group_id")]
        public string GroupId { get; set; }
        [Description("group_urlname")]
        public string GroupUrlName { get; set; }
        [Description("answer_{0}")]
        public Dictionary<int, string> Answers { get; set; }
        [Description("intro")]
        public string Intro { get; set; }
        [Description("site_name")]
        public string SiteName { get; set; }
        [Description("site_url")]
        public string SiteUrl { get; set; }
    }
}
