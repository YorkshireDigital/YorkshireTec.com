using Nancy;

namespace YorkshireDigital.Api.Events.Modules
{
    using System;
    using AutoMapper;
    using Nancy.ModelBinding;
    using NHibernate;
    using YorkshireDigital.Api.Events.ViewModels;
    using YorkshireDigital.Api.Infrastructure;
    using YorkshireTec.Data.Domain.Events;
    using YorkshireTec.Data.Services;

    public class EventsModule : BaseModule
    {
        public EventsModule(ISessionFactory sessionFactory)
            : base(sessionFactory, "/events")
        {
            var service = new EventService(RequestSession);

            Get["/{id}"] = p =>
            {
                if (p.id == null)
                {
                    return Negotiate.WithStatusCode(HttpStatusCode.NotFound);
                }

                var model = service.Get((int)p.id);

                if (model == null)
                {
                    return Negotiate.WithStatusCode(HttpStatusCode.NotFound);
                }

                var viewModel = Mapper.DynamicMap<EventViewModel>(model);

                return Negotiate.WithModel(viewModel)
                    .WithStatusCode(HttpStatusCode.OK);
            };

            Post["/"] = _ =>
            {
                var newEvent = this.Bind<EventViewModel>();

                service.Save(Mapper.DynamicMap<Event>(newEvent));

                return Negotiate.WithStatusCode(HttpStatusCode.Created);
            };

            Put["/"] = _ => HttpStatusCode.ImATeapot;

            Delete["/{id}"] = p => HttpStatusCode.ImATeapot;
        }
    }
}