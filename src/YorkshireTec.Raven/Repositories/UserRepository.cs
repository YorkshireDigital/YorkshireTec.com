namespace YorkshireTec.Raven.Repositories
{
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
            user.Providers.Add(provider);
            documentSession.SaveChanges();
        }
    }
}
