namespace YorkshireTec.ViewModels.Account
{
    using FluentValidation;

    public class AccountLogInViewModel
    {
        public string Username { get; set; }
        public string Password { get; set; }
    }

    public class AccountLogInViewModelValidator : AbstractValidator<AccountLogInViewModel>
    {
        public AccountLogInViewModelValidator()
        {
            RuleFor(x => x.Username).NotEmpty();
            RuleFor(x => x.Password).NotEmpty();
        }
    }
}