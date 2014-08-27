namespace YorkshireDigital.Api.Tests.ModuleTests
{
    using FakeItEasy;
    using FluentAssertions;
    using Nancy;
    using Nancy.Testing;
    using NHibernate;
    using NUnit.Framework;
    using YorkshireDigital.Api.Events.Modules;

    [TestFixture]
    public class EventsModuleTests
    {
        private Browser _browser;

        [SetUp]
        public void Setup()
        {
            var session = A.Fake<ISession>();
            var sessionFactory = A.Fake<ISessionFactory>();
            A.CallTo(() => sessionFactory.OpenSession()).Returns(session);

            _browser = new Browser(with =>
            {
                with.Module<EventsModule>();
                with.Dependency(sessionFactory);
            });
        }

        [Test]
        public void Get_request_with_int_should_return_200()
        {
            // Arrange

            // Act
            var result = _browser.Get("/events/1", with =>
            {
                with.HttpRequest();
                with.Header("accept", "application/json");
            });

            // Asset
            result.StatusCode.ShouldBeEquivalentTo(HttpStatusCode.OK);
        }

        [Test]
        public void Get_request_with_no_id_should_return_404()
        {
            // Arrange

            // Act
            var result = _browser.Get("/events/", with =>
            {
                with.HttpRequest();
                with.Header("accept", "application/json");
            });

            // Asset
            result.StatusCode.ShouldBeEquivalentTo(HttpStatusCode.NotFound);
        }

        [Test]
        public void Get_request_with_non_int_id_should_return_404()
        {
            // Arrange

            // Act
            var result = _browser.Get("/events/ABC", with =>
            {
                with.HttpRequest();
                with.Header("accept", "application/json");
            });

            // Asset
            result.StatusCode.ShouldBeEquivalentTo(HttpStatusCode.NotFound);
        }
    }
}
