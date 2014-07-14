namespace YorkshireTec.Data.Domain.Events
{
    using System.Collections.Generic;

    public class Category
    {
        public virtual string Name { get; set; }
        public virtual IList<Event> Events { get; set; }
    }
}
