namespace YorkshireTec.Api.Infrastructure.Models
{
    using System.Collections.Generic;
    using Nancy.Validation;

    public class PageModel
    {
        public string TitleSuffix { get; set; }
        public string Title { get; set; }
        public string CurrentNav { get; set; }
        public bool IsAuthenticated { get; set; }
        public string FriendlyName { get; set; }
        public string CurrentUser { get; set; }
        public string Email { get; set; }
        public string ImageUrl { get; set; }
        public List<NotificationModel> Notifications { get; set; }

        public void AddError(string errorMessage, string member)
        {
            Notifications.Add(new NotificationModel { Type = NotificationType.Error, Name = member, Message = errorMessage });
        }

        public void AddErrors(ModelValidationResult result)
        {
            foreach (var kvp in result.Errors)
            {
                foreach (var item in kvp.Value)
                {
                    foreach (var member in item.MemberNames)
                    {
                        Notifications.Add(new NotificationModel { Type = NotificationType.Error, Name = member, Message = item.ErrorMessage });
                    }
                }
            }
        }
    }
}