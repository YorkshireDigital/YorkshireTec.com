namespace YorkshireTec.Raven.Domain.Account
{
    using System;
    using SimpleAuthentication.Core;

    public class Provider
    {
        public string Name { get; set; }
        public string PublicToken { get; set; }
        public string SecretToken { get; set; }
        public DateTime ExpiresOn { get; set; }
        public string Username { get; set; }

        public bool Expired
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
