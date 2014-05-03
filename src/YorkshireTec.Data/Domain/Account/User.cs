namespace YorkshireTec.Data.Domain.Account
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using SimpleAuthentication.Core;
    using YorkshireTec.Data.Domain.Account.Enums;

    public class User
    {
        public virtual Guid Id { get; set; }
        public virtual bool IsAdmin { get; set; }
        public virtual string Username { get; set; }
        public virtual string Password { get; set; }
        public virtual string Name { get; set; }
        public virtual string Email { get; set; }
        public virtual GenderType Gender { get; set; }
        public virtual string Locale { get; set; }
        public virtual string Picture { get; set; }
        public virtual IList<Provider> Providers { get; set; }
        public virtual bool Validated { get; set; }
        public virtual MailingListState MailingListState { get; set; }

        public User()
        {
            Providers = new List<Provider>();
        }

        public virtual string Twitter
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
