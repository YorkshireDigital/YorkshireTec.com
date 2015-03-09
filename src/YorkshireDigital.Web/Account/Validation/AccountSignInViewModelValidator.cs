namespace YorkshireDigital.Web.Account.Validation
{
    using FluentValidation;
    using YorkshireDigital.Web.Account.ViewModels;

    public class AccountSignInViewModelValidator : AbstractValidator<AccountSignInViewModel>
    {
        public AccountSignInViewModelValidator()
        {
            RuleFor(x => x.Username).NotEmpty();
            RuleFor(x => x.Password).NotEmpty();
        }
    }
}