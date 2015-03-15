namespace YorkshireDigital.Web.MailingList.Modules
{
    using System;
    using System.Collections.Generic;
    using Nancy;
    using NHibernate;
    using YorkshireDigital.Data.Domain.Account;
    using YorkshireDigital.Data.Domain.Account.Enums;
    using YorkshireDigital.Data.Services;
    using YorkshireDigital.Web.Infrastructure;
    using YorkshireDigital.Web.Infrastructure.Helpers;
    using YorkshireDigital.Web.MailingList.ViewModels;

    public class MailingListModule : BaseModule
    {
        public MailingListModule(ISessionFactory sessionFactory)
            : base(sessionFactory, "mailinglist")
        {
            Get["/archive"] = _ =>
            {
                var viewModel = new ArchiveNewsletterViewModel
                {
                    Archives = MailChimpHelper.GetPastCampaigns()
                };

                return Negotiate.WithModel(viewModel)
                    .WithView("Archive");
            };

            Get["/confirmation"] = _ => View["Confirmation"];

            Get["/subscribed"] = _ => View["Confirmation"];

            Post["/subscribe"] = _ => ManageMailingListSubscription(true);

            Post["/unsubscribe"] = _ => ManageMailingListSubscription(false);

            Post["/webhooks"] = _ => ProcessWebHooks();
            Get["/webhooks"] = _ => ProcessWebHooks();
        }

        private Response ProcessWebHooks()
        {
            MailChimpWebHookPostModel model;
            var result = BindAndValidateModel(out model);

            if (!result.IsValid) return HttpStatusCode.OK;
            
            switch (model.Type)
            {
                case MailChimpWebHookType.subscribe:
                    ProcessSubscribeWebhook(model.Data);
                    break;
            }

            return HttpStatusCode.OK;
        }

        private void ProcessSubscribeWebhook(IReadOnlyDictionary<string, object> data)
        {
            var userService = new UserService(RequestSession);

            var email = data["Email"].ToString();
            var user = userService.GetUserByEmail(email);

            if (user == null) return;
            
            user.MailingListEmail = email;
            user.MailingListState = MailingListState.Subscribed;

            userService.SaveUser(user);
        }

        private Response ManageMailingListSubscription(bool subscribe)
        {
            MailingListAjaxPostModel model;
            var result = BindAndValidateModel(out model);

            AjaxValidateCsrfToken(model);

            if (!result.IsValid)
                return Response.AsJson(new {message = "Request failed validation"})
                               .WithStatusCode(HttpStatusCode.BadRequest);

            var userService = new UserService(RequestSession);
            var user = userService.GetUser(Context.CurrentUser.UserName);

            user.MailingListEmail = model.Email;

            try
            {
                if (subscribe)
                {
                    user.MailingListState = MailingListState.PendingSubscribe;
                    MailChimpHelper.AddSubscriber(model.Email, user.Name, user.Twitter, string.Empty);
                }
                else
                {
                    user.MailingListState = MailingListState.PendingUnsubscribe;
                    MailChimpHelper.Unsubscribe(model.Email, user.Name, user.Twitter, string.Empty);
                }
            }
            catch (Exception ex)
            {
                return Response.AsJson(ex.Message).WithStatusCode(HttpStatusCode.InternalServerError);
            }

            userService.SaveUser(user);

            return Response.AsJson(new { message =  "Confirmation email sent."}).WithStatusCode(HttpStatusCode.OK);
        }
    }
}