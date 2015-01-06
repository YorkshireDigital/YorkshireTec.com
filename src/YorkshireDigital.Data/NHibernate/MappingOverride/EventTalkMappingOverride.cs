namespace YorkshireDigital.Data.NHibernate.MappingOverride
{
    using FluentNHibernate.Automapping;
    using FluentNHibernate.Automapping.Alterations;
    using YorkshireDigital.Data.Domain.Events;

    public class EventTalkMappingOverride : IAutoMappingOverride<EventTalk>
    {
        public void Override(AutoMapping<EventTalk> mapping)
        {
            mapping.Id(x => x.Id).UniqueKey("Id").GeneratedBy.Identity();
            mapping.Map(x => x.Synopsis)
                .CustomSqlType("varchar(10000)")
                .Length(10000);
        }
    }
}
