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
            restRequest.Parameters.Count.ShouldBeEquivalentTo(3);
            restRequest.Parameters[0].Type.ShouldBeEquivalentTo(ParameterType.GetOrPost);
            restRequest.Parameters[0].Name.ShouldBeEquivalentTo("key");
            restRequest.Parameters[0].Value.ShouldBeEquivalentTo("api-key");
            restRequest.Parameters[1].Type.ShouldBeEquivalentTo(ParameterType.GetOrPost);
            restRequest.Parameters[1].Name.ShouldBeEquivalentTo("group_id");
            restRequest.Parameters[1].Value.ShouldBeEquivalentTo("12345");
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

            restRequest.Parameters.Count.ShouldBeEquivalentTo(3);
            restRequest.Parameters[0].Type.ShouldBeEquivalentTo(ParameterType.GetOrPost);
            restRequest.Parameters[0].Name.ShouldBeEquivalentTo("key");
            restRequest.Parameters[0].Value.ShouldBeEquivalentTo("api-key");
            restRequest.Parameters[1].Type.ShouldBeEquivalentTo(ParameterType.GetOrPost);
            restRequest.Parameters[1].Name.ShouldBeEquivalentTo("group_urlname");
            restRequest.Parameters[1].Value.ShouldBeEquivalentTo("Test-Group");
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

            restRequest.Parameters.Count.ShouldBeEquivalentTo(4);
            restRequest.Parameters[0].Type.ShouldBeEquivalentTo(ParameterType.GetOrPost);
            restRequest.Parameters[0].Name.ShouldBeEquivalentTo("key");
            restRequest.Parameters[0].Value.ShouldBeEquivalentTo("api-key");
            restRequest.Parameters[1].Type.ShouldBeEquivalentTo(ParameterType.GetOrPost);
            restRequest.Parameters[1].Name.ShouldBeEquivalentTo("answer_123");
            restRequest.Parameters[1].Value.ShouldBeEquivalentTo("test answer 1");
            restRequest.Parameters[2].Type.ShouldBeEquivalentTo(ParameterType.GetOrPost);
            restRequest.Parameters[2].Name.ShouldBeEquivalentTo("answer_125");
            restRequest.Parameters[2].Value.ShouldBeEquivalentTo("test answer 2");
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

            restRequest.Parameters.Count.ShouldBeEquivalentTo(3);
            restRequest.Parameters[0].Type.ShouldBeEquivalentTo(ParameterType.GetOrPost);
            restRequest.Parameters[0].Name.ShouldBeEquivalentTo("key");
            restRequest.Parameters[0].Value.ShouldBeEquivalentTo("api-key");
            restRequest.Parameters[1].Type.ShouldBeEquivalentTo(ParameterType.GetOrPost);
            restRequest.Parameters[1].Name.ShouldBeEquivalentTo("intro");
            restRequest.Parameters[1].Value.ShouldBeEquivalentTo("Test intro");
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

            restRequest.Parameters.Count.ShouldBeEquivalentTo(3);
            restRequest.Parameters[0].Type.ShouldBeEquivalentTo(ParameterType.GetOrPost);
            restRequest.Parameters[0].Name.ShouldBeEquivalentTo("key");
            restRequest.Parameters[0].Value.ShouldBeEquivalentTo("api-key");
            restRequest.Parameters[1].Type.ShouldBeEquivalentTo(ParameterType.GetOrPost);
            restRequest.Parameters[1].Name.ShouldBeEquivalentTo("site_name");
            restRequest.Parameters[1].Value.ShouldBeEquivalentTo("YorkshireDigital");
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

            restRequest.Parameters.Count.ShouldBeEquivalentTo(3);
            restRequest.Parameters[0].Type.ShouldBeEquivalentTo(ParameterType.GetOrPost);
            restRequest.Parameters[0].Name.ShouldBeEquivalentTo("key");
            restRequest.Parameters[0].Value.ShouldBeEquivalentTo("api-key");
            restRequest.Parameters[1].Type.ShouldBeEquivalentTo(ParameterType.GetOrPost);
            restRequest.Parameters[1].Name.ShouldBeEquivalentTo("site_url");
            restRequest.Parameters[1].Value.ShouldBeEquivalentTo("www.yorkshiredigital.com");
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

            restRequest.Parameters.Count.ShouldBeEquivalentTo(4);
            restRequest.Parameters[0].Type.ShouldBeEquivalentTo(ParameterType.GetOrPost);
            restRequest.Parameters[0].Name.ShouldBeEquivalentTo("key");
            restRequest.Parameters[0].Value.ShouldBeEquivalentTo("api-key");
            restRequest.Parameters[1].Type.ShouldBeEquivalentTo(ParameterType.GetOrPost);
            restRequest.Parameters[1].Name.ShouldBeEquivalentTo("group_id");
            restRequest.Parameters[1].Value.ShouldBeEquivalentTo("12345");
            restRequest.Parameters[2].Type.ShouldBeEquivalentTo(ParameterType.GetOrPost);
            restRequest.Parameters[2].Name.ShouldBeEquivalentTo("group_urlname");
            restRequest.Parameters[2].Value.ShouldBeEquivalentTo("Test-Group");
            restRequest.Method.ShouldBeEquivalentTo(Method.POST);
            restRequest.Resource.ShouldBeEquivalentTo("profile");
        }
    }
}
