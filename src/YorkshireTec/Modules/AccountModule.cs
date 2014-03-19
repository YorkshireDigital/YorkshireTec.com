namespace YorkshireTec.Modules
{
    using System;
    using System.Web.Helpers;
    using global::Raven.Client;
    using Nancy;
    using Nancy.Authentication.Forms;
    using Nancy.ModelBinding;
    using Nancy.Validation;
    using YorkshireTec.Raven.Repositories;
    using YorkshireTec.ViewModels.Account;

    public class AccountModule : BaseModule
    {
        public AccountModule(IDocumentSession documentSession)
            : base("account")
        {
            Get["/log-in"] = _ =>
            {
                var model = GetBaseModel(new AccountLogInViewModel());
                model.Page.Title = "Log In";
                return Negotiate.WithModel(model).WithView("LogIn");
            };

            Post["/log-in"] = _ =>
            {
                var viewModel = this.Bind<AccountLogInViewModel>();
                var result = this.Validate(viewModel);

                if (result.IsValid)
                {
                    var userRepository = new UserRepository(documentSession);

                    var user = userRepository.GetUser(viewModel.Username);

                    if (user != null)
                    {
                        if (Crypto.VerifyHashedPassword(user.Password, viewModel.Password))
                        {
                            var expiry = viewModel.RememberMe ? DateTime.Now.AddDays(7) : (DateTime?)null;
                            return this.LoginAndRedirect(user.Id, expiry, "~/");
                        }
                    }
                }

                var model = GetBaseModel(viewModel);
                model.Page.Title = "Log In";
                return Negotiate.WithModel(model).WithView("LogIn");
            };
        }
    }
}