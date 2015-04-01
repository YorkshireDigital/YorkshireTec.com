namespace YorkshireDigital.Data.Tests.HelperTests
{
    using FluentAssertions;
    using NUnit.Framework;
    using YorkshireDigital.Data.Helpers;

    [TestFixture]
    public class DiscourseHelperTests
    {
        private string secret = "d836444a9e4084d5b224a60c208dce14";

        [Test]
        public void DiscourseHelper_GetHash_ReturnsHMACSHA256Hash()
        {
            // Arrange
            var payload = "bm9uY2U9Y2I2ODI1MWVlZmI1MjExZTU4YzAwZmYxMzk1ZjBjMGI=\n";
            IDiscourseHelper discourseHelper = new DiscourseHelper(secret);

            // Act
            var hash = discourseHelper.GetHash(payload);

            // Assert
            hash.ShouldBeEquivalentTo("2828aa29899722b35a2f191d34ef9b3ce695e0e6eeec47deb46d588d70c7cb56");
        }

        [Test]
        public void DiscourseHelper_GetNonceFromPayload_ReturnsExpectedNonce()
        {
            // Arrange
            var payload = "bm9uY2U9Y2I2ODI1MWVlZmI1MjExZTU4YzAwZmYxMzk1ZjBjMGI=\n";
            IDiscourseHelper discourseHelper = new DiscourseHelper(secret);

            // Act
            var nonce = discourseHelper.GetNonceFromPayload(payload);

            // Assert
            nonce.ShouldBeEquivalentTo("cb68251eefb5211e58c00ff1395f0c0b");
        }

        [TestCase("2828aa29899722b35a2f191d34ef9b3ce695e0e6eeec47deb46d588d70c7cb56", true)]
        [TestCase("2828aa29899722b35a2f191d34ef9b3ce695e0e6eeec47deb46d588d70c7cb5", false)]
        public void DiscourseHelper_ValidatePayloadSignature_ReturnsTrueIfSignatureIsValid(string signature, bool expected)
        {
            // Arrange
            const string payload = "bm9uY2U9Y2I2ODI1MWVlZmI1MjExZTU4YzAwZmYxMzk1ZjBjMGI=\n";
            IDiscourseHelper discourseHelper = new DiscourseHelper(secret);

            // Act 
            var result = discourseHelper.ValidatePayloadSignature(payload, signature);

            // Assert
            result.ShouldBeEquivalentTo(expected);
        }

        [Test]
        public void DiscourseHelper_GetRedirectUrl_ReturnsExpectedUrl()
        {
            // Arrange
            const string name = "sam";
            const string externalId = "hello123";
            const string email = "test@test.com";
            const string username = "samsam";
            const string nonce = "cb68251eefb5211e58c00ff1395f0c0b";
            const string discourseDomain = "http://discuss.example.com";
            IDiscourseHelper discourseHelper = new DiscourseHelper(secret);

            // Act
            var hash = discourseHelper.GetRedirectUrl(discourseDomain, name, externalId, email, username, nonce);

            // Assert
            hash.Should().Be("http://discuss.example.com/session/sso_login?sso=bm9uY2U9Y2I2ODI1MWVlZmI1MjExZTU4YzAwZmYxMzk1ZjBjMGImbmFtZT1z%0aYW0mdXNlcm5hbWU9c2Ftc2FtJmVtYWlsPXRlc3QlNDB0ZXN0LmNvbSZleHRl%0acm5hbF9pZD1oZWxsbzEyMw%3d%3d%0a&sig=1c884222282f3feacd76802a9dd94e8bc8deba5d619b292bed75d63eb3152c0b");
        }
    }
}
