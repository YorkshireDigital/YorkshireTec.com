namespace YorkshireDigital.Data.Tasks
{
    using System;
    using System.Configuration;
    using System.Linq;
    using global::NHibernate;
    using Hangfire;
    using YorkshireDigital.Data.Domain.Events;
    using YorkshireDigital.Data.NHibernate;
    using YorkshireDigital.Data.Services;
    using YorkshireDigital.MeetupApi.Clients;

    public class GroupSyncTask : IDisposable
    {
        private readonly IUserService userService;
        private readonly IEventService eventService;
        private readonly IMeetupService meetupService;
        private readonly IGroupService groupService;
        private readonly ISession session;

        public GroupSyncTask()
        {
            var sessionFactory = NHibernateSessionFactoryProvider.BuildSessionFactory(ConfigurationManager.ConnectionStrings["Database"].ConnectionString);
            session = sessionFactory.OpenSession();

            userService = new UserService(session);
            eventService = new EventService(session);
            meetupService = new MeetupService(new MeetupClient(ConfigurationManager.AppSettings["Meetup_Bot_ApiKey"], ConfigurationManager.AppSettings["Meetup_Bot_MemberId"]));
            groupService = new GroupService(session);

            session.BeginTransaction();
        }

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
                Event @event;
                // Don't do anything if the event has already been created
                if (@group.Events.All(x => x.MeetupId != upcomingEvent.Id))
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
                    meetupService.AddOrUpdateJob<EventSyncTask>(@event.UniqueName, x => x.Execute(@event.UniqueName), Cron.Hourly);
                    @event.EventSyncJobId = @event.UniqueName;

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
        }

        public void Dispose()
        {
            session.Transaction.Commit();
            session.Dispose();
        }
    }
}
