namespace YorkshireDigital.Api.Tests.ModuleTests
{
    using System.Collections.Generic;
    using System.Linq;
    using FakeItEasy;
    using FluentAssertions;
    using Nancy;
    using Nancy.Testing;
    using NHibernate;
    using NUnit.Framework;
    using YorkshireDigital.Api.Events.Modules;
    using YorkshireTec.Data.Domain.Events;

    [TestFixture]
    public class EventsModuleTests
    {
        private Browser _browser;
        private List<Event> _eventList;

        [SetUp]
        public void Setup()
        {
            _eventList = new List<Event>();

            var session = A.Fake<ISession>();
            A.CallTo(() => session.SaveOrUpdate(A<Event>.Ignored))
                .Invokes((Event entity) => _eventList.Add(entity));
            A.CallTo(() => session.Get<Event>(A<object>.Ignored))
                .ReturnsLazily((object id) => _eventList.FirstOrDefault(x => x.Id == int.Parse(id.ToString())));

            var sessionFactory = A.Fake<ISessionFactory>();
            A.CallTo(() => sessionFactory.GetCurrentSession()).Returns(session);

            _browser = new Browser(with =>
            {
                with.Module<EventsModule>();
                with.Dependency(sessionFactory);
            });
        }

        [Test]
        public void Get_request_with_valid_id_should_return_200()
        {
            // Arrange
            _eventList.Add(new Event { Id = 1 });

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
        public void Get_request_with_non_existant_id_should_return_404()
        {
            // Arrange

            // Act
            var result = _browser.Get("/events/1", with =>
            {
                with.HttpRequest();
                with.Header("accept", "application/json");
            });

            // Asset
            result.StatusCode.ShouldBeEquivalentTo(HttpStatusCode.NotFound);
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
