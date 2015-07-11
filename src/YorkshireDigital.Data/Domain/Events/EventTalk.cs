namespace YorkshireDigital.Data.Domain.Events
{
    using YorkshireDigital.Data.Domain.Shared;

    public class EventTalk
    {
        public virtual int Id { get; set; }
        public virtual string Title { get; set; }
        public virtual string Synopsis { get; set; }
        public virtual string Speaker { get; set; }
        public virtual TextFormat SynopsisFormat { get; set; }
        public virtual string Link { get; set; }
        // TODO: public virtual IList<User> Speakers { get; set; }

        public virtual Event Event { get; set; }
    }
}
