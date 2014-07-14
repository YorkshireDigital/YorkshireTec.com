namespace YorkshireTec.Data.Domain.Events
{
    using YorkshireTec.Data.Domain.Events.Enums;

    public class SocialLink
    {
        public virtual SocialLinkType Type { get; set; }
        public virtual string Description { get; set; }
        public virtual string Url { get; set; }
    }
}
