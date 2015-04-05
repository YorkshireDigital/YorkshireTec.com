namespace YorkshireDigital.Web.Tests.Admin.Modules
{
    using System;
    using System.Collections.Generic;
    using FakeItEasy;
    using FluentAssertions;
    using Nancy.Testing;
    using NUnit.Framework;
    using YorkshireDigital.Data.Domain.Account;
    using YorkshireDigital.Data.Domain.Account.Enums;
    using YorkshireDigital.Data.Domain.Events;
    using YorkshireDigital.Data.Domain.Organisations;
    using YorkshireDigital.Data.Services;
    using YorkshireDigital.Web.Admin.Modules;
    using YorkshireDigital.Web.Admin.ViewModels;
    using YorkshireDigital.Web.Infrastructure;
    using YorkshireDigital.Web.Infrastructure.Handlers;

    [TestFixture]
    public class AdminModuleTests
    {
        private Browser browser;
        private IUserService userService;
        private IEventService eventService;
        private IGroupService groupService;

        [SetUp]
        public void SetUp()
        {
            userService = A.Fake<IUserService>();
            eventService = A.Fake<IEventService>();
            groupService = A.Fake<IGroupService>();

            browser = new Browser(with =>
            {
                with.Module<AdminModule>();
                with.ViewFactory<ApiViewFactory>();
                with.Dependency(userService);
                with.Dependency(eventService);
                with.Dependency(groupService);
                with.RequestStartup((container, pipelines, context) =>
                {
                    context.CurrentUser = new UserIdentity { UserName = "admin", Claims = new [] { "Admin" }};
                    pipelines.OnError += (ctx, exception) =>
                    {
                        ctx.Items.Add("OnErrorException", exception);
                        return null;
                    };
                });
                with.StatusCodeHandler<InternalServerErrorStatusCodeHandler>();
            });
        }

        [Test]
        public void GetRequestToAdmin_ModelContainsAddedUsers()
        {
            // Arrange
            A.CallTo(() => userService.GetActiveUsers(A<int>.Ignored, A<int>.Ignored))
                .Returns(new List<User>
                {
                    new User {Username = "User1"}, 
                    new User {Username = "User2", Name = "User 2", Email = "user2@email.com", MailingListState = MailingListState.Subscribed }
                });

            // Act
            var response = browser.Get("/admin", with => with.HttpRequest());
            var model = response.GetModel<AdminIndexViewModel>();

            // Assert
            model.Users.Count.ShouldBeEquivalentTo(2);
            model.Users[0].Username.ShouldBeEquivalentTo("User1");
            model.Users[1].Username.ShouldBeEquivalentTo("User2");
            model.Users[1].Name.ShouldBeEquivalentTo("User 2");
            model.Users[1].Email.ShouldBeEquivalentTo("user2@email.com");
            model.Users[1].MailingListState.ShouldBeEquivalentTo(MailingListState.Subscribed);
        }

        [Test]
        public void GetRequestToAdmin_ModelContainsAddedGroups()
        {
            // Arrange
            A.CallTo(() => groupService.GetActiveGroups(A<int>.Ignored, A<int>.Ignored))
                .Returns(new List<Group>
                {
                    new Group { Id = "Group1"}, 
                    new Group { Id = "Group2", Name = "Group 2", ShortName = "GP2", Colour = "yellow"}
                });

            // Act
            var response = browser.Get("/admin", with => with.HttpRequest());
            var model = response.GetModel<AdminIndexViewModel>();

            // Assert
            model.Groups.Count.ShouldBeEquivalentTo(2);
            model.Groups[0].Id.ShouldBeEquivalentTo("Group1");
            model.Groups[1].Id.ShouldBeEquivalentTo("Group2");
            model.Groups[1].Name.ShouldBeEquivalentTo("Group 2");
            model.Groups[1].ShortName.ShouldBeEquivalentTo("GP2");
            model.Groups[1].Colour.ShouldBeEquivalentTo("yellow");
        }

        [Test]
        public void GetRequestToAdmin_ModelContainsAddedFutureEvents()
        {
            // Arrange
            var startDate = DateTime.Now.AddDays(1);
            A.CallTo(() => eventService.Query(A<DateTime?>.Ignored, A<DateTime?>.Ignored, A<string[]>.Ignored, A<string[]>.Ignored,
                        A<int?>.Ignored, A<int?>.Ignored, A<bool>.Ignored))
                .Returns(new List<Event>
                {
                    new Event { UniqueName = "Event1", Start = startDate }, 
                    new Event { UniqueName = "Event2", Start = startDate, Title = "Event 2", Region = "The Internet", Group = new Group { Name = "Group Name"}}, 
                    new Event { UniqueName = "PastEvent", Start = DateTime.Now.AddDays(-1) }
                });

            // Act
            var response = browser.Get("/admin", with => with.HttpRequest());
            var model = response.GetModel<AdminIndexViewModel>();

            // Assert
            model.FutureEvents.Count.ShouldBeEquivalentTo(2);
            model.FutureEvents[0].UniqueName.ShouldBeEquivalentTo("Event1");
            model.FutureEvents[1].UniqueName.ShouldBeEquivalentTo("Event2");
            model.FutureEvents[1].Title.ShouldBeEquivalentTo("Event 2");
            model.FutureEvents[1].Start.ShouldBeEquivalentTo(startDate);
            model.FutureEvents[1].Region.ShouldBeEquivalentTo("The Internet");
            model.FutureEvents[1].GroupName.ShouldBeEquivalentTo("Group Name");
        }

        [Test]
        public void GetRequestToAdmin_ModelContainsAddedPastEvents()
        {
            // Arrange
            var startDate = DateTime.Now.AddDays(-1);
            A.CallTo(() => eventService.Query(A<DateTime?>.Ignored, A<DateTime?>.Ignored, A<string[]>.Ignored, A<string[]>.Ignored,
                        A<int?>.Ignored, A<int?>.Ignored, A<bool>.Ignored))
                .Returns(new List<Event>
                {
                    new Event { UniqueName = "Event1", Start = startDate }, 
                    new Event { UniqueName = "Event2", Start = startDate, Title = "Event 2", Region = "The Internet", Group = new Group { Name = "Group Name"} }, 
                    new Event { UniqueName = "FutureEvent", Start = DateTime.Now.AddDays(1) }
                });

            // Act
            var response = browser.Get("/admin", with => with.HttpRequest());
            var model = response.GetModel<AdminIndexViewModel>();

            // Assert
            model.PastEvents.Count.ShouldBeEquivalentTo(2);
            model.PastEvents[0].UniqueName.ShouldBeEquivalentTo("Event1");
            model.PastEvents[1].UniqueName.ShouldBeEquivalentTo("Event2");
            model.PastEvents[1].Title.ShouldBeEquivalentTo("Event 2");
            model.PastEvents[1].Start.ShouldBeEquivalentTo(startDate);
            model.PastEvents[1].Region.ShouldBeEquivalentTo("The Internet");
            model.PastEvents[1].GroupName.ShouldBeEquivalentTo("Group Name");
        }
    }
}
