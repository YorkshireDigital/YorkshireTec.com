namespace YorkshireTec.Modules
{
    using System.Xml.Linq;
    using Nancy;
    using YorkshireTec.ViewModels.Calendar;

    public class CalendarModule : NancyModule
    {
        public CalendarModule()
            : base("calendar")
        {
            const string calendarId = "info%40yorkshiretec.com";
            Get["/"] = _ =>
            {
                var feed = XDocument.Load(string.Format("https://www.google.com/calendar/feeds/{0}/public/basic", calendarId));

                var model = new CalendarIndexViewModel();
                model.LoadFromAtomFeed(feed);

                return Negotiate.WithModel(model).WithView("Index");
            };
        }
    }
}