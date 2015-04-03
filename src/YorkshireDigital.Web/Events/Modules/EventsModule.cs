namespace YorkshireDigital.Web.Events.Modules
{
    using System;
    using System.Globalization;
    using System.Linq;
    using AutoMapper;
    using Nancy;
    using NHibernate;
    using YorkshireDigital.Web.Events.ViewModels;
    using YorkshireDigital.Web.Infrastructure;
    using YorkshireDigital.Data.Domain.Events;
    using YorkshireDigital.Data.Services;

    public class EventsModule : BaseModule
    {
        public EventsModule(ISessionFactory sessionFactory, IEventService service)
            : base(sessionFactory, "/events")
        {
            Get["/"] = _ =>
            {
                dynamic errorResponse;
                CalendarSearchModel model;
                var result = BindAndValidateModel(out model, out errorResponse);
                if (!result.IsValid) return errorResponse;

                DateTime? from = null;
                if (!string.IsNullOrEmpty(model.From))
                {
                    from = DateTime.ParseExact(model.From, "dd/MM/yyyy", CultureInfo.CurrentCulture);
                }
                DateTime? to = null;
                if (!string.IsNullOrEmpty(model.To))
                {
                    to = DateTime.ParseExact(model.To, "dd/MM/yyyy", CultureInfo.CurrentCulture);
                }

                var events = service.Query(from, to, model.Interests, model.Locations, model.Skip, model.Take);

                Mapper.CreateMap<Event, CalendarEventModel>()
                    .ForMember(dest => dest.Start, opt => opt.MapFrom(src => src.Start.ToString("yyyy-MM-dd")))
                    .ForMember(dest => dest.End, opt => opt.MapFrom(src => src.End.ToString("yyyy-MM-dd")))
                    .ForMember(dest => dest.Interests, opt => opt.MapFrom(src => src.Interests.Select(x => x.Name).Distinct().ToArray()))
                    .ForMember(dest => dest.Colour, opt => opt.MapFrom(src => src.Group.Colour))
                    .ForMember(dest => dest.Title, opt => opt.MapFrom(src => src.Group.Name))
                    .ForMember(dest => dest.ShortTitle, opt => opt.MapFrom(src => src.Group.ShortName));

                var viewModel = events.Select(Mapper.DynamicMap<CalendarEventModel>).ToList();

                return Negotiate.WithModel(viewModel)
                    .WithStatusCode(HttpStatusCode.OK);
            };
        }
    }
}