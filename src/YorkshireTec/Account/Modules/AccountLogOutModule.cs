namespace YorkshireTec.Account.Modules
{
    using Nancy.Authentication.Forms;
    using YorkshireTec.Infrastructure;

    public class AccountLogOutModule : BaseModule
    {
        public AccountLogOutModule()
            : base("account/log-out")
        {
            this.RequiresFeature("Account");
            Get["/"] = _ => this.LogoutAndRedirect("~/");
        }
    }
}