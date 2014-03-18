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

        public User GetUser(string username)
        {
            return documentSession.Query<User>().FirstOrDefault(x => x.Username == username);
        }

        public User GetUserByIdentity(string providerName, string username)
        {
            return documentSession.Query<User>().FirstOrDefault(x => x.Providers.Any(p => p.Name == providerName && p.Username == username));
        }

        public void AddUser(User user)
        {
            documentSession.Store(user);
            documentSession.SaveChanges();
        }

        public void LinkIdentity(Provider provider, User user)
        {
            user.Providers.Add(provider);
            documentSession.SaveChanges();
        }
    }
}
