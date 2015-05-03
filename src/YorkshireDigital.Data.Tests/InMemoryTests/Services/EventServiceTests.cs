namespace YorkshireDigital.Data.Tests.InMemoryTests.Services
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using FluentAssertions;
    using NUnit.Framework;
    using YorkshireDigital.Data.Domain.Account;
    using YorkshireDigital.Data.Domain.Events;
    using YorkshireDigital.Data.Domain.Organisations;
    using YorkshireDigital.Data.Exceptions;
    using YorkshireDigital.Data.Services;

    [TestFixture]
    public class EventServiceTests : InMemoryFixtureBase
    {
        private IEventService service;

        [SetUp]
        public void SetUp()
        {
            service = new EventService(Session);
        }

        [Test]
        public void SaveEvent_SavesEvent()
        {
            // Arrange
            var myEvent = new Event
            {
                UniqueName = "1",
                Title = string.Format("Test Event {0}", DateTime.Now.ToString("yyyyMMddhhmmssss")),
                Interests = new[] { new Interest { Name = "Development" }, new Interest { Name = "Design" } },
                Talks = new []{ new EventTalk { Id = 1, Title = "Super talk", } }
            };
            var user = new User
            {
                Id = Guid.NewGuid(),
                Username = "test-user"
            };
            Session.Save(user);
            var saveStart = DateTime.UtcNow;

            // Act
            service.Save(myEvent, user);
            var result = Session.Get<Event>("1");

            // Assert
            result.Title.ShouldBeEquivalentTo(myEvent.Title);
            result.UniqueName.ShouldBeEquivalentTo("1");
            result.LastEditedOn.Should().BeOnOrAfter(saveStart);
            result.LastEditedBy.ShouldBeEquivalentTo(user);
            result.Interests.Count.ShouldBeEquivalentTo(2);
            result.Interests[0].Name.ShouldBeEquivalentTo("Development");
            result.Interests[1].Name.ShouldBeEquivalentTo("Design");
            result.Talks.Count.ShouldBeEquivalentTo(1);
            result.Talks[0].Id.ShouldBeEquivalentTo(1);
            result.Talks[0].Title.ShouldBeEquivalentTo("Super talk");
        }

        [Test]
        public void SaveEvent_UpdatesEvent()
        {
            // Arrange
            var myEvent = new Event
            {
                UniqueName = "1",
                Title = string.Format("Test Event {0}", DateTime.Now.ToString("yyyyMMddhhmmssss")),
                LastEditedOn = DateTime.UtcNow.AddDays(-1),
                LastEditedBy = null,
                Interests = new[] { new Interest { Name = "Development" } }
            };
            Session.Save(myEvent);
            var user = new User
            {
                Id = Guid.NewGuid(),
                Username = "test-user"
            };
            Session.Save(user);
            myEvent.Title = string.Format("Updated Event {0}", DateTime.Now.ToString("yyyyMMddhhmmssss"));
            myEvent.Interests = new[] {new Interest {Name = "Design"}};
            myEvent.Interests = new[] {new Interest {Name = "Design"}};
            var saveStart = DateTime.UtcNow;

            // Act
            service.Save(myEvent, user);
            var result = Session.Get<Event>("1");

            // Assert
            result.Title.ShouldBeEquivalentTo(myEvent.Title);
            result.UniqueName.ShouldBeEquivalentTo("1");
            result.LastEditedOn.Should().BeOnOrAfter(saveStart);
            result.LastEditedBy.ShouldBeEquivalentTo(user);
            result.Interests.Count.ShouldBeEquivalentTo(1);
            result.Interests[0].Name.ShouldBeEquivalentTo("Design");
        }

        [Test]
        public void Get_ReturnsEvent()
        {
            // Arrange
            var myEvent = new Event
            {
                UniqueName = "1",
                Title = string.Format("Test Event {0}", DateTime.Now.ToString("yyyyMMddhhmmssss")),
            };
            Session.Save(myEvent);

            // Act
            var result = service.Get(myEvent.UniqueName);

            // Assert
            result.Title.ShouldBeEquivalentTo(myEvent.Title);
        }

        [Test]
        public void Delete_MarksEventAsDeleted_WhenEventExists()
        {
            // Arrange
            var user = new User
            {
                Id = Guid.NewGuid(),
                Username = "test-user"
            };
            Session.Save(user);
            var myEvent = new Event
            {
                UniqueName = "1",
                Title = string.Format("Test Event {0}", DateTime.Now.ToString("yyyyMMddhhmmssss")),
            };
            Session.Save(myEvent);

            // Act
            service.Delete("1", user);
            var result = Session.Get<Event>("1");

            // Assert
            result.IsDeleted.Should().BeTrue();
            result.DeletedBy.ShouldBeEquivalentTo(user);
        }

        [Test]
        [ExpectedException(typeof(EventNotFoundException), ExpectedMessage = "No event found with unique name 1")]
        public void Delete_ThrowException_WhenNoEventExists()
        {
            // Arrange

            // Act
            service.Delete("1", null);

            // Assert
        }

        [Test]
        public void GetWithinRange_ReturnsExpectedEvents()
        {
            // Arrange
            for (int i = 1; i < 4; i++)
            {
                var event1 = new Event
                {
                    UniqueName = i.ToString(),
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
        public void Query_WithNoConstraints_ReturnsAllEvents()
        {
            // Arrange
            Session.Save(new Event
            {
                UniqueName = "1",
                Title = "Event 1",
                Start = DateTime.Now.AddDays(1)
            });
            Session.Save(new Event
            {
                UniqueName = "2",
                Title = "Event 2",
                Start = DateTime.Now.AddDays(2)
            });

            // Act
            var result = service.Query(null, null, new string[0], new string[0], null, null);

            // Assert
            result.Count().ShouldBeEquivalentTo(2);
        }

        [Test]
        public void Query_WithNoConstraints_DoesNotIncludeDeleted()
        {
            // Arrange
            Session.Save(new Event
            {
                UniqueName = "1",
                Title = "Event 1",
                Start = DateTime.Now.AddDays(1),
                DeletedOn = DateTime.Now.AddDays(1)
            });
            Session.Save(new Event
            {
                UniqueName = "2",
                Title = "Event 2",
                Start = DateTime.Now.AddDays(2)
            });

            // Act
            var result = service.Query(null, null, new string[0], new string[0], null, null);

            // Assert
            result.Count().ShouldBeEquivalentTo(1);
        }

        [Test]
        public void Query_WithNoConstraints_DoesNotIncludeEventsAfterGroupWasDeleted()
        {
            // Arrange
            Session.Save(new Event
            {
                UniqueName = "0",
                Title = "Event 0",
                Start = DateTime.Now.AddDays(-1),
                Group = new Group
                {
                    Id = "test-group-1",
                    DeletedOn = DateTime.Now
                }
            });
            Session.Save(new Event
            {
                UniqueName = "1",
                Title = "Event 1",
                Start = DateTime.Now.AddDays(1),
                Group = new Group
                {
                    Id = "test-group-2",
                    DeletedOn = DateTime.Now
                }
            });
            Session.Save(new Event
            {
                UniqueName = "2",
                Title = "Event 2",
                Start = DateTime.Now.AddDays(2)
            });

            // Act
            var result = service.Query(null, null, new string[0], new string[0], null, null);

            // Assert
            result.Count().ShouldBeEquivalentTo(2);
            result[0].Title.ShouldBeEquivalentTo("Event 0");
            result[1].Title.ShouldBeEquivalentTo("Event 2");
        }

        [Test]
        public void Query_WithFilteredFrom_ReturnsFilteredEvents()
        {
            // Arrange
            Session.Save(new Event
            {
                UniqueName = "1",
                Title = "Event 1",
                Start = DateTime.Today.AddDays(1)
            });
            Session.Save(new Event
            {
                UniqueName = "2",
                Title = "Event 2",
                Start = DateTime.Today.AddDays(2)
            });

            // Act
            var result = service.Query(DateTime.Now.AddDays(1), null, new string[0], new string[0], null, null);

            // Assert
            result.Count().ShouldBeEquivalentTo(1);
            result[0].Title.ShouldAllBeEquivalentTo("Event 2");
        }

        [Test]
        public void Query_WithFilteredTo_ReturnsFilteredEvents()
        {
            // Arrange
            Session.Save(new Event
            {
                UniqueName = "1",
                Title = "Event 1",
                Start = DateTime.Today.AddDays(1)
            });
            Session.Save(new Event
            {
                UniqueName = "2",
                Title = "Event 2",
                Start = DateTime.Today.AddDays(2)
            });

            // Act
            var result = service.Query(null, DateTime.Now.AddDays(1), new string[0], new string[0], null, null);

            // Assert
            result.Count().ShouldBeEquivalentTo(1);
            result[0].Title.ShouldAllBeEquivalentTo("Event 1");
        }

        [TestCase("Development", "1", "3")]
        [TestCase("Design", "2", "3")]
        public void Query_WithFilteredInterests_ReturnsFilteredEvents(string interest, string expectedId1, string expectedId2)
        {
            // Arrange
            Session.Save(new Event
            {
                UniqueName = "1",
                Interests = new List<Interest> {new Interest {Name = "Development"}}
            });
            Session.Save(new Event
            {
                UniqueName = "2",
                Interests = new List<Interest> {new Interest {Name = "Design"}}
            });
            Session.Save(new Event
            {
                UniqueName = "3",
                Interests = new List<Interest> { new Interest { Name = "Development" }, new Interest { Name = "Design" } }
            });

            // Act
            var result = service.Query(null, null, new [] { interest }, new string[0], null, null);

            // Assert
            result.Count().ShouldBeEquivalentTo(2);
            result[0].UniqueName.ShouldBeEquivalentTo(expectedId1);
            result[1].UniqueName.ShouldBeEquivalentTo(expectedId2);
        }

        [Test]
        public void Query_WithMultipleInterests_ReturnsAllEventsContainingOneOrMore()
        {
            // Arrange
            Session.Save(new Event
            {
                UniqueName = "1",
                Interests = new List<Interest> { new Interest { Name = "Development" } }
            });
            Session.Save(new Event
            {
                UniqueName = "2",
                Interests = new List<Interest> { new Interest { Name = "Business" } }
            });
            Session.Save(new Event
            {
                UniqueName = "3",
                Interests = new List<Interest> { new Interest { Name = "Development" }, new Interest { Name = "Design" } }
            });

            // Act
            var result = service.Query(null, null, new[] { "Development", "Design" }, new string[0], null, null);

            // Assert
            result.Count().ShouldBeEquivalentTo(2);
            result[0].UniqueName.ShouldBeEquivalentTo("1");
            result[1].UniqueName.ShouldBeEquivalentTo("3");
        }

        [TestCase("Leeds", "1")]
        [TestCase("Sheffield", "2")]
        public void Query_WithFilteredLocation_ReturnsFilteredEvents(string location, string expectedId)
        {
            // Arrange
            Session.Save(new Event
            {
                UniqueName = "1",
                Location = "Leeds"
            });
            Session.Save(new Event
            {
                UniqueName = "2",
                Location = "Sheffield"
            });

            // Act
            var result = service.Query(null, null, new string[0], new [] { location }, null, null);

            // Assert
            result.Count().ShouldBeEquivalentTo(1);
            result[0].UniqueName.ShouldBeEquivalentTo(expectedId);
        }

        [Test]
        public void Query_WithMultipleLocations_ReturnsAllLocations()
        {
            // Arrange
            Session.Save(new Event
            {
                UniqueName = "1",
                Location = "Leeds"
            });
            Session.Save(new Event
            {
                UniqueName = "2",
                Location = "Sheffield"
            });
            Session.Save(new Event
            {
                UniqueName = "3",
                Location = "Hull"
            });

            // Act
            var result = service.Query(null, null, new string[0], new[] { "Leeds", "Hull" }, null, null);

            // Assert
            result.Count().ShouldBeEquivalentTo(2);
            result[0].UniqueName.ShouldBeEquivalentTo("1");
            result[1].UniqueName.ShouldBeEquivalentTo("3");
        }

        [Test]
        public void Query_WithTakeOnly_ReturnsSpecifiedAmount()
        {
            // Arrange
            for (int i = 0; i < 100; i++)
            {
                Session.Save(new Event { UniqueName = i.ToString() });
            }

            // Act
            var result = service.Query(null, null, new string[0], new string[0], null, 25);

            // Assert
            result.Count().ShouldBeEquivalentTo(25);
        }

        [Test]
        public void Query_WithTakeOnly_ReturnsAllIfLessThanTakeValue()
        {
            // Arrange
            for (int i = 0; i < 10; i++)
            {
                Session.Save(new Event { UniqueName = i.ToString() });
            }

            // Act
            var result = service.Query(null, null, new string[0], new string[0], null, 25);

            // Assert
            result.Count().ShouldBeEquivalentTo(10);
        }

        [Test]
        public void Query_WithSkipOnly_ReturnsEventsAfterSkip()
        {
            // Arrange
            for (int i = 0; i < 100; i++)
            {
                Session.Save(new Event { UniqueName = i.ToString() });
            }

            // Act
            var result = service.Query(null, null, new string[0], new string[0], 25, null);

            // Assert
            result.Count().ShouldBeEquivalentTo(75);
            result[0].UniqueName.ShouldBeEquivalentTo("25");
        }

        [Test]
        public void Query_DoesNotIncludeDeletedEvents_ByDefault()
        {
            // Arrange
            Session.Save(new Event { UniqueName = "1" });
            Session.Save(new Event { UniqueName = "2", DeletedOn = DateTime.UtcNow});
            Session.Save(new Event { UniqueName = "3" });

            // Act
            var result = service.Query(null, null, new string[0], new string[0], null, null);

            // Assert
            result.Count().ShouldBeEquivalentTo(2);
            result[0].UniqueName.ShouldBeEquivalentTo("1");
            result[1].UniqueName.ShouldBeEquivalentTo("3");
        }

        [Test]
        public void Query_DoesIncludeDeletedEvents_WhenSpecified()
        {
            // Arrange
            Session.Save(new Event { UniqueName = "1" });
            Session.Save(new Event { UniqueName = "2", DeletedOn = DateTime.UtcNow });
            Session.Save(new Event { UniqueName = "3" });

            // Act
            var result = service.Query(null, null, new string[0], new string[0], null, null, true);

            // Assert
            result.Count().ShouldBeEquivalentTo(3);
            result[0].UniqueName.ShouldBeEquivalentTo("1");
            result[1].UniqueName.ShouldBeEquivalentTo("2");
            result[2].UniqueName.ShouldBeEquivalentTo("3");
        }

        [Test]
        public void GetInterests_ReturnsAllInterests()
        {
            // Arrange
            var interest1 = new Interest
            {
                Id = 1,
                Name = "Development"
            };
            var interest2 = new Interest
            {
                Id = 1,
                Name = "Design"
            };
            Session.Save(interest1);
            Session.Save(interest2);

            // Act
            var interests = service.GetInterests();

            // Assert
            interests.Count.ShouldBeEquivalentTo(2);
            interests[0].Name.ShouldBeEquivalentTo("Development");
            interests[1].Name.ShouldBeEquivalentTo("Design");
        }

        [Test]
        public void EventExists_IfEventExists_ReturnsTrue()
        {
            // Arrange
            Session.Save(new Event { UniqueName = "1" });

            // Act
            var result = service.EventExists("1");

            // Assert
            result.Should().BeTrue();
        }

        [Test]
        public void EventExists_IfEventNotExists_ReturnsFalse()
        {
            // Arrange

            // Act
            var result = service.EventExists("1");

            // Assert
            result.Should().BeFalse();
        }
    }
}
