namespace YorkshireDigital.Data.Services
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Hangfire;
    using YorkshireDigital.Data.Domain.Events;
    using YorkshireDigital.MeetupApi.Clients;
    using YorkshireDigital.MeetupApi.Models;
    using YorkshireDigital.MeetupApi.Requests;
    using YorkshireDigital.MeetupApi.Requests.Enum;
    using Category = YorkshireDigital.Data.Domain.Events.Category;
    using Event = YorkshireDigital.MeetupApi.Models.Event;

    public interface IMeetupService
    {
        bool GroupExists(string groupName);
        Group GetGroup(string groupName);
        void LinkGroup(Domain.Group.Group @group, string groupName);
        List<Event> GetUpcomingEventsForGroup(int groupId);
        Event GetEvent(string eventId);
        void SyncEvents(Domain.Group.Group @group);
        void RemoveJobIfExists(string jobId);
    }

    public class MeetupService : IMeetupService
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

        public void LinkGroup(Domain.Group.Group @group, string groupName)
        {
            var meetupGroup = GetGroup(groupName);

            if (meetupGroup == null)
            {
                throw new Exception(string.Format("No group found with name {0}", groupName));
            }

            @group.MeetupId = meetupGroup.Id;
            
            // TODO: Create New event sync background task

            // var id = BackgroundJob.Enqueue<NewEventSyncTask>(x => x.Execute(@group.Id, meetupGroup.Id), Cron.Hourly);
            // @group.NewEventSyncTaskId = id;
        }

        public List<Event> GetUpcomingEventsForGroup(int groupId)
        {
            var request = new EventsRequest
            {
                GroupId = groupId, Status = EventStatus.Upcoming
            };

            var response = meetupClient.Events.Get(request);

            return response.Results;
        }

        public Event GetEvent(string eventId)
        {
            var request = new EventsRequest
            {
                EventId = eventId
            };

            var response = meetupClient.Events.Get(request);

            return response.Results.SingleOrDefault();
        }

        public void SyncEvents(Domain.Group.Group @group)
        {
            var upcomingEvents = GetUpcomingEventsForGroup(@group.MeetupId);

            foreach (var upcomingEvent in upcomingEvents)
            {
                if (@group.Events.Any(x => x.MeetupId.ToString() == upcomingEvent.Id)) continue;

                var newEvent = new Domain.Events.Event
                {
                    Categories = new List<Category> {new Category {Name = upcomingEvent.Group.Category.Name}},
                    DeletedBy = null,
                    DeletedOn = null,
                    End =
                        upcomingEvent.Duration.HasValue
                            ? upcomingEvent.StartDate.AddMilliseconds(upcomingEvent.Duration.Value)
                            : upcomingEvent.StartDate,
                    Start = upcomingEvent.StartDate,
                    Group = @group,
                    Interests = upcomingEvent.Group.Topics.Select(x => new Interest {Name = x.Name}).ToList(),
                    Location = upcomingEvent.Venue.Address1,
                    Region = upcomingEvent.Venue.City,
                    LastEditedOn = DateTime.UtcNow,
                    Synopsis = upcomingEvent.Description,
                    Title = upcomingEvent.Name
                };
                @group.Events.Add(newEvent);
            }

            foreach (var @event in @group.Events.ToList())
            {
                if (@event.Start > DateTime.UtcNow && upcomingEvents.All(x => x.Id != @event.MeetupId.ToString()))
                {
                    @group.Events.Remove(@event);
                }
            }
        }

        public void RemoveJobIfExists(string jobId)
        {
            RecurringJob.RemoveIfExists(jobId);
        }
    }
}
