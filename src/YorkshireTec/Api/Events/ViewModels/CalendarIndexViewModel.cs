namespace YorkshireTec.Api.Calendar.ViewModels
{
    using System.Collections.Generic;
    using System.Xml.Linq;

    public class CalendarIndexViewModel
    {
        private const string NsAtom = "http://www.w3.org/2005/Atom";

        public List<CalendarEventViewModel> Events { get; set; }

        public CalendarIndexViewModel()
        {
            Events = new List<CalendarEventViewModel>();
        }

        public void LoadFromAtomFeed(XDocument feed)
        {
            if (feed.Root == null) return;

            foreach (var entry in feed.Root.Elements(string.Format("{{{0}}}entry", NsAtom)))
            {
                Events.Add(new CalendarEventViewModel(entry));
            }
        }
    }
}