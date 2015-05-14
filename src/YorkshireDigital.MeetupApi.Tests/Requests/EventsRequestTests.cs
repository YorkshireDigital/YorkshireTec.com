namespace YorkshireDigital.MeetupApi.Tests.Requests
{
    using NUnit.Framework;
    using RestSharp;
    using YorkshireDigital.MeetupApi.Requests;
    using YorkshireDigital.MeetupApi.Requests.Enum;
    using FluentAssertions;

    [TestFixture]
    public class EventsRequestTests
    {
        [Test]
        public void EventsRequest_EventId_MapsCorrectly()
        {
            // Arrange
            var request = new EventsRequest { EventId = 12345 };

            // Act
            var restRequest = request.ToRestRequest();

            // Assert
            restRequest.Parameters.Count.ShouldBeEquivalentTo(1);
            restRequest.Parameters[0].Name.ShouldBeEquivalentTo("event_id");
            restRequest.Parameters[0].Value.ShouldBeEquivalentTo(12345);
            restRequest.Method.ShouldBeEquivalentTo(Method.GET);
            restRequest.Resource.ShouldBeEquivalentTo("events");
        }

        [Test]
        public void EventsRequest_GroupId_MapsCorrectly()
        {
            // Arrange
            var request = new EventsRequest { GroupId = 12345 };

            // Act
            var restRequest = request.ToRestRequest();

            // Assert
            restRequest.Parameters.Count.ShouldBeEquivalentTo(1);
            restRequest.Parameters[0].Name.ShouldBeEquivalentTo("group_id");
            restRequest.Parameters[0].Value.ShouldBeEquivalentTo(12345);
            restRequest.Method.ShouldBeEquivalentTo(Method.GET);
            restRequest.Resource.ShouldBeEquivalentTo("events");
        }

        [Test]
        public void EventsRequest_GroupDomain_MapsCorrectly()
        {
            // Arrange
            var request = new EventsRequest { GroupDomain = "Test-Domain" };

            // Act
            var restRequest = request.ToRestRequest();

            // Assert
            restRequest.Parameters.Count.ShouldBeEquivalentTo(1);
            restRequest.Parameters[0].Name.ShouldBeEquivalentTo("group_domain");
            restRequest.Parameters[0].Value.ShouldBeEquivalentTo("Test-Domain");
            restRequest.Method.ShouldBeEquivalentTo(Method.GET);
            restRequest.Resource.ShouldBeEquivalentTo("events");
        }

        [Test]
        public void EventsRequest_GroupUrlName_MapsCorrectly()
        {
            // Arrange
            var request = new EventsRequest { GroupUrlName = "Test-Name" };

            // Act
            var restRequest = request.ToRestRequest();

            // Assert
            restRequest.Parameters.Count.ShouldBeEquivalentTo(1);
            restRequest.Parameters[0].Name.ShouldBeEquivalentTo("group_urlname");
            restRequest.Parameters[0].Value.ShouldBeEquivalentTo("Test-Name");
            restRequest.Method.ShouldBeEquivalentTo(Method.GET);
            restRequest.Resource.ShouldBeEquivalentTo("events");
        }

        [Test]
        public void EventsRequest_Status_MapsCorrectly()
        {
            // Arrange
            var request = new EventsRequest { Status = EventStatus.Proposed };

            // Act
            var restRequest = request.ToRestRequest();

            // Assert
            restRequest.Parameters.Count.ShouldBeEquivalentTo(1);
            restRequest.Parameters[0].Name.ShouldBeEquivalentTo("status");
            restRequest.Parameters[0].Value.ShouldBeEquivalentTo("proposed");
            restRequest.Method.ShouldBeEquivalentTo(Method.GET);
            restRequest.Resource.ShouldBeEquivalentTo("events");
        }
    }
}
