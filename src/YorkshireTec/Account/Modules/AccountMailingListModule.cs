namespace YorkshireTec.Account.Modules
{
    using global::Raven.Client;
    using Nancy.ModelBinding;
    using Nancy.Validation;
    using YorkshireTec.Account.ViewModels;
    using YorkshireTec.Infrastructure;
    using YorkshireTec.Infrastructure.Helpers;
    using YorkshireTec.Raven.Repositories;

    public class AccountMailingListModule : BaseModule
    {
        public AccountMailingListModule(IDocumentSession documentSession)
            : base("account/mailinglist")
        {
            Post["/"] = _ =>
            {
                var viewModel = this.Bind<MailingListViewModel>();
                var result = this.Validate(viewModel);

                if (result.IsValid)
                {
                    var userRepository = new UserRepository(documentSession);
                    var user = userRepository.GetUserById(viewModel.UserId);

                    user.Email = viewModel.Email;
                    user.OnMailingList = viewModel.JoinMailingList;
                    if (viewModel.JoinMailingList)
                    {
                        MailChimpHelper.AddSubscriber(user.Email, user.Name, user.Twitter, string.Empty);
                    }
                    else
                    {
                        MailChimpHelper.Unsubscribe(user.Email, user.Name, user.Twitter, string.Empty);
                    }

                    
                    userRepository.SaveUser(user);
                }
                return 200;
            };
        }
    }
}