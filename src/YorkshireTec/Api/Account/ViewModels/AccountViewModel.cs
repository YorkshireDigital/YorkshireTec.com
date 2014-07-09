namespace YorkshireTec.Api.Account.ViewModels
{
    using System;
    using System.Linq;
    using FluentValidation;
    using YorkshireTec.Data.Domain.Account;

    public class AccountViewModel
    {
        public Guid Id { get; set; }
        public bool IsAdmin { get; set; }
        public string Username { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string Picture { get; set; }
        public ProviderListViewModel Providers { get; set; }
        public bool Validated { get; set; }
        public int MailingListState { get; set; }

        public AccountViewModel()
        {
            
        }

        public AccountViewModel(User user)
        {
            Id = user.Id;
            Username = user.Username;
            Name = user.Name;
            Email = user.Email;
            Picture = user.Picture;
            Providers = new ProviderListViewModel();
            Providers.AddRange(user.Providers.Select(x => new ProviderViewModel(x)));
            Validated = user.Validated;
            MailingListState = (int)user.MailingListState;
        }
    }

    public class AccountViewModelValidator : AbstractValidator<AccountViewModel>
    {
        public AccountViewModelValidator()
        {
            RuleFor(x => x.Username).NotEmpty();
            RuleFor(x => x.Name).NotEmpty();
            RuleFor(x => x.Email).NotEmpty();
            RuleFor(x => x.Email).EmailAddress();
        }
    }
}