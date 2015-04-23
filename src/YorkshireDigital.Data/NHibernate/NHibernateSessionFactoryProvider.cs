using CallSessionContext = NHibernate.Context.CallSessionContext;
using Configuration = NHibernate.Cfg.Configuration;
using ISessionFactory = NHibernate.ISessionFactory;

namespace YorkshireDigital.Data.NHibernate
{
    using FluentNHibernate.Cfg;
    using FluentNHibernate.Cfg.Db;
    using FluentNHibernate.Conventions.Helpers;

    public class NHibernateSessionFactoryProvider
    {
        public static ISessionFactory BuildSessionFactory(string connectionString)
        {
            return GetConfiguration(connectionString)
                // .ExposeConfiguration(cfg => new SchemaExport(cfg).Execute(false, true, false))
                .BuildSessionFactory();
        }

        public static ISessionFactory BuildInMemorySessionFactory(out Configuration configuration)
        {
            var config = new Configuration();
            var sessionFactory = GetInMemoryConfiguration()
                .ExposeConfiguration(cfg =>
                {
                    // Remove comment to auto upgrade the database.
                    // new SchemaUpdate(cfg).Execute(false, true);
                    config = cfg;
                })
                .BuildSessionFactory();
            configuration = config;
            return sessionFactory;
        }

        public static FluentConfiguration GetConfiguration(string connectionString)
        {
            return Fluently.Configure()
                .Database(MsSqlConfiguration.MsSql2012.ConnectionString(connectionString))
                .Mappings(m => m.FluentMappings.AddFromAssemblyOf<NHibernateSessionFactoryProvider>()
                                .Conventions.Add(ForeignKey.EndsWith("Id"))
                                .Conventions.Add<CascadeConvention>())
                .CurrentSessionContext<CallSessionContext>();
        }

        public static FluentConfiguration GetInMemoryConfiguration()
        {
            return Fluently.Configure()
                .Database(SQLiteConfiguration.Standard.InMemory())
                .Mappings(m => m.FluentMappings.AddFromAssemblyOf<NHibernateSessionFactoryProvider>()
                                .Conventions.Add(ForeignKey.EndsWith("Id"))
                                .Conventions.Add<CascadeConvention>())
                .CurrentSessionContext<CallSessionContext>();
        }
    }
}
