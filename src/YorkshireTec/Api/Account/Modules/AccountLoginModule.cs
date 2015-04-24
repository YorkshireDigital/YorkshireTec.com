namespace YorkshireTec.Api.Account.Modules
{
    using System;
    using System.Web.Helpers;
    using Nancy;
    using Nancy.Authentication.Forms;
    using Nancy.ModelBinding;
    using Nancy.Validation;
    using NHibernate;
    using YorkshireDigital.Data.Services;
    using YorkshireTec.Api.Account.ViewModels;
    using YorkshireTec.Api.Infrastructure;

    public class AccountLoginModule : BaseModule
    {
        public AccountLoginModule(ISessionFactory sessionFactory)
            : base(sessionFactory, "account/log-in")
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
                    var userService = new UserService(RequestSession);

                    var user = userService.GetUser(viewModel.Username);

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