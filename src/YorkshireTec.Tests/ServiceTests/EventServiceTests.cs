namespace YorkshireTec.Tests.ServiceTests
{
    using System;
    using System.Linq;
    using FluentAssertions;
    using Nancy.Session;
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
    }
}
