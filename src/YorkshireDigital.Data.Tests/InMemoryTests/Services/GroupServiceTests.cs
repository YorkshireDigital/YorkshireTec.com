﻿namespace YorkshireDigital.Data.Tests.InMemoryTests.Services
{
    using System;
    using FluentAssertions;
    using NUnit.Framework;
    using YorkshireDigital.Data.Domain.Organisations;
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
            groupService.Save(group);
            var result = Session.Load<Group>("test-group-1");

            // Assert
            result.Id.ShouldBeEquivalentTo("test-group-1");
            result.Name.ShouldBeEquivalentTo("Test Group 1");
            result.LastEditedOn.Should().BeOnOrAfter(saveStart);
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
            groupService.Save(group);
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
            var group = new Group { Id = "test-group-1", Name = "Test Group 1" };
            Session.Save(group);

            // Act
            groupService.Delete("test-group-1");
            var result = Session.Load<Group>("test-group-1");

            // Assert
            result.IsDeleted.Should().BeTrue();
        }

        [Test]
        [ExpectedException(typeof(GroupNotFoundException), ExpectedMessage = "Unable to find group with id test-group-1")]
        public void Delete_ThrowsException_WhereGroupDoesNotExist()
        {
            // Arrange

            // Act
            groupService.Delete("test-group-1");
            
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
    }
}