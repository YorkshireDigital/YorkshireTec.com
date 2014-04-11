namespace YorkshireTec.Account.ViewModels
{
    using System;
    using YorkshireTec.Raven.Domain.Account;

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