namespace YorkshireTec.Api.Infrastructure.Helpers
{
    using System;
    using System.Collections.Generic;
    using System.Configuration;
    using System.Linq;
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

        public static List<MailChimpCampaign> GetPastCampaigns()
        {
            var mailChimp = new MailChimpManager(ApiKey);

            var result = mailChimp.GetCampaigns();

            DateTime sentTime;
            var archives = result.Data.Where(x => DateTime.TryParse(x.SendTime, out sentTime))
                .OrderByDescending(x => DateTime.Parse(x.SendTime))
                .Select(campaign => new MailChimpCampaign
                {
                    Title = campaign.Title,
                    Link = campaign.ArchiveUrl,
                    SentOn = DateTime.Parse(campaign.SendTime)
                })
                .ToList();

            return archives;
        }
    }

    public class MailChimpCampaign
    {
        public string Title { get; set; }
        public string Link { get; set; }
        public DateTime SentOn { get; set; }
    }
}