namespace YorkshireDigital.Web.Tests.Admin.Modules
{
    using FakeItEasy;
    using Nancy.Testing;
    using NUnit.Framework;
    using YorkshireDigital.Data.Services;
    using YorkshireDigital.Web.Admin.Modules;
    using YorkshireDigital.Web.Infrastructure;
    using YorkshireDigital.Web.Infrastructure.Handlers;

    [TestFixture]
    public class AdminModuleTests
    {
        private Browser browser;
        private IUserService userService;
        private IEventService eventService;
        private IGroupService groupService;

        [SetUp]
        public void SetUp()
        {
            userService = A.Fake<IUserService>();
            eventService = A.Fake<IEventService>();
            groupService = A.Fake<IGroupService>();

            browser = new Browser(with =>
            {
                with.Module<AdminModule>();
                with.ViewFactory<ApiViewFactory>();
                with.Dependency(userService);
                with.Dependency(eventService);
                with.Dependency(groupService);
                with.RequestStartup((container, pipelines, context) =>
                {
                    context.CurrentUser = new UserIdentity { UserName = "admin", Claims = new [] { "Admin" }};
                    pipelines.OnError += (ctx, exception) =>
                    {
                        ctx.Items.Add("OnErrorException", exception);
                        return null;
                    };
                });
                with.StatusCodeHandler<InternalServerErrorStatusCodeHandler>();
            });
        }
    }
}
