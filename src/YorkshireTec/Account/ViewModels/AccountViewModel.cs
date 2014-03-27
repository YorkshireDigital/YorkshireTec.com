﻿namespace YorkshireTec.Account.ViewModels
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using global::Raven.Abstractions.Extensions;
    using YorkshireTec.Raven.Domain.Account;

    public class AccountViewModel
    {
        public Guid Id { get; set; }
        public bool IsAdmin { get; set; }
        public string Username { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string Picture { get; set; }
        public ProviderListViewModel Providers { get; set; }
        public bool IsAuthenticated { get; set; }

        public AccountViewModel(User user)
        {
            Id = user.Id;
            IsAdmin = user.IsAdmin;
            Username = user.Username;
            Name = user.Name;
            Email = user.Email;
            Picture = user.Picture;
            Providers = new ProviderListViewModel();
            Providers.AddRange(user.Providers.Select(x => new ProviderViewModel(x)));
            IsAuthenticated = user.IsAuthenticated;
        }
    }
}