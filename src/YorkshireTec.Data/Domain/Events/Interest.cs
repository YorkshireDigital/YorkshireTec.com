namespace YorkshireTec.Data.Domain.Events
{
    using System.Collections.Generic;

    public class Interest
    {
        public virtual int Id { get; set; }
        public virtual string Name { get; set; }
        public virtual IList<Event> Events { get; set; }
    }
}
