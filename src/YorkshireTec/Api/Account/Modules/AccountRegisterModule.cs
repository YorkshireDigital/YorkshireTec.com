namespace YorkshireTec.Api.Account.Modules
{
    using Nancy;
    using Nancy.Authentication.Forms;
    using Nancy.ModelBinding;
    using Nancy.Validation;
    using NHibernate;
    using YorkshireDigital.Data.Services;
    using YorkshireTec.Api.Account.ViewModels;
    using YorkshireTec.Api.Infrastructure;
    using YorkshireTec.Api.Infrastructure.Helpers;

    public class AccountRegisterModule : BaseModule
    {
        public AccountRegisterModule(ISessionFactory sessionFactory)
            : base(sessionFactory, "account/register")
        {
            this.RequiresFeature("Account");
            Get["/"] = _ =>
            {
                var model = GetBaseModel(new AccountRegisterViewModel());
                model.Page.Title = "Register";
                return Negotiate.WithModel(model).WithView("Register");
            };

            Post["/"] = _ =>
            {
                var viewModel = this.Bind<AccountRegisterViewModel>();
                var result = this.Validate(viewModel);

                var model = GetBaseModel(viewModel);
                model.Page.Title = "Register";

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
                            var updateText = string.Format("{0} just signed up at {1}. Go {0}!", user.Name, Context.Request.Url.SiteBase);
                            SlackHelper.PostToSlack(new SlackUpdate { channel = "#website", icon_emoji = ":yorks:", username = "New User", text = updateText });
                            return this.LoginAndRedirect(user.Id, null, "~/account/welcome");
                        }
                        model.Page.AddError("This email is already registered", "Email");
                    }
                    else
                    {
                        model.Page.AddError("Username taken", "Username");
                    }
                }
                else
                {
                    model.Page.AddErrors(result);
                }
                return Negotiate.WithModel(model).WithView("Register");
            };
        }
    }
}