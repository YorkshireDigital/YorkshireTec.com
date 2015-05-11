namespace YorkshireDigital.Data.Domain.Events
{
    using System;
    using System.Collections.Generic;
    using YorkshireDigital.Data.Domain.Account;
    using YorkshireDigital.Data.Domain.Group;

    public class Event
    {
        public virtual string UniqueName { get; set; }
        public virtual Group Group { get; set; }
        public virtual string Title { get; set; }
        public virtual string Synopsis { get; set; }
        public virtual DateTime Start { get; set; }
        public virtual DateTime End { get; set; }
        public virtual string Location { get; set; }
        public virtual string Region { get; set; }
        public virtual decimal Price { get; set; }
        public virtual byte[] Photo { get; set; }
        public virtual IList<Category> Categories { get; set; }
        public virtual IList<Interest> Interests { get; set; }
        public virtual IList<EventTalk> Talks { get; set; }
        public virtual DateTime LastEditedOn { get; set; }
        public virtual User LastEditedBy { get; set; }
        public virtual DateTime? DeletedOn { get; set; }
        public virtual User DeletedBy { get; set; }

        public virtual bool IsDeleted { get { return DeletedOn.HasValue; } }

        // TODO: public virtual IList<User> Attendees { get; set; }
    }
}
