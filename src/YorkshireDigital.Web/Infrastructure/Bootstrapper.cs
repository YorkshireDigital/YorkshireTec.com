namespace YorkshireDigital.Web.Infrastructure
{
    using System.Configuration;
    using Hangfire;
    using Humanizer;
    using Nancy;
    using Nancy.Authentication.Forms;
    using Nancy.Bootstrapper;
    using Nancy.Conventions;
    using Nancy.Cryptography;
    using Nancy.Diagnostics;
    using Nancy.Security;
    using Nancy.TinyIoc;
    using NHibernate;
    using NHibernate.Context;
    using YorkshireDigital.Data.NHibernate;
    using YorkshireDigital.Data.Services;
    using YorkshireDigital.Web.Infrastructure.Models;

    public class Bootstrapper : DefaultNancyBootstrapper
    {
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

            ConfigureViewLocations();

            Conventions.StaticContentsConventions.Add(StaticContentConventionBuilder.AddDirectory("public"));
        }

        protected override void ApplicationStartup(TinyIoCContainer container, IPipelines pipelines)
        {
            base.ApplicationStartup(container, pipelines);

            GlobalConfiguration.Configuration.UseActivator(new HangfireContainerJobActivator(container));

            Csrf.Enable(pipelines);
        }

        protected override void ConfigureRequestContainer(TinyIoCContainer container, NancyContext context)
        {
            base.ConfigureRequestContainer(container, context);

            var sessionFactory = NHibernateSessionFactoryProvider.BuildSessionFactory(ConfigurationManager.ConnectionStrings["Database"].ConnectionString);

            container.Register(sessionFactory);

            CreateSession(container, sessionFactory);

            container.Register<IUserMapper, UserMapper>();
        }

        protected override void RequestStartup(TinyIoCContainer container, IPipelines pipelines, NancyContext context)
        {
            base.RequestStartup(container, pipelines, context);

            context.ViewBag.Beta = FeaturesModel.Beta;
            context.ViewBag.GoogleAnalytics = FeaturesModel.GoogleAnalytics;

            StaticConfiguration.DisableErrorTraces = false;

            #region Cryto

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
                RedirectUrl = "~/account/sign-in",
                UserMapper = container.Resolve<IUserMapper>(),
            };
            FormsAuthentication.Enable(pipelines, formsAuthConfiguration);

            #endregion

            ConfigureNHibernateSessionPerRequest(container, pipelines);
            pipelines.OnError += (ctx, exception) =>
            {
                ctx.Items.Add("OnErrorException", exception);
                return null;
            };
        }

        #region NHibernate Session Setup

        private void ConfigureNHibernateSessionPerRequest(TinyIoCContainer container, IPipelines pipelines)
        {
            pipelines.AfterRequest += ctx => CommitSession(container);
            pipelines.OnError += (ctx, ex) => RollbackSession(container);
        }

        private static void CreateSession(TinyIoCContainer container, ISessionFactory sessionFactory)
        {
            var requestSession = sessionFactory.OpenSession();
            CurrentSessionContext.Bind(requestSession);
            requestSession.BeginTransaction();

            container.Register<IEventService>(new EventService(requestSession));
            container.Register<IUserService>(new UserService(requestSession));
            container.Register<IGroupService>(new GroupService(requestSession));
        }

        private Response RollbackSession(TinyIoCContainer container)
        {
            var sessionFactory = container.Resolve<ISessionFactory>();
            if (!CurrentSessionContext.HasBind(sessionFactory)) return null;
            var requestSession = sessionFactory.GetCurrentSession();
            requestSession.Transaction.Rollback();
            CurrentSessionContext.Unbind(sessionFactory);
            requestSession.Dispose();
            return null;
        }

        private static AfterPipeline CommitSession(TinyIoCContainer container)
        {
            var sessionFactory = container.Resolve<ISessionFactory>();
            if (!CurrentSessionContext.HasBind(sessionFactory)) return null;

            var requestSession = sessionFactory.GetCurrentSession();
            if (requestSession.Transaction.IsActive)
            {
                requestSession.Transaction.Commit();
            }
            CurrentSessionContext.Unbind(sessionFactory);
            requestSession.Dispose();
            return null;
        }

        #endregion

        private void ConfigureViewLocations()
        {
            Conventions.ViewLocationConventions.Add((viewName, model, context) => string.Concat(context.ModuleName, "/Views/", viewName));
            Conventions.ViewLocationConventions.Add((viewName, model, context) => string.Concat(context.ModuleName.Pluralize(false), "/Views/", viewName));
            Conventions.ViewLocationConventions.Add((viewName, model, context) => string.Concat(context.ModulePath.TrimStart('/'), "/Views/", viewName));
            Conventions.ViewLocationConventions.Add((viewName, model, context) => string.Concat(context.ModulePath.TrimStart('/').Split('/')[0], "/Views/", viewName));
        }
    }
}