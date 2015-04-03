namespace YorkshireDigital.Web.Account.Modules
{
    using System;
    using Nancy;
    using Nancy.Security;
    using YorkshireDigital.Data.Services;
    using YorkshireDigital.Web.Account.ViewModels;
    using YorkshireDigital.Web.Infrastructure;

    public class AccountWelcomeModule : BaseModule
    {
        public AccountWelcomeModule(IUserService userService)
            : base("Account/Welcome")
        {
            this.RequiresFeature("Account");
            this.RequiresAuthentication();

            Get["/"] = _ =>
            {
                var user = userService.GetUserById(new Guid(((UserIdentity)Context.CurrentUser).UserId));

                var viewModel = new AccountWelcomeViewModel(user);
                @ViewBag.Title = "Welcome";

                return Negotiate.WithModel(viewModel).WithView("Welcome");
            };
        }
    }
}