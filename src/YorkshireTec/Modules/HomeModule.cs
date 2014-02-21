using Nancy;

namespace YorkshireTec.Modules
{
    public class HomeModule : NancyModule
    {
        public HomeModule()
        {
            Get["/"] = _ => View["LandingPage"];
            Get["/Logo"] = _ => View["Logo"];
        }
    }
}