namespace YorkshireDigital.Api.Events.Modules
{
    using System.Linq;
    using AutoMapper;
    using Nancy;
    using Nancy.ModelBinding;
    using NHibernate;
    using YorkshireDigital.Api.Events.ViewModels;
    using YorkshireDigital.Api.Infrastructure;
    using YorkshireTec.Data.Services;

    public class CalendarModule : BaseModule
    {
        public CalendarModule(ISessionFactory sessionFactory, IEventService service)
            : base(sessionFactory, "/events/calendar")
        {
            Get["/"] = _ =>
            {
                var searchOptions = this.Bind<CalendarSearchModel>();

                var events = service.Query( searchOptions.From, 
                                            searchOptions.To, 
                                            searchOptions.Interests, 
                                            searchOptions.Locations,
                                            searchOptions.Skip, 
                                            searchOptions.Take);

                var viewModel = events.Select(Mapper.DynamicMap<EventViewModel>).ToList();

                return Negotiate.WithModel(viewModel)
                    .WithStatusCode(HttpStatusCode.OK);
            };
        }
    }
}