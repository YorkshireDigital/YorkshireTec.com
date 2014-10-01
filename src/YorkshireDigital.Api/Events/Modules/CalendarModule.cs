namespace YorkshireDigital.Api.Events.Modules
{
    using System;
    using System.Globalization;
    using System.Linq;
    using AutoMapper;
    using Nancy;
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
                dynamic errorResponse;
                CalendarSearchModel model;
                if (!BindAndValidateModel(out model, out errorResponse)) return errorResponse;

                DateTime? from = null;
                if (!string.IsNullOrEmpty(model.From))
                {
                    from = DateTime.ParseExact(model.From, "dd/MM/yyyy", CultureInfo.CurrentCulture);
                }

                var events = service.Query(from,
                                            model.To,
                                            model.Interests,
                                            model.Locations,
                                            model.Skip,
                                            model.Take);

                var viewModel = events.Select(Mapper.DynamicMap<EventViewModel>).ToList();

                return Negotiate.WithModel(viewModel)
                    .WithStatusCode(HttpStatusCode.OK);
            };
        }
    }
}