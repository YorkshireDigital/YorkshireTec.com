namespace YorkshireTec.Account.ViewModels
{
    using FluentValidation;

    public class MailingListViewModel
    {
        public string Email { get; set; }
        public bool OnMailingList { get; set; }
    }

    public class MailingListViewModelValidator : AbstractValidator<MailingListViewModel>
    {
        public MailingListViewModelValidator()
        {
            RuleFor(x => x.Email).NotEmpty();
            RuleFor(x => x.Email).EmailAddress();
        }
    }
}