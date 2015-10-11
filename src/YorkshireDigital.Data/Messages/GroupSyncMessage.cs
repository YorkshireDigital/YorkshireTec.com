using Hangfire;
using NHibernate;
using System;
using System.Configuration;
using System.Linq;
using YorkshireDigital.Data.Domain.Events;
using YorkshireDigital.Data.NHibernate;
using YorkshireDigital.Data.Services;
using YorkshireDigital.Data.Tasks;

namespace YorkshireDigital.Data.Messages
{
    public class GroupSyncMessage : IHandleMeetupRequest
    {
        private readonly IEventService eventService;
        private readonly IUserService userService;
        private readonly IGroupService groupService;
        private readonly IHangfireService hangfireService;
        private readonly ISession session;

        public string GroupId { get; set; }

        public GroupSyncMessage()
        {
            var sessionFactory = NHibernateSessionFactoryProvider.BuildSessionFactory(ConfigurationManager.ConnectionStrings["Database"].ConnectionString);
            session = sessionFactory.OpenSession();

            userService = new UserService(session);
            eventService = new EventService(session);
            groupService = new GroupService(session);
            hangfireService = new HangfireService();

            session.BeginTransaction();
        }

        public GroupSyncMessage(string groupId)
            : base()
        {
            GroupId = groupId;
        }

        public GroupSyncMessage(string groupId, IGroupService groupService, IEventService eventService, IUserService userService, IHangfireService hangfireService)
        {
            GroupId = groupId;

            this.groupService = groupService;
            this.eventService = eventService;
            this.userService = userService;
            this.hangfireService = hangfireService;
        }

        public void Handle(IMeetupService meetupService)
        {
            Console.WriteLine("Processing Group Sync Message for GroupID " + GroupId);
            
            var group = groupService.Get(GroupId);
            var system = userService.GetUser("system");

            if (group == null)
            {
                throw new Exception(string.Format("No group found with an ID of {0}", GroupId));
            }

            var upcomingEvents = meetupService.GetUpcomingEventsForGroup(group.MeetupId);

            foreach (var upcomingEvent in upcomingEvents)
            {
                Event @event;
                // Don't do anything if the event has already been created
                if (group.Events.All(x => x.MeetupId != upcomingEvent.Id))
                {
                    @event = Event.FromMeetupGroup(upcomingEvent);
                    @event.UniqueName = string.Format("{0}-{1}", @group.Id, upcomingEvent.Id);
                    @event.Group = group;

                    group.Events.Add(@event);

                    eventService.Save(@event, system);
                }
                else
                {
                    @event = @group.Events.Single(x => x.MeetupId == upcomingEvent.Id);
                }

                if (string.IsNullOrEmpty(@event.EventSyncJobId))
                {
                    @event.EventSyncJobId = @event.UniqueName;

                    hangfireService.AddOrUpdateJob<EventSyncTask>(@event.UniqueName, x => x.Execute(@event.UniqueName), Cron.Hourly);
                    hangfireService.Trigger(@event.EventSyncJobId);

                    eventService.Save(@event, system);
                }
            }

            // Delete future events that are no longer on meetup
            foreach (var @event in @group.Events.ToList())
            {
                if (@event.Start > DateTime.UtcNow && upcomingEvents.All(x => string.IsNullOrEmpty(@event.MeetupId) || x.Id != @event.MeetupId.ToString()))
                {
                    @group.Events.Remove(@event);
                    eventService.Delete(@event.UniqueName, system);
                }
            }

            Console.WriteLine("Processing Complete");
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
