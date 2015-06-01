namespace YorkshireDigital.MeetupApi.Tests.Requests
{
    using FluentAssertions;
    using NUnit.Framework;
    using RestSharp;
    using YorkshireDigital.MeetupApi.Requests;

    [TestFixture]
    public class ProfileDeleteRequestTests
    {
        [Test]
        public void ProfileDeleteRequest_GroupIdAndMemberId_MapsCorrectly()
        {
            // Arrange
            var request = new ProfileDeleteRequest { GroupId = "12345", MemberId = "54321"};
            
            // Act
            RestRequest restRequest = request.ToRestRequest(Method.DELETE, "api-key");

            // Assert
            restRequest.Parameters.Count.ShouldBeEquivalentTo(1);
            restRequest.Parameters[0].Name.ShouldBeEquivalentTo("key");
            restRequest.Parameters[0].Value.ShouldBeEquivalentTo("api-key");
            restRequest.Resource.ShouldBeEquivalentTo("profile/12345/54321");

        }
    }
}
