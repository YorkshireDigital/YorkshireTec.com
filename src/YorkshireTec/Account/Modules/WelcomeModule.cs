using Nancy;

namespace YorkshireTec.Account.Modules
{
    using Nancy.Security;
    using YorkshireTec.Account.ViewModels;
    using YorkshireTec.Infrastructure;

    public class WelcomeModule : BaseModule
    {
        public WelcomeModule()
            : base("Account/Welcome")
        {
            this.RequiresAuthentication();

            Get["/"] = _ =>
            {
                var model = GetBaseModel(new WelcomeViewModel());
                model.Page.Title = "Welcome";
                model.ViewModel.Email = ((UserIdentity) Context.CurrentUser).Email;
                return Negotiate.WithModel(model).WithView("Welcome");
            };
        }
    }
}