namespace YorkshireTec.Account.Modules
{
    using System;
    using System.Web.Helpers;
    using global::Raven.Client;
    using Nancy;
    using Nancy.Authentication.Forms;
    using Nancy.ModelBinding;
    using Nancy.Validation;
    using YorkshireTec.Account.ViewModels;
    using YorkshireTec.Infrastructure;
    using YorkshireTec.Raven.Repositories;

    public class AccountLoginModule : BaseModule
    {
        public AccountLoginModule(IDocumentSession documentSession)
            : base("account/log-in")
        {
            this.RequiresFeature("Account");
            Get["/"] = _ =>
            {
                var model = GetBaseModel(new AccountLogInViewModel());
                model.Page.Title = "Log In";
                return Negotiate.WithModel(model).WithView("LogIn");
            };

            Post["/"] = _ =>
            {
                var viewModel = this.Bind<AccountLogInViewModel>();
                var result = this.Validate(viewModel);

                var model = GetBaseModel(viewModel);
                model.Page.Title = "Log In";

                if (result.IsValid)
                {
                    var userRepository = new UserRepository(documentSession);

                    var user = userRepository.GetUser(viewModel.Username);

                    if (user != null && user.Password != null)
                    {
                        if (Crypto.VerifyHashedPassword(user.Password, viewModel.Password))
                        {
                            var expiry = viewModel.RememberMe ? DateTime.Now.AddDays(7) : (DateTime?)null;
                            return this.LoginAndRedirect(user.Id, expiry);
                        }
                    }
                }
                else
                {
                    model.Page.AddErrors(result);
                }
                model.Page.AddError("Log in attempt failed", "");
                return Negotiate.WithModel(model).WithView("LogIn");
            };
        }
    }
}