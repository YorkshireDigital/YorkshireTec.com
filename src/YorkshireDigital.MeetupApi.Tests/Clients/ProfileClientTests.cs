namespace YorkshireDigital.MeetupApi.Tests.Clients
{
    using FakeItEasy;
    using FluentAssertions;
    using NUnit.Framework;
    using RestSharp;
    using YorkshireDigital.MeetupApi.Clients;
    using YorkshireDigital.MeetupApi.Helpers;
    using YorkshireDigital.MeetupApi.Requests;

    [TestFixture]
    public class ProfileClientTests
    {
        private static string DeleteProfileSuccessResponse()
        {
            return @"{
    'message': 'member is removed from group'
}";
        }

        private static string DeleteProfileNotFoundResponse()
        {
            return @"{
    'details': 'The resource you have requested can not be found',
    'code': 'not_found',
    'problem': 'Not Found'
}";
        }

        private static string CreateProfileResponse()
        {
            return
                @"{
    'member_id': 187831112,
    'other_services': {},
    'profile_url': 'http://www.meetup.com/Leeds-Sharp/members/187831112/',
    'created': 1432669683000,
    'name': 'Yorkshire Digital Bot',
    'visited': 1432669683000,
    'photo': {
        'highres_link': 'http://photos2.meetupstatic.com/photos/member/4/f/3/9/highres_246500281.jpeg',
        'photo_id': 246500281,
        'photo_link': 'http://photos2.meetupstatic.com/photos/member/4/f/3/9/member_246500281.jpeg',
        'thumb_link': 'http://photos2.meetupstatic.com/photos/member/4/f/3/9/thumb_246500281.jpeg'
    },
    'photo_url': 'http://photos2.meetupstatic.com/photos/member/4/f/3/9/member_246500281.jpeg',
    'updated': 1432669682767,
    'status': 'active',
    'group': {
        'join_mode': 'open',
        'created': 1397043507000,
        'name': 'Leeds Sharp',
        'group_lon': -1.559999942779541,
        'id': 13818462,
        'urlname': 'Leeds-Sharp',
        'group_lat': 53.79999923706055,
        'who': 'LS#ers'
    }
}";
        }

        [Test]
        public void Create_WithValidGroupId_Returns201WithResponseModel()
        {
            // Arrange
            var client = A.Fake<IRestClient>();
            var meetup = new MeetupClient(client);
            var response = new RestResponse {Content = CreateProfileResponse()};

            A.CallTo(() => client.Execute(A<IRestRequest>.Ignored))
                .Returns(response);

            // Act
            var profile = meetup.Profile.Create(new ProfileCreateRequest {GroupId = "12345"});

            // Assert
            profile.MemberId.ShouldBeEquivalentTo(187831112);
            profile.ProfileUrl.ShouldBeEquivalentTo("http://www.meetup.com/Leeds-Sharp/members/187831112/");
            profile.Created.ShouldBeEquivalentTo(1432669683000);
            profile.Name.ShouldBeEquivalentTo("Yorkshire Digital Bot");
            profile.Visited.ShouldBeEquivalentTo(1432669683000);

            profile.Photo.HighresLink.ShouldBeEquivalentTo("http://photos2.meetupstatic.com/photos/member/4/f/3/9/highres_246500281.jpeg");
            profile.Photo.PhotoId.ShouldBeEquivalentTo(246500281);
            profile.Photo.PhotoLink.ShouldBeEquivalentTo("http://photos2.meetupstatic.com/photos/member/4/f/3/9/member_246500281.jpeg");
            profile.Photo.ThumbLink.ShouldBeEquivalentTo("http://photos2.meetupstatic.com/photos/member/4/f/3/9/thumb_246500281.jpeg");
            profile.PhotoUrl.ShouldBeEquivalentTo("http://photos2.meetupstatic.com/photos/member/4/f/3/9/member_246500281.jpeg");

            profile.Updated.ShouldBeEquivalentTo(1432669682767);
            profile.Status.ShouldBeEquivalentTo("active");

            profile.Group.JoinMode.ShouldBeEquivalentTo("open");
            profile.Group.Created.ShouldBeEquivalentTo(1397043507000);
            profile.Group.CreatedDate.ShouldBeEquivalentTo(DateHelpers.MeetupTimeStampToDateTime(1397043507000));
            profile.Group.Name.ShouldBeEquivalentTo("Leeds Sharp");
            profile.Group.GroupLon.ShouldBeEquivalentTo(-1.559999942779541);
            profile.Group.Id.ShouldBeEquivalentTo(13818462);
            profile.Group.UrlName.ShouldBeEquivalentTo("Leeds-Sharp");
            profile.Group.GroupLat.ShouldBeEquivalentTo(53.79999923706055);
            profile.Group.Who.ShouldBeEquivalentTo("LS#ers");
        }

        [Test]
        public void Delete_WithValidGroupAndMemberId_ReturnsTrue()
        {
            // Arrange
            var client = A.Fake<IRestClient>();
            var meetup = new MeetupClient(client);
            var response = new RestResponse { Content = DeleteProfileSuccessResponse() };

            A.CallTo(() => client.Execute(A<IRestRequest>.Ignored))
                .Returns(response);

            // Act
            bool deleted = meetup.Profile.Delete(new ProfileDeleteRequest { GroupId = "12345", MemberId = "12345" });

            // Assert
            deleted.Should().BeTrue();
        }

        [Test]
        public void Delete_WithNotFoundGroupAndMemberId_ReturnsFalse()
        {
            // Arrange
            var client = A.Fake<IRestClient>();
            var meetup = new MeetupClient(client);
            var response = new RestResponse { Content = DeleteProfileNotFoundResponse() };

            A.CallTo(() => client.Execute(A<IRestRequest>.Ignored))
                .Returns(response);

            // Act
            bool deleted = meetup.Profile.Delete(new ProfileDeleteRequest { GroupId = "12345", MemberId = "12345" });

            // Assert
            deleted.Should().BeFalse();
        }
    }
}
