namespace YorkshireDigital.Web.Account.ViewModels
{
    using System.Web.Helpers;
    using YorkshireDigital.Data.Domain.Account;
    using YorkshireDigital.Data.Domain.Account.Enums;

    public class AccountRegisterViewModel
    {
        public string Username { get; set; }
        public string Password { get; set; }
        public string ConfirmPassword { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public bool MailingList { get; set; }

        public User ToUser()
        {
            return new User
            {
                Username = Username,
                Password = Crypto.HashPassword(Password),
                Email = Email,
                Validated = false,
                Name = Name,
                MailingListState = MailingList ? MailingListState.PendingSubscribe : MailingListState.Unsubscribed,
                MailingListEmail = MailingList ? Email : string.Empty
            };
        }
    }
}