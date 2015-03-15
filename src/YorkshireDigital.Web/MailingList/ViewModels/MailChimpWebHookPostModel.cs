namespace YorkshireDigital.Web.MailingList.ViewModels
{
    using System;
    using System.Collections.Generic;

    public class MailChimpWebHookPostModel
    {
        public MailChimpWebHookType Type { get; set; }
        public DateTime Fired_at { get; set; }
        public Dictionary<string, object> Data { get; set; }
    }
}