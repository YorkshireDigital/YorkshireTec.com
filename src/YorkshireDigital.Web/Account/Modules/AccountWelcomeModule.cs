namespace YorkshireDigital.Web.Account.Modules
{
    using System;
    using Nancy;
    using Nancy.Security;
    using NHibernate;
    using YorkshireDigital.Data.Services;
    using YorkshireDigital.Web.Account.ViewModels;
    using YorkshireDigital.Web.Infrastructure;

    public class AccountWelcomeModule : BaseModule
    {
        public AccountWelcomeModule(ISessionFactory sessionFactory)
            : base(sessionFactory, "Account/Welcome")
        {
            this.RequiresFeature("Account");
            this.RequiresAuthentication();

            Get["/"] = _ =>
            {
                var userSession = new UserService(RequestSession);
                var user = userSession.GetUserById(new Guid(((UserIdentity)Context.CurrentUser).UserId));

                var viewModel = new AccountWelcomeViewModel(user);
                @ViewBag.Title = "Welcome";

                return Negotiate.WithModel(viewModel).WithView("Welcome");
            };
        }
    }
}