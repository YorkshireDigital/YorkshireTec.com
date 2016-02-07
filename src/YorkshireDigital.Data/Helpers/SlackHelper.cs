namespace YorkshireDigital.Data.Helpers
{
    using System;
    using System.Collections.Generic;
    using System.Configuration;
    using Slack.Webhooks;

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
                Fallback = $"New user <{webaddress}/users/{username}|{name} user profile> created",
                Text = $"New user <{webaddress}/users/{username}|{name} user profile> created",
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

        public static void PostNewEventUpdate(string webaddress, string eventId, string eventTitle, string createdBy, DateTime eventStart, string eventLocation, string eventGroup, string groupColour)
        {
            var updateText = $"{createdBy} just created the event {eventTitle} at {webaddress}!";

            var slackMessage = new SlackMessage
            {
                Channel = "#notifications",
                IconEmoji = ":tada:",
                Username = "New Event Created",
                Text = updateText
            };

            var slackAttachment = new SlackAttachment
            {
                Fallback = $"New event <{webaddress}/event/{eventId}|{eventTitle}> created by {createdBy}",
                Text = $"New event <{webaddress}/event/{eventId}|{eventTitle}> created by {createdBy}",
                Color = string.IsNullOrEmpty(groupColour) ? "#b9306a" : groupColour,
                Fields = new List<SlackField>
                {
                    new SlackField { Title = "Title", Value = eventTitle },
                    new SlackField { Title = "Start", Value = eventStart.ToLocalTime().ToString("dd/MM/yyyy HH:mm") }
                }
            };
            if (!string.IsNullOrEmpty(eventGroup))
            {
                slackAttachment.Fields.Add(new SlackField { Title = "Group", Value = eventGroup });
            }
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
                Text = slackUpdate,
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