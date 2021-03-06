﻿namespace YorkshireTec.Api.Account.ViewModels
{
    using YorkshireDigital.Data.Domain.Account;

    public class ProviderViewModel
    {
        public string Name { get; set; }
        public string Username { get; set; }
        public string ImageUrl { get; set; }

        public ProviderViewModel(Provider provider)
        {
            Name = provider.Name;
            Username = provider.Username;
            ImageUrl = provider.Name;
        }
    }
}