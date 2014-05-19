namespace YorkshireTec.Archive.Modules
{
    using Nancy;
    using NHibernate;
    using YorkshireTec.Archive.ViewModels;
    using YorkshireTec.Infrastructure;
    using YorkshireTec.Infrastructure.Helpers;

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