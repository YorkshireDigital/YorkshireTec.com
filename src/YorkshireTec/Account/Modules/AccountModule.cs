namespace YorkshireTec.Account.Modules
{
    using global::Raven.Client;
    using Nancy;
    using Nancy.ModelBinding;
    using Nancy.Security;
    using Nancy.Validation;
    using YorkshireTec.Account.ViewModels;
    using YorkshireTec.Infrastructure;
    using YorkshireTec.Infrastructure.Models;
    using YorkshireTec.Raven.Repositories;

    public class AccountModule : BaseModule
    {
        public AccountModule(IDocumentSession documentSession)
            : base("account")
        {
            this.RequiresAuthentication();
            Get[""] = _ =>
            {
                var userRepository = new UserRepository(documentSession);
                var user = userRepository.GetUser(Context.CurrentUser.UserName);
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
                    var userRepository = new UserRepository(documentSession);
                    var user = userRepository.GetUserById(viewModel.Id);

                    if (user.Username != viewModel.Username)
                    {
                        var existing = userRepository.GetUser(viewModel.Username);
                        if (existing != null)
                        {
                            model.Page.AddError("Username is already taken", "Username");
                        }
                    }
                    if (user.Email != viewModel.Email)
                    {
                        var existing = userRepository.GetUser(viewModel.Email);
                        if (existing != null)
                        {
                            model.Page.AddError("Email is already registered", "Email");
                        }
                    }

                    user.Username = viewModel.Username;
                    user.Name = viewModel.Name;
                    user.Email = viewModel.Email;
                    user.Picture = viewModel.Picture;

                    userRepository.SaveUser(user);
                    model.Page.Notifications.Add(new NotificationModel("Details Updated", "", NotificationType.Success));

                    model = GetBaseModel(new AccountViewModel(user));
                }
                model.Page.Title = "Account";
                return Negotiate.WithModel(model).WithView("Index");
            };
        }
    }
}