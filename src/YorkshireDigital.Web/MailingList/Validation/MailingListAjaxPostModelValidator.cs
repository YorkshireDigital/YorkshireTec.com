namespace YorkshireDigital.Web.MailingList.Validation
{
    using FluentValidation;
    using YorkshireDigital.Web.MailingList.ViewModels;

    public class MailingListAjaxPostModelValidator : AbstractValidator<MailingListAjaxPostModel>
    {
        public MailingListAjaxPostModelValidator()
        {
            RuleFor(x => x.Email).NotEmpty();
            RuleFor(x => x.Email).EmailAddress();
        }
    }
}