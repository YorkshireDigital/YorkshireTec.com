namespace YorkshireDigital.Data.NHibernate.Mappings
{
    using FluentNHibernate.Mapping;
    using YorkshireDigital.Data.Domain.Events;

    public class EventTalkMap : ClassMap<EventTalk>
    {
        public EventTalkMap()
        {
            Id(x => x.Id).GeneratedBy.Identity();

            Map(x => x.Title);
            Map(x => x.Synopsis)
                .CustomSqlType("varchar(10000)")
                .Length(10000);
            Map(x => x.Speaker);
            Map(x => x.Link);

            References(x => x.Event);
        }
    }
}
