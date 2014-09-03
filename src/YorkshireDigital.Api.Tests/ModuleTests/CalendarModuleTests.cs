namespace YorkshireDigital.Api.Tests.ModuleTests
{
    using System;
    using System.Collections.Generic;
    using FakeItEasy;
    using FluentAssertions;
    using Nancy;
    using Nancy.Testing;
    using NHibernate;
    using NUnit.Framework;
    using YorkshireDigital.Api.Events.Modules;
    using YorkshireDigital.Api.Events.ViewModels;
    using YorkshireTec.Data.Domain.Events;
    using YorkshireTec.Data.Services;

    [TestFixture]
    public class CalendarModuleTests
    {
        private Browser _browser;
        private List<Event> _eventList;
        private ISessionFactory sessionFactory;
        private IEventService service;

        [SetUp]
        public void SetUp()
        {
            _eventList = new List<Event>();

            #region Setup Fakes

            var session = A.Fake<ISession>();
            sessionFactory = A.Fake<ISessionFactory>();
            A.CallTo(() => sessionFactory.GetCurrentSession()).Returns(session);

            service = A.Fake<IEventService>();
            A.CallTo(
                () =>
                    service.Query(A<DateTime?>.Ignored, A<DateTime?>.Ignored, A<string[]>.Ignored, A<string[]>.Ignored,
                        A<int?>.Ignored, A<int?>.Ignored))
                .Returns(_eventList);

            #endregion

            _browser = new Browser(with =>
            {
                with.Module<CalendarModule>();
                with.ViewFactory<ApiViewFactory>();
                with.Dependency(sessionFactory);
                with.Dependency(service);
            });
        }

        [Test]
        public void Get_request_should_return_200()
        {
            // Arrange

            // Act
            var result = _browser.Get("/events/calendar", with =>
            {
                with.HttpRequest();
                with.Header("accept", "application/json");
            });

            // Asset
            result.StatusCode.ShouldBeEquivalentTo(HttpStatusCode.OK);
        }

        [Test]
        public void Get_request_with_no_filters_should_return_the_viewmodel()
        {
            // Arrange
            for (int i = 0; i < 100; i++)
            {
                _eventList.Add(new Event { Id = i });
            }
            // Act
            var result = _browser.Get("/events/calendar", with => with.HttpRequest());
            var model = result.GetModel<List<EventViewModel>>();
            
            // Asset
            model.Count.ShouldBeEquivalentTo(100);
        }
    }
}
