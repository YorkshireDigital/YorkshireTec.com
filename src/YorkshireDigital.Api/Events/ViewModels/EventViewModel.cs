namespace YorkshireDigital.Api.Events.ViewModels
{
    using System.Collections.Generic;
    using YorkshireTec.Data.Domain.Events;

    public class EventViewModel
    {
        public int Id { get; set; }
        public string UniqueName { get; set; }
        public string Title { get; set; }
        public string ShortDescription { get; set; }
        public string LongDescription { get; set; }
        public string Website { get; set; }
        public string Colour { get; set; }
        public byte[] Logo { get; set; }
        public string Start { get; set; }
        public string End { get; set; }
        public string Location { get; set; }
        public decimal Price { get; set; }
        public byte[] Photo { get; set; }
        public string[] Categories { get; set; }
        public string[] Interests { get; set; }
        public IList<SocialLink> SocialLinks { get; set; }
    }
}