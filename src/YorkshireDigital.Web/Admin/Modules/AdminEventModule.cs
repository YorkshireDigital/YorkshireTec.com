namespace YorkshireDigital.Web.Admin.Modules
{
    using Nancy.Security;
    using YorkshireDigital.Web.Infrastructure;

    public class AdminEventModule : BaseModule
    {
        public AdminEventModule()
            : base("admin/event")
        {
            this.RequiresAuthentication();
            this.RequiresClaims(new[] { "Admin" });

            Get["/"] = _ => View["Event"];
        }
    }
}