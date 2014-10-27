namespace YorkshireTec.Data.Domain.Organisations
{
    using System.Collections.Generic;
    using YorkshireTec.Data.Domain.Events;

    public class Organisation
    {
        public virtual string Id { get; set; }
        public virtual string Name { get; set; }
        public virtual string ShortName { get; set; }
        public virtual string About { get; set; }
        public virtual string Colour { get; set; }
        public virtual IList<ContactLink> ContactLinks { get; set; }
        public virtual IList<Event> Events { get; set; }
        public virtual byte[] Logo { get; set; }
        public virtual byte[] Photo { get; set; }
    }
}
