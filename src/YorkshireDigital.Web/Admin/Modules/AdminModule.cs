namespace YorkshireDigital.Web.Admin.Modules
{
    using Nancy.Security;
    using NHibernate;
    using YorkshireDigital.Web.Infrastructure;

    public class AdminModule : BaseModule
    {
        public AdminModule(ISessionFactory sessionFactory) : base(sessionFactory, "admin")
        {
            this.RequiresClaims(new [] { "Admin" });

            Get["/"] = _ => View["Index"];
        }
    }
}