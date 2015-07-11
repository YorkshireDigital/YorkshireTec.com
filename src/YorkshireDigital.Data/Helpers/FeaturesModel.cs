namespace YorkshireDigital.Data.Helpers
{
    using System.Configuration;

    public static class FeaturesModel
    {
        public static bool Calendar { get { return IsFeatureEnabled("Calendar"); } }
        public static bool Account { get { return IsFeatureEnabled("Account"); } }
        public static bool Beta { get { return IsFeatureEnabled("Beta"); } }
        public static bool GoogleAnalytics { get { return IsFeatureEnabled("GA"); } }
        public static bool MailChimp { get { return IsFeatureEnabled("MailChimp"); } }
        public static bool Slack { get { return IsFeatureEnabled("Slack"); } }
        public static bool Sentry { get { return IsFeatureEnabled("Sentry"); } }

        private static bool IsFeatureEnabled(string feature)
        {
            bool enabled;
            bool.TryParse(ConfigurationManager.AppSettings[string.Format("Feature:{0}", feature)], out enabled);
            return enabled;
        }
    }
}