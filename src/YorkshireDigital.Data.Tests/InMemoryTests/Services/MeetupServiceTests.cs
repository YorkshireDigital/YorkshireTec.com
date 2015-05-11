namespace YorkshireDigital.Data.Tests.InMemoryTests.Services
{
    using System;
    using System.Collections.Generic;
    using FakeItEasy;
    using FluentAssertions;
    using NUnit.Framework;
    using YorkshireDigital.Data.Services;
    using Domain = YorkshireDigital.Data.Domain;
    using YorkshireDigital.MeetupApi;
    using YorkshireDigital.MeetupApi.Groups;
    using YorkshireDigital.MeetupApi.Groups.Requests;
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
                Groups = A.Fake<IGroupsClient>()
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
    }
}
