﻿using Hangfire;
using NHibernate;
using System;
using System.Linq;
using Serilog;
using YorkshireDigital.Data.Domain.Events;
using YorkshireDigital.Data.Services;
using YorkshireDigital.Data.Tasks;

namespace YorkshireDigital.Data.Messages
{
    public class GroupSyncMessage : IHandleMeetupRequest
    {
        private IEventService eventService;
        private IUserService userService;
        private IHangfireService hangfireService;
        private IGroupService groupService;

        public string GroupId { get; set; }

        public GroupSyncMessage()
        {
        }

        public GroupSyncMessage(string groupId)
            : base()
        {
            GroupId = groupId;
        }

        public GroupSyncMessage(string groupId, IGroupService groupService, IEventService eventService, IUserService userService, IHangfireService hangfireService)
        {
            this.groupService = groupService;
            this.eventService = eventService;
            this.userService = userService;
            this.hangfireService = hangfireService;

            GroupId = groupId;
        }

        public void Handle(ISession session, IMeetupService meetupService)
        {
            eventService = eventService ?? new EventService(session);
            userService = userService ?? new UserService(session);
            groupService = groupService ?? new GroupService(session);
            hangfireService = hangfireService ?? new HangfireService();

            Log.Information("Processing Group Sync Message for GroupID " + GroupId);
            
            var group = groupService.Get(GroupId);
            var system = userService.GetUser("system");

            try
            {
                if (group == null)
                {
                    throw new Exception($"No group found with an ID of {GroupId}");
                }

                var upcomingEvents = meetupService.GetUpcomingEventsForGroup(group.MeetupId);

                foreach (var upcomingEvent in upcomingEvents)
                {
                    Log.Information($"[{upcomingEvent.Id}] Processing...");
                    Event @event;
                    // Don't do anything if the event has already been created
                    if (group.Events.All(x => x.MeetupId != upcomingEvent.Id))
                    {
                        Log.Information($"[{upcomingEvent.Id}] Adding event to group.");

                        @event = Event.FromMeetupGroup(upcomingEvent);
                        @event.UniqueName = $"{@group.Id}-{upcomingEvent.Id}";
                        @event.Group = group;

                        group.Events.Add(@event);
                    }
                    else
                    {
                        @event = @group.Events.Single(x => x.MeetupId == upcomingEvent.Id);
                    }

                    if (string.IsNullOrEmpty(@event.EventSyncJobId))
                    {
                        Log.Information($"[{upcomingEvent.Id}] Adding sync task.");
                        @event.EventSyncJobId = @event.UniqueName;

                        hangfireService.AddOrUpdateJob<EventSyncTask>(@event.UniqueName, x => x.Execute(@event.UniqueName), Cron.Hourly);
                        hangfireService.Trigger(@event.EventSyncJobId);
                    }

                    eventService.Save(@event, system);
                }

                // Delete future events that are no longer on meetup
                foreach (var @event in @group.Events.ToList())
                {
                    if (@event.Start > DateTime.UtcNow && upcomingEvents.All(x => string.IsNullOrEmpty(@event.MeetupId) || x.Id != @event.MeetupId.ToString()))
                    {
                        Log.Information($"[{@event.UniqueName}] Removing deleted event.");
                        @group.Events.Remove(@event);
                        eventService.Delete(@event.UniqueName, system);
                    }
                }
            }
            catch (Exception ex)
            {
                Log.Error(ex, "An error occurred while processing the group sync. Message: {0}", ex.Message);
            }

            Log.Information("Processing Complete");
        }
    }
}
