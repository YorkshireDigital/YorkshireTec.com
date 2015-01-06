namespace YorkshireDigital.Data.Domain.Events
{
    public class EventTalk
    {
        public virtual int Id { get; set; }
        public virtual string Title { get; set; }
        public virtual string Synopsis { get; set; }
        public virtual string Speaker { get; set; }
        public virtual string Link { get; set; }
        // TODO: public virtual IList<User> Speakers { get; set; }
    }
}
