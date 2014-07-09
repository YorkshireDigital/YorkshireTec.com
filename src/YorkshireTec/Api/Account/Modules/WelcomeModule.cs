namespace YorkshireTec.Api.Account.Modules
{
    using System;
    using Nancy;
    using Nancy.Security;
    using NHibernate;
    using YorkshireTec.Api.Account.ViewModels;
    using YorkshireTec.Api.Infrastructure;
    using YorkshireTec.Data.Services;

    public class WelcomeModule : BaseModule
    {
        public WelcomeModule(ISessionFactory sessionFactory)
            : base(sessionFactory, "Account/Welcome")
        {
            this.RequiresFeature("Account");
            this.RequiresAuthentication();

            Get["/"] = _ =>
            {
                var userSession = new UserService(RequestSession);
                var user = userSession.GetUserById(new Guid(((UserIdentity)Context.CurrentUser).UserId));

                var model = GetBaseModel(new WelcomeViewModel(user));
                model.Page.Title = "Welcome";
                model.ViewModel.Email = ((UserIdentity) Context.CurrentUser).Email;
                return Negotiate.WithModel(model).WithView("Welcome");
            };
        }
    }
}