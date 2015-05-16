namespace YorkshireDigital.Data.Tasks
{
    using System;
    using YorkshireDigital.Data.Services;

    public class EventSyncTask
    {
        private readonly IEventService eventService;
        private readonly IMeetupService meetupService;
        private readonly IUserService userService;

        public EventSyncTask(IEventService eventService, IMeetupService meetupService, IUserService userService)
        {
            this.eventService = eventService;
            this.meetupService = meetupService;
            this.userService = userService;
        }

        public void Execute(string eventId)
        {
            var @event = eventService.Get(eventId);

            var system = userService.GetUser("system");

            if (@event.End <= DateTime.UtcNow)
            {
                meetupService.RemoveJobIfExists(@event.EventSyncJobId);
                @event.EventSyncJobId = null;
                eventService.Save(@event, system);
            }

            var meetupEvent = meetupService.GetEvent(@event.MeetupId);

            if (meetupEvent == null)
            {
                // Delete the event if it has been deleted from meetup
                eventService.Delete(eventId, system);
                return;
            }

            // Don't update if meetup hasn't been updated
            if (meetupEvent.UpdatedDate <= @event.LastEditedOn) return;

            @event.UpdateFromMeetup(meetupEvent);

            eventService.Save(@event, system);
        }
    }
}
