namespace YorkshireTec.Api.Account.Modules
{
    using System;
    using Nancy;
    using Nancy.ModelBinding;
    using Nancy.Security;
    using Nancy.Validation;
    using NHibernate;
    using YorkshireTec.Api.Account.ViewModels;
    using YorkshireTec.Api.Infrastructure;
    using YorkshireTec.Api.Infrastructure.Helpers;
    using YorkshireTec.Data.Domain.Account.Enums;
    using YorkshireTec.Data.Services;

    public class AccountMailingListModule : BaseModule
    {
        public AccountMailingListModule(ISessionFactory sessionFactory)
            : base(sessionFactory, "account/mailinglist")
        {
            this.RequiresFeature("Account");
            this.RequiresAuthentication();

            Post["/subscribe"] = _ =>
            {
                var viewModel = this.Bind<MailingListViewModel>();
                var result = this.Validate(viewModel);

                if (result.IsValid)
                {
                    var userService = new UserService(RequestSession);
                    var user = userService.GetUserById(viewModel.UserId);

                    user.Email = viewModel.Email;
                    user.MailingListState = MailingListState.PendingSubscribe;
                    
                    MailChimpHelper.AddSubscriber(user.Email, user.Name, user.Twitter, string.Empty);

                    userService.SaveUser(user);

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
                    var userService = new UserService(RequestSession);
                    var user = userService.GetUserById(viewModel.UserId);

                    user.Email = viewModel.Email;
                    user.MailingListState = MailingListState.PendingUnsubscribe;
                   
                    MailChimpHelper.Unsubscribe(user.Email, user.Name, user.Twitter, string.Empty);

                    userService.SaveUser(user);

                    return 200;
                }
                return 500;
            };

            Get["/subscribed"] = _ =>
            {
                var userService = new UserService(RequestSession);
                userService.GetUserById(new Guid(((UserIdentity)Context.CurrentUser).UserId));

                var model = GetBaseModel(new SubscribedViewModel());
                model.Page.Title = "Welcome";

                return Negotiate.WithModel(model).WithView("Subscribed");
            };

            Get["/unsubscribed"] = _ =>
            {
                var userService = new UserService(RequestSession);
                userService.GetUserById(new Guid(((UserIdentity)Context.CurrentUser).UserId));

                var model = GetBaseModel(new UnsubscribedViewModel());
                model.Page.Title = "Sorry";

                return Negotiate.WithModel(model).WithView("Unsubscribed");
            };
        }
    }
}