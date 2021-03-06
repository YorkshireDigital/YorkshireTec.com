namespace YorkshireTec.Api.Infrastructure
{
    using System.Configuration;
    using System.Linq;
    using Cassette.Nancy;
    using Nancy;
    using Nancy.Authentication.Forms;
    using Nancy.Bootstrapper;
    using Nancy.Cryptography;
    using Nancy.Diagnostics;
    using Nancy.TinyIoc;
    using NHibernate;
    using NHibernate.Context;
    using YorkshireDigital.Data.NHibernate;

    public class Bootstrapper : DefaultNancyBootstrapper
    {
        public Bootstrapper()
        {
            bool optimizeOutput;
            bool.TryParse(ConfigurationManager.AppSettings["Cassette_OptimizeOutput"], out optimizeOutput);
            CassetteNancyStartup.OptimizeOutput = optimizeOutput;
        }

        protected override DiagnosticsConfiguration DiagnosticsConfiguration
        {
            get { return new DiagnosticsConfiguration { Password = ConfigurationManager.AppSettings["Nancy_Diagnostics"] }; }
        }

        // The bootstrapper enables you to reconfigure the composition of the framework,
        // by overriding the various methods and properties.
        // For more information https://github.com/NancyFx/Nancy/wiki/Bootstrapper
        protected override void ConfigureApplicationContainer(TinyIoCContainer container)
        {
            base.ConfigureApplicationContainer(container);

            var inMemoryDb = bool.Parse(ConfigurationManager.AppSettings["Database:InMemory"]);

            if (inMemoryDb)
            {
                NHibernate.Cfg.Configuration configuration;
                container.Register(NHibernateSessionFactoryProvider.BuildInMemorySessionFactory(out configuration));
            }
            else
            {
                container.Register(
                    NHibernateSessionFactoryProvider.BuildSessionFactory(
                        ConfigurationManager.ConnectionStrings["Database"].ConnectionString));
            }
            Conventions.ViewLocationConventions.Add((viewName, model, context) => string.Concat("Api/", context.ModuleName, "/Views/", viewName));
            Conventions.ViewLocationConventions.Add((viewName, model, context) => string.Concat("Api/", context.ModulePath, "/Views/", viewName));
            Conventions.ViewLocationConventions.Add((viewName, model, context) => string.Concat("Api/", context.ModulePath.Split('/').First(), "/Views/", viewName));
        }

        protected override void ConfigureRequestContainer(TinyIoCContainer container, NancyContext context)
        {
            base.ConfigureRequestContainer(container, context);
            
            CreateSession(container);

            container.Register<IUserMapper, UserMapper>();
        }

        protected override void RequestStartup(TinyIoCContainer container, IPipelines pipelines, NancyContext context)
        {
            base.RequestStartup(container, pipelines, context);

            StaticConfiguration.DisableErrorTraces = false;

            var cryptographyConfiguration =
                new CryptographyConfiguration(
                    new RijndaelEncryptionProvider(new PassphraseKeyGenerator("SuperSecretPass",
                        new byte[]
                        {
                            100, 111, 110, 116, 32, 109, 97, 107, 101, 32, 109, 101, 32, 108, 111, 103, 32, 105, 110, 32,
                            101, 97, 99, 104, 32, 116, 105, 109, 101, 32, 105, 32, 98, 117, 105, 108, 100, 33
                        })),
                    new DefaultHmacProvider(new PassphraseKeyGenerator("UberSecretPass",
                        new byte[]
                        {
                            90, 71, 57, 117, 100, 67, 66, 116, 89, 87, 116, 108, 73, 71, 49, 108, 73, 71, 120, 118, 90, 121,
                            66, 112, 98, 105, 66, 108, 89, 87, 78, 111, 73, 72, 82, 112, 98, 87, 85, 103, 97, 83, 66,
                            105, 100, 87, 108, 115, 90, 67, 69, 61
                        })));

            var formsAuthConfiguration = new FormsAuthenticationConfiguration
            {
                CryptographyConfiguration = cryptographyConfiguration,
                RedirectUrl = "~/account/log-in",
                UserMapper = container.Resolve<IUserMapper>(),
            };
            FormsAuthentication.Enable(pipelines, formsAuthConfiguration);
        }

        protected override void ApplicationStartup(TinyIoCContainer container, IPipelines pipelines)
        {
            base.ApplicationStartup(container, pipelines);

            ConfigureNHibernateSessionPerRequest(container, pipelines);

            // TODO: Handle Errors
            // pipelines.OnError += InvalidOrderOperationHandler;
        }

        #region NHibernate Session Setup

        private static void ConfigureNHibernateSessionPerRequest(TinyIoCContainer container, IPipelines pipelines)
        {
            pipelines.BeforeRequest += ctx => CreateSession(container);
            pipelines.AfterRequest += ctx => CommitSession(container);
            pipelines.OnError += (ctx, ex) => RollbackSession(container);
        }

        private static Response RollbackSession(TinyIoCContainer container)
        {
            var sessionFactory = container.Resolve<ISessionFactory>();
            if (!CurrentSessionContext.HasBind(sessionFactory)) return null;
            var requestSession = sessionFactory.GetCurrentSession();
            requestSession.Transaction.Rollback();
            CurrentSessionContext.Unbind(sessionFactory);
            requestSession.Dispose();
            return null;
        }

        private static Response CreateSession(TinyIoCContainer container)
        {
            var sessionFactory = container.Resolve<ISessionFactory>();
            var requestSession = sessionFactory.OpenSession();
            CurrentSessionContext.Bind(requestSession);
            requestSession.BeginTransaction();

            return null;
        }

        private static AfterPipeline CommitSession(TinyIoCContainer container)
        {
            var sessionFactory = container.Resolve<ISessionFactory>();
            if (!CurrentSessionContext.HasBind(sessionFactory)) return null;

            var requestSession = sessionFactory.GetCurrentSession();
            requestSession.Transaction.Commit();
            CurrentSessionContext.Unbind(sessionFactory);
            requestSession.Dispose();
            return null;
        }
        #endregion
    }
}