namespace YorkshireTec.Data.Domain.Events
{
    using System;
    using System.Collections.Generic;
    using YorkshireTec.Data.Domain.Organisations;

    public class Event
    {
        public virtual string UniqueName { get; set; }
        public virtual string Title { get; set; }
        public virtual string ShortDescription { get; set; }
        public virtual string LongDescription { get; set; }
        public virtual DateTime Start { get; set; }
        public virtual DateTime End { get; set; }
        public virtual string Location { get; set; }
        public virtual decimal Price { get; set; }
        public virtual byte[] Photo { get; set; }
        public virtual IList<Category> Categories { get; set; }
        public virtual IList<Interest> Interests { get; set; }
        public virtual IList<EventTalk> Talks { get; set; }
        // TODO: public virtual IList<User> Attendees { get; set; }
        public virtual Organisation Organisation { get; set; }
    }
}
