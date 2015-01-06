using CallSessionContext = NHibernate.Context.CallSessionContext;
using Configuration = NHibernate.Cfg.Configuration;
using ISessionFactory = NHibernate.ISessionFactory;
using SchemaUpdate = NHibernate.Tool.hbm2ddl.SchemaUpdate;

namespace YorkshireDigital.Data.NHibernate
{
    using System;
    using FluentNHibernate.Automapping;
    using FluentNHibernate.Cfg;
    using FluentNHibernate.Cfg.Db;
    using FluentNHibernate.Conventions.Helpers;
    using YorkshireDigital.Data.Domain.Account;
    using YorkshireDigital.Data.Domain.Events;

    public class NHibernateSessionFactoryProvider
    {
        public static ISessionFactory BuildSessionFactory(string connectionString)
        {
            return GetConfiguration(connectionString)
                .ExposeConfiguration(cfg => new SchemaUpdate(cfg).Execute(false, true))
                .BuildSessionFactory();
        }

        public static ISessionFactory BuildInMemorySessionFactory(out Configuration configuration)
        {
            var config = new Configuration();
            var sessionFactory = GetInMemoryConfiguration()
                .ExposeConfiguration(cfg =>
                {
                    new SchemaUpdate(cfg).Execute(false, true);
                    config = cfg;
                })
                .BuildSessionFactory();
            configuration = config;
            return sessionFactory;
        }

        public static FluentConfiguration GetConfiguration(string connectionString)
        {
            return Fluently.Configure()
                .Database(MySQLConfiguration.Standard
                    .ConnectionString(connectionString))
                .Mappings(m => m.AutoMappings.Add(CreateAutomappings))
                .CurrentSessionContext<CallSessionContext>();
        }

        public static FluentConfiguration GetInMemoryConfiguration()
        {
            return Fluently.Configure()
                .Database(SQLiteConfiguration.Standard.InMemory())
                .Mappings(m => m.AutoMappings.Add(CreateAutomappings))
                .CurrentSessionContext<CallSessionContext>();
        }

        private static AutoPersistenceModel CreateAutomappings()
        {
            return AutoMap.AssemblyOf<NHibernateSessionFactoryProvider>(new AutomappingConfiguration())
                // Automapping overrides
                .UseOverridesFromAssemblyOf<NHibernateSessionFactoryProvider>()
                .Override<User>(map => map.IgnoreProperty(x => x.Twitter))
                .Override<Provider>(map => map.IgnoreProperty(x => x.Expired))
                .Conventions.Add<CascadeConvention>()
                .Conventions.Add(ForeignKey.EndsWith("Id"));
        }
    }
}
