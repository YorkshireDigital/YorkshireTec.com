namespace YorkshireDigital.Data.NHibernate.Mappings
{
    using FluentNHibernate.Mapping;
    using YorkshireDigital.Data.Domain.Events;

    public class CategoryMap : ClassMap<Category>
    {
        public CategoryMap()
        {
            Id(x => x.Id);

            Map(x => x.Name);

            HasManyToMany(x => x.Events);
        }
    }
}
