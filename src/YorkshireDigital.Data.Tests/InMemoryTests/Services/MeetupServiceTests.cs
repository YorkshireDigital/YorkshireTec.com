namespace YorkshireDigital.Data.Tests.InMemoryTests.Services
{
    using System;
    using System.Collections.Generic;
    using FakeItEasy;
    using FluentAssertions;
    using NUnit.Framework;
    using YorkshireDigital.Data.Services;
    using YorkshireDigital.MeetupApi;
    using YorkshireDigital.MeetupApi.Groups;
    using YorkshireDigital.MeetupApi.Groups.Requests;
    using YorkshireDigital.MeetupApi.Models;

    [TestFixture]
    public class MeetupServiceTests
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
    }
}
