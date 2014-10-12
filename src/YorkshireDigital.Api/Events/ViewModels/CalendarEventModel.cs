namespace YorkshireDigital.Api.Events.ViewModels
{
    public class CalendarEventModel
    {
        public int Id { get; set; }
        public string UniqueName { get; set; }
        public string Title { get; set; }
        public string ShortDescription { get; set; }
        public string LongDescription { get; set; }
        public string Website { get; set; }
        public string Colour { get; set; }
        public string Start { get; set; }
        public string End { get; set; }
        public string Location { get; set; }
        public decimal Price { get; set; }
    }
}