namespace YorkshireDigital.Data.NHibernate.Mappings
{
    using FluentNHibernate.Mapping;
    using global::NHibernate.Type;
    using YorkshireDigital.Data.Domain.Events;

    public class EventMap : ClassMap<Event>
    {
        public EventMap()
        {
            Table("[Event]");

            Id(x => x.UniqueName).GeneratedBy.Assigned();

            Map(x => x.Title)
                .CustomSqlType("varchar(1000)")
                .Length(1000);
            Map(x => x.Synopsis)
                .CustomSqlType("varchar(8000)")
                .Length(8000);
            Map(x => x.Start);
            Map(x => x.End).Column("[End]");
            Map(x => x.Location);
            Map(x => x.Region);
            Map(x => x.Price);
            Map(x => x.Photo).CustomType<BinaryBlobType>();
            Map(x => x.LastEditedOn);
            References(x => x.LastEditedBy);
            Map(x => x.MeetupId);
            Map(x => x.DeletedOn);
            References(x => x.DeletedBy);

            References(x => x.Group);

            HasManyToMany(x => x.Categories);
            HasManyToMany(x => x.Interests);

            HasMany(x => x.Talks)
                .Cascade.All();
        }
    }
}
