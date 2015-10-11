namespace YorkshireDigital.Data.Tests.Tasks
{
    using System;
    using System.Collections.Generic;
    using System.Linq.Expressions;
    using FakeItEasy;
    using NUnit.Framework;
    using Services;
    using MeetupApi.Helpers;
    using MeetupApi.Models;
    using Messages;
    using Data.Tasks;
    using Data.Domain.Account;

    [TestFixture]
    public class GroupSyncMessageTests
    {
        private IGroupService groupService;
        private IMeetupService meetupService;
        private IEventService eventService;
        private IHangfireService hangfireService;
        private GroupSyncMessage task;

        [SetUp]
        public void SetUp()
        {
            groupService = A.Fake<IGroupService>();
            meetupService = A.Fake<IMeetupService>();
            hangfireService = A.Fake<IHangfireService>();
            eventService = A.Fake<IEventService>();
            var userService = A.Fake<IUserService>();

            A.CallTo(() => userService.GetUser("system"))
                .Returns(new User());

            task = new GroupSyncMessage("test-group", groupService, eventService, userService, hangfireService);
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
            task.Handle(meetupService);

            // Assert
            A.CallTo(() => eventService.Save(A<Data.Domain.Events.Event>.Ignored, A<User>.Ignored)).MustHaveHappened();
            A.CallTo(() => hangfireService.AddOrUpdateJob<EventSyncTask>("test-group-12345", A<Expression<Action<EventSyncTask>>>.Ignored, A<Func<string>>.Ignored)).MustHaveHappened();
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
            task.Handle(meetupService);

            // Assert
            A.CallTo(() => eventService.Save(A<Data.Domain.Events.Event>.Ignored, A<User>.Ignored)).MustNotHaveHappened();
            A.CallTo(() => hangfireService.AddOrUpdateJob<EventSyncTask>("test-group-12345", A<Expression<Action<EventSyncTask>>>.Ignored, A<Func<string>>.Ignored)).MustNotHaveHappened();
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
            task.Handle(meetupService);

            // Assert
            A.CallTo(() => eventService.Save(A<Data.Domain.Events.Event>.Ignored, A<User>.Ignored)).MustHaveHappened();
            A.CallTo(() => hangfireService.AddOrUpdateJob<EventSyncTask>("test-group-12345", A<Expression<Action<EventSyncTask>>>.Ignored, A<Func<string>>.Ignored)).MustHaveHappened();
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
            task.Handle(meetupService);

            // Assert
            A.CallTo(() => eventService.Delete("Test-Event", A<User>.Ignored)).MustHaveHappened();
            A.CallTo(() => hangfireService.AddOrUpdateJob<EventSyncTask>("test-group-12345", A<Expression<Action<EventSyncTask>>>.Ignored, A<Func<string>>.Ignored)).MustNotHaveHappened();
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
            task.Handle(meetupService);

            // Assert
            A.CallTo(() => eventService.Delete("Test-Event", A<User>.Ignored)).MustNotHaveHappened();
            A.CallTo(() => hangfireService.AddOrUpdateJob<EventSyncTask>("test-group-12345", A<Expression<Action<EventSyncTask>>>.Ignored, A<Func<string>>.Ignored)).MustNotHaveHappened();
        }
    }
}
