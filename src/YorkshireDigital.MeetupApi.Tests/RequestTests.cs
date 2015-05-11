namespace YorkshireDigital.MeetupApi.Tests
{
    using FluentAssertions;
    using NUnit.Framework;
    using RestSharp;
    using YorkshireDigital.MeetupApi.Groups.Requests;

    [TestFixture]
    public class RequestTests
    {
        [Test]
        public void GroupsRequest_WithGroupUrlNameSpecified_CreatesRestRequestWithGroupNameParameter()
        {
            // Arrange
            var request = new GroupsRequest {GroupUrlName = "Test-Name"};

            // Act
            var restRequest = request.ToRestRequest();

            // Assert
            restRequest.Parameters.Count.ShouldBeEquivalentTo(1);
            restRequest.Parameters[0].Name.ShouldBeEquivalentTo("group_urlname");
            restRequest.Parameters[0].Value.ShouldBeEquivalentTo("Test-Name");
            restRequest.Method.ShouldBeEquivalentTo(Method.GET);
            restRequest.Resource.ShouldBeEquivalentTo("groups");
        }
    }
}
