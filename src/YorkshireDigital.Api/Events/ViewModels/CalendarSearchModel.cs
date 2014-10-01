namespace YorkshireDigital.Api.Events.ViewModels
{
    using System;

    public class CalendarSearchModel
    {
        public string From { get; set; }
        public string To { get; set; }
        public string[] Interests { get; set; }
        public string[] Locations { get; set; }
        public int? Skip { get; set; }
        public int? Take { get; set; }
    }
}