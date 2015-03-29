namespace YorkshireDigital.Web.Infrastructure
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
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

            if (userRecord == null) return null;

            var userIdentity = new UserIdentity
            {
                UserName = userRecord.Username,
                FriendlyName = userRecord.Name,
                UserId = userRecord.Id.ToString(),
                Email = userRecord.Email
            };

            var claims = new List<string>();

            if (userRecord.Roles.Any())
            {
                claims.AddRange(userRecord.Roles.SelectMany(x => x.Claims.Split('|')).Distinct());
            }

            userIdentity.Claims = claims;

            return userIdentity;
        }
    }
}