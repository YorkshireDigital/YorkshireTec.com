namespace YorkshireTec.Tests.RepositoryTests
{
    using FluentAssertions;
    using NUnit.Framework;
    using YorkshireTec.Raven.Domain.Account;
    using YorkshireTec.Raven.Repositories;

    [TestFixture]
    public class UserRepositoryTests : BaseRavenFixture
    {
        private UserRepository userRepository;

        [SetUp]
        public void SetUp()
        {
            userRepository = new UserRepository(DocumentSession);
        }

        [TestCase("userA", false)]
        [TestCase("userB", true)]
        public void UsernameAvailable_returns_if_username_is_free(string username, bool expectedResult)
        {
            // Arrange
            var user = new User { Username = "userA" };
            DocumentSession.Store(user);
            DocumentSession.SaveChanges();

            // Act
            var result = userRepository.UsernameAvailable(username);

            // Assert
            result.ShouldBeEquivalentTo(expectedResult);
        }

        [TestCase("existing@email.com", true)]
        [TestCase("new@email.com", false)]
        public void EmailAlreadyRegistered_returns_if_email_exists(string email, bool expectedResult)
        {
            // Arrange
            var user = new User { Email = "existing@email.com" };
            DocumentSession.Store(user);
            DocumentSession.SaveChanges();

            // Act
            var result = userRepository.EmailAlreadyRegistered(email);

            // Assert
            result.ShouldBeEquivalentTo(expectedResult);
        }

        [Test]
        public void GetUser_ReturnsCorrectUser()
        {
            // Arrange
            var user = new User { Username = "UnitTest", Name = "Unit Test", Email = "existing@email.com" };
            DocumentSession.Store(user);
            DocumentSession.SaveChanges();

            // Act
            var result = userRepository.GetUser("UnitTest");

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
            var result = userRepository.GetUser("UnitTest");

            // Assert
            result.Should().BeNull();
        }
    }
}
