namespace YorkshireDigital.Web.Account.Modules
{
    using Nancy;
    using Nancy.Security;
    using YorkshireDigital.Data.Services;
    using YorkshireDigital.Web.Account.ViewModels;
    using YorkshireDigital.Web.Infrastructure;

    public class AccountModule : BaseModule
    {
        public AccountModule(IUserService userService)
            : base("account")
        {
            this.RequiresFeature("Account");
            this.RequiresAuthentication();

            Get[""] = _ =>
            {
                var user = userService.GetUser(Context.CurrentUser.UserName);
                var viewModel = new AccountViewModel(user);

                @ViewBag.Title = "Account : YorkshireDigital";
                return View["Index", viewModel];
            };


            Post[""] = _ =>
            {
                this.ValidateCsrfToken();

                AccountViewModel viewModel;
                var result = BindAndValidateModel(out viewModel);

                if (result.IsValid)
                {
                    var user = userService.GetUserById(viewModel.Id);

                    if (user.Username != viewModel.Username)
                    {
                        var existing = userService.GetUser(viewModel.Username);
                        if (existing != null)
                        {
                            AddError("Username", "Username is already taken");
                        }
                    }
                    if (user.Email != viewModel.Email)
                    {
                        var existing = userService.GetUser(viewModel.Email);
                        if (existing != null)
                        {
                            AddError("Email", "Email is already registered");
                        }
                    }

                    user.Username = viewModel.Username;
                    user.Name = viewModel.Name;
                    user.Email = viewModel.Email;
                    user.Picture = viewModel.Picture;

                    userService.SaveUser(user);
                    // model.Page.Notifications.Add(new NotificationModel("Details Updated", "", NotificationType.Success));
                }

                @ViewBag.Title = "Account : YorkshireDigital";
                return Response.AsRedirect("/account");
            };
        }
    }
}