namespace YorkshireDigital.Web.Infrastructure.Responses
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Xml;
    using System.Xml.Linq;
    using Nancy;
    using YorkshireDigital.Data.Domain.Events;

    public class SitemapResponse : Response
    {
        private readonly string siteUrl;

        private readonly List<string> pages = new List<string>
        {
            "",
            "mailinglist/confirmation",
            "mailinglist/subscribed",
            "mailinglist/archive",
        };

        public SitemapResponse(IEnumerable<Event> model, string siteUrl)
        {
            this.siteUrl = siteUrl;
            Contents = GetXmlContents(model);
            ContentType = "application/xml";
            StatusCode = HttpStatusCode.OK;
        }

        private Action<Stream> GetXmlContents(IEnumerable<Event> model)
        {
            var blank = XNamespace.Get(@"http://www.sitemaps.org/schemas/sitemap/0.9");

            var xDocument = new XDocument(new XDeclaration("1.0", "utf-8", "yes"), new XElement(blank + "urlset", new XAttribute("xmlns", blank.NamespaceName)));

            foreach (var page in pages)
            {
                xDocument.Root.Add(new XElement(blank + "url",
                                    new XElement(blank + "loc", string.Format("{0}{1}", siteUrl, page)),
                    // TODO: Add LastModified to Event
                                    new XElement(blank + "lastmod", DateTime.Now.ToString("yyyy-MM-dd")),
                                    new XElement(blank + "changefreq", "weekly"),
                                    new XElement(blank + "priority", "1.00")));
            }

            foreach (var item in model)
            {
                xDocument.Root.Add(new XElement(blank + "url",
                                    new XElement(blank + "loc", string.Format("{0}event/{1}", siteUrl, item.UniqueName)),
                    // TODO: Add LastModified to Event
                                    new XElement(blank + "lastmod", DateTime.Now.ToString("yyyy-MM-dd")),
                                    new XElement(blank + "changefreq", "weekly"),
                                    new XElement(blank + "priority", "1.00")));
            }

            return stream =>
            {
                using (XmlWriter writer = XmlWriter.Create(stream))
                {

                    xDocument.Save(writer);
                }
            };
        }
    }
}