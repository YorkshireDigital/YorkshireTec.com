namespace YorkshireDigital.Web.Admin.Modules
{
    using System.Linq;
    using Hangfire;
    using Nancy;
    using Nancy.ModelBinding;
    using Nancy.Security;
    using YorkshireDigital.Data.Services;
    using YorkshireDigital.Data.Tasks;
    using YorkshireDigital.Web.Admin.ViewModels;
    using YorkshireDigital.Web.Infrastructure;

    public class AdminGroupIntegrationsModule : BaseModule
    {
        public AdminGroupIntegrationsModule(IGroupService groupService, IMeetupService meetupService, IUserService userService)
            : base("admin/group")
        {
            this.RequiresAuthentication();
            this.RequiresClaims(new[] { "Admin" });

            Get["/{groupId}/integrations"] = _ =>
            {
                string groupId = _.groupId.ToString();

                var group = groupService.Get(groupId);

                if (group == null)
                {
                    return HttpStatusCode.NotFound;
                }

                return Negotiate.WithModel(AdminGroupIntegrationsViewModel.FromDomain(group))
                                .WithView("GroupIntegrations");
            };

            Post["/{groupId}/integrations"] = _ =>
            {
                this.ValidateCsrfToken();

                var viewModel = this.Bind<AdminGroupIntegrationsViewModel>();

                var meetupGroup = meetupService.GetGroup(viewModel.MeetupUrlName);

                if (meetupGroup == null)
                {
                    AddError("MeetupUrlName", "No event exists with this name");
                    viewModel.ShowMeetup = true;

                    return Negotiate.WithModel(viewModel)
                                .WithView("GroupIntegrations")
                                .WithStatusCode(HttpStatusCode.BadRequest);
                }

                string groupId = _.groupId.ToString();

                var group = groupService.Get(groupId);
                group.MeetupId = meetupGroup.Id;
                group.MeetupUrlName = viewModel.MeetupUrlName;
                group.GroupSyncId = string.Format("{0}-groupSync", @group.Id);

                meetupService.AddOrUpdateJob<GroupSyncTask>(group.GroupSyncId, x => x.Execute(groupId), Cron.Hourly);
                meetupService.Trigger(group.GroupSyncId);

                var currentUser = userService.GetUser(Context.CurrentUser.UserName);
                groupService.Save(group, currentUser);

                return Negotiate.WithModel(AdminGroupIntegrationsViewModel.FromDomain(group))
                                .WithView("GroupIntegrations");
            };


            Get["/{groupId}/integrations/validate-meetup"] = _ =>
            {
                var viewModel = this.Bind<AdminGroupIntegrationsViewModel>();

                var exists = meetupService.GroupExists(viewModel.MeetupUrlName);

                return exists;
            };


            Delete["/{groupId}/integrations/"] = _ =>
            {
                string groupId = _.groupId.ToString();

                var group = groupService.Get(groupId);

                meetupService.RemoveJobIfExists(group.GroupSyncId);

                foreach (var @event in @group.Events.Where(@event => !string.IsNullOrEmpty(@event.EventSyncJobId)))
                {
                    meetupService.RemoveJobIfExists(@event.EventSyncJobId);
                    @event.EventSyncJobId = null;
                }

                group.GroupSyncId = null;
                group.MeetupId = 0;
                group.MeetupUrlName = null;

                var currentUser = userService.GetUser(Context.CurrentUser.UserName);
                groupService.Save(group, currentUser);

                return HttpStatusCode.OK;
            };
        }
    }
}