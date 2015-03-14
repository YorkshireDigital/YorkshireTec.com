namespace YorkshireDigital.Web.Feedback.Modules
{
    using Nancy.ModelBinding;
    using NHibernate;
    using YorkshireDigital.Web.Feedback.Models;
    using YorkshireDigital.Web.Infrastructure;
    using YorkshireDigital.Web.Infrastructure.Helpers;

    public class FeedbackModule : BaseModule
    {
        public FeedbackModule(ISessionFactory sessionFactory)
            : base(sessionFactory, "feedback/")
        {
            Post["/raise"] = _ =>
            {
                var model = this.Bind<FeedbackAjaxPostModel>();

                AjaxValidateCsrfToken(model);

                SlackHelper.PostFeedbackUpdate(model.Details, model.SlackUpdate);

                return true;
            };
        }
    }
}