namespace YorkshireDigital.Web.Account.Modules
{
    using Nancy;
    using Nancy.Authentication.Forms;
    using Nancy.Security;
    using NHibernate;
    using YorkshireDigital.Data.Domain.Account.Enums;
    using YorkshireDigital.Data.Helpers;
    using YorkshireDigital.Data.Services;
    using YorkshireDigital.Web.Account.ViewModels;
    using YorkshireDigital.Web.Infrastructure;
    using YorkshireDigital.Web.Infrastructure.Helpers;

    public class AccountRegisterModule : BaseModule
    {
        public AccountRegisterModule(ISessionFactory sessionFactory)
            : base(sessionFactory, "account/register")
        {
            this.RequiresFeature("Account");
            Get["/"] = _ =>
            {
                var model = new AccountRegisterViewModel();
                @ViewBag.Title = "Register : YorkshireDigital";
                return Negotiate.WithModel(model).WithView("Register");
            };

            Post["/"] = _ =>
            {
                this.ValidateCsrfToken();

                AccountRegisterViewModel viewModel;
                var result = BindAndValidateModel(out viewModel);

                if (result.IsValid)
                {
                    var userService = new UserService(RequestSession);

                    if (userService.UsernameAvailable(viewModel.Username))
                    {
                        if (!userService.EmailAlreadyRegistered(viewModel.Email))
                        {
                            var user = userService.SaveUser(viewModel.ToUser());

                            if (viewModel.MailingList && user.MailingListState == MailingListState.Unsubscribed)
                            {
                                MailChimpHelper.AddSubscriber(viewModel.Email, viewModel.Name, string.Empty, string.Empty);
                            }

                            SlackHelper.PostNewUserUpdate(viewModel.Username, viewModel.Name, viewModel.Email, user.MailingListState != MailingListState.Unsubscribed, Context.Request.Url.SiteBase);
                            return this.LoginAndRedirect(user.Id, null, "~/account/welcome");
                        }
                        AddError("Email", "This email is already registered");
                    }
                    else
                    {
                        AddError("Username", "Username taken");
                    }
                }

                @ViewBag.Title = "Register : YorkshireDigital";
                return Negotiate.WithModel(viewModel).WithView("Register");
            };
        }
    }
}