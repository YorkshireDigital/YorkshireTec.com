namespace YorkshireDigital.Web.MailingList.Modules
{
    using Nancy;
    using NHibernate;
    using YorkshireDigital.Web.Infrastructure;
    using YorkshireDigital.Web.Infrastructure.Helpers;
    using YorkshireDigital.Web.MailingList.ViewModels;

    public class MailingListModule : BaseModule
    {
        public MailingListModule(ISessionFactory sessionFactory)
            : base(sessionFactory, "mailinglist")
        {
            Get["/archive"] = _ =>
            {
                var viewModel = new ArchiveNewsletterViewModel
                {
                    Archives = MailChimpHelper.GetPastCampaigns()
                };

                return Negotiate.WithModel(viewModel)
                    .WithView("Archive");
            };

            Get["/confirmation"] = _ => View["Confirmation"];

            Get["/subscribed"] = _ => View["Confirmation"];
        }
    }
}