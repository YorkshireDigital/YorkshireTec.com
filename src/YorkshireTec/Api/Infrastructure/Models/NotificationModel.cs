namespace YorkshireTec.Api.Infrastructure.Models
{
    public class NotificationModel
    {
        public string Name { get; set; }
        public string Message { get; set; }
        public NotificationType Type { get; set; }

        public NotificationModel()
        {
            
        }

        public NotificationModel(string message, string name, NotificationType type)
        {
            Name = name;
            Message = message;
            Type = type;
        }
    }
}