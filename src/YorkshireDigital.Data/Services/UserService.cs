using ISession = NHibernate.ISession;

namespace YorkshireDigital.Data.Services
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using global::NHibernate.Hql.Ast.ANTLR;
    using global::NHibernate.Linq;
    using YorkshireDigital.Data.Domain.Account;
    using YorkshireDigital.Data.Domain.Account.Enums;
    using YorkshireDigital.Data.Exceptions;
    using YorkshireDigital.Data.Helpers;

    public interface IUserService
    {
        bool UsernameAvailable(string username);
        bool EmailAlreadyRegistered(string email);
        User GetUser(string username);
        User GetUserByIdentity(string providerName, string username);
        User SaveUser(User user);
        void LinkIdentity(Provider provider, User user);
        User GetUserById(Guid id);
        User GetUserByEmail(string email);
        IList<User> GetActiveUsers(int take = 20, int skip = 0);
        User Disable(Guid username);
    }

    public class UserService : IUserService
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
            if (MailChimpHelper.IsEmailRegistered(user.Email))
            {
                user.MailingListState = MailingListState.Subscribed;
                user.MailingListEmail = user.Email;
            }

            user.LastEditedOn = DateTime.UtcNow;

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

        public IList<User> GetActiveUsers(int take = 20, int skip = 0)
        {
            return session.QueryOver<User>()
                .Where(x => x.DisabledOn == null)
                .Skip(skip)
                .Take(take)
                .List();
        }

        public User Disable(Guid userId)
        {
            var user = session.Get<User>(userId);

            if (user == null)
                throw new UserNotFoundException(string.Format("No user found with id {0}", userId));

            user.DisabledOn = DateTime.UtcNow;

            session.SaveOrUpdate(user);

            return user;
        }
    }
}
