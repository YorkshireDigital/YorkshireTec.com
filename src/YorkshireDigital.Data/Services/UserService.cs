using ISession = NHibernate.ISession;
using LinqExtensionMethods = NHibernate.Linq.LinqExtensionMethods;

namespace YorkshireDigital.Data.Services
{
    using System;
    using System.Linq;
    using global::NHibernate.Linq;
    using YorkshireDigital.Data.Domain.Account;

    public class UserService
    {
        private readonly ISession session;

        public UserService(ISession session)
        {
            this.session = session;
        }

        public bool UsernameAvailable(string username)
        {
            return !LinqExtensionMethods.Query<User>(session).Any(x => x.Username != null && x.Username == username);
        }

        public bool EmailAlreadyRegistered(string email)
        {
            if (email == null) return false;

            return LinqExtensionMethods.Query<User>(session).Any(x => x.Email != null && x.Email == email);
        }

        public User GetUser(string username)
        {
            return session.Query<User>()
                          .Fetch(x => x.Providers)
                          .FirstOrDefault(x => x.Username != null && x.Username == username
                                            || x.Email != null && x.Email == username);
        }

        public User GetUserByIdentity(string providerName, string username)
        {
            return LinqExtensionMethods.Query<User>(session).FirstOrDefault(x => x.Providers.Any(p => p.Name == providerName && p.Username == username));
        }

        public User SaveUser(User user)
        {
            session.Save(user);

            return user;
        }

        public void LinkIdentity(Provider provider, User user)
        {
            if (user.Providers.Any(x => x.Name == provider.Name))
            {
                var existing = user.Providers.FirstOrDefault(x => x.Name == provider.Name && x.Username == provider.Username);
                
                if (existing == null || !existing.Expired) return;

                user.Providers.Remove(existing);
                user.Providers.Add(provider);
            }
            else
            {
                user.Providers.Add(provider);
            }
            session.Save(user);
        }
        
        public User GetUserById(Guid id)
        {
            return session.Get<User>(id);
        }

        public User GetUserByEmail(string email)
        {
            return session.QueryOver<User>().Where(x => x.Email == email)
                .SingleOrDefault();
        }
    }
}
