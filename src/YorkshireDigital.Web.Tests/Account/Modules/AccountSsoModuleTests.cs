namespace YorkshireDigital.Web.Tests.Account.Modules
{
    using System;
    using FakeItEasy;
    using FluentAssertions;
    using Nancy;
    using Nancy.Testing;
    using NUnit.Framework;
    using YorkshireDigital.Data.Domain.Account;
    using YorkshireDigital.Data.Services;
    using YorkshireDigital.Web.Account.Modules;
    using YorkshireDigital.Web.Infrastructure;
    using YorkshireDigital.Web.Infrastructure.Handlers;
    using YorkshireDigital.Web.Infrastructure.Models;

    [TestFixture]
    public class AccountSsoModuleTests
    {
        private Browser browser;
        private IUserService userService;

        [SetUp]
        public void SetUp()
        {
            userService = A.Fake<IUserService>();

            browser = new Browser(with =>
            {
                with.Module<AccountSsoModule>();
                with.ViewFactory<ApiViewFactory>();
                with.Dependency(userService);
                with.RequestStartup((container, pipelines, context) =>
                {
                    context.CurrentUser = new UserIdentity { UserName = "samsam" };
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
        public void GetRequest_WithValidSignature_AndLoggedInUser_ShouldRedirectToDiscourseUrlWithValidSignature()
        {
            // Arrange
            // NOTE: The sso secret in the app settings for this test library is d836444a9e4084d5b224a60c208dce14
            // NOTE: The return url in the app settings for this test library is http://discuss.example.com
            A.CallTo(() => userService.GetUser(A<string>.Ignored))
                .Returns(new User
                {
                    Email = "test@test.com",
                    Id = new Guid(),
                    Username = "samsam",
                    Name = "sam"
                });

            // Act
            var result = browser.Get("/account/sso/discourse", with =>
            {
                with.HttpRequest();
                with.Query("sso", "bm9uY2U9Y2I2ODI1MWVlZmI1MjExZTU4YzAwZmYxMzk1ZjBjMGI%3D%0A");
                with.Query("sig", "2828aa29899722b35a2f191d34ef9b3ce695e0e6eeec47deb46d588d70c7cb56");
                with.Header("accept", "text/html");
            });

            // Asset
            result.ShouldHaveRedirectedTo("http://discuss.example.com/session/sso_login?sso=bm9uY2U9Y2I2ODI1MWVlZmI1MjExZTU4YzAwZmYxMzk1ZjBjMGImbmFtZT1z%0aYW0mdXNlcm5hbWU9c2Ftc2FtJmVtYWlsPXRlc3QlNDB0ZXN0LmNvbSZleHRl%0acm5hbF9pZD0wMDAwMDAwMC0wMDAwLTAwMDAtMDAwMC0wMDAwMDAwMDAwMDA%3d%0a&sig=aa807deb154dddd68a3fba7f34a7494ca99f5cf5d512ff0aa48c1057b390b5b5");
        }

        [Test]
        public void GetRequest_WithInvalidSignature_Returns403Forbidden()
        {
            // Arrange
            // NOTE: The sso secret in the app settings for this test library is d836444a9e4084d5b224a60c208dce14

            // Act
            var result = browser.Get("/account/sso/discourse", with =>
            {
                with.HttpRequest();
                with.Query("sso", "bm9uY2U9Y2I2ODI1MWVlZmI1MjExZTU4YzAwZmYxMzk1ZjBjMGI%3D%0A");
                with.Query("sig", "FAIL");
                with.Header("accept", "text/html");
            });

            // Asset
            result.StatusCode.ShouldBeEquivalentTo(HttpStatusCode.Forbidden);
        }

        [Test]
        public void GetRequest_MissingRequiredQueryParams_Returns400BadRequest()
        {
            // Arrange

            // Act
            var response = browser.Get("/account/sso/discourse", with =>
            {
                with.HttpRequest();
                with.Header("accept", "text/html");
            });

            // Asset
            var model = response.GetModel<ErrorPageViewModel>();
            model.Title.ShouldAllBeEquivalentTo("Sorry, something went wrong");
            model.Summary.ShouldBeEquivalentTo("The request does not contain the required parameters\r\nParameter name: sso");
        }
    }
}
