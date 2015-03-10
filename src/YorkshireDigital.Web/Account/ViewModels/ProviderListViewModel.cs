using System.Collections.Generic;

namespace YorkshireDigital.Web.Account.ViewModels
{
    using System.Linq;

    public class ProviderListViewModel : List<ProviderViewModel>
    {
        public bool HasTwitter
        {
            get { return this.Any(x => x.Name == "twitter"); }
        }

        public bool HasFacebook
        {
            get { return this.Any(x => x.Name == "facebook"); }
        }

        public bool HasLinkedIn
        {
            get { return this.Any(x => x.Name == "linkedin"); }
        }

        public bool HasGoogle
        {
            get { return this.Any(x => x.Name == "google"); }
        }
    }
}