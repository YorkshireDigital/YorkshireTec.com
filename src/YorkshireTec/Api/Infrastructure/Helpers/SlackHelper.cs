namespace YorkshireTec.Api.Infrastructure.Helpers
{
    using System.Configuration;
    using System.Net.Http;
    using System.Text;
    using Nancy.Json;

    public class SlackHelper
    {
        private static readonly string ApiKey = ConfigurationManager.AppSettings["Slack_ApiKey"];
        private static readonly string Project = ConfigurationManager.AppSettings["Slack_Project"];
        private static readonly bool Enabled = bool.Parse(ConfigurationManager.AppSettings["Slack_Enabled"]);

        public static async void PostToSlack(SlackUpdate slackUpdate)
        {
            if (Enabled)
            {
                var client = new HttpClient();

                var jsonString = new JavaScriptSerializer().Serialize(slackUpdate);

                // Get the response.
                await client.PostAsync(
                    string.Format("https://{0}.slack.com/services/hooks/incoming-webhook?token={1}", Project, ApiKey),
                    new StringContent(jsonString, Encoding.UTF8, "application/json"));
            }
        }
    }

    public class SlackUpdate
    {
        public string channel { get; set; }
        public string username { get; set; }
        public string icon_emoji { get; set; }
        public string text { get; set; }
    }
}