namespace YorkshireDigital.Web.Home.Modules
{
    using YorkshireDigital.Web.Infrastructure;

    public class HomeModule : BaseModule
    {
        public HomeModule()
            : base("")
        {
            Get["/"] = _ =>
            {
                ViewBag.Title = "YorkshireDigital";
                return View["Home"];
            };
        }
    }
}