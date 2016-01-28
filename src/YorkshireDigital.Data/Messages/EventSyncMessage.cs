using NHibernate;
using System;
using YorkshireDigital.Data.Services;

namespace YorkshireDigital.Data.Messages
{
    public class EventSyncMessage : IHandleMeetupRequest
    {
        private IEventService eventService;
        private IUserService userService;
        private IHangfireService hangfireService;
        public string EventId { get; set; }

        public EventSyncMessage()
        {

        }

        public EventSyncMessage(string eventId) 
            : base()
        {
            EventId = eventId;
        }

        public EventSyncMessage(string eventId, IEventService eventService, IUserService userService, IHangfireService hangfireService)
        {
            this.eventService = eventService;
            this.userService = userService;
            this.hangfireService = hangfireService;
            EventId = eventId;
        }

        public void Handle(ISession session, IMeetupService meetupService)
        {
            eventService = eventService ?? new EventService(session);
            userService = userService ?? new UserService(session);
            hangfireService = hangfireService ??new HangfireService();

            Console.WriteLine("Processing Event Sync Message for EventID " + EventId);

            try
            {
                var @event = eventService.Get(EventId);

                var system = userService.GetUser("system");

                if (@event.End <= DateTime.UtcNow)
                {
                    hangfireService.RemoveJobIfExists(@event.EventSyncJobId);
                    @event.EventSyncJobId = null;
                    eventService.Save(@event, system);
                }

                var meetupEvent = meetupService.GetEvent(@event.MeetupId);

                if (meetupEvent == null)
                {
                    // Delete the event if it has been deleted from meetup
                    eventService.Delete(EventId, system);
                    return;
                }

                // Don't update if meetup hasn't been updated on meetup
                if (meetupEvent.UpdatedDate <= @event.LastEditedOn) return;

                // otherwise update the event
                @event.UpdateFromMeetup(meetupEvent);
                eventService.Save(@event, system);
                
            }
            catch (Exception ex)
            {
                Console.WriteLine("An error occurred while processing the event sync. Message: {0}", ex.Message);
            }

            Console.WriteLine("Processing Complete");
        }
    }
}
