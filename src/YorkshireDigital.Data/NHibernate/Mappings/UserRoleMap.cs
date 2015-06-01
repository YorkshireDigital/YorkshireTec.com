namespace YorkshireDigital.Data.NHibernate.Mappings
{
    using FluentNHibernate.Mapping;
    using YorkshireDigital.Data.Domain.Account;

    public class UserRoleMap : ClassMap<UserRole>
    {
        public UserRoleMap()
        {
            Id(x => x.Id).GeneratedBy.Identity();

            Map(x => x.Role);
            Map(x => x.Claims);
        }
    }
}
