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
        private readonly IGroupService groupService;

        public AdminEventModule(IEventService eventService, IGroupService groupService)
            : base("admin/event")
        {
            this.eventService = eventService;
            this.groupService = groupService;
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

                var existing = eventService.Get(model.UniqueName);
                if (existing != null)
                {
                    AddError("UniqueName", "An event already exists with this unique name.");
                    return Negotiate.WithModel(model)
                                .WithView("NewEvent")
                                .WithStatusCode(HttpStatusCode.BadRequest);
                }

                var @event = model.ToDomain();

                var group = groupService.Get(model.GroupId);
                if (group == null)
                {
                    AddError("GroupId", "No group exists with this id.");
                    return Negotiate.WithModel(model)
                                .WithView("NewEvent")
                                .WithStatusCode(HttpStatusCode.BadRequest);
                }

                @event.Group = group;

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