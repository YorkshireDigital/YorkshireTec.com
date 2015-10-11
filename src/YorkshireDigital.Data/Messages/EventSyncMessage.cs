using NHibernate;
using System;
using System.Configuration;
using YorkshireDigital.Data.NHibernate;
using YorkshireDigital.Data.Services;

namespace YorkshireDigital.Data.Messages
{
    public class EventSyncMessage : IHandleMeetupRequest
    {
        private readonly IEventService eventService;
        private readonly IUserService userService;
        private readonly IHangfireService hangfireService;
        private readonly ISession session;
        
        public string EventId { get; set; }

        public EventSyncMessage()
        {
            var sessionFactory = NHibernateSessionFactoryProvider.BuildSessionFactory(ConfigurationManager.ConnectionStrings["Database"].ConnectionString);
            session = sessionFactory.OpenSession();

            userService = new UserService(session);
            eventService = new EventService(session);
            hangfireService = new HangfireService();

            session.BeginTransaction();
        }

        public EventSyncMessage(string eventId) 
            : base()
        {
            EventId = eventId;
        }

        public EventSyncMessage(string eventId, IEventService eventService, IUserService userService, IHangfireService hangfireService)
        {
            EventId = eventId;

            this.eventService = eventService;
            this.userService = userService;
            this.hangfireService = hangfireService;
        }

        public void Handle(IMeetupService meetupService)
        {
            
            Console.WriteLine("Processing Event Sync Message for EventID " + EventId);

            try
            {
                var @event = eventService.Get(EventId);

                var system = userService.GetUser("system");

                if (@event.End <= DateTime.UtcNow)
                {
                    CancelEventSubscription(@event, system);
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

        private void CancelEventSubscription(Domain.Events.Event @event, Domain.Account.User system)
        {
            hangfireService.RemoveJobIfExists(@event.EventSyncJobId);
            @event.EventSyncJobId = null;
            eventService.Save(@event, system);
        }

        public void Dispose()
        {
            if (session != null)
            {
                session.Dispose();
            }
        }
    }
}
