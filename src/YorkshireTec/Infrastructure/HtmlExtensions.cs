namespace YorkshireTec.Infrastructure
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using Nancy.ViewEngines.Razor;
    using YorkshireTec.ViewModels;

    public static class HtmlExtensions
    {
        public static IHtmlString CheckBox<T>(this HtmlHelpers<T> helper, string name, dynamic modelProperty)
        {
            string input;
            bool checkedState;

            if (!bool.TryParse(modelProperty.ToString(), out checkedState))
            {
                input = "<input name=\"" + name + "\" type=\"checkbox\" value=\"true\" />";
            }
            else
            {
                if (checkedState)
                    input = "<input name=\"" + name + "\" type=\"checkbox\" value=\"true\" checked />";
                else
                    input = "<input name=\"" + name + "\" type=\"checkbox\" value=\"true\" />";
            }


            return new NonEncodedHtmlString(input);
        }

        public static IHtmlString NotificationSummary<T>(this HtmlHelpers<T> helper, List<NotificationModel> notifications)
        {

            if (!notifications.Any())
                return new NonEncodedHtmlString("");

            StringBuilder div = new StringBuilder("<div class=\"notifications\">");

            foreach (var notification in notifications)
            {
                switch (notification.Type)
                {
                    case NotificationType.Success:
                        div.Append("<span class='alert-box success'>" +
                                   notification.Message + "</span>");
                        break;
                    case NotificationType.Warning:
                        div.Append("<span class='alert-box'>" +
                                   notification.Message + "</span>");
                        break;
                    case NotificationType.Error:
                        div.Append("<span class='alert-box alert'>" +
                                   notification.Message + "</span>");
                        break;
                }
            }

            div.Append("</div>");

            return new NonEncodedHtmlString(div.ToString());
        }


        public static IHtmlString ValidationSummary<T>(this HtmlHelpers<T> helper, List<NotificationModel> notifications)
        {

            if (notifications.All(x => x.Type != NotificationType.Error))
                return new NonEncodedHtmlString("");

            string div = notifications.Where(x => x.Type == NotificationType.Error).Aggregate("<div class=\"ui error message show \"><div class=\"ui list divided\">", (current, error) => current + ("<div class=\"item\">" + error.Message + "</div>"));

            div += "</div></div>";

            return new NonEncodedHtmlString(div);
        }

        public static IHtmlString ValidationMessageFor<T>(this HtmlHelpers<T> helper, List<NotificationModel> notifications, string propertyName)
        {
            if (!notifications.Any(x => x.Type == NotificationType.Error && x.Name == propertyName))
                return new NonEncodedHtmlString("");

            string span = String.Empty;

            foreach (var error in notifications.Where(x => x.Type == NotificationType.Error && x.Name == propertyName))
            {
                span += "<div class=\"ui red pointing above ui label\">" + error.Message + "</div>";
                break;
            }

            return new NonEncodedHtmlString(span);
        }

        public static bool IsDebug<T>(this HtmlHelpers<T> helper)
        {
#if DEBUG
            return true;
#else
            return false;
#endif
        }
    }
}