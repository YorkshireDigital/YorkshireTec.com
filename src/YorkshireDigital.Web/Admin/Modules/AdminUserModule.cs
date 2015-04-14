namespace YorkshireDigital.Web.Admin.Modules
{
    using Nancy.Security;
    using YorkshireDigital.Web.Infrastructure;

    public class AdminUserModule : BaseModule
    {
        public AdminUserModule()
            : base("admin/user")
        {
            this.RequiresAuthentication();
            this.RequiresClaims(new[] { "Admin" });

            Get["/"] = _ => View["User"];
        }
    }
}