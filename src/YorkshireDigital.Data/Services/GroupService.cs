namespace YorkshireDigital.Data.Services
{
    using System;
    using System.Collections.Generic;
    using global::NHibernate;
    using YorkshireDigital.Data.Domain.Account;
    using YorkshireDigital.Data.Domain.Group;
    using YorkshireDigital.Data.Exceptions;

    public interface IGroupService
    {
        Group Get(string id);
        Group Save(Group group, User lastEditedBy);
        Group Delete(string id, User deletedBy);
        IList<Group> GetActiveGroups(int take = 20, int skip = 0);
    }

    public class GroupService : IGroupService
    {
        private readonly ISession session;

        public GroupService(ISession session)
        {
            this.session = session;
        }

        public Group Get(string id)
        {
            return session.Get<Group>(id);
        }

        public Group Save(Group @group, User lastEditedBy)
        {
            @group.LastEditedOn = DateTime.UtcNow;
            @group.LastEditedBy = lastEditedBy;

            session.SaveOrUpdate(@group);
            
            return @group;
        }

        public Group Delete(string id, User deletedBy)
        {
            var group = session.Get<Group>(id);
            if (group == null)
                throw new GroupNotFoundException(string.Format("Unable to find group with id {0}", id));
            group.DeletedOn = DateTime.UtcNow;
            group.DeletedBy = deletedBy;

            session.SaveOrUpdate(group);

            return group;
        } 

        public IList<Group> GetActiveGroups(int take = 20, int skip = 0)
        {
            return session.QueryOver<Group>()
                .Where(x => x.DeletedOn == null)
                .Skip(skip)
                .Take(take)
                .List();
        }
    }
}
