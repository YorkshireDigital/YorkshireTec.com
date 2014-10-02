namespace YorkshireDigital.Api.Infrastructure
{
    using Microsoft.Owin.Hosting;

    public class ApiRunner
    {
        public void Start()
        {
            WebApp.Start<Startup>("http://+:61140");
            //Console.WriteLine("API running on {0}", url);
        }
    }
}
