namespace YorkshireDigital.Web.Admin.Modules
{
    using Nancy.Security;
    using YorkshireDigital.Web.Infrastructure;

    public class AdminModule : BaseModule
    {
        public AdminModule() : base("admin")
        {
            this.RequiresFeature("Account");
            this.RequiresAuthentication();
            this.RequiresClaims(new [] { "Admin" });

            Get["/"] = _ => View["Index"];
        }
    }
}