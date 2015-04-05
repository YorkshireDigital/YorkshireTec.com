namespace YorkshireDigital.Web.Admin.ViewModels
{
    using YorkshireDigital.Data.Domain.Account.Enums;

    public class AdminUserListViewModel
    {
        public string Username { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public MailingListState MailingListState { get; set; }
    }
}