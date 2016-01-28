using NHibernate;

namespace YorkshireDigital.Data.Tests.Tasks
{
    using System;
    using FakeItEasy;
    using NUnit.Framework;
    using Services;
    using MeetupApi.Helpers;
    using Messages;
    using Data.Domain.Account;
    using Data.Domain.Events;

    [TestFixture]
    public class EventSyncMessageTests
    {
        private IEventService eventService;
        private IMeetupService meetupService;
        private IHangfireService hangfireService;
        private EventSyncMessage task;
        private ISession session;

        [SetUp]
        public void SetUp()
        {
            eventService = A.Fake<IEventService>();
            meetupService = A.Fake<IMeetupService>();
            hangfireService = A.Fake<IHangfireService>();
            session = A.Fake<ISession>();
            var userService = A.Fake<IUserService>();
            

            A.CallTo(() => userService.GetUser("system"))
                .Returns(new User());

            task = new EventSyncMessage("event-123", eventService, userService, hangfireService);
        }

        [Test]
        public void Execute_UpdatesEvent_WhenEventHasBeenUpdated()
        {
            // Arrange
            A.CallTo(() => eventService.Get("event-123"))
                .Returns(new Event
                {
                    Title = "Test Event",
                    Synopsis = "Initial details...",
                    LastEditedOn = DateTime.Now.AddHours(-1),
                    MeetupId = "12345",
                    End = DateTime.UtcNow.AddDays(1)
                });
            A.CallTo(() => meetupService.GetEvent("12345"))
                .Returns(new MeetupApi.Models.Event
                {
                    Name = "Updated Name",
                    Description = "Updated details.",
                    Updated = DateHelpers.DateTimeToMeetupTimeStamp(DateTime.Now)
                });

            // Act
            task.Handle(session, meetupService);

            // Assert
            A.CallTo(() => eventService.Save(A<Event>.Ignored, A<User>.Ignored)).MustHaveHappened();
        }

        [Test]
        public void Execute_DoesNotUpdateEvent_WhenEventHasNotBeenUpdated()
        {
            // Arrange
            A.CallTo(() => eventService.Get("event-123"))
                .Returns(new Event
                {
                    Title = "Test Event",
                    Synopsis = "Initial details...",
                    LastEditedOn = DateTime.UtcNow,
                    MeetupId = "12345",
                    End = DateTime.UtcNow.AddDays(1)
                });
            A.CallTo(() => meetupService.GetEvent("12345"))
                .Returns(new MeetupApi.Models.Event
                {
                    Name = "Updated Name",
                    Description = "Updated details.",
                    Updated = DateHelpers.DateTimeToMeetupTimeStamp(DateTime.UtcNow.AddHours(-1))
                });

            // Act
            task.Handle(session, meetupService);

            // Assert
            A.CallTo(() => eventService.Save(A<Event>.Ignored, A<User>.Ignored)).MustNotHaveHappened();
        }

        [Test]
        public void Execute_DeletedEvent_WhenEventDoesNotExist()
        {
            // Arrange
            A.CallTo(() => eventService.Get("event-123"))
                .Returns(new Event
                {
                    Title = "Test Event",
                    Synopsis = "Initial details...",
                    LastEditedOn = DateTime.Now,
                    MeetupId = "12345",
                    End = DateTime.UtcNow.AddDays(1)
                });
            A.CallTo(() => meetupService.GetEvent("12345"))
                .Returns(null);

            // Act
            task.Handle(session, meetupService);

            // Assert
            A.CallTo(() => eventService.Delete("event-123", A<User>.Ignored)).MustHaveHappened();
            A.CallTo(() => eventService.Save(A<Event>.Ignored, A<User>.Ignored)).MustNotHaveHappened();
        }

        [Test]
        public void Execute_CancelsTask_WhenEventHasEnded()
        {
            // Arrange
            A.CallTo(() => eventService.Get("event-123"))
                .Returns(new Event { Title = "Test Event", Synopsis = "Initial details...", LastEditedOn = DateTime.Now, EventSyncJobId = "54321", End = DateTime.Now.AddHours(-1)});

            // Act
            task.Handle(session, meetupService);

            // Assert
            A.CallTo(() => hangfireService.RemoveJobIfExists("54321")).MustHaveHappened();
        }
    }
}
