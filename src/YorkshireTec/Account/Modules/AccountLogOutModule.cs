namespace YorkshireTec.Account.Modules
{
    using Nancy.Authentication.Forms;
    using NHibernate;
    using YorkshireTec.Infrastructure;

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