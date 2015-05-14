namespace YorkshireDigital.Data.Tests.InMemoryTests.Services
{
    using System;
    using System.Collections.Generic;
    using FakeItEasy;
    using FluentAssertions;
    using NUnit.Framework;
    using YorkshireDigital.Data.Services;
    using YorkshireDigital.MeetupApi.Clients;
    using YorkshireDigital.MeetupApi.Requests;
    using Domain = YorkshireDigital.Data.Domain;
    using YorkshireDigital.MeetupApi.Models;

    [TestFixture]
    public class MeetupServiceTests : InMemoryFixtureBase
    {
        private MeetupClient meetupClient;
        private MeetupService service;

        [SetUp]
        public void Setup()
        {
            meetupClient = new MeetupClient("test")
            {
                Groups = A.Fake<IGroupsClient>(),
                Events = A.Fake<IEventsClient>()
            };
            service = new MeetupService(meetupClient);
        }

        [Test]
        public void GroupExists_WhenGroupExists_ReturnsTrue()
        {
            // Arrange
            A.CallTo(() => meetupClient.Groups.Get(A<GroupsRequest>.Ignored))
                .Returns(new ApiResponse<List<Group>>
                {
                    Results = new List<Group>
                    {
                        new Group{ Id = 1, Name = "Test Group", UrlName = "test-group"}
                    }
                });

            // Act
            var result = service.GroupExists("test-group");

            // Assert
            result.Should().BeTrue();
        }

        [Test]
        public void GroupExists_WhenGroupDoesNotExists_ReturnsFalse()
        {
            // Arrange
            A.CallTo(() => meetupClient.Groups.Get(A<GroupsRequest>.Ignored))
                .Returns(new ApiResponse<List<Group>>
                {
                    Results = new List<Group>()
                });

            // Act
            var result = service.GroupExists("test-group");

            // Assert
            result.Should().BeFalse();
        }

        [Test]
        public void GetGroup_WhenGroupExists_ReturnsGroupModel()
        {
            // Arrange
            A.CallTo(() => meetupClient.Groups.Get(A<GroupsRequest>.Ignored))
                .Returns(new ApiResponse<List<Group>>
                {
                    Results = new List<Group>
                    {
                        new Group{ Id = 1, Name = "Test Group", UrlName = "test-group"}
                    }
                });

            // Act
            var result = service.GetGroup("test-group");

            // Assert
            result.Name.ShouldBeEquivalentTo("Test Group");
            result.Id.ShouldBeEquivalentTo(1);
            result.UrlName.ShouldBeEquivalentTo("test-group");
        }

        [Test]
        public void GetGroup_WhenGroupDoesNotExists_ReturnsNull()
        {
            // Arrange
            A.CallTo(() => meetupClient.Groups.Get(A<GroupsRequest>.Ignored))
                .Returns(new ApiResponse<List<Group>>
                {
                    Results = new List<Group>()
                });

            // Act
            var result = service.GetGroup("test-group");

            // Assert
            result.Should().BeNull();
        }

        [Test]
        [ExpectedException(typeof(InvalidOperationException), ExpectedMessage = "Sequence contains more than one element")]
        public void GetGroup_WhenMultipleGroupsExists_ThrowsException()
        {
            // Arrange
            A.CallTo(() => meetupClient.Groups.Get(A<GroupsRequest>.Ignored))
                .Returns(new ApiResponse<List<Group>>
                {
                    Results = new List<Group>
                    {
                        new Group{ Id = 1, Name = "Test Group", UrlName = "test-group"},
                        new Group{ Id = 1, Name = "Test Group", UrlName = "test-group"}
                    }
                });

            // Act
            service.GetGroup("test-group");

            // Assert
        }

        [Test]
        public void LinkGroup_WhenGroupExists_AddsMeetupInformationToGroup()
        {
            // Arrange
            A.CallTo(() => meetupClient.Groups.Get(A<GroupsRequest>.Ignored))
                .Returns(new ApiResponse<List<Group>>
                {
                    Results = new List<Group>
                    {
                        new Group{ Id = 12345, Name = "Test Group", UrlName = "test-group"}
                    }
                });
            var group = new Domain.Group.Group
            {
                Id = "test",
                Name = "Test Group",
            };
            Session.Save(group);
            
            // Act
            service.LinkGroup(group, "test-group");
            var result = Session.Get<Domain.Group.Group>("test");

            // Assert
            result.MeetupId.ShouldBeEquivalentTo(12345);
            // TODO : Check that the task has been created
            // result.NewEventSyncTaskId.Should().BeGreaterThan(0);
            // A.CallTo(() => client.Create(A<Job>.That.Matches(x => job.Class.Name == "NewEventSyncTask" 
            //                                                    && job.Method.Name == "Execute" 
            //                                                    && job.Arguments[0] == "test" 
            //                                                    && job.Arguments[1] == 12345)))
            //      .MustHaveHappened();
        }

        [Test]
        public void GetUpcomingEventsForGroup_ReturnsEvents()
        {
            // Arrange
            A.CallTo(() => meetupClient.Events.Get(A<EventsRequest>.Ignored))
                .Returns(new ApiResponse<List<Event>>
                {
                    Results = new List<Event>
                    {
                        new Event{ Name = "Test Event", Id = "test-event"}
                    }
                });

            // Act
            var result = service.GetUpcomingEventsForGroup(12345);

            // Assert
            result.Count.ShouldBeEquivalentTo(1);
            result[0].Name.ShouldBeEquivalentTo("Test Event");
            result[0].Id.ShouldBeEquivalentTo("test-event");
        }

        [Test]
        public void GetEvent_WhenEventExists_ReturnsEvent()
        {
            // Arrange
            A.CallTo(() => meetupClient.Events.Get(A<EventsRequest>.Ignored))
                .Returns(new ApiResponse<List<Event>>
                {
                    Results = new List<Event>
                    {
                        new Event{ Name = "Test Event", Id = "test-event"}
                    }
                });

            // Act
            var result = service.GetEvent(12345);

            // Assert
            result.Name.ShouldBeEquivalentTo("Test Event");
            result.Id.ShouldBeEquivalentTo("test-event");
        }

        [Test]
        public void GetEvent_WhenNoEventExists_ReturnsNull()
        {
            // Arrange
            A.CallTo(() => meetupClient.Events.Get(A<EventsRequest>.Ignored))
                .Returns(new ApiResponse<List<Event>>
                {
                    Results = new List<Event>()
                });

            // Act
            var result = service.GetEvent(12345);

            // Assert
            result.Should().BeNull();
        }

        [Test]
        [ExpectedException(typeof(InvalidOperationException), ExpectedMessage = "Sequence contains more than one element")]
        public void GetEvent_WhenMultipleEventExists_ReturnsNull()
        {
            // Arrange
            A.CallTo(() => meetupClient.Events.Get(A<EventsRequest>.Ignored))
                .Returns(new ApiResponse<List<Event>>
                {
                    Results = new List<Event>
                    {
                        new Event{ Name = "Test Event", Id = "test-event"},
                        new Event{ Name = "Test Event", Id = "test-event-1"}
                    }
                });

            // Act
            var result = service.GetEvent(12345);

            // Assert
            result.Should().BeNull();
        }

        [Test]
        public void SyncEvents_NewEventsFound_CreatesNewEvent()
        {
            // Arrange
            A.CallTo(() => meetupClient.Events.Get(A<EventsRequest>.Ignored))
                .Returns(new ApiResponse<List<Event>>
                {
                    Results = new List<Event>
                    {
                        new Event
                        {
                            Name = "Test Event",
                            Id = "test-event",
                            Group =
                                new Group
                                {
                                    Name = "Test Group",
                                    Category = new Category {Name = "Tech"},
                                    Topics = new List<Topic> {new Topic {Name = "Dev"}}
                                },
                            Description = "Test Description",
                            Duration = 123,
                            Created = 1420727597000,
                            Venue = new Venue
                            {
                                Address1 = "Test Address",
                                City = "Leeds"
                            }

                        },
                        new Event {Name = "Test Event", Id = "test-event-1"}
                    }
                });

            var group = new Domain.Group.Group
            {
                Id = "test",
                Name = "Test Group",
                Events = new List<Domain.Events.Event>
                {
                    new Domain.Events.Event { UniqueName = "test-event-1", MeetupId = "test-event-1"}
                }
            };
            Session.Save(group);

            // Act
            service.SyncEvents(group);

            // Assert
            group.Events.Count.ShouldBeEquivalentTo(2);
        }

        [Test]
        public void SyncEvents_NoNewEvents_NotUpdated()
        {
            // Arrange
            A.CallTo(() => meetupClient.Events.Get(A<EventsRequest>.Ignored))
                .Returns(new ApiResponse<List<Event>>
                {
                    Results = new List<Event>
                    {
                        new Event{ Name = "Test Event", Id = "test-event"}
                    }
                });

            var group = new Domain.Group.Group
            {
                Id = "test",
                Name = "Test Group",
                Events = new List<Domain.Events.Event>
                {
                    new Domain.Events.Event { UniqueName = "test-event", MeetupId = "test-event"}
                }
            };
            Session.Save(group);

            // Act
            service.SyncEvents(group);

            // Assert
            group.Events.Count.ShouldBeEquivalentTo(1);
        }

        [Test]
        public void SyncEvents_EventsRemoved_DeleteEvent()
        {
            // Arrange
            A.CallTo(() => meetupClient.Events.Get(A<EventsRequest>.Ignored))
                .Returns(new ApiResponse<List<Event>>
                {
                    Results = new List<Event>
                    {
                        new Event{ Name = "Test Event", Id = "test-event"}
                    }
                });

            var group = new Domain.Group.Group
            {
                Id = "test",
                Name = "Test Group",
                Events = new List<Domain.Events.Event>
                {
                    new Domain.Events.Event { UniqueName = "test-event", MeetupId = "test-event", Start = DateTime.UtcNow.AddDays(2)},
                    new Domain.Events.Event { UniqueName = "test-event-1", MeetupId = "test-event-1", Start = DateTime.UtcNow.AddDays(2)}
                }
            };
            Session.Save(group);

            // Act
            service.SyncEvents(group);

            // Assert
            group.Events.Count.ShouldBeEquivalentTo(1);
            group.Events[0].MeetupId.ShouldBeEquivalentTo("test-event");
        }
    }
}
