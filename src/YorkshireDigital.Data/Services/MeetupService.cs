namespace YorkshireDigital.Data.Services
{
    using System.Linq;
    using YorkshireDigital.MeetupApi;
    using YorkshireDigital.MeetupApi.Groups.Requests;

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

            var results = meetupClient.Groups.Get(request);

            return results.Results.Any();
        }
    }
}
