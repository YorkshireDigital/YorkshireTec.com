namespace YorkshireDigital.Web.Events.ViewModels
{
    using System.Linq;
    using YorkshireDigital.Web.Infrastructure;
    using YorkshireDigital.Data.Domain.Events;

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
            Organiser = e.Organisation.Name;
            OrganiserShortName = e.Organisation.ShortName;
            Title = e.Title;
            Synopsis = new MarkdownSharp.Markdown().Transform(e.Synopsis);
            Colour = e.Organisation.Colour;
            Start = e.Start.ToString("yyyy-MM-dd");
            StartFormat = e.Start.ToLyndensFancyFormat();
            Location = e.Location;
            UniqueName = e.UniqueName;
            ContactLinks = e.Organisation.ContactLinks.Select(x => new ContactLinkModel(x)).ToArray();
            Website = e.Organisation.Website.IndexOf("http://", System.StringComparison.Ordinal) == -1
                        ? string.Format("http://{0}", e.Organisation.Website)
                        : e.Organisation.Website; ;
            Headline = e.Organisation.Headline;
            About = new MarkdownSharp.Markdown().Transform(e.Organisation.About);
            Talks = e.Talks.Select(x => new TalkDetailModel(x)).ToArray();
        }
    }
}