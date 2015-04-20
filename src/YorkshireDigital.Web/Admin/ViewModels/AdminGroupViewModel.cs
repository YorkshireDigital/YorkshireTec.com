namespace YorkshireDigital.Web.Admin.ViewModels
{
    using AutoMapper;
    using YorkshireDigital.Data.Domain.Organisations;

    public class AdminGroupViewModel
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string ShortName { get; set; }
        public string About { get; set; }
        public string Colour { get; set; }
        public string Headline { get; set; }

        public static AdminGroupViewModel FromDomain(Group @group)
        {
            return Mapper.DynamicMap<Group, AdminGroupViewModel>(@group);
        }

        public Group ToDomain()
        {
            return Mapper.DynamicMap<AdminGroupViewModel, Group>(this);
        }

        public void UpdateDomain(Group @group)
        {
            Mapper.DynamicMap(this, @group);
        }
    }
}