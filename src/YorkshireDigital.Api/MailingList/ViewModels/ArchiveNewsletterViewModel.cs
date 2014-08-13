namespace YorkshireDigital.Api.MailingList.ViewModels
{
    using System.Collections.Generic;
    using YorkshireDigital.Api.Infrastructure.Helpers;

    public class ArchiveNewsletterViewModel
    {
        public List<MailChimpCampaign> Archives { get; set; }

        public ArchiveNewsletterViewModel()
        {
            Archives = new List<MailChimpCampaign>();
        }
    }
}