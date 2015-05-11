namespace YorkshireDigital.Data.Services
{
    using System.Linq;
    using YorkshireDigital.MeetupApi;
    using YorkshireDigital.MeetupApi.Groups.Requests;
    using YorkshireDigital.MeetupApi.Models;

    public class MeetupService
    {
        private readonly IMeetupClient meetupClient;

        public MeetupService(IMeetupClient meetupClient)
        {
            this.meetupClient = meetupClient;
        }

        public bool GroupExists(string groupName)
        {
            var request = new GroupsRequest {GroupUrlName = groupName};

            var response = meetupClient.Groups.Get(request);

            return response.Results.Any();
        }

        public Group GetGroup(string groupName)
        {
            var request = new GroupsRequest { GroupUrlName = groupName };

            var response = meetupClient.Groups.Get(request);

            return response.Results.SingleOrDefault();
        }
    }
}
