namespace YorkshireTec.Data.Domain.Events
{
    using System;
    using System.Collections.Generic;

    public class Event
    {
        public virtual int Id { get; set; }
        public virtual string UniqueName { get; set; }
        public virtual string Title { get; set; }
        public virtual string ShortDescription { get; set; }
        public virtual string LongDescription { get; set; }
        public virtual string Website { get; set; }
        public virtual string Colour { get; set; }
        public virtual byte[] Logo { get; set; }
        public virtual DateTime Start { get; set; }
        public virtual DateTime End { get; set; }
        public virtual string Location { get; set; }
        public virtual decimal Price { get; set; }
        public virtual byte[] Photo { get; set; }
        // TODO: Categories
        public virtual IList<Category> Categories { get; set; }
        // TODO: Interests
        public virtual IList<Interest> Interests { get; set; }
        // TODO: Social Links
        public virtual IList<SocialLink> SocialLinks { get; set; }
        // TODO: Contact Details
        // TODO: Speakers
        // TODO: Attendees
        // TODO: Organisation
    }
}
