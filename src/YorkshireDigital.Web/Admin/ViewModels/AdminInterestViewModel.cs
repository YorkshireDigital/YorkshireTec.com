namespace YorkshireDigital.Web.Admin.ViewModels
{
    using YorkshireDigital.Data.Domain.Events;

    public class AdminInterestViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public bool Selected { get; set; }

        public static AdminInterestViewModel FromDomain(Interest domain)
        {
            return new AdminInterestViewModel
            {
                Id = domain.Id,
                Name = domain.Name
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