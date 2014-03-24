namespace YorkshireTec.Raven.Repositories
{
    using System;
    using System.Linq;
    using global::Raven.Client;
    using YorkshireTec.Raven.Domain.Account;

    public class UserRepository
    {
        private readonly IDocumentSession documentSession;

        public UserRepository(IDocumentSession documentSession)
        {
            this.documentSession = documentSession;
        }

        public bool UsernameAvailable(string username)
        {
            return !documentSession.Query<User>().Any(x => x.Username == username);
        }

        public bool EmailAlreadyRegistered(string email)
        {
            return documentSession.Query<User>().Any(x => x.Email == email);
        }

        public User GetUser(string username)
        {
            return documentSession.Query<User>().FirstOrDefault(x => x.Username == username || x.Email == username);
        }

        public User GetUserByIdentity(string providerName, string username)
        {
            return documentSession.Query<User>().FirstOrDefault(x => x.Providers.Any(p => p.Name == providerName && p.Username == username));
        }

        public User AddUser(User user)
        {
            documentSession.Store(user);
            documentSession.SaveChanges();

            return user;
        }

        public void LinkIdentity(Provider provider, User user)
        {
            if (user.Providers.Any(x => x.Name == provider.Name))
            {
                var existing = user.Providers.FirstOrDefault(x => x.Name == provider.Name && x.Username == provider.Username);
                if (existing != null && existing.Expired)
                {
                    user.Providers.Remove(existing);
                    user.Providers.Add(provider);
                }
            }
            else
            {
                user.Providers.Add(provider);
            }
            documentSession.SaveChanges();
        }
    }
}
