namespace YorkshireDigital.Web.Admin.Modules
{
    using Nancy.Security;
    using YorkshireDigital.Web.Infrastructure;

    public class AdminGroupModule : BaseModule
    {
        public AdminGroupModule() 
            : base("admin/group")
        {
            this.RequiresAuthentication();
            this.RequiresClaims(new[] { "Admin" });

            Get["/"] = _ => View["Group"];
        }
    }
}