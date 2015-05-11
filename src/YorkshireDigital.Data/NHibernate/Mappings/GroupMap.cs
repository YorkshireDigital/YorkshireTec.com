namespace YorkshireDigital.Data.NHibernate.Mappings
{
    using FluentNHibernate.Mapping;
    using YorkshireDigital.Data.Domain.Group;

    public class GroupMap : ClassMap<Group>
    {
        public GroupMap()
        {
            Table("[Group]");

            Id(x => x.Id).GeneratedBy.Assigned();

            Map(x => x.Name);
            Map(x => x.ShortName);
            Map(x => x.Headline)
                .CustomSqlType("varchar(8000)")
                .Length(8000);
            Map(x => x.About)
                .CustomSqlType("varchar(8000)")
                .Length(8000);
            Map(x => x.Colour);
            Map(x => x.Logo);
            Map(x => x.Photo);
            Map(x => x.Website);
            Map(x => x.LastEditedOn);
            References(x => x.LastEditedBy);
            Map(x => x.DeletedOn);
            References(x => x.DeletedBy);

            HasMany(x => x.ContactLinks)
                .Cascade.All();
            HasMany(x => x.Events)
                .Cascade.All();

        }
    }
}
