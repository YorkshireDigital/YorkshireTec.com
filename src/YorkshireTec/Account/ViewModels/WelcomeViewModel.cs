namespace YorkshireTec.Account.ViewModels
{
    using System;

    public class WelcomeViewModel
    {
        public WelcomeViewModel(Raven.Domain.Account.User user)
        {
            UserId = user.Id;
            Email = user.Email;
            OnMailingList = user.OnMailingList;
        }

        public Guid UserId { get; set; }
        public string Email { get; set; }
        public bool OnMailingList { get; set; }
    }
}