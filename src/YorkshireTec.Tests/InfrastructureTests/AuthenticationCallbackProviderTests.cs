namespace YorkshireTec.Tests.InfrastructureTests
{
    using FluentAssertions;
    using NUnit.Framework;
    using YorkshireTec.Infrastructure;

    [TestFixture]
    public class AuthenticationCallbackProviderTests
    {
        [TestCase("", "~/")]
        [TestCase("Invalid", "~/")]
        [TestCase("http://www.valid.com/account/log-in?returnUrl=/account", "~/account")]
        [TestCase("http://www.valid.com/account/log-in?returnUrl=account", "~/account")]
        [TestCase("http://www.valid.com/account/log-in?invalid=/account", "~/")]
        [TestCase("http://www.valid.com/account/log-in", "~/")]
        public void GetReturnUrl_returnsExpectedUrl(string returnUrl, string expectedResult)
        {
            // Arrange

            // Act
            var result = AuthenticationCallbackProvider.GetReturnUrl(returnUrl);

            // Assert
            result.Should().BeEquivalentTo(expectedResult);
        }
    }
}
