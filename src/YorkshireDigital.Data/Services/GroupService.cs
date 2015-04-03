namespace YorkshireDigital.Data.Services
{
    using System;
    using System.Collections.Generic;
    using global::NHibernate;
    using YorkshireDigital.Data.Domain.Organisations;
    using YorkshireDigital.Data.Exceptions;

    public interface IGroupService
    {
        Group Get(string id);
        Group Save(Group group);
        Group Delete(string id);
        IList<Group> GetActiveGroups();
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

        public Group Save(Group @group)
        {
            session.SaveOrUpdate(@group);
            
            return @group;
        }

        public Group Delete(string id)
        {
            var group = session.Get<Group>(id);
            if (group == null)
                throw new GroupNotFoundException(string.Format("Unable to find group with id {0}", id));
            group.DeletedOn = DateTime.UtcNow;

            session.SaveOrUpdate(group);

            return group;
        } 

        public IList<Group> GetActiveGroups()
        {
            return session.QueryOver<Group>()
                .Where(x => x.DeletedOn == null)
                .List();
        }
    }
}
