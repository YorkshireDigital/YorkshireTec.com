namespace YorkshireTec.Home.Modules
{
    using System.Configuration;
    using Nancy;
    using YorkshireTec.Home.ViewModels;
    using YorkshireTec.Infrastructure;

    public class HomeModule : BaseModule
    {
        public HomeModule()
        {
            Get["/"] = _ =>
            {
                
                var consumerKey = ConfigurationManager.AppSettings["consumerKey"];
                var consumerSecret = ConfigurationManager.AppSettings["consumerSecret"];
                var accessToken = ConfigurationManager.AppSettings["accessToken"];
                var accessTokenSecret = ConfigurationManager.AppSettings["accessTokenSecret"];

                var viewModel = new LandingPageViewModel(consumerKey, consumerSecret, accessToken, accessTokenSecret);
                var model = GetBaseModel(viewModel);

                model.Page.Title = "Home";
                return Negotiate.WithModel(model).WithView("LandingPage");
            };

            Get["/Logo"] = _ => View["Logo"];
        }
    }
}