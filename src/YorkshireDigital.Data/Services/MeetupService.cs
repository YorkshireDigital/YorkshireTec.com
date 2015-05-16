﻿namespace YorkshireDigital.Data.Services
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Hangfire;
    using YorkshireDigital.MeetupApi.Clients;
    using YorkshireDigital.MeetupApi.Models;
    using YorkshireDigital.MeetupApi.Requests;
    using YorkshireDigital.MeetupApi.Requests.Enum;
    using Event = YorkshireDigital.MeetupApi.Models.Event;
    using System.Linq.Expressions;

    public interface IMeetupService
    {
        bool GroupExists(string groupName);
        Group GetGroup(string groupName);
        void LinkGroup(Domain.Group.Group @group, string groupName);
        List<Event> GetUpcomingEventsForGroup(int groupId);
        Event GetEvent(string eventId);
        void RemoveJobIfExists(string jobId);
        void AddOrUpdateJob(string recurringJobId, Expression<Action> methodCall, Func<string> cronExpression);
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

        public void RemoveJobIfExists(string jobId)
        {
            RecurringJob.RemoveIfExists(jobId);
        }

        public void AddOrUpdateJob(string recurringJobId, Expression<Action> methodCall, Func<string> cronExpression)
        {
            RecurringJob.AddOrUpdate(recurringJobId, methodCall, cronExpression);
        }
    }
}
