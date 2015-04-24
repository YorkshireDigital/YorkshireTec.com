namespace YorkshireDigital.Web.Feedback.Modules
{
    using Nancy;
    using Nancy.ModelBinding;
    using YorkshireDigital.Web.Feedback.Models;
    using YorkshireDigital.Web.Infrastructure.Helpers;

    public class FeedbackModule : NancyModule
    {
        public FeedbackModule() : base("feedback/")
        {
            Post["/raise"] = _ =>
            {
                var model = this.Bind<FeedbackPostModel>();

                var update = new SlackUpdate
                {
                    channel = "#feedback",
                    username = "Bug Report",
                    icon_emoji = ":bug:",
                    unfurl_links = true,
                    Attachments = new [] {  new SlackAttachment
                    {
                        Fallback = model.Details,
                        Pretext = model.SlackUpdate,
                        Color = "#D00000",
                        Fields = new[]
                        {
                            new SlackAttachmentField
                            {
                                Title = "Details",
                                Value = model.Details,
                                Short = false
                            }
                            
                        }
                    } }
                };

                SlackHelper.PostToSlackAsync(update);

                return true;
            };
        }
    }
}