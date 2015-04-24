namespace YorkshireDigital.Web.Events.Validation
{
    using System;
    using System.Globalization;
    using FluentValidation;
    using YorkshireDigital.Web.Events.ViewModels;

    public class CalendarSearchModelValidator : AbstractValidator<CalendarSearchModel>
    {
        public CalendarSearchModelValidator()
        {
            DateTime validationDate;
            RuleFor(x => x.From).Matches(@"^(\d{2}/\d{2}/\d{4})?$")
                .WithMessage("From date is not a valid date. Please supply a date in the format dd/MM/yyyy");
            RuleFor(x => x.From)
                .Must(x => string.IsNullOrEmpty(x) ||
                        DateTime.TryParseExact(x, "dd/MM/yyyy", CultureInfo.CurrentCulture, DateTimeStyles.None,
                            out validationDate))
                .WithMessage("From date is not a valid date. Please supply a date in the format dd/MM/yyyy");
            
            RuleFor(x => x.To).Matches(@"^(\d{2}/\d{2}/\d{4})?$")
                .WithMessage("To date is not a valid date. Please supply a date in the format dd/MM/yyyy");
            RuleFor(x => x.To)
                .Must(x => string.IsNullOrEmpty(x) ||
                        DateTime.TryParseExact(x, "dd/MM/yyyy", CultureInfo.CurrentCulture, DateTimeStyles.None,
                            out validationDate))
                .WithMessage("To date is not a valid date. Please supply a date in the format dd/MM/yyyy");
        }
    }
}