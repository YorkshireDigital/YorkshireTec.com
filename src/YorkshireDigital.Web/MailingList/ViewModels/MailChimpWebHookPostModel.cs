namespace YorkshireDigital.Web.MailingList.ViewModels
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text.RegularExpressions;

    public class MailChimpWebHookPostModel
    {
        public MailChimpWebHookType Type { get; set; }
        public DateTime FiredAt { get; set; }
        public Dictionary<string, object> Data { get; set; }
        public Dictionary<string, object> Merges { get; set; }

        public MailChimpWebHookPostModel()
        {
            Data = new Dictionary<string, object>();
            Merges = new Dictionary<string, object>();
        }

        public bool PopulateData(IDictionary<string, object> data)
        {
            MailChimpWebHookType type;
            if(!Enum.TryParse(data["type"].ToString(), true, out type)) return false;
            Type = type;

            DateTime firedAt;
            if (!DateTime.TryParse(data["fired_at"].ToString(), out firedAt)) return false;
            FiredAt = firedAt;

            foreach (var item in data.Where(item => item.Key.Contains("data")))
            {
                if (item.Key.Contains("merges"))
                {
                    var matches = Regex.Match(item.ToString(), @"data\[merges\]\[([^\]]+)]");
                    if (matches.Groups.Count != 2) return false;

                    var key = matches.Groups[1].Value;
                    Merges.Add(key, item.Value);
                }
                else
                {
                    var matches = Regex.Match(item.ToString(), @"data\[([^\]]+)]");
                    if (matches.Groups.Count != 2) return false;

                    var key = matches.Groups[1].Value;
                    Data.Add(key, item.Value);
                }
            }

            return true;
        }
    }
}