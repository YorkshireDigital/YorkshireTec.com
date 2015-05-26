namespace YorkshireDigital.MeetupApi.Tests.Requests
{
    using System.Collections.Generic;
    using FluentAssertions;
    using NUnit.Framework;
    using RestSharp;
    using YorkshireDigital.MeetupApi.Requests;

    [TestFixture]
    public class ProfileCreateRequestTests
    {
        [Test]
        public void ProfileCreateRequest_GroupId_MapsCorrectly()
        {
            // Arrange
            var request = new ProfileCreateRequest { GroupId = "12345" };

            // Act
            RestRequest restRequest = request.ToRestRequest(Method.POST, "api-key");

            // Assert
            restRequest.Parameters.Count.ShouldBeEquivalentTo(1);
            restRequest.Parameters[0].Type.ShouldBeEquivalentTo(ParameterType.RequestBody);
            restRequest.Parameters[0].Name.ShouldBeEquivalentTo("application/json");
            restRequest.Parameters[0].Value.ShouldBeEquivalentTo("{\"key\":\"api-key\",\"group_id\":\"12345\"}");
            restRequest.Method.ShouldBeEquivalentTo(Method.POST);
            restRequest.Resource.ShouldBeEquivalentTo("profile");
        }

        [Test]
        public void ProfileCreateRequest_GroupUrlName_MapsCorrectly()
        {
            // Arrange
            var request = new ProfileCreateRequest { GroupUrlName = "Test-Group" };

            // Act
            RestRequest restRequest = request.ToRestRequest(Method.POST, "api-key");

            // Assert

            restRequest.Parameters.Count.ShouldBeEquivalentTo(1);
            restRequest.Parameters[0].Type.ShouldBeEquivalentTo(ParameterType.RequestBody);
            restRequest.Parameters[0].Name.ShouldBeEquivalentTo("application/json");
            restRequest.Parameters[0].Value.ShouldBeEquivalentTo("{\"key\":\"api-key\",\"group_urlname\":\"Test-Group\"}");
            restRequest.Method.ShouldBeEquivalentTo(Method.POST);
            restRequest.Resource.ShouldBeEquivalentTo("profile");
        }

        [Test]
        public void ProfileCreateRequest_AnswerId_MapsCorrectly()
        {
            // Arrange
            var request = new ProfileCreateRequest { Answers = new Dictionary<int, string> { { 123, "test answer 1" }, { 125, "test answer 2" } } };

            // Act
            RestRequest restRequest = request.ToRestRequest(Method.POST, "api-key");

            // Assert

            restRequest.Parameters.Count.ShouldBeEquivalentTo(1);
            restRequest.Parameters[0].Type.ShouldBeEquivalentTo(ParameterType.RequestBody);
            restRequest.Parameters[0].Name.ShouldBeEquivalentTo("application/json");
            restRequest.Parameters[0].Value.ShouldBeEquivalentTo("{\"key\":\"api-key\",\"answer_123\":\"test answer 1\",\"answer_125\":\"test answer 2\"}");
            restRequest.Method.ShouldBeEquivalentTo(Method.POST);
            restRequest.Resource.ShouldBeEquivalentTo("profile");
        }

        [Test]
        public void ProfileCreateRequest_Intro_MapsCorrectly()
        {
            // Arrange
            var request = new ProfileCreateRequest { Intro = "Test intro" };

            // Act
            RestRequest restRequest = request.ToRestRequest(Method.POST, "api-key");

            // Assert

            restRequest.Parameters.Count.ShouldBeEquivalentTo(1);
            restRequest.Parameters[0].Type.ShouldBeEquivalentTo(ParameterType.RequestBody);
            restRequest.Parameters[0].Name.ShouldBeEquivalentTo("application/json");
            restRequest.Parameters[0].Value.ShouldBeEquivalentTo("{\"key\":\"api-key\",\"intro\":\"Test intro\"}");
            restRequest.Method.ShouldBeEquivalentTo(Method.POST);
            restRequest.Resource.ShouldBeEquivalentTo("profile");
        }

        [Test]
        public void ProfileCreateRequest_SiteName_MapsCorrectly()
        {
            // Arrange
            var request = new ProfileCreateRequest { SiteName = "YorkshireDigital" };

            // Act
            RestRequest restRequest = request.ToRestRequest(Method.POST, "api-key");

            // Assert

            restRequest.Parameters.Count.ShouldBeEquivalentTo(1);
            restRequest.Parameters[0].Type.ShouldBeEquivalentTo(ParameterType.RequestBody);
            restRequest.Parameters[0].Name.ShouldBeEquivalentTo("application/json");
            restRequest.Parameters[0].Value.ShouldBeEquivalentTo("{\"key\":\"api-key\",\"site_name\":\"YorkshireDigital\"}");
            restRequest.Method.ShouldBeEquivalentTo(Method.POST);
            restRequest.Resource.ShouldBeEquivalentTo("profile");
        }

        [Test]
        public void ProfileCreateRequest_SiteUrl_MapsCorrectly()
        {
            // Arrange
            var request = new ProfileCreateRequest { SiteUrl = "www.yorkshiredigital.com" };

            // Act
            RestRequest restRequest = request.ToRestRequest(Method.POST, "api-key");

            // Assert

            restRequest.Parameters.Count.ShouldBeEquivalentTo(1);
            restRequest.Parameters[0].Type.ShouldBeEquivalentTo(ParameterType.RequestBody);
            restRequest.Parameters[0].Name.ShouldBeEquivalentTo("application/json");
            restRequest.Parameters[0].Value.ShouldBeEquivalentTo("{\"key\":\"api-key\",\"site_url\":\"www.yorkshiredigital.com\"}");
            restRequest.Method.ShouldBeEquivalentTo(Method.POST);
            restRequest.Resource.ShouldBeEquivalentTo("profile");
        }

        [Test]
        public void ProfileCreateRequest_MultipleParameters_MapsCorrectly()
        {
            // Arrange
            var request = new ProfileCreateRequest { GroupUrlName = "Test-Group", GroupId = "12345" };

            // Act
            RestRequest restRequest = request.ToRestRequest(Method.POST, "api-key");

            // Assert

            restRequest.Parameters.Count.ShouldBeEquivalentTo(1);
            restRequest.Parameters[0].Type.ShouldBeEquivalentTo(ParameterType.RequestBody);
            restRequest.Parameters[0].Name.ShouldBeEquivalentTo("application/json");
            restRequest.Parameters[0].Value.ShouldBeEquivalentTo("{\"key\":\"api-key\",\"group_id\":\"12345\",\"group_urlname\":\"Test-Group\"}");
            restRequest.Method.ShouldBeEquivalentTo(Method.POST);
            restRequest.Resource.ShouldBeEquivalentTo("profile");
        }
    }
}
