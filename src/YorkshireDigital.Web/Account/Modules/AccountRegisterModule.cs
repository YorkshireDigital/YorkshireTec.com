namespace YorkshireDigital.Web.Account.Modules
{
    using Nancy;
    using Nancy.Authentication.Forms;
    using NHibernate;
    using Slack.Webhooks;
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
                AccountRegisterViewModel viewModel;
                var result = BindAndValidateModel(out viewModel);

                if (result.IsValid)
                {
                    var userService = new UserService(RequestSession);

                    if (userService.UsernameAvailable(viewModel.Username))
                    {
                        if (!userService.EmailAlreadyRegistered(viewModel.Email))
                        {
                            if (viewModel.MailingList)
                            {
                                MailChimpHelper.AddSubscriber(viewModel.Email, viewModel.Name, string.Empty, string.Empty);
                            }
                            var user = userService.SaveUser(viewModel.ToUser());

                            SlackHelper.PostNewUserUpdate(viewModel.Username, viewModel.Name, viewModel.Email, viewModel.MailingList, Context.Request.Url.SiteBase);
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