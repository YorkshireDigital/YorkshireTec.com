namespace YorkshireDigital.Api.Infrastructure
{
    using Nancy;
    using NHibernate;

    public class BaseModule : NancyModule
    {
        internal ISession RequestSession;

        public BaseModule()
        {
            Get["/"] = _ => HttpStatusCode.ImATeapot;
            Post["/"] = _ => HttpStatusCode.ImATeapot;
            Put["/"] = _ => HttpStatusCode.ImATeapot;
            Delete["/"] = _ => HttpStatusCode.ImATeapot;
        }


        public BaseModule(ISessionFactory sessionFactory, string modulePath)
            : base(string.Format("/{0}", modulePath))
        {
            RequestSession = sessionFactory.GetCurrentSession();
        }
    }
}