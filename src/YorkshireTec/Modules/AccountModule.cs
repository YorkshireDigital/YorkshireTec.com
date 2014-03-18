namespace YorkshireTec.Modules
{
    using Nancy;
    using Nancy.ModelBinding;
    using Nancy.Validation;
    using YorkshireTec.ViewModels.Account;

    public class AccountModule : NancyModule
    {
        public AccountModule()
            : base("account")
        {
            Get["/log-in"] = _ =>
            {
                return Negotiate.WithModel(new AccountLogInViewModel()).WithView("LogIn");
            };

            Post["/log-in"] = _ =>
            {
                var model = this.Bind<AccountLogInViewModel>();
                var result = this.Validate(model);

                if (!result.IsValid)
                {
                    model.Username = "FAIL";
                    return Negotiate.WithModel(model).WithView("LogIn");
                }
                return Negotiate.WithModel(model).WithView("LogIn");
            };
        }
    }
}