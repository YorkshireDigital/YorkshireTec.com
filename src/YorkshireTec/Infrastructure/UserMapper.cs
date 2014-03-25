using System;

namespace YorkshireTec.Infrastructure
{
    using global::Raven.Client;
    using Nancy;
    using Nancy.Authentication.Forms;
    using Nancy.Security;
    using YorkshireTec.Raven.Domain.Account;

    public class UserMapper : IUserMapper
    {
        private readonly IDocumentSession documentSession;

        public UserMapper(IDocumentSession documentSession)
        {
            this.documentSession = documentSession;

        }

        public IUserIdentity GetUserFromIdentifier(Guid identifier, NancyContext context)
        {
            var userRecord = documentSession.Load<User>(identifier);

            return userRecord == null ? null : new UserIdentity { UserName = userRecord.Username, FriendlyName = userRecord.Name, UserId = userRecord.Id.ToString() };
        }
    }
}