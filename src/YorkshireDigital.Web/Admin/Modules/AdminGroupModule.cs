namespace YorkshireDigital.Web.Admin.Modules
{
    using Nancy;
    using Nancy.Security;
    using YorkshireDigital.Data.Services;
    using YorkshireDigital.Web.Admin.ViewModels;
    using YorkshireDigital.Web.Infrastructure;

    public class AdminGroupModule : BaseModule
    {
        private readonly IGroupService groupService;
        private readonly IUserService userService;

        public AdminGroupModule(IGroupService groupService, IUserService userService) 
            : base("admin/group")
        {
            this.groupService = groupService;
            this.userService = userService;
            this.RequiresAuthentication();
            this.RequiresClaims(new[] { "Admin" });

            Get["/"] = _ => Negotiate.WithModel(new AdminGroupViewModel())
                                     .WithView("NewGroup");

            Get["/{groupId}"] = _ =>
            {
                string groupId = _.groupId.ToString();

                var group = groupService.Get(groupId);

                if (group == null)
                {
                    return HttpStatusCode.NotFound;
                }

                return Negotiate.WithModel(AdminGroupViewModel.FromDomain(group))
                                .WithView("Group");
            };

            Post["/"] = _ =>
            {
                this.ValidateCsrfToken();

                AdminGroupViewModel model;
                var result = BindAndValidateModel(out model);

                if (!result.IsValid)
                {
                    return Negotiate.WithModel(model)
                                .WithView("NewGroup")
                                .WithStatusCode(HttpStatusCode.BadRequest);
                }

                var group = model.ToDomain();

                var existing = groupService.Get(group.Id);
                if (existing != null)
                {
                    AddError("Id", "A group already exists with this unique name.");
                    return Negotiate.WithModel(model)
                                .WithView("NewGroup")
                                .WithStatusCode(HttpStatusCode.BadRequest);
                }

                var currentUser = userService.GetUser(Context.CurrentUser.UserName);

                groupService.Save(group, currentUser);

                return Negotiate.WithModel(AdminGroupViewModel.FromDomain(group))
                                .WithView("Group")
                                .WithStatusCode(HttpStatusCode.Created);
            };

            Post["/{groupId}"] = _ =>
            {
                this.ValidateCsrfToken();

                string groupId = _.groupId.ToString();
                AdminGroupViewModel model;
                var result = BindAndValidateModel(out model);

                var group = groupService.Get(groupId);

                if (group == null)
                {
                    return HttpStatusCode.NotFound;
                }

                //if (!result.IsValid)
                //{
                //    return Negotiate.WithModel(model)
                //                .WithView("NewGroup")
                //                .WithStatusCode(HttpStatusCode.BadRequest);
                //}

                // TODO: How do we handle Id updates?
                //if (group.Id != model.Id)
                //{
                //    throw new Exception("");
                //}

                model.UpdateDomain(group);

                var currentUser = userService.GetUser(Context.CurrentUser.UserName);

                groupService.Save(group, currentUser);

                return Negotiate.WithModel(AdminGroupViewModel.FromDomain(group))
                                .WithView("Group")
                                .WithStatusCode(HttpStatusCode.OK);
            };

            Delete["/"] = _ => HttpStatusCode.NotFound;

            Delete["/{groupId}"] = _ =>
            {
                string groupId = _.groupId.ToString();

                var group = groupService.Get(groupId);

                if (group == null)
                {
                    return HttpStatusCode.NotFound;
                }

                groupService.Delete(groupId);

                return Response.AsRedirect("admin")
                                .WithStatusCode(HttpStatusCode.OK);
            };
        }
    }
}