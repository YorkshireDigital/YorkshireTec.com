namespace YorkshireDigital.Data.NHibernate.Mappings
{
    using FluentNHibernate.Mapping;
    using YorkshireDigital.Data.Domain.Events;

    public class InterestMap : ClassMap<Interest>
    {
        public InterestMap()
        {
            Id(x => x.Id).GeneratedBy.Identity();

            Map(x => x.Name);

            HasManyToMany(x => x.Events);
        }
    }
}
