namespace YorkshireTec.Tests.ServiceTests
{
    using System;
    using System.Collections.Generic;
    using System.Data.Entity.ModelConfiguration.Configuration;
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

        [Test]
        public void EventService_Query_with_filtered_from_returns_filtered_events()
        {
            // Arrange
            Session.Save(new Event { Title = "Event 1", Start = DateTime.Today.AddDays(1) });
            Session.Save(new Event { Title = "Event 2", Start = DateTime.Today.AddDays(2) });

            // Act
            var result = service.Query(DateTime.Now.AddDays(1), null, new string[0], new string[0], null, null);

            // Assert
            result.Count().ShouldBeEquivalentTo(1);
            result[0].Title.ShouldAllBeEquivalentTo("Event 2");
        }

        [Test]
        public void EventService_Query_with_filtered_to_returns_filtered_events()
        {
            // Arrange
            Session.Save(new Event { Title = "Event 1", Start = DateTime.Today.AddDays(1) });
            Session.Save(new Event { Title = "Event 2", Start = DateTime.Today.AddDays(2) });

            // Act
            var result = service.Query(null, DateTime.Now.AddDays(1), new string[0], new string[0], null, null);

            // Assert
            result.Count().ShouldBeEquivalentTo(1);
            result[0].Title.ShouldAllBeEquivalentTo("Event 1");
        }

        [TestCase("Development", 1, 3)]
        [TestCase("Design", 2, 3)]
        public void EventService_Query_with_filtered_interests_returns_filtered_events(string interest, int expectedId1, int expectedId2)
        {
            // Arrange
            Session.Save(new Event
            {
                Id = 1,
                Interests = new List<Interest> {new Interest {Name = "Development"}}
            });
            Session.Save(new Event
            {
                Id = 2,
                Interests = new List<Interest> {new Interest {Name = "Design"}}
            });
            Session.Save(new Event
            {
                Id = 3,
                Interests = new List<Interest> { new Interest { Name = "Development" }, new Interest { Name = "Design" } }
            });

            // Act
            var result = service.Query(null, null, new [] { interest }, new string[0], null, null);

            // Assert
            result.Count().ShouldBeEquivalentTo(2);
            result[0].Id.ShouldBeEquivalentTo(expectedId1);
            result[1].Id.ShouldBeEquivalentTo(expectedId2);
        }

        [Test]
        public void EventService_Query_with_multiple_interests_returns_all_events_containing_one_or_more()
        {
            // Arrange
            Session.Save(new Event
            {
                Id = 1,
                Interests = new List<Interest> { new Interest { Name = "Development" } }
            });
            Session.Save(new Event
            {
                Id = 2,
                Interests = new List<Interest> { new Interest { Name = "Business" } }
            });
            Session.Save(new Event
            {
                Id = 3,
                Interests = new List<Interest> { new Interest { Name = "Development" }, new Interest { Name = "Design" } }
            });

            // Act
            var result = service.Query(null, null, new[] { "Development", "Design" }, new string[0], null, null);

            // Assert
            result.Count().ShouldBeEquivalentTo(2);
            result[0].Id.ShouldBeEquivalentTo(1);
            result[1].Id.ShouldBeEquivalentTo(3);
        }

        [TestCase("Leeds", 1)]
        [TestCase("Sheffield", 2)]
        public void EventService_Query_with_filtered_location_returns_filtered_events(string location, int expectedId)
        {
            // Arrange
            Session.Save(new Event
            {
                Id = 1,
                Location = "Leeds"
            });
            Session.Save(new Event
            {
                Id = 2,
                Location = "Sheffield"
            });

            // Act
            var result = service.Query(null, null, new string[0], new [] { location }, null, null);

            // Assert
            result.Count().ShouldBeEquivalentTo(1);
            result[0].Id.ShouldBeEquivalentTo(expectedId);
        }

        [Test]
        public void EventService_Query_with_multiple_locations_returns_all_locations()
        {
            // Arrange
            Session.Save(new Event
            {
                Id = 1,
                Location = "Leeds"
            });
            Session.Save(new Event
            {
                Id = 2,
                Location = "Sheffield"
            });
            Session.Save(new Event
            {
                Id = 3,
                Location = "Hull"
            });

            // Act
            var result = service.Query(null, null, new string[0], new[] { "Leeds", "Hull" }, null, null);

            // Assert
            result.Count().ShouldBeEquivalentTo(2);
            result[0].Id.ShouldBeEquivalentTo(1);
            result[1].Id.ShouldBeEquivalentTo(3);
        }
    }
}
