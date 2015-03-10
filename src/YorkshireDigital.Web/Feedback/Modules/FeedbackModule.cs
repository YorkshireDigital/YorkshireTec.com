namespace YorkshireDigital.Web.Feedback.Modules
{
    using System.Collections.Generic;
    using Nancy;
    using Nancy.ModelBinding;
    using Slack.Webhooks;
    using YorkshireDigital.Web.Feedback.Models;
    using YorkshireDigital.Web.Infrastructure.Helpers;

    public class FeedbackModule : NancyModule
    {
        public FeedbackModule() : base("feedback/")
        {
            Post["/raise"] = _ =>
            {
                var model = this.Bind<FeedbackPostModel>();

                SlackHelper.PostFeedbackUpdate(model.Details, model.SlackUpdate);

                return true;
            };
        }
    }
}