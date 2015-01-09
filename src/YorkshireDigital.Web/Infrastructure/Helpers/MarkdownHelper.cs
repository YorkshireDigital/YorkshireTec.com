namespace YorkshireDigital.Web.Infrastructure.Helpers
{
    using Microsoft.Security.Application;

    public static class MarkdownHelper
    {
        public static string Sanitize(this string html)
        {
            return Sanitizer.GetSafeHtmlFragment(html);
        }

        public static string MarkdownToHtml(this string markdown)
        {
            return Sanitize(new MarkdownSharp.Markdown().Transform(markdown));
        }
    }
}