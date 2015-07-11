namespace YorkshireDigital.Web.Infrastructure.Helpers
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using Nancy.ViewEngines.Razor;

    public static class HtmlHelper
    {
        public static IHtmlString ValidationSummary<T>(this HtmlHelpers<T> helper)
        {
            var viewBag = helper.RenderContext.Context.ViewBag;

            if (viewBag.Errors == null || viewBag.Errors.Value == null)
                return new NonEncodedHtmlString("");

            var errors = viewBag["Errors"].Value as IDictionary<string, List<string>>;

            if (errors == null)
                return new NonEncodedHtmlString("");

            var errorHtml = new StringBuilder();
            errorHtml.Append("<div class=\"field-errors-summary\">");
            errorHtml.Append("<ul>");
            foreach (var field in errors)
            {
                foreach (var error in field.Value)
                {
                    errorHtml.AppendFormat("<li class=\"field-error\">{0}</li>", error);
                }
            }
            errorHtml.Append("</ul>");
            errorHtml.Append("</div>");

            return new NonEncodedHtmlString(errorHtml.ToString());
        }

        public static IHtmlString ValidationMessageFor<T>(this HtmlHelpers<T> helper, string propertyName)
        {
            var viewBag = helper.RenderContext.Context.ViewBag;

            if (viewBag.Errors == null)
                return new NonEncodedHtmlString("");

            var errors = viewBag["Errors"].Value as IDictionary<string, List<string>>;

            if (errors == null)
                return new NonEncodedHtmlString("");

            var errorHtml = new StringBuilder();
            foreach (var field in errors.Where(x => x.Key == propertyName))
            {
                errorHtml.Append("<div class=\"field-errors\">");
                foreach (var error in field.Value)
                {
                    errorHtml.Append(string.Format("<span class=\"field-error\">{0}</span>", error));
                }
                errorHtml.Append("</div>");
            }

            return new NonEncodedHtmlString(errorHtml.ToString());
        }
    }
}