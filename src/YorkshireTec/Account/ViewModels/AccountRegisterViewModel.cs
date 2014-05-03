namespace YorkshireTec.Account.ViewModels
{
    using System.Text.RegularExpressions;
    using System.Web.Helpers;
    using FluentValidation;
    using YorkshireTec.Data.Domain.Account;
    using YorkshireTec.Data.Domain.Account.Enums;

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
                IsAdmin = false,
                Validated = false,
                Name = Name,
                MailingListState = MailingList ? MailingListState.PendingSubscribe : MailingListState.Unsubscribed
            };
        }
    }

    public class AccountRegisterViewModelValidator : AbstractValidator<AccountRegisterViewModel>
    {
        public AccountRegisterViewModelValidator()
        {
            RuleFor(x => x.Username).NotEmpty();
            RuleFor(x => x.Username).Matches("^[a-z0-9]+$", RegexOptions.IgnoreCase).WithMessage("Username may only contain numbers and letters");
            RuleFor(x => x.Password).NotEmpty();
            RuleFor(x => x.Password.Length).GreaterThan(5);
            RuleFor(x => x.ConfirmPassword).Equal(x => x.Password);
            RuleFor(x => x.Name).NotEmpty();
            RuleFor(x => x.Name).Matches("^[a-z ]+$", RegexOptions.IgnoreCase).WithMessage("Username may only contain numbers and letters");
            RuleFor(x => x.Email).NotEmpty();
            RuleFor(x => x.Email).EmailAddress();
        }
    }
}