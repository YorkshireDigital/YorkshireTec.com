namespace YorkshireDigital.Web.Admin.Modules
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Web.UI.WebControls;
    using Nancy;
    using Nancy.ModelBinding;
    using Nancy.Security;
    using NHibernate.Linq;
    using YorkshireDigital.Data.Domain.Events;
    using YorkshireDigital.Data.Services;
    using YorkshireDigital.Web.Admin.ViewModels;
    using YorkshireDigital.Web.Infrastructure;

    public class AdminEventModule : BaseModule
    {
        private readonly IEventService eventService;
        private readonly IGroupService groupService;
        private readonly IUserService userService;

        public AdminEventModule(IEventService eventService, IGroupService groupService, IUserService userService)
            : base("admin/event")
        {
            this.eventService = eventService;
            this.groupService = groupService;
            this.userService = userService;
            this.RequiresAuthentication();
            this.RequiresClaims(new[] { "Admin" });

            Get[""] = _ =>
            {
                var interests = eventService.GetInterests();

                var model = new AdminEventViewModel
                {
                    Start = DateTime.Today,
                    End = DateTime.Today,
                    AvailableInterests = interests.Select(x => AdminInterestViewModel.FromDomain(x, new List<Interest>())).ToList(),
                    Talks = new List<AdminEventTalkViewModel> { new AdminEventTalkViewModel() }
                };

                var groupId = Request.Query["groupId"];

                if (groupId == null)
                {
                    return Negotiate.WithModel(model)
                            .WithView("NewEvent");
                }

                var group = groupService.Get(groupId);
                if (group == null)
                {
                    AddError("GroupId", "No group exists with this id.");
                    return Negotiate.WithModel(model)
                                .WithView("NewEvent")
                                .WithStatusCode(HttpStatusCode.BadRequest);
                }
                model.UniqueName = string.Format("{0}-{1}-{2}", groupId, DateTime.Now.ToString("MMM").ToLower(),
                    @DateTime.Now.ToString("yyyy"));
                model.GroupId = groupId;

                return Negotiate.WithModel(model)
                    .WithView("NewEvent");
            };

            Get["/{eventId}"] = _ =>
            {
                string eventId = _.eventId.ToString();

                var @event = eventService.Get(eventId);

                if (@event == null)
                {
                    return HttpStatusCode.NotFound;
                }
                
                var model = AdminEventViewModel.FromDomain(@event);
                var interests = eventService.GetInterests();
                model.AvailableInterests = interests.Select(x => AdminInterestViewModel.FromDomain(x, @event.Interests)).ToList();

                return Negotiate.WithModel(model)
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

                var selectedInterests = GetSelectedInterests(eventService, model);

                var existing = eventService.Get(model.UniqueName);
                if (existing != null)
                {
                    AddError("UniqueName", "An event already exists with this unique name.");
                    return Negotiate.WithModel(model)
                                .WithView("NewEvent")
                                .WithStatusCode(HttpStatusCode.BadRequest);
                }

                var @event = model.ToDomain();
                @event.Interests = selectedInterests;

                var group = groupService.Get(model.GroupId);
                if (group == null)
                {
                    AddError("GroupId", "No group exists with this id.");
                    return Negotiate.WithModel(model)
                                .WithView("NewEvent")
                                .WithStatusCode(HttpStatusCode.BadRequest);
                }

                @event.Group = group;
                var currentUser = userService.GetUser(Context.CurrentUser.UserName);
                
                eventService.Save(@event, currentUser);

                return Response.AsRedirect("/admin/event/" + @event.UniqueName);
            };

            Post["/{eventId}"] = _ =>
            {
                this.ValidateCsrfToken();

                string eventId = _.eventId.ToString();
                AdminEventViewModel model;
                var result = BindAndValidateModel(out model);
                model.Talks = this.Bind();

                if (!eventService.EventExists(eventId))
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

                var selectedInterests = GetSelectedInterests(eventService, model);

                var @event = model.ToDomain();
                @event.Interests = selectedInterests;

                var group = groupService.Get(model.GroupId);
                if (group == null)
                {
                    AddError("GroupId", "No group exists with this id.");
                    return Negotiate.WithModel(model)
                                .WithView("NewEvent")
                                .WithStatusCode(HttpStatusCode.BadRequest);
                }

                @event.Group = group;

                var currentUser = userService.GetUser(Context.CurrentUser.UserName);

                eventService.Save(@event, currentUser);

                return Response.AsRedirect("/admin/event/" + @event.UniqueName);
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

                var currentUser = userService.GetUser(Context.CurrentUser.UserName);
                eventService.Delete(eventId, currentUser);

                return Response.AsRedirect("admin")
                                .WithStatusCode(HttpStatusCode.OK);
            };
        }

        private static List<Interest> GetSelectedInterests(IEventService eventService, AdminEventViewModel model)
        {
            var interests = eventService.GetInterests();

            var selectedInterestIds = string.IsNullOrEmpty(model.Interests) ? new int[0] : model.Interests.Split(',').Select(int.Parse).ToArray();
            var selectedInterests = interests.Where(i => selectedInterestIds.Contains(i.Id)).ToList();
            model.AvailableInterests = interests.Select(x => AdminInterestViewModel.FromDomain(x, selectedInterests)).ToList();
            return selectedInterests;
        }
    }
}