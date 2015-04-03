namespace YorkshireDigital.Web.Events.ViewModels
{
    using System.Linq;
    using YorkshireDigital.Web.Infrastructure;
    using YorkshireDigital.Data.Domain.Events;
    using YorkshireDigital.Web.Infrastructure.Helpers;

    public class EventDetailsModel
    {
        public string UniqueName { get; set; }
        public string Title { get; set; }
        public string Synopsis { get; set; }
        public string Colour { get; set; }
        public string Start { get; set; }
        public string StartFormat { get; set; }
        public string Location { get; set; }
        public ContactLinkModel[] ContactLinks { get; set; }
        public string Website { get; set; }
        public TalkDetailModel[] Talks { get; set; }
        public string About { get; set; }
        public string Headline { get; set; }
        public string Organiser { get; set; }
        public string OrganiserShortName { get; set; }

        public EventDetailsModel(Event e)
        {
            Organiser = e.Group.Name;
            OrganiserShortName = e.Group.ShortName;
            Title = e.Title;
            Synopsis = e.Synopsis.MarkdownToHtml();
            Colour = e.Group.Colour;
            Start = e.Start.ToString("yyyy-MM-dd");
            StartFormat = e.Start.ToLyndensFancyFormat();
            Location = e.Location;
            UniqueName = e.UniqueName;
            ContactLinks = e.Group.ContactLinks.Select(x => new ContactLinkModel(x)).ToArray();
            Website = e.Group.Website.IndexOf("http://", System.StringComparison.Ordinal) == -1
                        ? string.Format("http://{0}", e.Group.Website)
                        : e.Group.Website; ;
            Headline = e.Group.Headline;
            About = e.Group.About.MarkdownToHtml();
            Talks = e.Talks.Select(x => new TalkDetailModel(x)).ToArray();
        }
    }
}