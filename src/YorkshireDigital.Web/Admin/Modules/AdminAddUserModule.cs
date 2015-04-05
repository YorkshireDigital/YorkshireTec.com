namespace YorkshireDigital.Web.Admin.Modules
{
    using Nancy.Security;
    using YorkshireDigital.Web.Infrastructure;

    public class AdminAddUserModule : BaseModule
    {
        public AdminAddUserModule()
            : base("admin/add-user")
        {
            this.RequiresAuthentication();
            this.RequiresClaims(new[] { "Admin" });

            Get["/"] = _ => View["NewUser"];
        }
    }
}