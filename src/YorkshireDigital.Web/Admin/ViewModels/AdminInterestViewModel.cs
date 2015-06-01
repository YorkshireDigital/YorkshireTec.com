namespace YorkshireDigital.Web.Admin.ViewModels
{
    using System.Collections.Generic;
    using System.Linq;
    using YorkshireDigital.Data.Domain.Events;

    public class AdminInterestViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public bool Selected { get; set; }

        public static AdminInterestViewModel FromDomain(Interest domain, IList<Interest> selectedInterests)
        {
            return new AdminInterestViewModel
            {
                Id = domain.Id,
                Name = domain.Name,
                Selected = selectedInterests.Any(x => x.Id == domain.Id)
            };
        }

        public Interest ToDomain()
        {
            return new Interest
            {
                Id = Id,
                Name = Name
            };
        }
    }
}