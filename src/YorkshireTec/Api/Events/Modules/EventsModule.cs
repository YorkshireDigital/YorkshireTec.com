namespace YorkshireTec.Api.Events.Modules
{
    using NHibernate;
    using YorkshireTec.Api.Infrastructure;

    public class EventsModule : BaseModule
    {
        public EventsModule(ISessionFactory sessionFactory)
            : base(sessionFactory, "events")
        {
            this.RequiresFeature("Calendar");

            Get["/"] = _ => 200;
        }
    }
}