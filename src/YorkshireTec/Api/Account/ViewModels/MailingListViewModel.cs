namespace YorkshireTec.Api.Account.ViewModels
{
    using System;
    using FluentValidation;

    public class MailingListViewModel
    {
        public Guid UserId { get; set; }
        public string Email { get; set; }
    }

    public class MailingListViewModelValidator : AbstractValidator<MailingListViewModel>
    {
        public MailingListViewModelValidator()
        {
            RuleFor(x => x.UserId).NotEmpty();
            RuleFor(x => x.Email).NotEmpty();
            RuleFor(x => x.Email).EmailAddress();
        }
    }
}