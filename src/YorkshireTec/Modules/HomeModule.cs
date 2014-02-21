using Nancy;

namespace YorkshireTec.Modules
{
    using YorkshireTec.ViewModels.Home;

    public class HomeModule : NancyModule
    {
        public HomeModule()
        {
            Get["/"] = _ =>
            {
                var model = new LandingPageViewModel();
                return View["LandingPage", model];
            };
            Get["/Logo"] = _ => View["Logo"];
        }
    }
}