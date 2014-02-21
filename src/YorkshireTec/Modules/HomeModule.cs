using Nancy;

namespace YorkshireTec.Modules
{
    using System.Configuration;
    using YorkshireTec.ViewModels.Home;

    public class HomeModule : NancyModule
    {
        public HomeModule()
        {
            Get["/"] = _ =>
            {
                string consumerKey = ConfigurationManager.AppSettings["consumerKey"];
                string consumerSecret = ConfigurationManager.AppSettings["consumerSecret"];
                string accessToken = ConfigurationManager.AppSettings["accessToken"];
                string accessTokenSecret = ConfigurationManager.AppSettings["accessTokenSecret"];

                var model = new LandingPageViewModel(consumerKey, consumerSecret, accessToken, accessTokenSecret);
                return View["LandingPage", model];
            };
            Get["/Logo"] = _ => View["Logo"];
        }
    }
}