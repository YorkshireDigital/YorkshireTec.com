namespace YorkshireDigital.Web.Admin.ViewModels
{
    using AutoMapper;
    using YorkshireDigital.Data.Domain.Group;

    public class AdminGroupIntegrationsViewModel
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Colour { get; set; }
        public int MeetupId { get; set; }
        public string MeetupUrlName { get; set; }
        public bool ShowMeetup { get; set; }

        public static AdminGroupIntegrationsViewModel FromDomain(Group @group)
        {
            return Mapper.DynamicMap<Group, AdminGroupIntegrationsViewModel>(@group);
        }
    }
}