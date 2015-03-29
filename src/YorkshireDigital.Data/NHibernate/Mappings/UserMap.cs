namespace YorkshireDigital.Data.NHibernate.Mappings
{
    using FluentNHibernate.Mapping;
    using YorkshireDigital.Data.Domain.Account;

    public class UserMap : ClassMap<User>
    {
        public UserMap()
        {
            Id(x => x.Id).GeneratedBy.Guid();

            Map(x => x.Username);
            Map(x => x.Password);
            Map(x => x.Name);
            Map(x => x.Email);
            Map(x => x.MailingListEmail);
            Map(x => x.Gender);
            Map(x => x.Locale);
            Map(x => x.Picture);
            Map(x => x.Validated);
            Map(x => x.MailingListState);

            HasMany(x => x.Providers)
                .Cascade.All();

            HasManyToMany(x => x.Roles);
        }
    }
}
