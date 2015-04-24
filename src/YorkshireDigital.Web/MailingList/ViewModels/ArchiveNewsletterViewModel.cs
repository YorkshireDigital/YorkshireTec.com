namespace YorkshireDigital.Web.MailingList.ViewModels
{
    using System.Collections.Generic;
    using YorkshireDigital.Web.Infrastructure.Helpers;

    public class ArchiveNewsletterViewModel
    {
        public List<MailChimpCampaign> Archives { get; set; }

        public ArchiveNewsletterViewModel()
        {
            Archives = new List<MailChimpCampaign>();
        }
    }
}