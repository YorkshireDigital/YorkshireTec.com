namespace YorkshireDigital.MeetupApi.Tests.Requests
{
    using NUnit.Framework;
    using RestSharp;
    using YorkshireDigital.MeetupApi.Requests;
    using FluentAssertions;

    [TestFixture]
    public class GroupsRequestTests
    {
        [Test]
        public void GroupsRequest_GroupUrlName_MapsCorrectly()
        {
            // Arrange
            var request = new GroupsRequest {GroupUrlName = "Test-Name"};

            // Act
            var restRequest = request.ToRestRequest(Method.GET, "api-key");

            // Assert
            restRequest.Parameters.Count.ShouldBeEquivalentTo(2);
            restRequest.Parameters[1].Name.ShouldBeEquivalentTo("group_urlname");
            restRequest.Parameters[1].Value.ShouldBeEquivalentTo("Test-Name");
            restRequest.Method.ShouldBeEquivalentTo(Method.GET);
            restRequest.Resource.ShouldBeEquivalentTo("groups");
        }
    }
}
