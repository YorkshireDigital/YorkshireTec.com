namespace YorkshireDigital.Data.Tests.ServiceTests
{
    using FluentAssertions;
    using NUnit.Framework;
    using YorkshireDigital.Data.Domain.Account;
    using YorkshireDigital.Data.Services;

    public class UserServiceTests : InMemoryFixtureBase
    {
        private UserService service;

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
        public void UpdateUser_UpdatesTheUser()
        {
            // Arrange
            var user = new User { Username = "UnitTest", Name = "Unit Test", Email = "existing@email.com" };
            Session.SaveOrUpdate(user);
            user.Username = "UnitTest2";
            user.Name = "Jeff Test";
            user.Email = "new@email.com";

            // Act
            var result = service.SaveUser(user);

            // Assert
            result.Email.ShouldAllBeEquivalentTo(user.Email);
            result.Name.ShouldAllBeEquivalentTo(user.Name);
            result.Username.ShouldAllBeEquivalentTo(user.Username);
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
    }
}
