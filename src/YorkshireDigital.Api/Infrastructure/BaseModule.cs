namespace YorkshireDigital.Api.Infrastructure
{
    using Nancy;

    public class BaseModule : NancyModule
    {
        public BaseModule()
        {
            Get["/Test"] = _ => "Success";
        }
    }
}