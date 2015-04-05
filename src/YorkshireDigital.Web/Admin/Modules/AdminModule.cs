namespace YorkshireDigital.Web.Admin.Modules
{
    using System;
    using Nancy;
    using Nancy.Security;
    using YorkshireDigital.Data.Services;
    using YorkshireDigital.Web.Admin.ViewModels;
    using YorkshireDigital.Web.Infrastructure;

    public class AdminModule : BaseModule
    {
        public AdminModule(IUserService userService, IEventService eventService, IGroupService groupService)
            : base("admin")
        {
            this.RequiresAuthentication();
            this.RequiresClaims(new [] { "Admin" });

            Get["/"] = _ =>
            {
                var users = userService.GetActiveUsers(20, 0);
                var events = eventService.Query(DateTime.MinValue, DateTime.MaxValue, new string[0], new string[0], 0, 20);
                var groups = groupService.GetActiveGroups(20, 0);

                return Negotiate.WithModel(new AdminIndexViewModel(users, events, groups))
                    .WithView("Index");
            };
        }
    }
}