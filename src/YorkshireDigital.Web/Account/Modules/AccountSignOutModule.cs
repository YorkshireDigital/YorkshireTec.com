namespace YorkshireDigital.Web.Account.Modules
{
    using Nancy.Authentication.Forms;
    using YorkshireDigital.Data.Services;
    using YorkshireDigital.Web.Infrastructure;

    public class AccountSignOutModule : BaseModule
    {
        public AccountSignOutModule(IUserService userService)
            : base("account/sign-out")
        {
            this.RequiresFeature("Account");
            Get["/"] = _ => this.LogoutAndRedirect("~/");
        }
    }
}