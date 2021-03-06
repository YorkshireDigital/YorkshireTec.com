﻿namespace YorkshireDigital.Web.Infrastructure.Helpers
{
    using System.Collections.Generic;
    using System.Configuration;
    using Slack.Webhooks;
    using YorkshireDigital.Web.Infrastructure.Models;

    public class SlackHelper
    {
        private static readonly bool Enabled = FeaturesModel.Slack;
        private static readonly string WebHookUrl = ConfigurationManager.AppSettings["Slack_Webhook_Url"];

        public static void PostToSlack(SlackMessage slackUpdate)
        {
            if (Enabled)
            {
                var slackClient = new SlackClient(WebHookUrl);

                slackClient.Post(slackUpdate);
            }
        }

        public static void PostNewUserUpdate(string username, string name, string email, bool mailingList, string webaddress)
        {
            var updateText = string.Format("{0} just signed up at {1}. Go {0}!", name, webaddress);

            var slackMessage = new SlackMessage
            {
                Channel = "#notifications",
                IconEmoji = ":yorks:",
                Username = "New User",
                Text = updateText
            };

            var slackAttachment = new SlackAttachment
            {
                Fallback = string.Format("New user <{0}/users/{1}|{2} user profile> created", webaddress, username, name),
                Text = string.Format("New user <{0}/users/{1}|{2} user profile> created", webaddress, username, name),
                Color = "#b9306a",
                Fields = new List<SlackField>
                {
                    new SlackField { Title = "Name", Value = name },
                    new SlackField { Title = "Email", Value = email },
                    new SlackField { Title = "Subscribed to mailing list?", Value = mailingList.ToString() },
                }
            };
            slackMessage.Attachments = new List<SlackAttachment> { slackAttachment };

            PostToSlack(slackMessage);
        }

        public static void PostFeedbackUpdate(string details, string slackUpdate)
        {
            var slackMessage = new SlackMessage
            {
                Channel = "#feedback",
                Username = "Bug Report",
                IconEmoji = ":bug:"
            };

            var slackAttachment = new SlackAttachment
            {
                Fallback = details,
                PreText = slackUpdate,
                Color = "#D00000",
                Fields = new List<SlackField>
                        {
                            new SlackField
                            {
                                Title = "Details",
                                Value = details,
                                Short = false
                            }

                        }
            };

            slackMessage.Attachments = new List<SlackAttachment> { slackAttachment };

            PostToSlack(slackMessage);
        }
    }
}