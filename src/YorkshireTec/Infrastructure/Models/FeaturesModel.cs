namespace YorkshireTec.Infrastructure.Models
{
    using System.Configuration;

    public class FeaturesModel
    {
        public bool Calendar { get; set; }

        public FeaturesModel()
        {
            Calendar = IsFeatureEnabled("Calendar");
        }

        private static bool IsFeatureEnabled(string feature)
        {
            bool enabled;
            bool.TryParse(ConfigurationManager.AppSettings[string.Format("Feature_{0}", feature)], out enabled);
            return enabled;
        }
    }
}