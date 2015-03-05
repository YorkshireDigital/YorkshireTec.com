namespace YorkshireTec.Api.Infrastructure.Models
{
    using System.Configuration;

    public class FeaturesModel
    {
        public bool Calendar { get; set; }
        public bool Beta { get; set; }

        public FeaturesModel()
        {
            Calendar = IsFeatureEnabled("Calendar");
            Beta = IsFeatureEnabled("Beta");
        }

        private static bool IsFeatureEnabled(string feature)
        {
            bool enabled;
            bool.TryParse(ConfigurationManager.AppSettings[string.Format("Feature:{0}", feature)], out enabled);
            return enabled;
        }
    }
}