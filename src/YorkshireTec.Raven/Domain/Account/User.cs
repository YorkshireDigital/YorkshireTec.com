namespace YorkshireTec.Raven.Domain.Account
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
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
        public bool Validated { get; set; }
        public MailingListState MailingListState { get; set; }

        public User()
        {
            Providers = new List<Provider>();
        }

        public string Twitter
        {
            get
            {
                return 
                    Providers.Any(x => x.Name == "twitter")
                        ? string.Format("@{0}",Providers.First(x => x.Name == "twitter").Username)
                        : string.Empty;
            }
        }

        public static User FromAuthenticatedClient(IAuthenticatedClient authenticatedClient)
        {
            var newUser = new User
            {
                Username = authenticatedClient.UserInformation.UserName ?? string.Empty,
                Name = authenticatedClient.UserInformation.Name ?? string.Empty,
                Email = authenticatedClient.UserInformation.Email ?? string.Empty,
                Gender = GenderTypeHelpers.ToGenderType(authenticatedClient.UserInformation.Gender.ToString()),
                Locale = authenticatedClient.UserInformation.Locale ?? string.Empty,
                Picture = authenticatedClient.UserInformation.Picture ?? string.Empty,
                Validated = true,
                MailingListState = MailingListState.Unsubscribed
            };
            var accessToken = Provider.FromAuthenticatedClient(authenticatedClient);
            newUser.Providers.Add(accessToken);

            return newUser;
        }
    }
}
