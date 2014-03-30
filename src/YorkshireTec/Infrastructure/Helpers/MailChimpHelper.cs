namespace YorkshireTec.Infrastructure.Helpers
{
    using System.Configuration;
    using MailChimp;
    using MailChimp.Helper;

    public class MailChimpHelper
    {
        private static readonly string ApiKey = ConfigurationManager.AppSettings["MailChimp_ApiKey"];

        public static void AddSubscriber(string email, string name, string twitter, string company)
        {
            var mailChimp = new MailChimpManager(ApiKey);
            var emailParam = new EmailParameter
            {
                Email = email
            };
            var mergeVars = new { NAME = name, COMPANY = company, TWITTER = twitter };
            var results = mailChimp.Subscribe(ConfigurationManager.AppSettings["MailChimp_ListId"], emailParam, mergeVars);
        }

        public static void Unsubscribe(string email, string name, string twitter, string empty)
        {
            var mailChimp = new MailChimpManager(ApiKey);
            var emailParam = new EmailParameter
            {
                Email = email
            };
            var results = mailChimp.Unsubscribe(ConfigurationManager.AppSettings["MailChimp_ListId"], emailParam);
        }
    }
}