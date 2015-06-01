namespace YorkshireDigital.Data.Tests.InMemoryTests.Services
{
    using System;
    using FluentAssertions;
    using NUnit.Framework;
    using YorkshireDigital.Data.Domain.Account;
    using YorkshireDigital.Data.Domain.Group;
    using YorkshireDigital.Data.Exceptions;
    using YorkshireDigital.Data.Services;
    using YorkshireDigital.Data.Tests.InMemoryTests;

    [TestFixture]
    public class GroupServiceTests : InMemoryFixtureBase
    {
        private GroupService groupService;

        [SetUp]
        public void SetUp()
        {
            groupService = new GroupService(Session);
        }

        [Test]
        public void Get_ReturnsGroup_WhenGroupExistsWithId()
        {
            // Arrange
            var group = new Group
            {
                Id = "test-group-1",
                Name = "Test Group 1"
            };
            Session.Save(group);

            // Act
            var result = groupService.Get("test-group-1");

            // Assert
            result.Id.ShouldBeEquivalentTo("test-group-1");
            result.Name.ShouldBeEquivalentTo("Test Group 1");
        }

        [Test]
        public void Get_ReturnsNull_WhenNoEventExists()
        {
            // Arrange

            // Assert
            var result = groupService.Get("test-group-1");

            // Act
            result.Should().BeNull();
        }

        [Test]
        public void Save_CreatesNewGroup_WhenNoGroupExists()
        {
            // Arrange
            var group = new Group
            {
                Id = "test-group-1",
                Name = "Test Group 1"
            };
            var saveStart = DateTime.UtcNow;

            // Act
            groupService.Save(@group, null);
            var result = Session.Load<Group>("test-group-1");

            // Assert
            result.Id.ShouldBeEquivalentTo("test-group-1");
            result.Name.ShouldBeEquivalentTo("Test Group 1");
            result.LastEditedOn.Should().BeOnOrAfter(saveStart);
            result.LastEditedBy.Should().BeNull();
        }

        [Test]
        public void Save_SetsLastEditedBy_WhenUserIsSpecified()
        {
            // Arrange
            var group = new Group
            {
                Id = "test-group-1",
                Name = "Test Group 1"
            };
            var user = new User
            {
                Id = Guid.NewGuid(),
                Username = "test-user"
            };
            Session.Save(user);
            var saveStart = DateTime.UtcNow;

            // Act
            groupService.Save(group, user);
            var result = Session.Load<Group>("test-group-1");

            // Assert
            result.Id.ShouldBeEquivalentTo("test-group-1");
            result.Name.ShouldBeEquivalentTo("Test Group 1");
            result.LastEditedOn.Should().BeOnOrAfter(saveStart);
            result.LastEditedBy.Id.ShouldBeEquivalentTo(user.Id);
            result.LastEditedBy.Username.ShouldBeEquivalentTo("test-user");
        }

        [Test]
        public void Save_UpdatesGroup_WhenGroupAlreadyExists()
        {
            // Arrange
            var group = new Group
            {
                Id = "test-group-1",
                Name = "Test Group 1",
                LastEditedOn = DateTime.UtcNow.AddDays(-1)
            };
            Session.Save(group);
            group.Name = "New Name";
            var saveStart = DateTime.UtcNow;

            // Act
            groupService.Save(@group, null);
            var result = Session.Load<Group>("test-group-1");

            // Assert
            result.Id.ShouldBeEquivalentTo("test-group-1");
            result.Name.ShouldBeEquivalentTo("New Name");
            result.LastEditedOn.Should().BeOnOrAfter(saveStart);
        }

        [Test]
        public void Delete_MarksGroupAsDeleted_WhereGroupExists()
        {
            // Arrange
            var user = new User
            {
                Id = Guid.NewGuid(),
                Username = "test-user"
            };
            Session.Save(user);
            var group = new Group { Id = "test-group-1", Name = "Test Group 1" };
            Session.Save(group);

            // Act
            groupService.Delete("test-group-1", user);
            var result = Session.Load<Group>("test-group-1");

            // Assert
            result.IsDeleted.Should().BeTrue();
            result.DeletedBy.Username.ShouldBeEquivalentTo("test-user");
        }

        [Test]
        [ExpectedException(typeof(GroupNotFoundException), ExpectedMessage = "Unable to find group with id test-group-1")]
        public void Delete_ThrowsException_WhereGroupDoesNotExist()
        {
            // Arrange

            // Act
            groupService.Delete("test-group-1", null);
            
            // Assert
        }

        [Test]
        public void GetActiveGroups_ReturnsAllAddedGroups()
        {
            // Arrange
            var group1 = new Group { Id = "test-group-1", Name = "Test Group 1" };
            var group2 = new Group { Id = "test-group-2", Name = "Test Group 2" };
            var group3 = new Group { Id = "test-group-3", Name = "Test Group 3" };
            Session.Save(group1);
            Session.Save(group2);
            Session.Save(group3);

            // Act
            var groups = groupService.GetActiveGroups();

            // Assert
            groups.Count.ShouldBeEquivalentTo(3);
        }

        [Test]
        public void GetActiveGroups_DoesNotIncludeDeletedGroups()
        {
            // Arrange
            var group1 = new Group { Id = "test-group-1", Name = "Test Group 1" };
            var group2 = new Group { Id = "test-group-2", Name = "Test Group 2", DeletedOn = DateTime.Now };
            var group3 = new Group { Id = "test-group-3", Name = "Test Group 3" };
            Session.Save(group1);
            Session.Save(group2);
            Session.Save(group3);

            // Act
            var groups = groupService.GetActiveGroups();

            // Assert
            groups.Count.ShouldBeEquivalentTo(2);
            groups[0].Id.ShouldBeEquivalentTo("test-group-1");
            groups[1].Id.ShouldBeEquivalentTo("test-group-3");
        }

        [Test]
        public void GetActiveGroups_Returns20Groups_WhenNoTakeIsSpecified()
        {
            // Arrange

            for (int i = 0; i < 100; i++)
            {
                var group = new Group { Id = string.Format("test-group-{0}", i), Name = "Test Group" };
                Session.Save(group);
            }

            // Act
            var groups = groupService.GetActiveGroups();

            // Assert
            groups.Count.ShouldBeEquivalentTo(20);
        }

        [TestCase(10)]
        [TestCase(20)]
        [TestCase(30)]
        [TestCase(40)]
        public void GetActiveGroups_ReturnsCorrectAmountOfGroups_WhenTakeIsSpecified(int take)
        {
            // Arrange

            for (int i = 0; i < 100; i++)
            {
                var group = new Group { Id = string.Format("test-group-{0}", i), Name = "Test Group" };
                Session.Save(group);
            }

            // Act
            var groups = groupService.GetActiveGroups(take);

            // Assert
            groups.Count.ShouldBeEquivalentTo(take);
        }

        [Test]
        public void GetActiveGroups_ReturnsGroupsAfterSkip_WhenSkipIsSpecified()
        {
            // Arrange

            for (int i = 0; i < 100; i++)
            {
                var group = new Group { Id = string.Format("test-group-{0}", i), Name = "Test Group" };
                Session.Save(group);
            }

            // Act
            var groups = groupService.GetActiveGroups(30, 30);

            // Assert
            groups.Count.ShouldBeEquivalentTo(30);
            groups[0].Id.ShouldBeEquivalentTo("test-group-30");
        }
    }
}
