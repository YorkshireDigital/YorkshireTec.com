namespace YorkshireDigital.Api.MailingList.Modules
{
    using Nancy;
    using NHibernate;
    using YorkshireDigital.Api.Infrastructure;
    using YorkshireDigital.Api.Infrastructure.Helpers;
    using YorkshireDigital.Api.MailingList.ViewModels;

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