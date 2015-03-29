namespace YorkshireDigital.Data.NHibernate.Mappings
{
    using FluentNHibernate.Mapping;
    using YorkshireDigital.Data.Domain.Organisations;

    public class OrganisationMap : ClassMap<Organisation>
    {
        public OrganisationMap()
        {
            Id(x => x.Id).GeneratedBy.Assigned();

            Map(x => x.Name);
            Map(x => x.ShortName);
            Map(x => x.Headline)
                .CustomSqlType("varchar(10000)")
                .Length(10000);
            Map(x => x.About)
                .CustomSqlType("varchar(10000)")
                .Length(10000);
            Map(x => x.Colour);
            Map(x => x.Logo);
            Map(x => x.Photo);
            Map(x => x.Website);

            HasMany(x => x.ContactLinks)
                .Cascade.All();
            HasMany(x => x.Events)
                .Cascade.All();
        }
    }
}
