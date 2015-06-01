namespace YorkshireDigital.Data.Domain.Group
{
    using YorkshireDigital.Data.Domain.Group.Enums;

    public class ContactLink
    {
        public virtual int Id { get; set; }
        public virtual ContactLinkType Type { get; set; }
        public virtual string Value { get; set; }
    }
}
