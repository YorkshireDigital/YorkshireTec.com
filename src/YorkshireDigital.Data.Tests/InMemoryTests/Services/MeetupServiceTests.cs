namespace YorkshireDigital.Data.Tests.InMemoryTests.Services
{
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
            bool result = service.GroupExists("test-group");

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
            bool result = service.GroupExists("test-group");

            // Assert
            result.Should().BeFalse();
        }
    }
}
