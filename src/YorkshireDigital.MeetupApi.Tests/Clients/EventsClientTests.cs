namespace YorkshireDigital.MeetupApi.Tests.Clients
{
    using FakeItEasy;
    using FluentAssertions;
    using NUnit.Framework;
    using RestSharp;
    using YorkshireDigital.MeetupApi.Clients;
    using YorkshireDigital.MeetupApi.Models;
    using YorkshireDigital.MeetupApi.Requests;

    [TestFixture]
    public class EventsClientTests
    {
        #region Test Data
        private static string TestEventsResponse()
        {
            return @"
      {  
         'utc_offset':3600000,
         'venue':{  
            'country':'gb',
            'city':'Leeds',
            'address_1':'One Park Lane, LS3 1EP ',
            'name':'Callcredit Information Group Limited',
            'lon':-1.559184,
            'id':19447642,
            'lat':53.800026,
            'repinned':false
         },
         'rsvp_limit':50,
         'headcount':0,
         'visibility':'public',
         'waitlist_count':0,
         'created':1420727597000,
         'maybe_rsvp_count':0,
         'description':'<p><b>We\'re 3 years old this month. So come along, join the party and find out more about F#.<\/b><\/p> <p><br\/>This talk by <a href=\'http:\/\/trelford.com\/blog\/\'>Phil Trelford<\/a> is for C# programmers who are curious about F#, a new multi-paradigm programming language in Visual Studio.<\/p> <p>In: test driven development, classes and functions<\/p> <p>Out: maths, monads and moth-eaten jumpers<\/p>',
         'event_url':'http:\/\/www.meetup.com\/Leeds-Sharp\/events\/219692126\/',
         'yes_rsvp_count':20,
         'name':'F# Eye for the C# guy (+ 3rd Anniversary of LS#!)',
         'id':'219692126',
         'time':1432834200000,
         'updated':1431595402000,
         'group':{  
            'join_mode':'open',
            'created':1397043507000,
            'name':'Leeds Sharp',
            'group_lon':-1.559999942779541,
            'id':13818462,
            'urlname':'Leeds-Sharp',
            'group_lat':53.79999923706055,
            'who':'LS#ers'
         },
         'status':'upcoming'
      },
      {  
         'utc_offset':3600000,
         'venue':{  
            'country':'gb',
            'city':'Leeds',
            'address_1':'The Boulevard, Leeds Dock, LS10 1LR',
            'name':'The Google Digital Garage',
            'lon':-1.53237,
            'id':23833318,
            'lat':53.790497,
            'repinned':false
         },
         'rsvp_limit':20,
         'headcount':0,
         'visibility':'public',
         'waitlist_count':0,
         'created':1420730418000,
         'maybe_rsvp_count':0,
         'description':'<p><a href=\'http:\/\/codingdojo.org\/\'>&lt;\/a&gt;<\/p> <p>&lt;a href=\'http:\/\/codingdojo.org\/\'&gt;What is a coding dojo?<\/a><\/p> <p>All coding dojos will start off with a 25 minute <a href=\'http:\/\/en.wikipedia.org\/wiki\/Test-driven_development\'>TDD<\/a> and <a href=\'http:\/\/en.wikipedia.org\/wiki\/SOLID_%28object-oriented_design%29\'>Coding Principles<\/a> primer (the same each time, so if you don\'t understand all of it you will have plenty more opportunities to practise).<\/p> <p>This is the second of our quarterly code dojos. In this session we will attempt to create a Soduko solver.<\/p> <p><br\/>We will try and match coders into their preferrer language (C# or F# ... or VB if you dare) and also into their preferred testing methodology (TDD or BDD).<\/p> <p>Sessions are split into Pomodoros (25 minutes) giving each part of the pair a chance to code with some extra time for group discussions and planning.<\/p>',
         'event_url':'http:\/\/www.meetup.com\/Leeds-Sharp\/events\/219692955\/',
         'yes_rsvp_count':8,
         'name':'Coding Dojo: Soduko Solver',
         'id':'219692955',
         'time':1435253400000,
         'updated':1431598555000,
         'group':{  
            'join_mode':'open',
            'created':1397043507000,
            'name':'Leeds Sharp',
            'group_lon':-1.559999942779541,
            'id':13818462,
            'urlname':'Leeds-Sharp',
            'group_lat':53.79999923706055,
            'who':'LS#ers'
         },
         'status':'upcoming'
      },
      {  
         'utc_offset':3600000,
         'venue':{  
            'country':'gb',
            'city':'Leeds',
            'address_1':'The Boulevard, Leeds Dock, LS10 1LR',
            'name':'The Google Digital Garage',
            'lon':-1.53237,
            'id':23833318,
            'lat':53.790497,
            'repinned':false
         },
         'headcount':0,
         'visibility':'public',
         'waitlist_count':0,
         'created':1431583633000,
         'maybe_rsvp_count':0,
         'description':'<p>This is part 1 of <a href=\'http:\/\/richardtasker.co.uk\/\'>Richard Tasker<\/a>\'s Azure, The Good Parts series.<br\/>In this talk he will show you the features you can use to build web applications in Azure. He will also show you some alternative technologies you can use and how to get simple insights to your application.<\/p>',
         'how_to_find_us':'Opposite to the Royal Armouries Museum',
         'event_url':'http:\/\/www.meetup.com\/Leeds-Sharp\/events\/222523663\/',
         'yes_rsvp_count':3,
         'name':'Azure, The Good Parts: Web Apps',
         'id':'222523663',
         'time':1438277400000,
         'updated':1431583965000,
         'group':{  
            'join_mode':'open',
            'created':1397043507000,
            'name':'Leeds Sharp',
            'group_lon':-1.559999942779541,
            'id':13818462,
            'urlname':'Leeds-Sharp',
            'group_lat':53.79999923706055,
            'who':'LS#ers'
         },
         'status':'upcoming'
      },
      {  
         'utc_offset':3600000,
         'venue':{  
            'country':'gb',
            'city':'Leeds',
            'address_1':'One Park Lane, LS3 1EP ',
            'name':'Callcredit Information Group Limited',
            'lon':-1.559184,
            'id':19447642,
            'lat':53.800026,
            'repinned':false
         },
         'rsvp_limit':50,
         'headcount':0,
         'visibility':'public',
         'waitlist_count':0,
         'created':1420810737000,
         'maybe_rsvp_count':0,
         'description':'<p>LS#ers are invited to give a five to ten minute talk on a Design Pattern of their choice. We would like around ten of these, so come one LS#ers! We need you!<\/p> <p>We\'re planning to make this an annual session. The aim is to give everyone a refresh or learn about a new Design Pattern. There\'s nothing wrong with choosing one of the earlier, better known Design Patterns. You don\'t have to choose the latest, trendy Patterns.<\/p> <p>Checkout our <a href=\'http:\/\/www.meetup.com\/Leeds-Sharp\/messages\/boards\/thread\/48863546\'>list of design patterns<\/a> over on the discussion forum.<\/p>',
         'event_url':'http:\/\/www.meetup.com\/Leeds-Sharp\/events\/219715179\/',
         'yes_rsvp_count':12,
         'name':'Design Patterns Lightning Talks',
         'id':'219715179',
         'time':1440696600000,
         'updated':1428591791000,
         'group':{  
            'join_mode':'open',
            'created':1397043507000,
            'name':'Leeds Sharp',
            'group_lon':-1.559999942779541,
            'id':13818462,
            'urlname':'Leeds-Sharp',
            'group_lat':53.79999923706055,
            'who':'LS#ers'
         },
         'status':'upcoming'
      },
      {  
         'utc_offset':3600000,
         'venue':{  
            'country':'gb',
            'city':'Leeds',
            'address_1':'One Park Lane, LS3 1EP ',
            'name':'Callcredit Information Group Limited',
            'lon':-1.559184,
            'id':19447642,
            'lat':53.800026,
            'repinned':false
         },
         'rsvp_limit':40,
         'headcount':0,
         'visibility':'public',
         'waitlist_count':0,
         'created':1425332915000,
         'maybe_rsvp_count':0,
         'description':'<p>Data security is something that we as developers have to take seriously when developing solutions for our organizations. Cryptography can be a deeply complicated and mathematical subject but as developers we need to be pragmatic and use what is available to us to secure our data without disappearing down the mathematical rabbit hole.<\/p> <p>In this talk Stephen Haunts will take you through what is available in the .NET framework for enterprise desktop and server developers to allow you to securely protect your data to achieve confidentiality, data integrity and non-repudiation of exchanged data. Stephen will cover the following:<\/p> <p>• Cryptographically secure random number generation. <\/p> <p>• Hashing and Authenticated Hashes. <\/p> <p>• Secure Password Storage<\/p> <p><br\/>• Symmetric Encryption with DES, TripleDES, and AES. <\/p> <p>• Asymmetric Encryption with RSA. <\/p> <p>• Hybrid Encryption by using Symmetric and Asymmetric encryption together. <\/p> <p>• Digital Signatures. <\/p> <p>Stephen Haunts is a Development Manager working in the healthcare division at Boots and has been developing code since he was 10. Stephen is also an author with Pluralsight and a book author writing for the Syncfusion Succinctly series of books.<\/p>',
         'event_url':'http:\/\/www.meetup.com\/Leeds-Sharp\/events\/220877934\/',
         'yes_rsvp_count':6,
         'duration':9000000,
         'name':'Cryptography in .NET',
         'id':'220877934',
         'time':1443115800000,
         'updated':1425333278000,
         'group':{  
            'join_mode':'open',
            'created':1397043507000,
            'name':'Leeds Sharp',
            'group_lon':-1.559999942779541,
            'id':13818462,
            'urlname':'Leeds-Sharp',
            'group_lat':53.79999923706055,
            'who':'LS#ers'
         },
         'status':'upcoming'
      }";
        }

        private static string TestEventResponse()
        {
            return @"{  
         'utc_offset':3600000,
         'venue':{  
            'country':'gb',
            'city':'Leeds',
            'address_1':'One Park Lane, LS3 1EP ',
            'name':'Callcredit Information Group Limited',
            'lon':-1.559184,
            'id':19447642,
            'lat':53.800026,
            'repinned':false
         },
         'rsvp_limit':50,
         'headcount':0,
         'visibility':'public',
         'waitlist_count':0,
         'created':1420727597000,
         'maybe_rsvp_count':0,
         'description':'<p><b>We\'re 3 years old this month. So come along, join the party and find out more about F#.<\/b><\/p> <p><br\/>This talk by <a href=\'http:\/\/trelford.com\/blog\/\'>Phil Trelford<\/a> is for C# programmers who are curious about F#, a new multi-paradigm programming language in Visual Studio.<\/p> <p>In: test driven development, classes and functions<\/p> <p>Out: maths, monads and moth-eaten jumpers<\/p>',
         'event_url':'http:\/\/www.meetup.com\/Leeds-Sharp\/events\/219692126\/',
         'yes_rsvp_count':20,
         'name':'F# Eye for the C# guy (+ 3rd Anniversary of LS#!)',
         'id':'219692126',
         'time':1432834200000,
         'updated':1431595402000,
         'group':{  
            'join_mode':'open',
            'created':1397043507000,
            'name':'Leeds Sharp',
            'group_lon':-1.559999942779541,
            'id':13818462,
            'urlname':'Leeds-Sharp',
            'group_lat':53.79999923706055,
            'who':'LS#ers'
         },
         'status':'upcoming'
      }";
        }

        private static string TestResponseFormat()
        {
            return @"{  
            'results':[  
                {EVENTS}
            ],
            'meta':{  
                'next':'',
                'method':'Events',
                'total_count':5,
                'link':'http:\/\/api.meetup.com\/2\/events',
                'count':5,
                'description':'Access Meetup events using a group, member, or event id. Events in private groups are available only to authenticated members of those groups. To search events by topic or location, see [Open Events](\/meetup_api\/docs\/2\/open_events).',
                'lon':'',
                'title':'Meetup Events v2',
                'url':'http:\/\/api.meetup.com\/2\/events?offset=0&format=json&limited_events=False&text_format=html&group_id=13818462&page=200&fields=&key=952121b79781c682447161236c54&order=time&status=upcoming&desc=false',
                'id':'',
                'updated':1431598555000,
                'lat':''
            }
        }";
        }
        #endregion

        [Test]
        public void GetEvents_WithValidEventId_ReturnsEventModel()
        {
            // Arrange
            var client = A.Fake<IRestClient>();
            var meetup = new MeetupClient(client);
            var response = new RestResponse { Content = TestResponseFormat().Replace("{EVENTS}", TestEventResponse()) };

            A.CallTo(() => client.Execute(A<IRestRequest>.Ignored))
                .Returns(response);

            // Act

            var result = meetup.Events.Get(new EventsRequest { EventId = "12345" });
            Event @event = result.Results[0];

            // Assert
            @event.UtcOffset.ShouldBeEquivalentTo(3600000);
  
            @event.Venue.Country.ShouldBeEquivalentTo("gb");
            @event.Venue.City.ShouldBeEquivalentTo("Leeds");
            @event.Venue.Address1.ShouldBeEquivalentTo("One Park Lane, LS3 1EP ");
            @event.Venue.Name.ShouldBeEquivalentTo("Callcredit Information Group Limited");
            @event.Venue.Lon.ShouldBeEquivalentTo(-1.559184);
            @event.Venue.Id.ShouldBeEquivalentTo(19447642);
            @event.Venue.Lat.ShouldBeEquivalentTo(53.800026);
            @event.Venue.Repinned.ShouldBeEquivalentTo(false);

            @event.RsvpLimit.ShouldBeEquivalentTo(50);
            @event.Headcount.ShouldBeEquivalentTo(0);
            @event.Visibility.ShouldBeEquivalentTo("public");
            @event.WaitlistCount.ShouldBeEquivalentTo(0);
            @event.Created.ShouldBeEquivalentTo(1420727597000);
            @event.MaybeRsvpCount.ShouldBeEquivalentTo(0);
            @event.Description.ShouldBeEquivalentTo("<p><b>We're 3 years old this month. So come along, join the party and find out more about F#.</b></p> <p><br/>This talk by <a href='http://trelford.com/blog/'>Phil Trelford</a> is for C# programmers who are curious about F#, a new multi-paradigm programming language in Visual Studio.</p> <p>In: test driven development, classes and functions</p> <p>Out: maths, monads and moth-eaten jumpers</p>");
            @event.EventUrl.ShouldBeEquivalentTo("http://www.meetup.com/Leeds-Sharp/events/219692126/");
            @event.YesRsvpCount.ShouldBeEquivalentTo(20);
            @event.Name.ShouldBeEquivalentTo("F# Eye for the C# guy (+ 3rd Anniversary of LS#!)");
            @event.Id.ShouldBeEquivalentTo("219692126");
            @event.Time.ShouldBeEquivalentTo(1432834200000);
            @event.Updated.ShouldBeEquivalentTo(1431595402000);

            @event.Group.JoinMode.ShouldBeEquivalentTo("open");
            @event.Group.Created.ShouldBeEquivalentTo(1397043507000);
            @event.Group.Name.ShouldBeEquivalentTo("Leeds Sharp");
            @event.Group.GroupLon.ShouldBeEquivalentTo(-1.559999942779541);
            @event.Group.Id.ShouldBeEquivalentTo(13818462);
            @event.Group.UrlName.ShouldBeEquivalentTo("Leeds-Sharp");
            @event.Group.GroupLat.ShouldBeEquivalentTo(53.79999923706055);
            @event.Group.Who.ShouldBeEquivalentTo("LS#ers");

            @event.Status.ShouldBeEquivalentTo("upcoming");


        }

        [Test]
        public void GetEvents_ForGroupId_ReturnsListOfEvents()
        {
            // Arrange
            var client = A.Fake<IRestClient>();
            var meetup = new MeetupClient(client);
            var response = new RestResponse { Content = TestResponseFormat().Replace("{EVENTS}", TestEventsResponse()) };

            A.CallTo(() => client.Execute(A<IRestRequest>.Ignored))
                .Returns(response);

            // Act

            var result = meetup.Events.Get(new EventsRequest { GroupId = 12345 });
            var @events = result.Results;

            // Assert
            // Check all properties on events
            @events.Count.ShouldBeEquivalentTo(5);
        }
    }
}
