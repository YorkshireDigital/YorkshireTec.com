namespace YorkshireDigital.Web.Admin.Modules
{
    using Nancy.Security;
    using YorkshireDigital.Web.Infrastructure;

    public class AdminAddGroupModule : BaseModule
    {
        public AdminAddGroupModule() 
            : base("admin/add-group")
        {
            this.RequiresAuthentication();
            this.RequiresClaims(new[] { "Admin" });

            Get["/"] = _ => View["NewGroup"];
        }
    }
}