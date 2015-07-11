namespace YorkshireDigital.Web.Events.ViewModels
{
    using YorkshireDigital.Data.Domain.Group;
    using YorkshireDigital.Data.Domain.Group.Enums;

    public class ContactLinkModel
    {
        public ContactLinkModel(ContactLink link)
        {
            Description = link.Value;
            switch (link.Type)
            {
                case ContactLinkType.Twitter:
                    Link = string.Format("http://www.twitter.com/@{0}", link.Value);
                    break;
                case ContactLinkType.Email:
                    Link = string.Format("mailto:{0}", link.Value);
                    break;
                case ContactLinkType.Link:
                    Link = link.Value.IndexOf("http://", System.StringComparison.Ordinal) == -1
                        ? string.Format("http://{0}", link.Value)
                        : link.Value;
                    break;
            }
        }

        public string Description { get; set; }
        public string Link { get; set; }
    }
}