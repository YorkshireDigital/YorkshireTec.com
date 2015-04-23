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
    using YorkshireDigital.Data.Domain.Organisations;
    using YorkshireDigital.Data.Services;
    using YorkshireDigital.Web.Admin.Modules;
    using YorkshireDigital.Web.Admin.ViewModels;
    using YorkshireDigital.Web.Infrastructure;
    using YorkshireDigital.Web.Infrastructure.Handlers;

    [TestFixture]
    public class AdminGroupModuleTests
    {
        private IGroupService groupService;
        private Browser browser;
        private string csrfToken;

        [SetUp]
        public void SetUp()
        {
            groupService = A.Fake<IGroupService>();

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
                with.Module<AdminGroupModule>();
                with.ViewFactory<ApiViewFactory>();
                with.Dependency(groupService);
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
        public void GetRequest_NoParameters_ReturnsViewWithNewGroupModel()
        {
            // Arrange

            // Act
            var response = browser.Get("/admin/group", with => with.HttpRequest());
            var model = response.GetModel<AdminGroupViewModel>();

            // Assert
            response.GetViewName().ShouldBeEquivalentTo("NewGroup");
            model.Id.ShouldBeEquivalentTo(null);
        }

        [Test]
        public void GetRequest_WithValidGroupId_ReturnsViewWithGroupDetails()
        {
            // Arrange
            A.CallTo(() => groupService.Get("existing-group"))
                .Returns(new Group
                {
                    Id = "existing-group",
                    Name = "Existing Group",
                    ShortName = "EXT",
                    About = "This is an existing group",
                    Colour = "#FF00FF",
                    Headline = "Existing group for testing"
                });

            // Act
            var response = browser.Get("/admin/group/existing-group", with => with.HttpRequest());
            var model = response.GetModel<AdminGroupViewModel>();

            // Assert
            response.GetViewName().ShouldBeEquivalentTo("Group");
            model.Id.ShouldBeEquivalentTo("existing-group");
            model.Name.ShouldBeEquivalentTo("Existing Group");
            model.ShortName.ShouldBeEquivalentTo("EXT");
            model.About.ShouldBeEquivalentTo("This is an existing group");
            model.Colour.ShouldBeEquivalentTo("#FF00FF");
            model.Headline.ShouldBeEquivalentTo("Existing group for testing");
        }

        [Test]
        public void GetRequest_WithInvalidGroupId_Returns404()
        {
            // Arrange
            A.CallTo(() => groupService.Get("invalid-group")).Returns(null);

            // Act
            var response = browser.Get("/admin/group/invalid-group", with => with.HttpRequest());

            // Assert
            response.StatusCode.ShouldBeEquivalentTo(HttpStatusCode.NotFound);
        }

        [Test]
        public void PostRequest_CreatesNewGroup()
        {
            // Arrange
            A.CallTo(() => groupService.Get("new-group"))
                .Returns(null);

            // Act
            var response = browser.Post("/admin/group/", with =>
            {
                with.HttpRequest();
                with.FormValue("Id", "new-group");
                with.FormValue("Name", "New Group");
                with.FormValue("ShortName", "New");
                with.FormValue("About", "This is a new group");
                with.FormValue("Colour", "#FFFF00");
                with.FormValue("Headline", "New group for testing");
                with.Cookie(CsrfToken.DEFAULT_CSRF_KEY, csrfToken);
                with.FormValue(CsrfToken.DEFAULT_CSRF_KEY, csrfToken);
            });
            var model = response.GetModel<AdminGroupViewModel>();

            // Assert
            response.StatusCode.ShouldBeEquivalentTo(HttpStatusCode.Created);
            A.CallTo(() => groupService.Save(A<Group>.Ignored)).MustHaveHappened();
            model.Id.ShouldBeEquivalentTo("new-group");
            model.Name.ShouldBeEquivalentTo("New Group");
            model.ShortName.ShouldBeEquivalentTo("New");
            model.About.ShouldBeEquivalentTo("This is a new group");
            model.Colour.ShouldBeEquivalentTo("#FFFF00");
            model.Headline.ShouldBeEquivalentTo("New group for testing");
        }

        [Test]
        public void PostRequest_WithExistingGroupId_ReturnsAnError()
        {
            // Arrange
            A.CallTo(() => groupService.Get("existing-group"))
                .Returns(new Group
                {
                    Id = "existing-group",
                });

            // Act
            var response = browser.Post("/admin/group/", with =>
            {
                with.HttpRequest();
                with.Cookie(CsrfToken.DEFAULT_CSRF_KEY, csrfToken);
                with.FormValue(CsrfToken.DEFAULT_CSRF_KEY, csrfToken);
            });

            // Assert
            response.StatusCode.ShouldBeEquivalentTo(HttpStatusCode.BadRequest);

            var errors = response.Context.ViewBag["Errors"].Value as IDictionary<string, List<string>>;
            Assert.NotNull(errors);

            errors["Id"].Count.ShouldBeEquivalentTo(1);
            errors["Id"].First().ShouldBeEquivalentTo("A group already exists with this unique name.");
        }

        // TODO: Test validation

        [Test]
        public void PutRequest_WithGroupId_UpdatesGroup()
        {
            // Arrange
            A.CallTo(() => groupService.Get("existing-group"))
                .Returns(new Group
                {
                    Id = "existing-group",
                    Name = "Existing Group",
                    ShortName = "EXT",
                    About = "This is an existing group",
                    Colour = "#FF00FF",
                    Headline = "Existing group for testing"
                });

            // Act
            var response = browser.Put("/admin/group/existing-group", with =>
            {
                with.HttpRequest();
                with.FormValue("Id", "existing-group");
                with.FormValue("Name", "Updated Group");
                with.FormValue("ShortName", "UPD");
                with.FormValue("About", "This is an updated group");
                with.FormValue("Colour", "#00FF00");
                with.FormValue("Headline", "Updated group for testing");
                with.Cookie(CsrfToken.DEFAULT_CSRF_KEY, csrfToken);
                with.FormValue(CsrfToken.DEFAULT_CSRF_KEY, csrfToken);
            });
            var model = response.GetModel<AdminGroupViewModel>();

            // Assert
            response.StatusCode.ShouldBeEquivalentTo(HttpStatusCode.OK);
            A.CallTo(() => groupService.Save(A<Group>.Ignored)).MustHaveHappened();
            model.Id.ShouldBeEquivalentTo("existing-group");
            model.Name.ShouldBeEquivalentTo("Updated Group");
            model.ShortName.ShouldBeEquivalentTo("UPD");
            model.About.ShouldBeEquivalentTo("This is an updated group");
            model.Colour.ShouldBeEquivalentTo("#00FF00");
            model.Headline.ShouldBeEquivalentTo("Updated group for testing");
        }

        [Test]
        public void PutRequest_WithInvalidGroupId_Returns404()
        {
            // Arrange
            A.CallTo(() => groupService.Get("invalid-group"))
                .Returns(null);

            // Act
            var response = browser.Put("/admin/group/invalid-group", with =>
            {
                with.HttpRequest();
                with.Cookie(CsrfToken.DEFAULT_CSRF_KEY, csrfToken);
                with.FormValue(CsrfToken.DEFAULT_CSRF_KEY, csrfToken);
            });

            // Assert
            response.StatusCode.ShouldBeEquivalentTo(HttpStatusCode.NotFound);
        }

        [Test]
        public void DeleteRequest_WithValidGroupId_MarksGroupDeleted()
        {
            // Arrange
            A.CallTo(() => groupService.Get("existing-group"))
                .Returns(new Group
                {
                    Id = "existing-group",
                });

            // Act
            var response = browser.Delete("/admin/group/existing-group", with => with.HttpRequest());

            // Assert
            response.StatusCode.ShouldBeEquivalentTo(HttpStatusCode.OK);
            A.CallTo(() => groupService.Delete("existing-group")).MustHaveHappened();
        }

        [Test]
        public void DeleteRequest_WithInvalidGroupId_Returns404()
        {
            // Arrange
            A.CallTo(() => groupService.Get("invalid-group"))
                .Returns(null);

            // Act
            var response = browser.Delete("/admin/group/invalid-group", with => with.HttpRequest());

            // Assert
            response.StatusCode.ShouldBeEquivalentTo(HttpStatusCode.NotFound);
        }

        [Test]
        public void DeleteRequest_NoParams_Returns404()
        {
            // Arrange

            // Act
            var response = browser.Delete("/admin/group/", with => with.HttpRequest());

            // Assert
            response.StatusCode.ShouldBeEquivalentTo(HttpStatusCode.NotFound);
        }
    }
}
