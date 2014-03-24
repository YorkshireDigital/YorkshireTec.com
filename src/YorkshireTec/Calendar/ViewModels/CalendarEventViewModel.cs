namespace YorkshireTec.Calendar.ViewModels
{
    using System.Xml.Linq;

    public class CalendarEventViewModel
    {
        private const string NsAtom = "http://www.w3.org/2005/Atom";

        public string GoogleLink { get; set; }
        public string Title { get; set; }
        public string Summary { get; set; }

        public CalendarEventViewModel(XContainer entry)
        {
            var idElement = entry.Element(string.Format("{{{0}}}id", NsAtom));
            GoogleLink = idElement != null ? idElement.Value : "";
            var titleElement = entry.Element(string.Format("{{{0}}}title", NsAtom));
            Title = titleElement != null ? titleElement.Value : "";
            var summaryElement = entry.Element(string.Format("{{{0}}}summary", NsAtom));
            Summary = summaryElement != null ? summaryElement.Value : "";
        }
    }
}