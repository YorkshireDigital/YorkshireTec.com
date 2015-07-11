namespace YorkshireDigital.Web.Infrastructure.Helpers
{
    using System;
    using System.Configuration;
    using SharpRaven;
    using YorkshireDigital.Data.Helpers;

    public static class SentryHelper
    {
        private static readonly bool Enabled = FeaturesModel.Sentry;
        private static readonly string Dsn = ConfigurationManager.AppSettings["Sentry_DSN"];

        public static void LogException(Exception exception)
        {
            if (!Enabled) return;

            var ravenClient = new RavenClient(Dsn);

            ravenClient.CaptureException(exception);
        }

        public static void LogMessage(string message)
        {
            if (!Enabled) return;

            var ravenClient = new RavenClient(Dsn);

            ravenClient.CaptureMessage(message);
        }

    }
}