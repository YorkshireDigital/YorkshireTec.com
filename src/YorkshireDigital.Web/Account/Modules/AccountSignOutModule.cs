namespace YorkshireDigital.Web.Account.Modules
{
    using Nancy.Authentication.Forms;
    using NHibernate;
    using YorkshireDigital.Web.Infrastructure;

    public class AccountSignOutModule : BaseModule
    {
        public AccountSignOutModule(ISessionFactory sessionFactory)
            : base(sessionFactory, "account/sign-out")
        {
            this.RequiresFeature("Account");
            Get["/"] = _ => this.LogoutAndRedirect("~/");
        }
    }
}