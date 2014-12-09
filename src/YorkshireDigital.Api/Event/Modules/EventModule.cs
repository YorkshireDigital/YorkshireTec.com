using YorkshireDigital.Api.Infrastructure;

namespace YorkshireDigital.Api.Event.Modules
{
    using Nancy;
    using NHibernate;
    using YorkshireDigital.Api.Events.ViewModels;
    using YorkshireDigital.Data.Domain.Events;
    using YorkshireDigital.Data.Services;

    public class EventModule : BaseModule
    {
        public EventModule(ISessionFactory sessionFactory)
            : base(sessionFactory, "event")
        {
            var service = new EventService(RequestSession);

            Get["/{id?}"] = p =>
            {
                var id = p.id;

                if (string.IsNullOrEmpty(id))
                {
                    return Negotiate.WithStatusCode(HttpStatusCode.NotFound);
                }

                Event model = service.Get(id);

                if (model == null)
                {
                    return Negotiate.WithStatusCode(HttpStatusCode.NotFound);
                }

                var viewModel = new EventDetailsModel(model);

                return Negotiate.WithModel(viewModel)
                                .WithStatusCode(HttpStatusCode.OK)
                                .WithView("Event");
            };
        }
    }
}