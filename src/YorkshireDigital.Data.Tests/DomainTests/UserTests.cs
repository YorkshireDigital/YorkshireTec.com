namespace YorkshireDigital.Data.Tests.DomainTests
{
    using FluentAssertions;
    using NUnit.Framework;
    using YorkshireDigital.Data.Domain.Account;
    using YorkshireDigital.Data.Domain.Account.Enums;

    [TestFixture]
    public class UserTests
    {
        [Test]
        public void User_IsAdmin_ReturnsTrueIfUserIsAdmin()
        {
            // Arrange
            var user = new User();
            user.Roles.Add(new UserRole { Role = UserRoles.Admin });

            // Act
            var result = user.IsAdmin;

            // Assert
            result.ShouldBeEquivalentTo(true);
        }
    }
}
