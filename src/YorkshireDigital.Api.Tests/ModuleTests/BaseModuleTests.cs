namespace YorkshireDigital.Api.Tests.ModuleTests
{
    using FakeItEasy;
    using FluentAssertions;
    using Nancy;
    using Nancy.Testing;
    using NHibernate;
    using NHibernate.Hql.Ast.ANTLR;
    using NUnit.Framework;
    using YorkshireDigital.Api.Infrastructure;

    [TestFixture]
    public class BaseModuleTests
    {
        private Browser browser;

        [SetUp]
        public void SetUp()
        {
            var session = A.Fake<ISession>();
            var sessionFactory = A.Fake<ISessionFactory>();
            A.CallTo(() => sessionFactory.GetCurrentSession()).Returns(session);

            browser = new Browser(with =>
            {
                with.Module<BaseModule>();
                with.Dependency(sessionFactory);
            });
        }

        [Test]
        public void Should_return_status_418_for_Get()
        {
            // When
            var result = browser.Get("/", with => with.HttpRequest());

            // Then
            result.StatusCode.ShouldBeEquivalentTo(HttpStatusCode.ImATeapot);
        }

        [Test]
        public void Should_return_status_418_for_Post()
        {
            // When
            var result = browser.Post("/", with => with.HttpRequest());

            // Then
            result.StatusCode.ShouldBeEquivalentTo(HttpStatusCode.ImATeapot);
        }
        [Test]
        public void Should_return_status_418_for_Put()
        {
            // When
            var result = browser.Put("/", with => with.HttpRequest());

            // Then
            result.StatusCode.ShouldBeEquivalentTo(HttpStatusCode.ImATeapot);
        }
        [Test]
        public void Should_return_status_418_for_Delete()
        {
            // When
            var result = browser.Delete("/", with => with.HttpRequest());

            // Then
            result.StatusCode.ShouldBeEquivalentTo(HttpStatusCode.ImATeapot);
        }
    }
}
