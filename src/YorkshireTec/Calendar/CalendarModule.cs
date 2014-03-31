namespace YorkshireTec.Modules
{
    using System.Xml.Linq;
    using Nancy;
    using YorkshireTec.Calendar.ViewModels;
    using YorkshireTec.Infrastructure;

    public class CalendarModule : BaseModule
    {
        public CalendarModule()
            : base("calendar")
        {
            this.RequiresFeature("Calendar");

            const string calendarId = "info%40yorkshiretec.com";
            Get["/"] = _ =>
            {

                var feed = XDocument.Load(string.Format("https://www.google.com/calendar/feeds/{0}/public/basic", calendarId));

                var viewModel = new CalendarIndexViewModel();
                viewModel.LoadFromAtomFeed(feed);
                var model = GetBaseModel(viewModel);
                model.Page.Title = "Calendar";

                return Negotiate.WithModel(model).WithView("Index");
            };
        }
    }
}