namespace YorkshireDigital.Data.Tasks
{
    using System;
    using System.Linq;
    using Hangfire;
    using YorkshireDigital.Data.Domain.Events;
    using YorkshireDigital.Data.Services;

    public class GroupSyncTask
    {
        private readonly IUserService userService;
        private readonly IEventService eventService;
        private readonly IMeetupService meetupService;
        private readonly IGroupService groupService;

        public GroupSyncTask(IGroupService groupService, IMeetupService meetupService, IEventService eventService, IUserService userService)
        {
            this.userService = userService;
            this.eventService = eventService;
            this.meetupService = meetupService;
            this.groupService = groupService;
        }

        public void Execute(string groupId)
        {
            var group = groupService.Get(groupId);
            var system = userService.GetUser("system");

            if (group == null)
            {
                throw new Exception(string.Format("No group found with an ID of {0}", groupId));
            }

            var upcomingEvents = meetupService.GetUpcomingEventsForGroup(group.MeetupId);

            foreach (var upcomingEvent in upcomingEvents)
            {
                // Don't do anything if the event has already been created
                if (@group.Events.Any(x => x.MeetupId == upcomingEvent.Id)) continue;

                var newEvent = Event.FromMeetupGroup(upcomingEvent);
                newEvent.UniqueName = string.Format("{0}-{1}", @group.Id, upcomingEvent.Id);
                newEvent.Group = group;

                group.Events.Add(newEvent);

                eventService.Save(newEvent, system);

                var updateTask = new EventSyncTask(eventService, meetupService, userService);

                meetupService.AddOrUpdateJob(newEvent.UniqueName, () => updateTask.Execute(newEvent.UniqueName), Cron.Hourly);
            }

            // Delete future events that are no longer on meetup
            foreach (var @event in @group.Events.ToList())
            {
                if (@event.Start > DateTime.UtcNow && upcomingEvents.All(x => x.Id != @event.MeetupId.ToString()))
                {
                    @group.Events.Remove(@event);
                    eventService.Delete(@event.UniqueName, system);
                }
            }
        }
    }
}
