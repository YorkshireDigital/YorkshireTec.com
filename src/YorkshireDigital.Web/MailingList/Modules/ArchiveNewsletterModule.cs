namespace YorkshireDigital.Web.MailingList.Modules
{
    using Nancy;
    using NHibernate;
    using YorkshireDigital.Web.Infrastructure;
    using YorkshireDigital.Web.Infrastructure.Helpers;
    using YorkshireDigital.Web.MailingList.ViewModels;

    public class ArchiveNewsletterModule : BaseModule
    {
        public ArchiveNewsletterModule(ISessionFactory sessionFactory)
            : base(sessionFactory, "mailinglist/archive")
        {
            Get["/"] = _ =>
            {
                var viewModel = new ArchiveNewsletterViewModel
                {
                    Archives = MailChimpHelper.GetPastCampaigns()
                };

                return Negotiate.WithModel(viewModel);
            };
        }
    }
}