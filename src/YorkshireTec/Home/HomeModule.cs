namespace YorkshireTec.Home
{
    using System.Configuration;
    using YorkshireTec.Home.ViewModels;
    using YorkshireTec.Infrastructure;

    public class HomeModule : BaseModule
    {
        public HomeModule()
        {
            Get["/"] = _ =>
            {
                
                string consumerKey = ConfigurationManager.AppSettings["consumerKey"];
                string consumerSecret = ConfigurationManager.AppSettings["consumerSecret"];
                string accessToken = ConfigurationManager.AppSettings["accessToken"];
                string accessTokenSecret = ConfigurationManager.AppSettings["accessTokenSecret"];

                var viewModel = new LandingPageViewModel(consumerKey, consumerSecret, accessToken, accessTokenSecret);
                var model = GetBaseModel(viewModel);

                model.Page.Title = "Home";
                return View["LandingPage", model];
            };
            Get["/Logo"] = _ => View["Logo"];
        }
    }
}