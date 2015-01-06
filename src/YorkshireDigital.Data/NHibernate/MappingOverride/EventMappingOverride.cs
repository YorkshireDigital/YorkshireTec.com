using BinaryBlobType = NHibernate.Type.BinaryBlobType;

namespace YorkshireDigital.Data.NHibernate.MappingOverride
{
    using FluentNHibernate.Automapping;
    using FluentNHibernate.Automapping.Alterations;
    using YorkshireDigital.Data.Domain.Events;

    public class EventMappingOverride : IAutoMappingOverride<Event>
    {
        public void Override(AutoMapping<Event> mapping)
        {
            mapping.Id(x => x.UniqueName)
                .UniqueKey("UniqueName");
            mapping.Map(x => x.Photo)
                .CustomType<BinaryBlobType>();
            mapping.Map(x => x.Title)
                .CustomSqlType("varchar(1000)")
                .Length(1000);
            mapping.Map(x => x.Synopsis)
                .CustomSqlType("varchar(10000)")
                .Length(10000);
            mapping.HasManyToMany(x => x.Categories);
        }
    }
}
