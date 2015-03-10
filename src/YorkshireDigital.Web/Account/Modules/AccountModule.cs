namespace YorkshireDigital.Web.Account.Modules
{
    using Nancy;
    using Nancy.Security;
    using NHibernate;
    using YorkshireDigital.Data.Services;
    using YorkshireDigital.Web.Account.ViewModels;
    using YorkshireDigital.Web.Infrastructure;

    public class AccountModule : BaseModule
    {
        public AccountModule(ISessionFactory sessionFactory)
            : base(sessionFactory, "account")
        {
            this.RequiresFeature("Account");
            this.RequiresAuthentication();

            Get[""] = _ =>
            {
                var userSession = new UserService(RequestSession);
                var user = userSession.GetUser(Context.CurrentUser.UserName);
                var viewModel = new AccountViewModel(user);

                @ViewBag.Title = "Account : YorkshireDigital";
                return View["Index", viewModel];
            };


            Post[""] = _ =>
            {
                AccountViewModel viewModel;
                var result = BindAndValidateModel(out viewModel);

                if (result.IsValid)
                {
                    var userSession = new UserService(RequestSession);
                    var user = userSession.GetUserById(viewModel.Id);

                    if (user.Username != viewModel.Username)
                    {
                        var existing = userSession.GetUser(viewModel.Username);
                        if (existing != null)
                        {
                            AddError("Username", "Username is already taken");
                        }
                    }
                    if (user.Email != viewModel.Email)
                    {
                        var existing = userSession.GetUser(viewModel.Email);
                        if (existing != null)
                        {
                            AddError("Email", "Email is already registered");
                        }
                    }

                    user.Username = viewModel.Username;
                    user.Name = viewModel.Name;
                    user.Email = viewModel.Email;
                    user.Picture = viewModel.Picture;

                    userSession.SaveUser(user);
                    // model.Page.Notifications.Add(new NotificationModel("Details Updated", "", NotificationType.Success));
                }

                @ViewBag.Title = "Account : YorkshireDigital";
                return Response.AsRedirect("/account");
            };
        }
    }
}