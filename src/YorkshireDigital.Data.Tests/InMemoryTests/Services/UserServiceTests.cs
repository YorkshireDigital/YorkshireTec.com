namespace YorkshireDigital.Data.Tests.InMemoryTests.Services
{
    using System;
    using FluentAssertions;
    using NUnit.Framework;
    using YorkshireDigital.Data.Domain.Account;
    using YorkshireDigital.Data.Exceptions;
    using YorkshireDigital.Data.Services;
    using YorkshireDigital.Data.Tests.InMemoryTests;

    public class UserServiceTests : InMemoryFixtureBase
    {
        private IUserService service;

        [SetUp]
        public void SetUp()
        {
            service = new UserService(Session);
        }

        [TestCase("userA", false)]
        [TestCase("userB", true)]
        public void UsernameAvailable_returns_if_username_is_free(string username, bool expectedResult)
        {
            // Arrange
            var user = new User { Username = "userA" };
            Session.SaveOrUpdate(user);

            // Act
            var result = service.UsernameAvailable(username);

            // Assert
            result.ShouldBeEquivalentTo(expectedResult);
        }

        [TestCase("existing@email.com", true)]
        [TestCase("new@email.com", false)]
        public void EmailAlreadyRegistered_returns_if_email_exists(string email, bool expectedResult)
        {
            // Arrange
            var user = new User { Email = "existing@email.com" };
            Session.SaveOrUpdate(user);

            // Act
            var result = service.EmailAlreadyRegistered(email);

            // Assert
            result.ShouldBeEquivalentTo(expectedResult);
        }

        [Test]
        public void GetUser_ReturnsCorrectUser()
        {
            // Arrange
            var user = new User { Username = "UnitTest", Name = "Unit Test", Email = "existing@email.com" };
            Session.SaveOrUpdate(user);

            // Act
            var result = service.GetUser("UnitTest");

            // Assert
            result.Email.ShouldAllBeEquivalentTo(user.Email);
            result.Name.ShouldAllBeEquivalentTo(user.Name);
            result.Username.ShouldAllBeEquivalentTo(user.Username);
        }

        [Test]
        public void GetUser_ReturnsNullIfNotFound()
        {
            // Arrange

            // Act
            var result = service.GetUser("UnitTest");

            // Assert
            result.Should().BeNull();
        }

        [Test]
        public void GetUserById_ReturnsCorrectUser()
        {
            // Arrange
            var user = new User { Username = "UnitTest", Name = "Unit Test", Email = "existing@email.com" };
            Session.SaveOrUpdate(user);

            // Act
            var result = service.GetUserById(user.Id);

            // Assert
            result.Email.ShouldAllBeEquivalentTo(user.Email);
            result.Name.ShouldAllBeEquivalentTo(user.Name);
            result.Username.ShouldAllBeEquivalentTo(user.Username);
        }

        [Test]
        public void SaveUser_UpdatesTheUser()
        {
            // Arrange
            var user = new User { Username = "UnitTest", Name = "Unit Test", Email = "existing@email.com" };
            Session.SaveOrUpdate(user);
            user.Username = "UnitTest2";
            user.Name = "Jeff Test";
            user.Email = "new@email.com";
            var updateStart = DateTime.UtcNow;
            // Act
            var result = service.SaveUser(user);

            // Assert
            result.Email.ShouldAllBeEquivalentTo(user.Email);
            result.Name.ShouldAllBeEquivalentTo(user.Name);
            result.Username.ShouldAllBeEquivalentTo(user.Username);
            result.LastEditedOn.Should().BeOnOrAfter(updateStart);
        }

        [Test]
        public void GetUserByEmail_ReturnsCorrectUser()
        {
            // Arrange
            var user = new User { Username = "UnitTest", Name = "Unit Test", Email = "existing@email.com" };
            Session.SaveOrUpdate(user);

            // Act
            var result = service.GetUserByEmail(user.Email);

            // Assert
            result.Id.ShouldBeEquivalentTo(user.Id);
            result.Email.ShouldBeEquivalentTo(user.Email);
            result.Name.ShouldBeEquivalentTo(user.Name);
            result.Username.ShouldBeEquivalentTo(user.Username);
        }

        [Test]
        public void Disable_SetsUserAsDisabled_WhereUserExists()
        {
            // Arrange
            var user1 = new User { Username = "User1", Name = "User 1", Email = "user1@email.com" };
            Session.SaveOrUpdate(user1);

            // Act
            service.Disable(user1.Id);
            var user = Session.Get<User>(user1.Id);

            // Assert
            user.IsDisabled.Should().BeTrue();
        }

        [Test]
        [ExpectedException(typeof(UserNotFoundException), ExpectedMessage = "No user found with id fe3bb38f-fa0a-46d4-b289-2b473770d061")]
        public void Disable_ThrowsException_WhereUserDoesNotExist()
        {
            // Arrange

            // Act
            service.Disable(Guid.Parse("fe3bb38f-fa0a-46d4-b289-2b473770d061"));

            // Assert
        }

        [Test]
        public void ListActiveUsers_ReturnsAllAddedUsers()
        {
            // Arrange
            var user1 = new User { Username = "User1", Name = "User 1", Email = "user1@email.com" };
            var user2 = new User { Username = "User2", Name = "User 2", Email = "user2@email.com" };
            var user3 = new User { Username = "User3", Name = "User 3", Email = "user3@email.com" };
            Session.SaveOrUpdate(user1);
            Session.SaveOrUpdate(user2);
            Session.SaveOrUpdate(user3);

            // Act
            var users = service.GetActiveUsers();

            // Assert
            users.Count.ShouldBeEquivalentTo(3);
        }

        [Test]
        public void ListActiveUsers_ShouldNotIncludeDisabledUsers()
        {
            // Arrange
            var user1 = new User { Username = "User1", Name = "User 1", Email = "user1@email.com" };
            var user2 = new User { Username = "User2", Name = "User 2", Email = "user2@email.com", DisabledOn = DateTime.Now };
            var user3 = new User { Username = "User3", Name = "User 3", Email = "user3@email.com" };
            Session.SaveOrUpdate(user1);
            Session.SaveOrUpdate(user2);
            Session.SaveOrUpdate(user3);

            // Act
            var users = service.GetActiveUsers();

            // Assert
            users.Count.ShouldBeEquivalentTo(2);
            users[0].Username.ShouldBeEquivalentTo("User1");
            users[1].Username.ShouldBeEquivalentTo("User3");
        }

        [Test]
        public void ListActiveUsers_Returns20Users_WhenNoTakeIsSpecified()
        {
            // Arrange
            for (int i = 0; i < 100; i++)
            {
                var user = new User { Username = string.Format("User{0}", i), Name = string.Format("User {0}", i), Email = string.Format("user{0}@email.com", i) };
                Session.SaveOrUpdate(user);
            }

            // Act
            var users = service.GetActiveUsers();

            // Assert
            users.Count.ShouldBeEquivalentTo(20);
        }

        [TestCase(10)]
        [TestCase(20)]
        [TestCase(30)]
        [TestCase(40)]
        public void ListActiveUsers_ReturnsCorrectAmountOfUsers_WhenTakeIsSpecified(int take)
        {
            // Arrange
            for (int i = 0; i < 100; i++)
            {
                var user = new User { Username = string.Format("User{0}", i), Name = string.Format("User {0}", i), Email = string.Format("user{0}@email.com", i) };
                Session.SaveOrUpdate(user);
            }

            // Act
            var users = service.GetActiveUsers(take);

            // Assert
            users.Count.ShouldBeEquivalentTo(take);
        }

        [Test]
        public void ListActiveUsers_ReturnsUsersAfterSkip_WhenSkipIsSpecified()
        {
            // Arrange
            for (int i = 0; i < 100; i++)
            {
                var user = new User { Username = string.Format("User{0}", i), Name = string.Format("User {0}", i), Email = string.Format("user{0}@email.com", i) };
                Session.SaveOrUpdate(user);
            }

            // Act
            var users = service.GetActiveUsers(30, 30);

            // Assert
            users.Count.ShouldBeEquivalentTo(30);
            users[0].Username.ShouldAllBeEquivalentTo("User30");
        }
    }
}
