namespace YorkshireTec.Account.Modules
{
    using global::Raven.Client;
    using Nancy;
    using Nancy.Authentication.Forms;
    using Nancy.ModelBinding;
    using Nancy.Validation;
    using YorkshireTec.Account.ViewModels;
    using YorkshireTec.Infrastructure;
    using YorkshireTec.Raven.Repositories;

    public class AccountRegisterModule : BaseModule
    {
        public AccountRegisterModule(IDocumentSession documentSession)
            : base("account/register")
        {
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
                    var userRepository = new UserRepository(documentSession);

                    if (userRepository.UsernameAvailable(viewModel.Username))
                    {
                        if (!userRepository.EmailAlreadyRegistered(viewModel.Email))
                        {
                            var user = userRepository.AddUser(viewModel.ToUser());
                            return this.LoginAndRedirect(user.Id, null, "~/");
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