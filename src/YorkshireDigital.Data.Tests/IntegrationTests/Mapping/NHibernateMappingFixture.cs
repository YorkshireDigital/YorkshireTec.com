using FluentNHibernate.Testing;
using NUnit.Framework;
using SimpleAuthentication.Core;
using System;
using YorkshireDigital.Data.Domain.Account;
using YorkshireDigital.Data.Domain.Account.Enums;
using YorkshireDigital.Data.Domain.Events;
using YorkshireDigital.Data.Domain.Group;
using YorkshireDigital.Data.Domain.Shared;
using YorkshireDigital.Data.Helpers;

namespace YorkshireDigital.Data.Tests.IntegrationTests.Mappings
{
    [TestFixture]
    public class NHibernateMappingFixture : IntegrationFixtureBase
    {
        [SetUp]
        public void SetUp()
        {

        }

        [Test]
        public void ValidateMappings_Event()
        {
            new PersistenceSpecification<Event>(Session)
                .CheckProperty(x => x.UniqueName, "TestUniqueName")
                //.CheckProperty(x => x.Group, "TestGroup")
                .CheckProperty(x => x.Title, "TestTitle")
                .CheckProperty(x => x.Synopsis, "TestSynopsis")
                .CheckProperty(x => x.SynopsisFormat, TextFormat.Html)
                .CheckProperty(x => x.Start, DateTime.Now.TruncateToSeconds())
                .CheckProperty(x => x.End, DateTime.Now.TruncateToSeconds())
                .CheckProperty(x => x.Location, "TestLocation")
                .CheckProperty(x => x.Region, "TestRegion")
                .CheckProperty(x => x.Price, 1.0m)
                //.CheckProperty(x => x.Photo, "TestPhoto")
                //.CheckProperty(x => x.Categories, "TestCategories")
                //.CheckProperty(x => x.Interests, "TestInterests")
                //.CheckProperty(x => x.Talks, "TestTalks")
                .CheckProperty(x => x.LastEditedOn, DateTime.Now.TruncateToSeconds())
                //.CheckProperty(x => x.LastEditedBy, "TestLastEditedBy")
                .CheckProperty(x => x.DeletedOn, DateTime.Now.TruncateToSeconds())
                //.CheckProperty(x => x.DeletedBy, "TestDeletedBy")
                .CheckProperty(x => x.MeetupId, "TestMeetupId")
                .CheckProperty(x => x.EventSyncJobId, "TestEventSyncJobId")
                .VerifyTheMappings();
        }

        [Test]
        public void ValidateMappings_Group()
        {
            new PersistenceSpecification<Group>(Session)
                .CheckProperty(x => x.Id, "TestId")
                .CheckProperty(x => x.Name, "TestName")
                .CheckProperty(x => x.ShortName, "TestShortName")
                .CheckProperty(x => x.Headline, "TestHeadline")
                .CheckProperty(x => x.About, "TestAbout")
                .CheckProperty(x => x.AboutFormat, TextFormat.Html)
                .CheckProperty(x => x.Colour, "TestColour")
                //.CheckProperty(x => x.ContactLinks, "TestContactLinks")
                //.CheckProperty(x => x.Events, "TestEvents")
                //.CheckProperty(x => x.Logo, "TestLogo")
                //.CheckProperty(x => x.Photo, "TestPhoto")
                .CheckProperty(x => x.Website, "TestWebsite")
                .CheckProperty(x => x.LastEditedOn, DateTime.Now.TruncateToSeconds())
                //.CheckProperty(x => x.LastEditedBy, "TestLastEditedBy")
                .CheckProperty(x => x.DeletedOn, DateTime.Now.TruncateToSeconds())
                //.CheckProperty(x => x.DeletedBy, "TestDeletedBy")
                .CheckProperty(x => x.MeetupId, 12345)
                .CheckProperty(x => x.GroupSyncId, "TestGroupSyncId")
                .CheckProperty(x => x.MeetupUrlName, "TestMeetupUrlName")
                .VerifyTheMappings();
        }

        [Test]
        public void ValidateMappings_User()
        {
            new PersistenceSpecification<User>(Session)
                .CheckProperty(x => x.Username, "TestUsername")
                .CheckProperty(x => x.Password, "TestPassword")
                .CheckProperty(x => x.Name, "TestName")
                .CheckProperty(x => x.Email, "TestEmail")
                .CheckProperty(x => x.MailingListEmail, "TestMailingListEmail")
                .CheckProperty(x => x.Gender, GenderType.Male)
                .CheckProperty(x => x.Locale, "TestLocale")
                .CheckProperty(x => x.Picture, "TestPicture")
                .CheckProperty(x => x.Validated, true)
                .CheckProperty(x => x.MailingListState, MailingListState.Subscribed)
                .CheckProperty(x => x.LastEditedOn, DateTime.Now.TruncateToSeconds())
                .CheckProperty(x => x.DisabledOn, DateTime.Now.TruncateToSeconds())
                //.CheckProperty(x => x.Providers, "TestProviders")
                //.CheckProperty(x => x.Roles, "TestRoles")
                .VerifyTheMappings();
        }
    }
}
