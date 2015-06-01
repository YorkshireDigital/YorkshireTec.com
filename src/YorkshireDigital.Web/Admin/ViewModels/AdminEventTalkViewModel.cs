namespace YorkshireDigital.Web.Admin.ViewModels
{
    using AutoMapper;
    using YorkshireDigital.Data.Domain.Events;

    public class AdminEventTalkViewModel
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Synopsis { get; set; }
        public string Speaker { get; set; }
        public string Link { get; set; }

        public EventTalk ToDomain()
        {
            return Mapper.DynamicMap<AdminEventTalkViewModel, EventTalk>(this);
        }
    }
}