namespace YorkshireTec.Api.Account.Modules
{
    using Nancy.Authentication.Forms;
    using NHibernate;
    using YorkshireTec.Api.Infrastructure;

    public class AccountLogOutModule : BaseModule
    {
        public AccountLogOutModule(ISessionFactory sessionFactory)
            : base(sessionFactory, "account/log-out")
        {
            this.RequiresFeature("Account");
            Get["/"] = _ => this.LogoutAndRedirect("~/");
        }
    }
}