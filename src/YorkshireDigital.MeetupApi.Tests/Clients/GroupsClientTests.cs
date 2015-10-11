namespace YorkshireDigital.MeetupApi.Tests.Clients
{
    using FakeItEasy;
    using FluentAssertions;
    using NUnit.Framework;
    using RestSharp;
    using YorkshireDigital.MeetupApi.Clients;
    using YorkshireDigital.MeetupApi.Requests;

    [TestFixture]
    public class GroupsClientTests
    {
        #region Test Data
        
        private static string TestGroupJson()
        {
            return @"{  
                    'utc_offset':3600000,
                    'country':'GB',
                    'visibility':'public',
                    'city':'Leeds',
                    'timezone':'Europe\/London',
                    'created':1397043507000,
                    'topics':[  
                        {  
                            'urlkey':'dotnet',
                            'name':'.NET',
                            'id':827
                        },
                        {  
                            'urlkey':'csharp',
                            'name':'C#',
                            'id':2260
                        },
                        {  
                            'urlkey':'programming',
                            'name':'Programming',
                            'id':17627
                        },
                        {  
                            'urlkey':'net-development',
                            'name':'.net development',
                            'id':52273
                        },
                        {  
                            'urlkey':'f-programming',
                            'name':'F# Programming',
                            'id':62405
                        },
                        {  
                            'urlkey':'asp-net-mvc',
                            'name':'ASP.NET MVC',
                            'id':121863
                        },
                        {  
                            'urlkey':'f-sharp',
                            'name':'F-Sharp',
                            'id':125965
                        },
                        {  
                            'urlkey':'c-sharp',
                            'name':'C Sharp',
                            'id':430852
                        },
                        {  
                            'urlkey':'c-sharp-development',
                            'name':'C Sharp Development',
                            'id':593862
                        }
                    ],
                    'link':'http:\/\/www.meetup.com\/Leeds-Sharp\/',
                    'rating':4.94,
                    'description':'<p>Welcome to <a href=\'http:\/\/www.leeds-sharp.org\/\'>Leeds Sharp<\/a>, a code club for developers using Microsoft technologies founded 31 May 2012.<br>\n\n<\/p>\n<p>If you\'re a developer using or interested in programming with Microsoft technologies please come along and join us. All skill levels welcome from absolute beginner to high level genius guru Martin-Fowler-look-a-like.<br>\n\n<\/p>\n<p>Our meetings are on the last thursday of the month and start at 6:30pm.<\/p>',
                    'lon':-1.559999942779541,
                    'group_photo':{  
                        'highres_link':'http:\/\/photos3.meetupstatic.com\/photos\/event\/d\/e\/highres_353100222.jpeg',
                        'photo_id':353100222,
                        'photo_link':'http:\/\/photos3.meetupstatic.com\/photos\/event\/d\/e\/600_353100222.jpeg',
                        'thumb_link':'http:\/\/photos3.meetupstatic.com\/photos\/event\/d\/e\/thumb_353100222.jpeg'
                    },
                    'join_mode':'open',
                    'organizer':{  
                        'member_id':141296792,
                        'name':'Matt Ross'
                    },
                    'members':185,
                    'name':'Leeds Sharp',
                    'id':13818462,
                    'state':'45',
                    'urlname':'Leeds-Sharp',
                    'category':{  
                        'name':'tech',
                        'id':34,
                        'shortname':'tech'
                    },
                    'lat':53.79999923706055,
                    'who':'LS#ers'
                }";
        }

        private static string TestGroupResponseFormat()
        {
            return @"{  
            'results':[  
                {EVENTS}
            ],
            'meta':{  
                'next':'',
                'method':'Groups',
                'total_count':1,
                'link':'http:\/\/api.meetup.com\/2\/groups',
                'count':1,
                'description':'',
                'lon':'',
                'title':'Meetup Groups v2',
                'url':'http:\/\/api.meetup.com\/2\/groups?offset=0&format=json&group_urlname=Leeds-Sharp&page=200&radius=25.0&fields=&key=12345&order=id&desc=false',
                'id':'',
                'updated':1431345304000,
                'lat':''
            }
        }";
        }

        #endregion

        [Test]
        public void GetGroup_WithValidGroupName_ReturnsGroupModel()
        {
            // Arrange
            var client = A.Fake<IRestClient>();
            var meetup = new MeetupClient(client);
            var response = new RestResponse { Content = TestGroupResponseFormat().Replace("{EVENTS}", TestGroupJson()) };
            response.Headers.Add(new Parameter { Name = "X-RateLimit-Limit", Value = 30 });
            response.Headers.Add(new Parameter { Name = "X-RateLimit-Remaining", Value = 30 });
            response.Headers.Add(new Parameter { Name = "X-RateLimit-Reset", Value = 30 });

            A.CallTo(() => client.Execute(A<IRestRequest>.Ignored))
                .Returns(response);

            // Act
            var result = meetup.Groups.Get(new GroupsRequest { GroupUrlName = "my-group" });
            var group = result.Results[0];

            // Assert
            group.Name.ShouldBeEquivalentTo("Leeds Sharp");
            group.Link.ShouldBeEquivalentTo("http://www.meetup.com/Leeds-Sharp/");
            group.Rating.ShouldBeEquivalentTo(4.94);
            group.Description.ShouldBeEquivalentTo("<p>Welcome to <a href='http://www.leeds-sharp.org/'>Leeds Sharp</a>, a code club for developers using Microsoft technologies founded 31 May 2012.<br>\n\n</p>\n<p>If you're a developer using or interested in programming with Microsoft technologies please come along and join us. All skill levels welcome from absolute beginner to high level genius guru Martin-Fowler-look-a-like.<br>\n\n</p>\n<p>Our meetings are on the last thursday of the month and start at 6:30pm.</p>");
            group.Lon.ShouldBeEquivalentTo(-1.559999942779541);
            group.GroupPhoto.HighresLink.ShouldBeEquivalentTo("http://photos3.meetupstatic.com/photos/event/d/e/highres_353100222.jpeg");
            group.GroupPhoto.PhotoId.ShouldBeEquivalentTo(353100222);
            group.GroupPhoto.PhotoLink.ShouldBeEquivalentTo("http://photos3.meetupstatic.com/photos/event/d/e/600_353100222.jpeg");
            group.GroupPhoto.ThumbLink.ShouldBeEquivalentTo("http://photos3.meetupstatic.com/photos/event/d/e/thumb_353100222.jpeg");
            group.JoinMode.ShouldBeEquivalentTo("open");
            group.Organizer.MemberId.ShouldBeEquivalentTo(141296792);
            group.Organizer.Name.ShouldBeEquivalentTo("Matt Ross");
            group.Members.ShouldBeEquivalentTo(185);
            group.Id.ShouldBeEquivalentTo(13818462);
            group.State.ShouldBeEquivalentTo("45");
            group.UrlName.ShouldBeEquivalentTo("Leeds-Sharp");
            group.Category.Name.ShouldBeEquivalentTo("tech");
            group.Category.Id.ShouldBeEquivalentTo(34);
            group.Category.Shortname.ShouldBeEquivalentTo("tech");
            group.Lat.ShouldBeEquivalentTo(53.79999923706055);
            group.Who.ShouldBeEquivalentTo("LS#ers");
        }
    }
}
