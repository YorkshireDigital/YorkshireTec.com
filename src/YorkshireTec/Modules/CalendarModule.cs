namespace YorkshireTec.Modules
{
    using Nancy;

    public class CalendarModule : NancyModule
    {
        public CalendarModule()
            : base("calendar")
        {
            Get["/"] = _ => View["Index"];
        }
    }
}