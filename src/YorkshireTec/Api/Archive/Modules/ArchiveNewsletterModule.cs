namespace YorkshireTec.Api.Archive.Modules
{
    using Nancy;
    using NHibernate;
    using YorkshireTec.Api.Archive.ViewModels;
    using YorkshireTec.Api.Infrastructure;
    using YorkshireTec.Api.Infrastructure.Helpers;

    public class ArchiveNewsletterModule : BaseModule
    {
        public ArchiveNewsletterModule(ISessionFactory sessionFactory)
            : base(sessionFactory, "archive/newsletter")
        {
            Get["/"] = _ =>
            {
                var viewModel = new ArchiveNewsletterViewModel
                {
                    Archives = MailChimpHelper.GetPastCampaigns()
                };

                var model = GetBaseModel(viewModel);

                model.Page.Title = "Archives";
                return Negotiate.WithModel(model).WithView("Newsletter");
            };
        }
    }
}