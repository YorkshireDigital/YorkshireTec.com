namespace YorkshireTec.Data.Domain.Organisations
{
    using YorkshireTec.Data.Domain.Organisations.Enums;

    public class ContactLink
    {
        public virtual int Id { get; set; }
        public virtual ContactLinkType Type { get; set; }
        public virtual string Value { get; set; }
    }
}
