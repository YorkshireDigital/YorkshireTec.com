namespace YorkshireDigital.Web.Tests.ModuleTests
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using FakeItEasy;
    using FluentAssertions;
    using FluentValidation;
    using Nancy;
    using Nancy.Testing;
    using Nancy.Validation;
    using Nancy.Validation.FluentValidation;
    using NHibernate;
    using NUnit.Framework;
    using YorkshireDigital.Web.Events.Modules;
    using YorkshireDigital.Web.Events.Validation;
    using YorkshireDigital.Web.Events.ViewModels;
    using YorkshireDigital.Web.Infrastructure.Models;
    using YorkshireDigital.Data.Domain.Events;
    using YorkshireDigital.Data.Services;

    [TestFixture]
    public class EventsModuleTests
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

            var adapterFactory = A.Fake<IFluentAdapterFactory>();
            var validators = new IValidator[] { new CalendarSearchModelValidator() };

            var factory = new FluentValidationValidatorFactory(adapterFactory, validators);

            var valiadtorLocator = new DefaultValidatorLocator(new List<IModelValidatorFactory> { factory });
            
            _browser = new Browser(with =>
            {
                with.Module<EventsModule>();
                with.ViewFactory<ApiViewFactory>();
                with.Dependency(sessionFactory);
                with.Dependency(service);
                with.ModelValidatorLocator(valiadtorLocator);
            });
        }

        [Test]
        public void Get_request_should_return_200()
        {
            // Arrange

            // Act
            var result = _browser.Get("/events", with =>
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
                _eventList.Add(new Event { UniqueName = i.ToString(), Interests = new Interest[0], Start = DateTime.Now, End = DateTime.Now });
            }
            // Act
            var result = _browser.Get("/events", with => with.HttpRequest());
            var model = result.GetModel<List<CalendarEventModel>>();
            
            // Asset
            model.Count.ShouldBeEquivalentTo(100);
        }

        #region From

        [Test]
        public void Get_request_with_filter_from_valid_should_return_the_viewmodel()
        {
            // Arrange
            for (int i = 0; i < 100; i++)
            {
                _eventList.Add(new Event { UniqueName = i.ToString(), Interests = new Interest[0], Start = DateTime.Now, End = DateTime.Now });
            }
            // Act
            var result = _browser.Get("/events", with =>
            {
                with.HttpRequest();
                with.FormValue("from", "31/01/2014");
            });
            var model = result.GetModel<List<CalendarEventModel>>();

            // Asset
            model.Count.ShouldBeEquivalentTo(100);
        }

        [Test]
        public void Get_request_with_filter_from_invalid_date_should_return_400()
        {
            // Arrange
            for (int i = 0; i < 100; i++)
            {
                _eventList.Add(new Event { UniqueName = i.ToString() });
            }
            // Act
            var result = _browser.Get("/events", with =>
            {
                with.HttpRequest();
                with.FormValue("from", "01/31/2014");
            });
            var model = result.GetModel<ErrorViewModel>();

            // Asset
            result.StatusCode.ShouldBeEquivalentTo(HttpStatusCode.BadRequest);
            model.Errors.Count().ShouldBeEquivalentTo(1);
            model.Errors[0].Name.ShouldBeEquivalentTo("From");
            model.Errors[0].Errors.Count().ShouldBeEquivalentTo(1);
            model.Errors[0].Errors[0].ShouldBeEquivalentTo("From date is not a valid date. Please supply a date in the format dd/MM/yyyy");
        }
        [Test]
        public void Get_request_with_filter_from_invalid_input_should_return_400()
        {
            // Arrange
            for (int i = 0; i < 100; i++)
            {
                _eventList.Add(new Event { UniqueName = i.ToString() });
            }
            // Act
            var result = _browser.Get("/events", with =>
            {
                with.HttpRequest();
                with.FormValue("from", "INVALID");
            });
            var model = result.GetModel<ErrorViewModel>();

            // Asset
            result.StatusCode.ShouldBeEquivalentTo(HttpStatusCode.BadRequest);
            model.Errors.Count().ShouldBeEquivalentTo(1);
            model.Errors[0].Name.ShouldBeEquivalentTo("From");
            model.Errors[0].Errors.Count().ShouldBeEquivalentTo(1);
            model.Errors[0].Errors[0].ShouldBeEquivalentTo("From date is not a valid date. Please supply a date in the format dd/MM/yyyy");
        }

        #endregion

        #region To
        [Test]
        public void Get_request_with_filter_to_valid_should_return_the_viewmodel()
        {
            // Arrange
            for (int i = 0; i < 100; i++)
            {
                _eventList.Add(new Event { UniqueName = i.ToString(), Interests = new Interest[0], Start = DateTime.Now, End = DateTime.Now });
            }
            // Act
            var result = _browser.Get("/events", with =>
            {
                with.HttpRequest();
                with.FormValue("to", "31/01/2014");
            });
            var model = result.GetModel<List<CalendarEventModel>>();

            // Asset
            model.Count.ShouldBeEquivalentTo(100);
        }

        [Test]
        public void Get_request_with_filter_to_invalid_date_should_return_400()
        {
            // Arrange
            for (int i = 0; i < 100; i++)
            {
                _eventList.Add(new Event { UniqueName = i.ToString() });
            }
            // Act
            var result = _browser.Get("/events", with =>
            {
                with.HttpRequest();
                with.FormValue("to", "01/31/2014");
            });
            var model = result.GetModel<ErrorViewModel>();

            // Asset
            result.StatusCode.ShouldBeEquivalentTo(HttpStatusCode.BadRequest);
            model.Errors.Count().ShouldBeEquivalentTo(1);
            model.Errors[0].Name.ShouldBeEquivalentTo("To");
            model.Errors[0].Errors.Count().ShouldBeEquivalentTo(1);
            model.Errors[0].Errors[0].ShouldBeEquivalentTo("To date is not a valid date. Please supply a date in the format dd/MM/yyyy");
        }
        [Test]
        public void Get_request_with_filter_to_invalid_input_should_return_400()
        {
            // Arrange
            for (int i = 0; i < 100; i++)
            {
                _eventList.Add(new Event { UniqueName = i.ToString() });
            }
            // Act
            var result = _browser.Get("/events", with =>
            {
                with.HttpRequest();
                with.FormValue("to", "INVALID");
            });
            var model = result.GetModel<ErrorViewModel>();

            // Asset
            result.StatusCode.ShouldBeEquivalentTo(HttpStatusCode.BadRequest);
            model.Errors.Count().ShouldBeEquivalentTo(1);
            model.Errors[0].Name.ShouldBeEquivalentTo("To");
            model.Errors[0].Errors.Count().ShouldBeEquivalentTo(1);
            model.Errors[0].Errors[0].ShouldBeEquivalentTo("To date is not a valid date. Please supply a date in the format dd/MM/yyyy");
        }

        #endregion
    }
}
