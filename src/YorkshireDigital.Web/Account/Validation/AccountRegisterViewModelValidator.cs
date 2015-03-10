namespace YorkshireDigital.Web.Account.Validation
{
    using System.Text.RegularExpressions;
    using FluentValidation;
    using YorkshireDigital.Web.Account.ViewModels;

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