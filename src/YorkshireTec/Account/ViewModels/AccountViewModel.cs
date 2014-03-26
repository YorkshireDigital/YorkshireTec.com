namespace YorkshireTec.Account.ViewModels
{
    using System;
    using System.Collections.Generic;
    using YorkshireTec.Raven.Domain.Account;

    public class AccountViewModel
    {
        public Guid Id { get; set; }
        public bool IsAdmin { get; set; }
        public string Username { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string Picture { get; set; }
        public IList<Provider> Providers { get; set; }
        public bool IsAuthenticated { get; set; }

        public AccountViewModel(User user)
        {
            Id = user.Id;
            IsAdmin = user.IsAdmin;
            Username = user.Username;
            Name = user.Name;
            Email = user.Email;
            Picture = user.Picture;
            Providers = user.Providers;
            IsAuthenticated = user.IsAuthenticated;
        }
    }
}