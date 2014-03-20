namespace YorkshireTec.ViewModels.Account
{
    using System.Web.Helpers;
    using FluentValidation;
    using YorkshireTec.Raven.Domain.Account;

    public class AccountRegisterViewModel
    {
        public string Username { get; set; }
        public string Password { get; set; }
        public string ConfirmPassword { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }

        public User ToUser()
        {
            return new User
            {
                Username = Username,
                Password = Crypto.HashPassword(Password),
                Email = Email,
                IsAdmin = false,
                IsAuthenticated = false,
                FirstName = FirstName,
                LastName = LastName
            };
        }
    }

    public class AccountRegisterViewModelValidator : AbstractValidator<AccountRegisterViewModel>
    {
        public AccountRegisterViewModelValidator()
        {
            RuleFor(x => x.Username).NotEmpty();
            RuleFor(x => x.Password).NotEmpty();
            RuleFor(x => x.Password.Length).GreaterThan(5);
            RuleFor(x => x.ConfirmPassword).Equal(x => x.Password);
            RuleFor(x => x.FirstName).NotEmpty();
            RuleFor(x => x.LastName).NotEmpty();
            RuleFor(x => x.Email).NotEmpty();
            RuleFor(x => x.Email).EmailAddress();
        }
    }
}