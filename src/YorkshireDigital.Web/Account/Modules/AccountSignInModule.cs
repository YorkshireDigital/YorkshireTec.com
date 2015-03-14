namespace YorkshireDigital.Web.Account.Modules
{
    using System;
    using System.Web.Helpers;
    using Nancy;
    using Nancy.Authentication.Forms;
    using Nancy.Security;
    using NHibernate;
    using YorkshireDigital.Data.Services;
    using YorkshireDigital.Web.Account.ViewModels;
    using YorkshireDigital.Web.Infrastructure;

    public class AccountSignInModule : BaseModule
    {
        public AccountSignInModule(ISessionFactory sessionFactory)
            : base(sessionFactory, "account/sign-in")
        {
            this.RequiresFeature("Account");

            Get["/"] = _ =>
            {
                var model = new AccountSignInViewModel();
                @ViewBag.Title = "Sign In : YorkshireDigital";
                return View["SignIn", model];
            };

            Post["/"] = _ =>
            {
                #region CSRF
                try
                {
                    this.ValidateCsrfToken();
                }
                catch (CsrfValidationException)
                {
                    return Response.AsText("Csrf Token not valid.").WithStatusCode(HttpStatusCode.Forbidden);
                }
                #endregion

                AccountSignInViewModel viewModel;
                var result = BindAndValidateModel(out viewModel);

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
                    AddError("", "Unrecognised username or password");
                }

                @ViewBag.Title = "SignIn In : YorkshireDigital";
                return Negotiate.WithModel(viewModel).WithView("SignIn");
            };
        }
    }
}