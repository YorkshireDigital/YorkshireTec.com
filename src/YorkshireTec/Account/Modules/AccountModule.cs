namespace YorkshireTec.Account.Modules
{
    using Nancy;
    using Nancy.ModelBinding;
    using Nancy.Security;
    using Nancy.Validation;
    using NHibernate;
    using YorkshireTec.Account.ViewModels;
    using YorkshireTec.Data.Services;
    using YorkshireTec.Infrastructure;
    using YorkshireTec.Infrastructure.Models;

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
                var model = GetBaseModel(new AccountViewModel(user));
                model.Page.Title = "Account";
                return Negotiate.WithModel(model).WithView("Index");
            };


            Post[""] = _ =>
            {
                var viewModel = this.Bind<AccountViewModel>();
                var result = this.Validate(viewModel);

                var model = GetBaseModel(viewModel);
                if (result.IsValid)
                {
                    var userSession = new UserService(RequestSession);
                    var user = userSession.GetUserById(viewModel.Id);

                    if (user.Username != viewModel.Username)
                    {
                        var existing = userSession.GetUser(viewModel.Username);
                        if (existing != null)
                        {
                            model.Page.AddError("Username is already taken", "Username");
                        }
                    }
                    if (user.Email != viewModel.Email)
                    {
                        var existing = userSession.GetUser(viewModel.Email);
                        if (existing != null)
                        {
                            model.Page.AddError("Email is already registered", "Email");
                        }
                    }

                    user.Username = viewModel.Username;
                    user.Name = viewModel.Name;
                    user.Email = viewModel.Email;
                    user.Picture = viewModel.Picture;

                    userSession.SaveUser(user);
                    model.Page.Notifications.Add(new NotificationModel("Details Updated", "", NotificationType.Success));

                    model = GetBaseModel(new AccountViewModel(user));
                }
                model.Page.Title = "Account";
                return Negotiate.WithModel(model).WithView("Index");
            };
        }
    }
}