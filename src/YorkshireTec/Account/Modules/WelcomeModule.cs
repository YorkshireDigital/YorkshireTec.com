using Nancy;

namespace YorkshireTec.Account.Modules
{
    using System;
    using global::Raven.Client;
    using Nancy.Security;
    using YorkshireTec.Account.ViewModels;
    using YorkshireTec.Infrastructure;
    using YorkshireTec.Raven.Repositories;

    public class WelcomeModule : BaseModule
    {
        public WelcomeModule(IDocumentSession documentSession)
            : base("Account/Welcome")
        {
            this.RequiresFeature("Account");
            this.RequiresAuthentication();

            Get["/"] = _ =>
            {
                var userRepository = new UserRepository(documentSession);
                var user = userRepository.GetUserById(new Guid(((UserIdentity)Context.CurrentUser).UserId));

                var model = GetBaseModel(new WelcomeViewModel(user));
                model.Page.Title = "Welcome";
                model.ViewModel.Email = ((UserIdentity) Context.CurrentUser).Email;
                return Negotiate.WithModel(model).WithView("Welcome");
            };
        }
    }
}