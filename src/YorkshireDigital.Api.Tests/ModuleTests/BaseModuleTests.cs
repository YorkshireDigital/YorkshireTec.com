namespace YorkshireDigital.Api.Tests.ModuleTests
{
    using FluentAssertions;
    using Nancy;
    using Nancy.Testing;
    using NUnit.Framework;
    using YorkshireDigital.Api.Infrastructure;

    [TestFixture]
    public class BaseModuleTests
    {
        [Test]
        public void Should_return_status_418_for_Get()
        {
            // Arrange
            var browser = new Browser(with => with.Module<BaseModule>());

            // When
            var result = browser.Get("/", with => with.HttpRequest());

            // Then
            result.StatusCode.ShouldBeEquivalentTo(HttpStatusCode.ImATeapot);
        }

        [Test]
        public void Should_return_status_418_for_Post()
        {
            // Arrange
            var browser = new Browser(with => with.Module<BaseModule>());

            // When
            var result = browser.Post("/", with => with.HttpRequest());

            // Then
            result.StatusCode.ShouldBeEquivalentTo(HttpStatusCode.ImATeapot);
        }
        [Test]
        public void Should_return_status_418_for_Put()
        {
            // Arrange
            var browser = new Browser(with => with.Module<BaseModule>());

            // When
            var result = browser.Put("/", with => with.HttpRequest());

            // Then
            result.StatusCode.ShouldBeEquivalentTo(HttpStatusCode.ImATeapot);
        }
        [Test]
        public void Should_return_status_418_for_Delete()
        {
            // Arrange
            var browser = new Browser(with => with.Module<BaseModule>());

            // When
            var result = browser.Delete("/", with => with.HttpRequest());

            // Then
            result.StatusCode.ShouldBeEquivalentTo(HttpStatusCode.ImATeapot);
        }
    }
}
