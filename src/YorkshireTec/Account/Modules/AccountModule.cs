namespace YorkshireTec.Account.Modules
{
    using global::Raven.Client;
    using Nancy;
    using Nancy.Security;
    using YorkshireTec.Account.ViewModels;
    using YorkshireTec.Infrastructure;
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
        }
    }
}