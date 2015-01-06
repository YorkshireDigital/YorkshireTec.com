﻿namespace YorkshireDigital.Web.Infrastructure
{
    using System;
    using Nancy;
    using Nancy.ModelBinding;
    using Nancy.Validation;
    using NHibernate;
    using YorkshireDigital.Web.Infrastructure.Models;
    using YorkshireDigital.Web.Infrastructure.Responses;
    using YorkshireDigital.Data.Services;

    public class BaseModule : NancyModule
    {
        internal ISession RequestSession;

        public BaseModule(ISessionFactory sessionFactory)
        {
            RequestSession = sessionFactory.GetCurrentSession();

            var service = new EventService(RequestSession);

            Get["/sitemap"] = _ =>
            {
                var events = service.GetWithinRange(DateTime.MinValue, DateTime.MaxValue);

                return new SitemapResponse(events, "http://www.yorkshiredigital.com/");
            };
        }


        public BaseModule(ISessionFactory sessionFactory, string modulePath)
            : base(string.Format("/{0}", modulePath))
        {
            RequestSession = sessionFactory.GetCurrentSession();
        }

        internal bool BindAndValidateModel<T>(out T model, out dynamic errorResponse)
        {
            errorResponse = null;
            model = this.Bind<T>();
            var result = this.Validate(model);

            if (result.IsValid) return true;

            var errorModel = new ErrorViewModel(result.Errors);
            {
                errorResponse = Negotiate.WithStatusCode(HttpStatusCode.BadRequest)
                    .WithModel(errorModel);
                return false;
            }
        }
    }
}