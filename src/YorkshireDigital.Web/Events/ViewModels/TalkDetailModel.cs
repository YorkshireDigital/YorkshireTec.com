namespace YorkshireDigital.Web.Events.ViewModels
{
    using YorkshireDigital.Data.Domain.Events;

    public class TalkDetailModel
    {
        public TalkDetailModel(EventTalk talk)
        {
            Speaker = talk.Speaker;
            SpeakerLink = talk.Link;
            Title = talk.Title;
            Synopsis = talk.Synopsis;
        }

        public string Speaker { get; set; }
        public string SpeakerLink { get; set; }
        public string Title { get; set; }
        public string Synopsis { get; set; }
    }
}