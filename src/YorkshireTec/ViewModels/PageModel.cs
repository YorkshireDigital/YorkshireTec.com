namespace YorkshireTec.ViewModels
{
    using System.Collections.Generic;

    public class PageModel
    {
        public string TitleSuffix { get; set; }
        public string Title { get; set; }
        public bool IsAuthenticated { get; set; }
        public string FriendlyName { get; set; }
        public string CurrentUser { get; set; }
        public string ImageUrl { get; set; }
        public List<NotificationModel> Notifications { get; set; }
    }
}