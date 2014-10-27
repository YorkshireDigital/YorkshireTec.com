﻿namespace YorkshireDigital.Data.Domain.Events
{
    public class EventTalk
    {
        public virtual string Id { get; set; }
        public virtual string Title { get; set; }
        public virtual string Synopsis { get; set; }
        public virtual string Speaker { get; set; }
        // TODO: public virtual IList<User> Speakers { get; set; }
    }
}
