namespace YorkshireTec.Data.Domain.Account
{
    using System;
    using SimpleAuthentication.Core;

    public class Provider
    {
        public virtual int Id { get; set; }
        public virtual string Name { get; set; }
        public virtual string PublicToken { get; set; }
        public virtual string SecretToken { get; set; }
        public virtual DateTime ExpiresOn { get; set; }
        public virtual string Username { get; set; }

        public virtual bool Expired
        {
            get { return ExpiresOn != new DateTime() && ExpiresOn < DateTime.Now; }
        }

        public static Provider FromAuthenticatedClient(IAuthenticatedClient authenticatedClient)
        {
            return new Provider
            {
                Name = authenticatedClient.ProviderName,
                PublicToken = authenticatedClient.AccessToken.PublicToken,
                SecretToken = authenticatedClient.AccessToken.SecretToken,
                ExpiresOn = authenticatedClient.AccessToken.ExpiresOn,
                Username = authenticatedClient.UserInformation.UserName
            };
        }
    }
}
