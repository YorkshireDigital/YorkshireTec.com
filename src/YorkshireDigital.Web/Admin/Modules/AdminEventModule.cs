namespace YorkshireDigital.Web.Admin.Modules
{
    using Nancy;
    using Nancy.Security;
    using YorkshireDigital.Data.Services;
    using YorkshireDigital.Web.Admin.ViewModels;
    using YorkshireDigital.Web.Infrastructure;

    public class AdminEventModule : BaseModule
    {
        private readonly IEventService eventService;

        public AdminEventModule(IEventService eventService)
            : base("admin/event")
        {
            this.eventService = eventService;
            this.RequiresAuthentication();
            this.RequiresClaims(new[] { "Admin" });

            Get["/"] = _ => Negotiate.WithModel(new AdminEventViewModel())
                                     .WithView("NewEvent");

            Get["/{eventId}"] = _ =>
            {
                string eventId = _.eventId.ToString();

                var @event = eventService.Get(eventId);

                if (@event == null)
                {
                    return HttpStatusCode.NotFound;
                }

                return Negotiate.WithModel(AdminEventViewModel.FromDomain(@event))
                                .WithView("Event");

            };

            Post["/"] = _ =>
            {
                this.ValidateCsrfToken();

                AdminEventViewModel model;
                var result = BindAndValidateModel(out model);

                if (!result.IsValid)
                {
                    return Negotiate.WithModel(model)
                                .WithView("NewEvent")
                                .WithStatusCode(HttpStatusCode.BadRequest);
                }

                var @event = model.ToDomain();

                var existing = eventService.Get(@event.UniqueName);
                if (existing != null)
                {
                    AddError("UniqueName", "An event already exists with this unique name.");
                    return Negotiate.WithModel(model)
                                .WithView("NewEvent")
                                .WithStatusCode(HttpStatusCode.BadRequest);
                }

                eventService.Save(@event);

                return Negotiate.WithModel(AdminEventViewModel.FromDomain(@event))
                                .WithView("Event")
                                .WithStatusCode(HttpStatusCode.Created);

            };

            Post["/{eventId}"] = _ =>
            {
                this.ValidateCsrfToken();

                string eventId = _.eventId.ToString();
                AdminEventViewModel model;
                var result = BindAndValidateModel(out model);

                var @event = eventService.Get(eventId);

                if (@event == null)
                {
                    return HttpStatusCode.NotFound;
                }

                //if (!result.IsValid)
                //{
                //    return Negotiate.WithModel(model)
                //                .WithView("NewEvent")
                //                .WithStatusCode(HttpStatusCode.BadRequest);
                //}

                // TODO: How do we handle Id updates?
                //if (@event.Id != model.Id)
                //{
                //    throw new Exception("");
                //}

                model.UpdateDomain(@event);

                eventService.Save(@event);

                return Negotiate.WithModel(AdminEventViewModel.FromDomain(@event))
                                .WithView("Event")
                                .WithStatusCode(HttpStatusCode.OK);
            };

            Delete["/"] = _ => HttpStatusCode.NotFound;

            Delete["/{eventId}"] = _ =>
            {
                string eventId = _.eventId.ToString();

                var @event = eventService.Get(eventId);

                if (@event == null)
                {
                    return HttpStatusCode.NotFound;
                }

                eventService.Delete(eventId);

                return Response.AsRedirect("admin")
                                .WithStatusCode(HttpStatusCode.OK);
            };
        }
    }
}