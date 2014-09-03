namespace YorkshireTec.Tests.ServiceTests
{
    using System;
    using System.Linq;
    using FluentAssertions;
    using NHibernate.Linq;
    using NUnit.Framework;
    using YorkshireTec.Data.Domain.Events;
    using YorkshireTec.Data.Services;

    [TestFixture]
    public class EventServiceTests : InMemoryFixtureBase
    {
        private EventService service;

        [SetUp]
        public void SetUp()
        {
            service = new EventService(Session);
        }

        [Test]
        public void EventService_SaveEvent_SavesEvent()
        {
            // Arrange
            var myEvent = new Event
            {
                Title = string.Format("Test Event {0}", DateTime.Now.ToString("yyyyMMddhhmmssss")),
            };

            // Act
            service.Save(myEvent);
            var events = Session.Query<Event>().Select(x => x);

            // Assert
            events.Count().ShouldBeEquivalentTo(1);
            events.First().Title.ShouldBeEquivalentTo(myEvent.Title);
        }

        [Test]
        public void EventService_SaveEvent_UpdatesEvent()
        {
            // Arrange
            var myEvent = new Event
            {
                Title = string.Format("Test Event {0}", DateTime.Now.ToString("yyyyMMddhhmmssss")),
            };
            Session.Save(myEvent);
            myEvent.Title = string.Format("Updated Event {0}", DateTime.Now.ToString("yyyyMMddhhmmssss"));

            // Act
            service.Save(myEvent);
            var events = Session.Query<Event>().Select(x => x);

            // Assert
            events.Count().ShouldBeEquivalentTo(1);
            events.First().Title.ShouldBeEquivalentTo(myEvent.Title);
        }

        [Test]
        public void EventService_Get_ReturnsEvent()
        {
            // Arrange
            var myEvent = new Event
            {
                Title = string.Format("Test Event {0}", DateTime.Now.ToString("yyyyMMddhhmmssss")),
            };
            Session.Save(myEvent);

            // Act
            var result = service.Get(myEvent.Id);

            // Assert
            result.Title.ShouldBeEquivalentTo(myEvent.Title);
        }

        [Test]
        public void EventService_Delete_RemovesEvent()
        {
            // Arrange
            var myEvent = new Event
            {
                Title = string.Format("Test Event {0}", DateTime.Now.ToString("yyyyMMddhhmmssss")),
            };
            Session.Save(myEvent);

            // Act
            service.Delete(myEvent);
            var events = Session.Query<Event>().Select(x => x);

            // Assert
            events.Count().ShouldBeEquivalentTo(0);
        }

        [Test]
        public void EventService_GetWithinRange_ReturnsExpectedEvents()
        {
            // Arrange
            for (int i = 1; i < 4; i++)
            {
                var event1 = new Event
                {
                    Title = string.Format("Test Event {0} {1}", i, DateTime.Now.ToString("yyyyMMddhhmmssss")),
                    Start = DateTime.Now.AddDays(i * 10)
                };
                Session.Save(event1);
            }

            // Act
            var result = service.GetWithinRange(DateTime.Now, DateTime.Now.AddDays(25));

            // Assert
            result.Count().ShouldBeEquivalentTo(2);
        }

        [Test]
        public void EventService_Query_with_no_constraints_returns_all_events()
        {
            // Arrange
            Session.Save(new Event {Title = "Event 1", Start = DateTime.Now.AddDays(1)});
            Session.Save(new Event {Title = "Event 2", Start = DateTime.Now.AddDays(2)});

            // Act
            var result = service.Query(null, null, new string[0], new string[0], null, null);

            // Assert
            result.Count().ShouldBeEquivalentTo(2);
        }
    }
}
