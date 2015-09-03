namespace YorkshireDigital.Data.Tests.Tasks
{
    using System;
    using System.Collections.Generic;
    using System.Linq.Expressions;
    using FakeItEasy;
    using NUnit.Framework;
    using YorkshireDigital.Data.Domain.Account;
    using YorkshireDigital.Data.Services;
    using YorkshireDigital.MeetupApi.Helpers;
    using YorkshireDigital.MeetupApi.Models;
    using Hangfire.Tasks;

    [TestFixture]
    public class GroupSyncTaskTests
    {
        private IGroupService groupService;
        private IMeetupService meetupService;
        private IEventService eventService;
        private GroupSyncTask task;

        [SetUp]
        public void SetUp()
        {
            groupService = A.Fake<IGroupService>();
            meetupService = A.Fake<IMeetupService>();
            eventService = A.Fake<IEventService>();
            var userService = A.Fake<IUserService>();

            A.CallTo(() => userService.GetUser("system"))
                .Returns(new User());

            task = new GroupSyncTask(groupService, meetupService, eventService, userService);
        }

        [Test]
        public void Execute_CreatesNewEvent_WhenNewEventFound()
        {
            // Arrange
            A.CallTo(() => groupService.Get("test-group"))
                .Returns(new Data.Domain.Group.Group
                {
                    Id = "test-group",
                    MeetupId = 12345,
                    Events = new List<Data.Domain.Events.Event>()
                });

            A.CallTo(() => meetupService.GetUpcomingEventsForGroup(12345))
                .Returns(new List<Event>
                {
                    new Event
                    {
                        Id = "12345",
                        Name = "New Event",
                        Description = "New details.",
                        Updated = DateHelpers.DateTimeToMeetupTimeStamp(DateTime.UtcNow)
                    }
                });

            // Act
            task.Execute("test-group");

            // Assert
            A.CallTo(() => eventService.Save(A<Data.Domain.Events.Event>.Ignored, A<User>.Ignored)).MustHaveHappened();
            A.CallTo(() => meetupService.AddOrUpdateJob<EventSyncTask>("test-group-12345", A<Expression<Action<EventSyncTask>>>.Ignored, A<Func<string>>.Ignored)).MustHaveHappened();
        }

        [Test]
        public void Execute_DoesNotCreateEvent_WhenEventExistsWithJob()
        {
            // Arrange
            A.CallTo(() => groupService.Get("test-group"))
                .Returns(new Data.Domain.Group.Group
                {
                    Id = "test-group",
                    MeetupId = 12345,
                    Events = new List<Data.Domain.Events.Event>
                    {
                        new Data.Domain.Events.Event
                        {
                            MeetupId = "54321",
                            EventSyncJobId = "12334"
                        }
                    }
                });

            A.CallTo(() => meetupService.GetUpcomingEventsForGroup(12345))
                .Returns(new List<Event>
                {
                    new Event
                    {
                        Id = "54321",
                        Name = "Existing Event",
                        Description = "Existing details.",
                        Updated = DateHelpers.DateTimeToMeetupTimeStamp(DateTime.UtcNow)
                    }
                });

            // Act
            task.Execute("test-group");

            // Assert
            A.CallTo(() => eventService.Save(A<Data.Domain.Events.Event>.Ignored, A<User>.Ignored)).MustNotHaveHappened();
            A.CallTo(() => meetupService.AddOrUpdateJob<EventSyncTask>("test-group-12345", A<Expression<Action<EventSyncTask>>>.Ignored, A<Func<string>>.Ignored)).MustNotHaveHappened();
        }

        [Test]
        public void Execute_CreatesJob_WhenEventExistsWithoutJob()
        {
            // Arrange
            A.CallTo(() => groupService.Get("test-group"))
                .Returns(new Data.Domain.Group.Group
                {
                    Id = "test-group",
                    MeetupId = 12345,
                    Events = new List<Data.Domain.Events.Event>
                    {
                        new Data.Domain.Events.Event
                        {
                            UniqueName = "test-group-12345",
                            MeetupId = "54321"
                        }
                    }
                });

            A.CallTo(() => meetupService.GetUpcomingEventsForGroup(12345))
                .Returns(new List<Event>
                {
                    new Event
                    {
                        Id = "54321",
                        Name = "Existing Event",
                        Description = "Existing details.",
                        Updated = DateHelpers.DateTimeToMeetupTimeStamp(DateTime.UtcNow)
                    }
                });

            // Act
            task.Execute("test-group");

            // Assert
            A.CallTo(() => eventService.Save(A<Data.Domain.Events.Event>.Ignored, A<User>.Ignored)).MustHaveHappened();
            A.CallTo(() => meetupService.AddOrUpdateJob<EventSyncTask>("test-group-12345", A<Expression<Action<EventSyncTask>>>.Ignored, A<Func<string>>.Ignored)).MustHaveHappened();
        }

        [Test]
        public void Execute_DeletesEvent_WhenEventNotFound()
        {
            // Arrange
            A.CallTo(() => groupService.Get("test-group"))
                .Returns(new Data.Domain.Group.Group
                {
                    Id = "test-group",
                    MeetupId = 12345,
                    Events = new List<Data.Domain.Events.Event>
                    {
                        new Data.Domain.Events.Event { UniqueName = "Test-Event", MeetupId = "12345", Start = DateTime.UtcNow.AddHours(1)}
                    }
                });

            A.CallTo(() => meetupService.GetUpcomingEventsForGroup(12345))
                .Returns(new List<Event>
                {});

            // Act
            task.Execute("test-group");

            // Assert
            A.CallTo(() => eventService.Delete("Test-Event", A<User>.Ignored)).MustHaveHappened();
            A.CallTo(() => meetupService.AddOrUpdateJob<EventSyncTask>("test-group-12345", A<Expression<Action<EventSyncTask>>>.Ignored, A<Func<string>>.Ignored)).MustNotHaveHappened();
        }

        [Test]
        public void Execute_DoesNotDeleteEvent_WhenEventIsInThePast()
        {
            // Arrange
            A.CallTo(() => groupService.Get("test-group"))
                .Returns(new Data.Domain.Group.Group
                {
                    Id = "test-group",
                    MeetupId = 12345,
                    Events = new List<Data.Domain.Events.Event>
                    {
                        new Data.Domain.Events.Event { UniqueName = "Test-Event", MeetupId = "12345", Start = DateTime.UtcNow.AddHours(-1)}
                    }
                });

            A.CallTo(() => meetupService.GetUpcomingEventsForGroup(12345))
                .Returns(new List<Event> { });

            // Act
            task.Execute("test-group");

            // Assert
            A.CallTo(() => eventService.Delete("Test-Event", A<User>.Ignored)).MustNotHaveHappened();
            A.CallTo(() => meetupService.AddOrUpdateJob<EventSyncTask>("test-group-12345", A<Expression<Action<EventSyncTask>>>.Ignored, A<Func<string>>.Ignored)).MustNotHaveHappened();
        }
    }
}
