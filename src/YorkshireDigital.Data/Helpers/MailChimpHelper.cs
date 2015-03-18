namespace YorkshireDigital.Data.Helpers
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
        private static readonly string ListId = ConfigurationManager.AppSettings["MailChimp_ListId"];

        public static void AddSubscriber(string email, string name, string twitter, string company)
        {
            var mailChimp = new MailChimpManager(ApiKey);
            var emailParam = new EmailParameter
            {
                Email = email
            };
            var mergeVars = new { NAME = name, COMPANY = company, TWITTER = twitter };
            var results = mailChimp.Subscribe(ListId, emailParam, mergeVars);
        }

        public static void Unsubscribe(string email, string name, string twitter, string empty)
        {
            var mailChimp = new MailChimpManager(ApiKey);
            var emailParam = new EmailParameter
            {
                Email = email
            };
            var results = mailChimp.Unsubscribe(ListId, emailParam);
        }

        public static bool IsEmailRegistered(string email)
        {
            var mailChimp = new MailChimpManager(ApiKey);
            var emailParam = new EmailParameter
            {
                Email = email
            };
            var results = mailChimp.GetMemberInfo(ListId, new List<EmailParameter> {emailParam});

            return results.Data.Count > 0;
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