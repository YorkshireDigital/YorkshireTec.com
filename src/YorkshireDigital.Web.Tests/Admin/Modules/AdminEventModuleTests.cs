namespace YorkshireDigital.Web.Tests.Admin.Modules
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using FakeItEasy;
    using FluentAssertions;
    using Nancy;
    using Nancy.Cryptography;
    using Nancy.Security;
    using Nancy.Testing;
    using NUnit.Framework;
    using YorkshireDigital.Data.Domain.Account;
    using YorkshireDigital.Data.Domain.Events;
    using YorkshireDigital.Data.Domain.Organisations;
    using YorkshireDigital.Data.Services;
    using YorkshireDigital.Web.Admin.Modules;
    using YorkshireDigital.Web.Admin.ViewModels;
    using YorkshireDigital.Web.Infrastructure;
    using YorkshireDigital.Web.Infrastructure.Handlers;

    [TestFixture]
    public class AdminEventModuleTests
    {
        private IGroupService groupService;
        private IEventService eventService;
        private IUserService userService;
        private Browser browser;
        private string csrfToken;
        private User adminUser;

        [SetUp]
        public void SetUp()
        {
            userService = A.Fake<IUserService>();
            groupService = A.Fake<IGroupService>();
            eventService = A.Fake<IEventService>();

            var cryptographyConfiguration = CryptographyConfiguration.Default;

            var objectSerializer = new DefaultObjectSerializer();
            var csrfStartup = new CsrfApplicationStartup(cryptographyConfiguration, objectSerializer, new DefaultCsrfTokenValidator(cryptographyConfiguration));

            var token = new CsrfToken
            {
                CreatedDate = DateTime.Now,
            };
            token.CreateRandomBytes();
            token.CreateHmac(cryptographyConfiguration.HmacProvider);

            csrfToken = new DefaultObjectSerializer().Serialize(token);

            adminUser = new User
            {
                Id = Guid.NewGuid(),
                Username = "admin"
            };

            browser = new Browser(with =>
            {
                with.Module<AdminEventModule>();
                with.ViewFactory<ApiViewFactory>();
                with.Dependency(groupService);
                with.Dependency(eventService);
                with.Dependency(userService);
                with.RequestStartup((container, pipelines, context) =>
                {
                    context.CurrentUser = new UserIdentity { UserId = adminUser.Id.ToString(), UserName = adminUser.Username, Claims = new[] { "Admin" } };
                    pipelines.OnError += (ctx, exception) =>
                    {
                        ctx.Items.Add("OnErrorException", exception);
                        return null;
                    };
                    csrfStartup.Initialize(pipelines);
                    Csrf.Enable(pipelines);

                });
                with.StatusCodeHandler<InternalServerErrorStatusCodeHandler>();
            });
        }

        [Test]
        public void GetRequest_NoParameters_ReturnsViewWithNewEventModel()
        {
            // Arrange
            A.CallTo(() => eventService.GetInterests())
                .Returns(new List<Interest>
                {
                    new Interest { Id = 1, Name = "Development"},
                    new Interest { Id = 2, Name = "Design"}
                });

            // Act
            var response = browser.Get("/admin/event", with => with.HttpRequest());
            var model = response.GetModel<AdminEventViewModel>();

            // Assert
            response.GetViewName().ShouldBeEquivalentTo("NewEvent");
            model.UniqueName.ShouldBeEquivalentTo(null);
            model.Start.ShouldBeEquivalentTo(DateTime.Today);
            model.End.ShouldBeEquivalentTo(DateTime.Today);
            model.AvailableInterests.Count.ShouldBeEquivalentTo(2);
            model.AvailableInterests[0].Name.ShouldBeEquivalentTo("Development");
            model.AvailableInterests[1].Name.ShouldBeEquivalentTo("Design");
            model.Talks.Count.ShouldBeEquivalentTo(1);
        }

        [Test]
        public void GetRequest_WithGroupId_ReturnsViewWithNewEventModel_WithGroupIdPopulated()
        {
            // Arrange
            A.CallTo(() => groupService.Get("existing-group"))
                .Returns(new Group
                {
                    Id = "existing-group",
                    Name = "Existing Group"
                });
            A.CallTo(() => eventService.GetInterests())
                .Returns(new List<Interest>
                {
                    new Interest { Id = 1, Name = "Development"},
                    new Interest { Id = 2, Name = "Design"}
                });

            // Act
            var response = browser.Get("/admin/event", with =>
            {
                with.HttpRequest();
                with.Query("groupId", "existing-group");
            });
            
            var model = response.GetModel<AdminEventViewModel>();

            // Assert
            response.GetViewName().ShouldBeEquivalentTo("NewEvent");
            model.GroupId.ShouldBeEquivalentTo("existing-group");
            model.UniqueName.ShouldBeEquivalentTo(string.Format("existing-group-{0}-{1}", DateTime.Now.ToString("MMM").ToLower(), @DateTime.Now.ToString("yyyy")));
            model.AvailableInterests.Count.ShouldBeEquivalentTo(2);
            model.AvailableInterests[0].Name.ShouldBeEquivalentTo("Development");
            model.AvailableInterests[1].Name.ShouldBeEquivalentTo("Design");
        }

        [Test]
        public void GetRequest_WithInvalidGroupId_ReturnsViewWithNewEventModel_WithGroupIdError()
        {
            // Arrange
            A.CallTo(() => groupService.Get("invalid-group"))
                .Returns(null);

            // Act
            var response = browser.Get("/admin/event", with =>
            {
                with.HttpRequest();
                with.Query("groupId", "invalid-group");
            });
            var model = response.GetModel<AdminEventViewModel>();

            // Assert
            response.GetViewName().ShouldBeEquivalentTo("NewEvent");
            model.UniqueName.ShouldBeEquivalentTo(null);
            model.GroupId.ShouldBeEquivalentTo(null);

            var errors = response.Context.ViewBag["Errors"].Value as IDictionary<string, List<string>>;
            Assert.NotNull(errors);

            errors["GroupId"].Count.ShouldBeEquivalentTo(1);
            errors["GroupId"].First().ShouldBeEquivalentTo("No group exists with this id.");
        }

        [Test]
        public void GetRequest_WithValidEventId_ReturnsViewWithEventDetails()
        {
            // Arrange
            var start = DateTime.UtcNow.AddHours(-1);
            var end = DateTime.UtcNow.AddHours(1);

            A.CallTo(() => eventService.GetInterests())
                .Returns(new List<Interest>
                {
                    new Interest { Id = 1, Name = "Development"},
                    new Interest { Id = 2, Name = "Design"}
                });

            A.CallTo(() => eventService.Get("existing-event"))
                .Returns(new Event
                {
                    UniqueName = "existing-event",
                    Title = "Existing Event",
                    Synopsis = "Existing event details",
                    Start = start,
                    End = end,
                    Location = "Venue X",
                    Region = "Leeds",
                    Price = 1.2m,
                    Group = new Group
                    {
                        Id = "existing-group",
                        Name = "Existing Group"
                    },
                    Interests = new List<Interest>
                    {
                        new Interest {Id = 1, Name = "Development"}
                    },
                    Talks = new List<EventTalk>
                    {
                        new EventTalk { Id = 1, Link = "http://google.com", Speaker = "Bob", Title = "Super talk", Synopsis = "Super talk details"}
                    }
                });

            // Act
            var response = browser.Get("/admin/event/existing-event", with => with.HttpRequest());
            var model = response.GetModel<AdminEventViewModel>();

            // Assert
            response.GetViewName().ShouldBeEquivalentTo("Event");
            model.UniqueName.ShouldBeEquivalentTo("existing-event");
            model.Title.ShouldBeEquivalentTo("Existing Event");
            model.Synopsis.ShouldBeEquivalentTo("Existing event details");
            model.Start.ShouldBeEquivalentTo(start);
            model.End.ShouldBeEquivalentTo(end);
            model.Location.ShouldBeEquivalentTo("Venue X");
            model.Region.ShouldBeEquivalentTo("Leeds");
            model.Price.ShouldBeEquivalentTo(1.2m);
            model.GroupName.ShouldBeEquivalentTo("Existing Group");
            model.GroupId.ShouldBeEquivalentTo("existing-group");
            model.AvailableInterests.Count.ShouldBeEquivalentTo(2);
            model.AvailableInterests[0].Name.ShouldBeEquivalentTo("Development");
            model.AvailableInterests[0].Selected.Should().BeTrue();
            model.AvailableInterests[1].Name.ShouldBeEquivalentTo("Design");
            model.AvailableInterests[1].Selected.Should().BeFalse();
            model.Talks.Count.ShouldBeEquivalentTo(1);
            model.Talks[0].Id.ShouldBeEquivalentTo(1);
            model.Talks[0].Link.ShouldBeEquivalentTo("http://google.com");
            model.Talks[0].Speaker.ShouldBeEquivalentTo("Bob");
            model.Talks[0].Title.ShouldBeEquivalentTo("Super talk");
            model.Talks[0].Synopsis.ShouldBeEquivalentTo("Super talk details");
        }

        [Test]
        public void GetRequest_WithInvalidEventId_Returns404()
        {
            // Arrange
            A.CallTo(() => eventService.Get("invalid-event")).Returns(null);

            // Act
            var response = browser.Get("/admin/event/invalid-event", with => with.HttpRequest());

            // Assert
            response.StatusCode.ShouldBeEquivalentTo(HttpStatusCode.NotFound);
        }

        [Test]
        public void PostRequest_CreatesNewEvent()
        {
            // Arrange
            A.CallTo(() => eventService.Get("new-event"))
                .Returns(null);
            A.CallTo(() => groupService.Get("existing-group"))
                .Returns(new Group
                {
                    Id = "existing-group",
                    Name = "Existing Group"
                });
            A.CallTo(() => userService.GetUser("admin"))
                .Returns(adminUser);
            A.CallTo(() => eventService.GetInterests())
                .Returns(new List<Interest>
                {
                    new Interest { Id = 1, Name = "Development"},
                    new Interest { Id = 2, Name = "Design"}
                });
            var start = DateTime.UtcNow.AddHours(-1);
            var end = DateTime.UtcNow.AddHours(1);

            // Act
            var response = browser.Post("/admin/event/", with =>
            {
                with.HttpRequest();
                with.FormValue("UniqueName", "new-event");
                with.FormValue("Title", "New Event");
                with.FormValue("Synopsis", "New event details");
                with.FormValue("Start", start.ToString("yyyy-MM-dd hh:mm"));
                with.FormValue("End", end.ToString("yyyy-MM-dd hh:mm"));
                with.FormValue("Location", "Venue X");
                with.FormValue("Region", "Leeds");
                with.FormValue("Price", "1.2");
                with.FormValue("GroupId", "existing-group");
                with.FormValue("Interests", "1,2");
                with.Cookie(CsrfToken.DEFAULT_CSRF_KEY, csrfToken);
                with.FormValue(CsrfToken.DEFAULT_CSRF_KEY, csrfToken);
            });

            // Assert
            A.CallTo(() => eventService.Save(A<Event>.Ignored, adminUser)).MustHaveHappened();
        }

        [Test]
        public void PostRequest_WithExistingEventId_ReturnsAnError()
        {
            // Arrange
            A.CallTo(() => eventService.Get("existing-event"))
                .Returns(new Event
                {
                    UniqueName = "existing-event",
                });

            // Act
            var response = browser.Post("/admin/event/", with =>
            {
                with.HttpRequest();
                with.FormValue("UniqueName", "existing-event");
                with.Cookie(CsrfToken.DEFAULT_CSRF_KEY, csrfToken);
                with.FormValue(CsrfToken.DEFAULT_CSRF_KEY, csrfToken);
            });

            // Assert
            response.StatusCode.ShouldBeEquivalentTo(HttpStatusCode.BadRequest);

            var errors = response.Context.ViewBag["Errors"].Value as IDictionary<string, List<string>>;
            Assert.NotNull(errors);

            errors["UniqueName"].Count.ShouldBeEquivalentTo(1);
            errors["UniqueName"].First().ShouldBeEquivalentTo("An event already exists with this unique name.");
        }

        [Test]
        public void PostRequest_WithInvalidGroupId_ReturnsAnError()
        {
            // Arrange
            A.CallTo(() => eventService.Get("new-event"))
                .Returns(null);
            A.CallTo(() => groupService.Get("invalid-group"))
                .Returns(null);

            // Act
            var response = browser.Post("/admin/event/", with =>
            {
                with.HttpRequest();
                with.FormValue("UniqueName", "new-event");
                with.FormValue("GroupId", "invalid-group");
                with.Cookie(CsrfToken.DEFAULT_CSRF_KEY, csrfToken);
                with.FormValue(CsrfToken.DEFAULT_CSRF_KEY, csrfToken);
            });

            // Assert
            response.StatusCode.ShouldBeEquivalentTo(HttpStatusCode.BadRequest);

            var errors = response.Context.ViewBag["Errors"].Value as IDictionary<string, List<string>>;
            Assert.NotNull(errors);

            errors["GroupId"].Count.ShouldBeEquivalentTo(1);
            errors["GroupId"].First().ShouldBeEquivalentTo("No group exists with this id.");
        }

        // TODO: Test validation

        [Test]
        public void PostRequest_WithEventId_UpdatesEvent()
        {
            // Arrange
            A.CallTo(() => eventService.EventExists(A<string>.Ignored))
                .Returns(true);
            A.CallTo(() => userService.GetUser("admin"))
                .Returns(adminUser);

            // Act
            var response = browser.Post("/admin/event/existing-event", with =>
            {
                with.HttpRequest();
                with.FormValue("UniqueName", "existing-event");
                with.FormValue("Title", "Updated Event");
                with.FormValue("Synopsis", "Updated event details");
                with.FormValue("Start", "2015-01-02 12:00");
                with.FormValue("End", "2015-01-02 13:00");
                with.FormValue("Location", "Venue Y");
                with.FormValue("Region", "York");
                with.FormValue("Price", "2.2");
                with.Cookie(CsrfToken.DEFAULT_CSRF_KEY, csrfToken);
                with.FormValue(CsrfToken.DEFAULT_CSRF_KEY, csrfToken);
            });

            // Assert
            A.CallTo(() => eventService.Save(A<Event>.Ignored, adminUser)).MustHaveHappened();
        }

        [Test]
        public void PostRequest_WithInvalidEventId_Returns404()
        {
            // Arrange
            A.CallTo(() => eventService.Get("invalid-event"))
                .Returns(null);

            // Act
            var response = browser.Post("/admin/event/invalid-event", with =>
            {
                with.HttpRequest();
                with.Cookie(CsrfToken.DEFAULT_CSRF_KEY, csrfToken);
                with.FormValue(CsrfToken.DEFAULT_CSRF_KEY, csrfToken);
            });

            // Assert
            response.StatusCode.ShouldBeEquivalentTo(HttpStatusCode.NotFound);
        }

        [Test]
        public void DeleteRequest_WithValidEventId_MarksEventDeleted()
        {
            // Arrange
            A.CallTo(() => eventService.Get("existing-event"))
                .Returns(new Event
                {
                    UniqueName = "existing-event",
                });
            A.CallTo(() => userService.GetUser("admin"))
                .Returns(adminUser);

            // Act
            var response = browser.Delete("/admin/event/existing-event", with => with.HttpRequest());

            // Assert
            response.StatusCode.ShouldBeEquivalentTo(HttpStatusCode.OK);
            A.CallTo(() => eventService.Delete("existing-event", adminUser)).MustHaveHappened();
        }

        [Test]
        public void DeleteRequest_WithInvalidEventId_Returns404()
        {
            // Arrange
            A.CallTo(() => eventService.Get("invalid-event"))
                .Returns(null);

            // Act
            var response = browser.Delete("/admin/event/invalid-event", with => with.HttpRequest());

            // Assert
            response.StatusCode.ShouldBeEquivalentTo(HttpStatusCode.NotFound);
        }

        [Test]
        public void DeleteRequest_NoParams_Returns404()
        {
            // Arrange

            // Act
            var response = browser.Delete("/admin/event/", with => with.HttpRequest());

            // Assert
            response.StatusCode.ShouldBeEquivalentTo(HttpStatusCode.NotFound);
        }

    }
}
