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
    using YorkshireDigital.Data.Domain.Events;
    using YorkshireDigital.Data.Services;
    using YorkshireDigital.Web.Admin.Modules;
    using YorkshireDigital.Web.Admin.ViewModels;
    using YorkshireDigital.Web.Infrastructure;
    using YorkshireDigital.Web.Infrastructure.Handlers;

    [TestFixture]
    public class AdminEventModuleTests
    {
        //private IGroupService groupService;
        private IEventService eventService;
        private Browser browser;
        private string csrfToken;

        [SetUp]
        public void SetUp()
        {
            //groupService = A.Fake<IGroupService>();
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

            browser = new Browser(with =>
            {
                with.Module<AdminEventModule>();
                with.ViewFactory<ApiViewFactory>();
                //with.Dependency(groupService);
                with.Dependency(eventService);
                with.RequestStartup((container, pipelines, context) =>
                {
                    context.CurrentUser = new UserIdentity { UserName = "admin", Claims = new[] { "Admin" } };
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

            // Act
            var response = browser.Get("/admin/event", with => with.HttpRequest());
            var model = response.GetModel<AdminEventViewModel>();

            // Assert
            response.GetViewName().ShouldBeEquivalentTo("NewEvent");
            model.UniqueName.ShouldBeEquivalentTo(null);
        }

        [Test]
        public void GetRequest_WithValidEventId_ReturnsViewWithEventDetails()
        {
            // Arrange
            var start = DateTime.UtcNow.AddHours(-1);
            var end = DateTime.UtcNow.AddHours(1);

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
                    Price = 1.2m
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
                with.Cookie(CsrfToken.DEFAULT_CSRF_KEY, csrfToken);
                with.FormValue(CsrfToken.DEFAULT_CSRF_KEY, csrfToken);
            });
            var model = response.GetModel<AdminEventViewModel>();

            // Assert
            response.StatusCode.ShouldBeEquivalentTo(HttpStatusCode.Created);
            A.CallTo(() => eventService.Save(A<Event>.Ignored)).MustHaveHappened();
            model.UniqueName.ShouldBeEquivalentTo("new-event");
            model.Title.ShouldBeEquivalentTo("New Event");
            model.Synopsis.ShouldBeEquivalentTo("New event details");
            model.Start.ShouldBeEquivalentTo(DateTime.Parse(start.ToString("yyyy-MM-dd hh:mm")));
            model.End.ShouldBeEquivalentTo(DateTime.Parse(end.ToString("yyyy-MM-dd hh:mm")));
            model.Location.ShouldBeEquivalentTo("Venue X");
            model.Region.ShouldBeEquivalentTo("Leeds");
            model.Price.ShouldBeEquivalentTo(1.2m);
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

        // TODO: Test validation

        [Test]
        public void PostRequest_WithEventId_UpdatesEvent()
        {
            // Arrange
            var start = DateTime.UtcNow.AddHours(-1);
            var end = DateTime.UtcNow.AddHours(1);
            A.CallTo(() => eventService.Get("existing-event"))
                .Returns(new Event
                {
                    UniqueName = "existing-event",
                    Title = "Existing Event",
                    Synopsis = "Existing event details",
                    Start = DateTime.UtcNow,
                    End = DateTime.UtcNow,
                    Location = "Venue X",
                    Region = "Leeds",
                    Price = 1.2m
                });

            // Act
            var response = browser.Post("/admin/event/existing-event", with =>
            {
                with.HttpRequest();
                with.FormValue("UniqueName", "existing-event");
                with.FormValue("Title", "Updated Event");
                with.FormValue("Synopsis", "Updated event details");
                with.FormValue("Start", start.ToString("yyyy-MM-dd hh:mm"));
                with.FormValue("End", end.ToString("yyyy-MM-dd hh:mm"));
                with.FormValue("Location", "Venue Y");
                with.FormValue("Region", "York");
                with.FormValue("Price", "2.2");
                with.Cookie(CsrfToken.DEFAULT_CSRF_KEY, csrfToken);
                with.FormValue(CsrfToken.DEFAULT_CSRF_KEY, csrfToken);
            });
            var model = response.GetModel<AdminEventViewModel>();

            // Assert
            response.StatusCode.ShouldBeEquivalentTo(HttpStatusCode.OK);
            A.CallTo(() => eventService.Save(A<Event>.Ignored)).MustHaveHappened();
            model.UniqueName.ShouldBeEquivalentTo("existing-event");
            model.Title.ShouldBeEquivalentTo("Updated Event");
            model.Synopsis.ShouldBeEquivalentTo("Updated event details");
            model.Start.ShouldBeEquivalentTo(DateTime.Parse(start.ToString("yyyy-MM-dd hh:mm")));
            model.End.ShouldBeEquivalentTo(DateTime.Parse(end.ToString("yyyy-MM-dd hh:mm")));
            model.Location.ShouldBeEquivalentTo("Venue Y");
            model.Region.ShouldBeEquivalentTo("York");
            model.Price.ShouldBeEquivalentTo(2.2m);
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

            // Act
            var response = browser.Delete("/admin/event/existing-event", with => with.HttpRequest());

            // Assert
            response.StatusCode.ShouldBeEquivalentTo(HttpStatusCode.OK);
            A.CallTo(() => eventService.Delete("existing-event")).MustHaveHappened();
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
