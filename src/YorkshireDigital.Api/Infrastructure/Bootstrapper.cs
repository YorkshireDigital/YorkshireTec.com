namespace YorkshireDigital.Api.Infrastructure
{
    using System.Configuration;
    using System.Linq;
    using Nancy;
    using Nancy.Bootstrapper;
    using Nancy.Diagnostics;
    using Nancy.TinyIoc;
    using NHibernate;
    using NHibernate.Context;
    using YorkshireTec.Data.NHibernate;

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

            container.Register(
                NHibernateSessionFactoryProvider.BuildSessionFactory(
                    ConfigurationManager.ConnectionStrings["Database"].ConnectionString));
        }

        protected override void ConfigureRequestContainer(TinyIoCContainer container, NancyContext context)
        {
            base.ConfigureRequestContainer(container, context);
            
            CreateSession(container);
        }

        protected override void RequestStartup(TinyIoCContainer container, IPipelines pipelines, NancyContext context)
        {
            base.RequestStartup(container, pipelines, context);

            StaticConfiguration.DisableErrorTraces = false;
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