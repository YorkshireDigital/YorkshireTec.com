namespace YorkshireDigital.Web.Infrastructure
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Nancy;
    using Nancy.ModelBinding;
    using Nancy.Security;
    using Nancy.Validation;
    using YorkshireDigital.Web.Infrastructure.Models;
    using YorkshireDigital.Web.Infrastructure.Responses;
    using YorkshireDigital.Data.Services;

    public class BaseModule : NancyModule
    {

        public BaseModule(IEventService eventService)
        {
            Get["/sitemap"] = _ =>
            {
                var events = eventService.GetWithinRange(DateTime.MinValue, DateTime.MaxValue);

                return new SitemapResponse(events, "http://www.yorkshiredigital.com/");
            };
        }


        public BaseModule(string modulePath)
            : base(string.Format("/{0}", modulePath))
        {
            
        }

        internal ModelValidationResult BindAndValidateModel<T>(out T model)
        {
            model = this.Bind<T>();
            var result = this.Validate(model);

            if (result.IsValid) return result;

            AddErrors(result.Errors);

            return result;
        }

        internal ModelValidationResult BindAndValidateModel<T>(out T model, out dynamic errorResponse)
        {
            errorResponse = null;
            var result = BindAndValidateModel(out model);
            if (result.IsValid) return result;

            var errorModel = new ErrorViewModel(result.Errors);
            errorResponse = Negotiate.WithStatusCode(HttpStatusCode.BadRequest)
                .WithModel(errorModel);
            return result;
        }

        internal void AddError(string field, string error)
        {
            var fieldErrors = GetFieldErrors();

            if (fieldErrors.ContainsKey(field))
            {
                fieldErrors[field].Add(error);
            }
            else
            {
                fieldErrors.Add(field, new List<string> { error });
            }

            @ViewBag.Errors = fieldErrors;
        }

        private IDictionary<string, List<string>> GetFieldErrors()
        {
            IDictionary<string, List<string>> fieldErrors = new Dictionary<string, List<string>>();

            if (@ViewBag["Errors"] != null)
            {
                fieldErrors = @ViewBag["Errors"].Value as IDictionary<string, List<string>>;
            }
            return fieldErrors ?? (new Dictionary<string, List<string>>());
        }

        internal void AddError(string field, IEnumerable<ModelValidationError> errors)
        {
            var fieldErrors = GetFieldErrors();

            fieldErrors.Add(field, errors.Select(x => x.ErrorMessage).ToList());

            @ViewBag.Errors = fieldErrors;
        }

        internal void AddErrors(IEnumerable<KeyValuePair<string, IList<ModelValidationError>>> errors)
        {
            foreach (var error in errors)
            {
                AddError(error.Key, error.Value);
            }
        }

        internal void AjaxValidateCsrfToken<T>(T model) where T : BaseAjaxPostModel
        {
            Request.Form[CsrfToken.DEFAULT_CSRF_KEY] = model.CsrfToken;

            this.ValidateCsrfToken();
        }
    }
}