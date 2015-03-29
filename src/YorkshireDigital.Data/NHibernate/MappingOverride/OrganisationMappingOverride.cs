namespace YorkshireDigital.Data.NHibernate.MappingOverride
{
    using FluentNHibernate.Automapping;
    using FluentNHibernate.Automapping.Alterations;
    using YorkshireDigital.Data.Domain.Organisations;

    public class OrganisationMappingOverride : IAutoMappingOverride<Organisation>
    {
        public void Override(AutoMapping<Organisation> mapping)
        {
            mapping.Table("Organisation");

            mapping.Map(x => x.Headline)
                .CustomSqlType("varchar(10000)")
                .Length(10000);

            mapping.Map(x => x.About)
                .CustomSqlType("varchar(10000)")
                .Length(10000);
        }
    }
}
