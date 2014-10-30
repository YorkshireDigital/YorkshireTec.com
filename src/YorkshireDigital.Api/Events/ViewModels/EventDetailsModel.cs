namespace YorkshireDigital.Api.Events.ViewModels
{
    using System.Linq;
    using YorkshireDigital.Data.Domain.Events;

    public class EventDetailsModel
    {
        public string Title { get; set; }
        public string ShortDescription { get; set; }
        public string Colour { get; set; }
        public string Start { get; set; }
        public string StartFormat { get; set; }
        public string Location { get; set; }
        public ContactLinkModel[] ContactLinks { get; set; }
        public string Website { get; set; }
        public string LongDescription { get; set; }
        public TalkDetailModel[] Talks { get; set; }
        public string About { get; set; }

        public EventDetailsModel(Event e)
        {
            Title = e.Title;
            ShortDescription = e.ShortDescription;
            LongDescription = e.LongDescription;
            Colour = e.Organisation.Colour;
            Start = e.Start.ToString("yyyy-MM-dd");
            StartFormat = e.Start.ToString("h:mmtt, dddd dd MMMM");
            Location = e.Location;
            ContactLinks = e.Organisation.ContactLinks.Select(x => new ContactLinkModel(x)).ToArray();
            Website = e.Organisation.Website;
            About = e.Organisation.About;
            Talks = e.Talks.Select(x => new TalkDetailModel(x)).ToArray();
        }
    }
}