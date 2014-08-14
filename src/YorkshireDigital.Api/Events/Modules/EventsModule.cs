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
            : base(sessionFactory, "/events/")
        {
            var service = new EventService(RequestSession);

            Get["/{id}"] = p =>
            {
                if (p.id == null)
                {
                    return Negotiate.WithStatusCode(404);
                }

                var returnValue = service.Get((int)p.id);

                if (returnValue == null)
                {
                    return Negotiate.WithStatusCode(404);
                }

                return Negotiate.WithModel(returnValue)
                    .WithStatusCode(200);
            };

            Post["/"] = _ =>
            {
                var newEvent = this.Bind<EventViewModel>();

                service.Save(Mapper.DynamicMap<Event>(newEvent));

                return HttpStatusCode.Created;
            };

            Put["/"] = _ => "Hello!";

            Delete["/{id}"] = p => "Hello!";
        }
    }
}