using BinaryBlobType = NHibernate.Type.BinaryBlobType;

namespace YorkshireTec.Data.NHibernate.MappingOverride
{
    using FluentNHibernate.Automapping;
    using FluentNHibernate.Automapping.Alterations;
    using YorkshireTec.Data.Domain.Events;

    public class EventMappingOverride : IAutoMappingOverride<Event>
    {
        public void Override(AutoMapping<Event> mapping)
        {
            mapping.Id(x => x.UniqueName).UniqueKey("UniqueName");
            mapping.Map(x => x.Photo).CustomType<BinaryBlobType>();
            mapping.HasManyToMany(x => x.Categories);
        }
    }
}
