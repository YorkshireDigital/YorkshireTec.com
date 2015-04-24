namespace YorkshireDigital.Data.Domain.Account
{
    using System;
    using SimpleAuthentication.Core;

    public class Provider
    {
        public virtual int Id { get; set; }
        public virtual string Name { get; set; }
        public virtual string PublicToken { get; set; }
        public virtual string SecretToken { get; set; }
        public virtual DateTime? ExpiresOn { get; set; }
        public virtual string Username { get; set; }

        public virtual bool Expired
        {
            get { return ExpiresOn.HasValue && ExpiresOn.Value != new DateTime() && ExpiresOn.Value < DateTime.Now; }
        }

        public static Provider FromAuthenticatedClient(IAuthenticatedClient authenticatedClient)
        {
            var provider = new Provider
            {
                Name = authenticatedClient.ProviderName,
                PublicToken = authenticatedClient.AccessToken.PublicToken,
                SecretToken = authenticatedClient.AccessToken.SecretToken,
                Username = authenticatedClient.UserInformation.UserName
            };
            if (authenticatedClient.AccessToken.ExpiresOn != new DateTime())
            {
                provider.ExpiresOn = authenticatedClient.AccessToken.ExpiresOn;
            }
            return provider;
        }
    }
}
