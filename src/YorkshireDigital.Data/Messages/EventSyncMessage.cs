using NHibernate;
using System;
using System.Configuration;
using YorkshireDigital.Data.NHibernate;
using YorkshireDigital.Data.Services;
using YorkshireDigital.MeetupApi.Clients;

namespace YorkshireDigital.Data.Messages
{
    public class EventSyncMessage : IHandleMessage
    {
        private string eventId;

        private readonly IEventService eventService;
        private readonly IMeetupService meetupService;
        private readonly IUserService userService;
        private readonly IHangfireService hangfireService;
        private readonly ISession session;

        public EventSyncMessage(string eventId)
        {
            this.eventId = eventId;

            var sessionFactory = NHibernateSessionFactoryProvider.BuildSessionFactory(ConfigurationManager.ConnectionStrings["Database"].ConnectionString);
            session = sessionFactory.OpenSession();

            userService = new UserService(session);
            eventService = new EventService(session);
            meetupService = new MeetupService(new MeetupClient(ConfigurationManager.AppSettings["Meetup_Bot_ApiKey"], ConfigurationManager.AppSettings["Meetup_Bot_MemberId"]));

            session.BeginTransaction();
        }

        public EventSyncMessage(string eventId, IEventService eventService, IMeetupService meetupService, IUserService userService, IHangfireService hangfireService)
        {
            this.eventId = eventId;

            this.eventService = eventService;
            this.meetupService = meetupService;
            this.userService = userService;
            this.hangfireService = hangfireService;
        }

        public void Handle()
        {
            var @event = eventService.Get(eventId);

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
