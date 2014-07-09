namespace YorkshireTec.Api.Archive.ViewModels
{
    using System.Collections.Generic;
    using YorkshireTec.Api.Infrastructure.Helpers;

    public class ArchiveNewsletterViewModel
    {
        public List<MailChimpCampaign> Archives { get; set; }

        public ArchiveNewsletterViewModel()
        {
            Archives = new List<MailChimpCampaign>();
        }
    }
}