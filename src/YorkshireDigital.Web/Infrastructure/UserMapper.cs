namespace YorkshireDigital.Web.Infrastructure
{
    using System;
    using Nancy;
    using Nancy.Authentication.Forms;
    using Nancy.Security;
    using NHibernate;
    using YorkshireDigital.Data.Domain.Account;

    public class UserMapper : IUserMapper
    {
        private readonly ISession session;

        public UserMapper(ISessionFactory sessionFactory)
        {
            session = sessionFactory.GetCurrentSession();
        }

        public IUserIdentity GetUserFromIdentifier(Guid identifier, NancyContext context)
        {
            var userRecord = session.Get<User>(identifier);

            return userRecord == null ? null : new UserIdentity { UserName = userRecord.Username, FriendlyName = userRecord.Name, UserId = userRecord.Id.ToString(), Email = userRecord.Email};
        }
    }
}