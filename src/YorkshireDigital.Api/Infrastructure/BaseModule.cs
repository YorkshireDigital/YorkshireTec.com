namespace YorkshireDigital.Api.Infrastructure
{
    using Nancy;
    using NHibernate;

    public class BaseModule : NancyModule
    {
        internal ISession RequestSession;

        public BaseModule()
        {
            Get["/Test"] = _ => "Success";
        }


        public BaseModule(ISessionFactory sessionFactory, string modulePath)
            : base(string.Format("/{0}", modulePath))
        {
            RequestSession = sessionFactory.GetCurrentSession();
        }
    }
}