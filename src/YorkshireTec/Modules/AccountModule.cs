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

    public class AccountModule : NancyModule
    {
        public AccountModule(IDocumentSession documentSession)
            : base("account")
        {
            Get["/log-in"] = _ =>
            {
                return Negotiate.WithModel(new AccountLogInViewModel()).WithView("LogIn");
            };

            Post["/log-in"] = _ =>
            {
                var model = this.Bind<AccountLogInViewModel>();
                var result = this.Validate(model);

                if (result.IsValid)
                {
                    var userRepository = new UserRepository(documentSession);

                    var user = userRepository.GetUser(model.Username);

                    if (user != null)
                    {
                        if (Crypto.VerifyHashedPassword(user.Password, model.Password))
                        {
                            var expiry = model.RememberMe ? DateTime.Now.AddDays(7) : (DateTime?)null;
                            return this.LoginAndRedirect(user.Id, expiry, "~/");
                        }
                    }
                }

                model.Username = "FAIL";
                return Negotiate.WithModel(model).WithView("LogIn");
            };
        }
    }
}