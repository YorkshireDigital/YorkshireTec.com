namespace YorkshireDigital.Api.Events.ViewModels
{
    using System;

    public class CalendarSearchModel
    {
        public DateTime? From { get; set; }
        public DateTime? To { get; set; }
        public string[] Interests { get; set; }
        public string[] Locations { get; set; }
        public int? Skip { get; set; }
        public int? Take { get; set; }
    }
}