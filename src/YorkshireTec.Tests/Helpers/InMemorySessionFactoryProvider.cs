using Configuration = NHibernate.Cfg.Configuration;
using ISession = NHibernate.ISession;
using ISessionFactory = NHibernate.ISessionFactory;
using SchemaExport = NHibernate.Tool.hbm2ddl.SchemaExport;

namespace YorkshireDigital.Data.Tests.Helpers
{
    using NHibernate;

    public class InMemorySessionFactoryProvider
    {
        private static InMemorySessionFactoryProvider _instance;
        public static InMemorySessionFactoryProvider Instance
        {
            get { return _instance ?? (_instance = new InMemorySessionFactoryProvider()); }
        }

        private ISessionFactory sessionFactory;
        private Configuration configuration;

        private InMemorySessionFactoryProvider() { }

        public void Initialize()
        {
            sessionFactory = NHibernateSessionFactoryProvider.BuildInMemorySessionFactory(out configuration);
        }

        public ISession OpenSession()
        {
            var session = sessionFactory.OpenSession();

            var export = new SchemaExport(configuration);
            export.Execute(false, true, false, session.Connection, null);

            return session;
        }

        public void Dispose()
        {
            if (sessionFactory != null)
                sessionFactory.Dispose();

            sessionFactory = null;
        }
    }
}
