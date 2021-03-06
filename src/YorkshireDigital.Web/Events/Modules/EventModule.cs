namespace YorkshireDigital.Web.Events.Modules
{
    using Nancy;
    using YorkshireDigital.Data.Domain.Events;
    using YorkshireDigital.Data.Services;
    using YorkshireDigital.Web.Events.ViewModels;
    using YorkshireDigital.Web.Infrastructure;

    public class EventModule : BaseModule
    {
        public EventModule(IEventService eventService)
            : base("event")
        {
            Get["/{id?}"] = p =>
            {
                var id = p.id;

                if (string.IsNullOrEmpty(id))
                {
                    return Negotiate.WithStatusCode(HttpStatusCode.NotFound);
                }

                Event model = eventService.Get(id);

                if (model == null)
                {
                    return Negotiate.WithStatusCode(HttpStatusCode.NotFound);
                }

                var viewModel = new EventDetailsModel(model);

                var partialParam = Request.Query["partial"];
                bool partial;

                if (bool.TryParse(partialParam, out partial) && partial)
                {
                    return Negotiate.WithModel(viewModel)
                                .WithStatusCode(HttpStatusCode.OK)
                                .WithView("_Event");
                }

                @ViewBag.Title = string.Format("{0} : YorkshireDigital", viewModel.Organiser);
                return Negotiate.WithModel(viewModel)
                                .WithStatusCode(HttpStatusCode.OK)
                                .WithView("Event");
            };
        }
    }
}