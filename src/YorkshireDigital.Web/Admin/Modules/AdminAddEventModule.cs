namespace YorkshireDigital.Web.Admin.Modules
{
    using Nancy.Security;
    using YorkshireDigital.Web.Infrastructure;

    public class AdminAddEventModule : BaseModule
    {
        public AdminAddEventModule()
            : base("admin/add-event")
        {
            this.RequiresAuthentication();
            this.RequiresClaims(new[] { "Admin" });

            Get["/"] = _ => View["NewEvent"];
        }
    }
}