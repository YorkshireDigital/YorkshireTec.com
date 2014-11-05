namespace YorkshireDigital.Api.Infrastructure.Helpers
{
    using System.Collections.Generic;
    using System.Configuration;
    using System.Net.Http;
    using System.Text;
    using System.Threading.Tasks;
    using Nancy.Json;

    public class SlackHelper
    {
        private static readonly string ApiKey = ConfigurationManager.AppSettings["Slack_ApiKey"];
        private static readonly string Project = ConfigurationManager.AppSettings["Slack_Project"];
        private static readonly bool Enabled = bool.Parse(ConfigurationManager.AppSettings["Slack_Enabled"]);

        public static void PostToSlack(SlackUpdate slackUpdate)
        {
            var result = PostToSlackAsync(slackUpdate).Result;
        }

        public static async Task<bool> PostToSlackAsync(SlackUpdate slackUpdate)
        {
            if (!Enabled) return false;

            var client = new HttpClient();

            var jsonString = new JavaScriptSerializer().Serialize(slackUpdate);

            // Get the response.
            await client.PostAsync(string.Format("https://{0}.slack.com/services/hooks/incoming-webhook?token={1}", Project, ApiKey),
                new StringContent(jsonString, Encoding.UTF8, "application/json"));

            return true;
        }
    }

    public class SlackUpdate
    {
        public string channel { get; set; }
        public string username { get; set; }
        public string icon_emoji { get; set; }
        public string text { get; set; }
        public bool unfurl_links { get; set; }
        public SlackAttachment[] Attachments { get; set; }
    }

    public class SlackAttachment
    {
        public string Fallback { get; set; }
        public string Pretext { get; set; }
        public string Color { get; set; }
        public SlackAttachmentField[] Fields { get; set; }
    }

    public class SlackAttachmentField
    {
        public string Title { get; set; }
        public string Value { get; set; }
        public bool Short { get; set; }
    }
}