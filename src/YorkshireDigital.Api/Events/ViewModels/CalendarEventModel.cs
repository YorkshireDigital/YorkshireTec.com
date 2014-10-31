namespace YorkshireDigital.Api.Events.ViewModels
{
    public class CalendarEventModel
    {
        public string UniqueName { get; set; }
        public string ShortTitle { get; set; }
        public string Title { get; set; }
        public string ShortDescription { get; set; }
        public string LongDescription { get; set; }
        public string Colour { get; set; }
        public string Start { get; set; }
        public string End { get; set; }
        public string Region { get; set; }
        public string[] Interests { get; set; }
    }
}