namespace YorkshireTec.Raven.Domain.Account
{
    using System;
    using System.Collections.Generic;
    using SimpleAuthentication.Core;

    public class User
    {
        public Guid Id { get; set; }
        public bool IsAdmin { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public GenderType Gender { get; set; }
        public string Locale { get; set; }
        public string Picture { get; set; }
        public IList<Provider> Providers { get; set; }
        public bool IsAuthenticated { get; set; }

        public User()
        {
            Providers = new List<Provider>();
        }

        public static User FromAuthenticatedClient(IAuthenticatedClient authenticatedClient)
        {
            var newUser = new User
            {
                Username = authenticatedClient.UserInformation.UserName,
                Name = authenticatedClient.UserInformation.Name,
                Email = authenticatedClient.UserInformation.Email,
                Gender = GenderTypeHelpers.ToGenderType(authenticatedClient.UserInformation.Gender.ToString()),
                Locale = authenticatedClient.UserInformation.Locale,
                Picture = authenticatedClient.UserInformation.Picture,
                IsAuthenticated = true
            };
            var accessToken = Provider.FromAuthenticatedClient(authenticatedClient);
            newUser.Providers.Add(accessToken);

            return newUser;
        }
    }
}
