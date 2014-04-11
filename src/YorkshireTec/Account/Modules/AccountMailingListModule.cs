﻿namespace YorkshireTec.Account.Modules
{
    using global::Raven.Client;
    using Nancy.ModelBinding;
    using Nancy.Security;
    using Nancy.Validation;
    using YorkshireTec.Account.ViewModels;
    using YorkshireTec.Infrastructure;
    using YorkshireTec.Infrastructure.Helpers;
    using YorkshireTec.Raven.Domain.Account;
    using YorkshireTec.Raven.Repositories;

    public class AccountMailingListModule : BaseModule
    {
        public AccountMailingListModule(IDocumentSession documentSession)
            : base("account/mailinglist")
        {
            this.RequiresAuthentication();

            Post["/subscribe"] = _ =>
            {
                var viewModel = this.Bind<MailingListViewModel>();
                var result = this.Validate(viewModel);

                if (result.IsValid)
                {
                    var userRepository = new UserRepository(documentSession);
                    var user = userRepository.GetUserById(viewModel.UserId);

                    user.Email = viewModel.Email;
                    user.MailingListState = MailingListState.PendingSubscribe;
                    
                    MailChimpHelper.AddSubscriber(user.Email, user.Name, user.Twitter, string.Empty);

                    userRepository.SaveUser(user);

                    return 200;
                }
                return 500;
            };

            Post["/unsubscribe"] = _ =>
            {
                var viewModel = this.Bind<MailingListViewModel>();
                var result = this.Validate(viewModel);

                if (result.IsValid)
                {
                    var userRepository = new UserRepository(documentSession);
                    var user = userRepository.GetUserById(viewModel.UserId);

                    user.Email = viewModel.Email;
                    user.MailingListState = MailingListState.PendingUnsubscribe;
                   
                    MailChimpHelper.Unsubscribe(user.Email, user.Name, user.Twitter, string.Empty);

                    userRepository.SaveUser(user);
                }
                return 200;
            };
        }
    }
}