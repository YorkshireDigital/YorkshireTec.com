namespace YorkshireTec.Account.Modules
{
    using System.Configuration;
    using MailChimp;
    using MailChimp.Helper;
    using Nancy.ModelBinding;
    using Nancy.Validation;
    using YorkshireTec.Account.ViewModels;
    using YorkshireTec.Infrastructure;

    public class AccountMailingListModule : BaseModule
    {
        public AccountMailingListModule()
            : base("account/mailinglist")
        {
            Post["/"] = _ =>
            {
                var viewModel = this.Bind<MailingListViewModel>();
                var result = this.Validate(viewModel);

                if (result.IsValid)
                {
                    var mc = new MailChimpManager(ConfigurationManager.AppSettings["MailChimp_ApiKey"]);
                    var email = new EmailParameter
                    {
                        Email = viewModel.Email
                    };
                    var results = mc.Subscribe(ConfigurationManager.AppSettings["MailChimp_ListId"], email);
                }
                return 200;
            };
        }
    }
}