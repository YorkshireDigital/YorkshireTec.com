namespace YorkshireTec.Api.Account.ViewModels
{
    using System;
    using YorkshireDigital.Data.Domain.Account;
    using YorkshireDigital.Data.Domain.Account.Enums;

    public class WelcomeViewModel
    {
        public WelcomeViewModel(User user)
        {
            UserId = user.Id;
            Email = user.Email;
            OnMailingList = user.MailingListState == MailingListState.Subscribed || user.MailingListState == MailingListState.PendingSubscribe;
        }

        public Guid UserId { get; set; }
        public string Email { get; set; }
        public bool OnMailingList { get; set; }
    }
}