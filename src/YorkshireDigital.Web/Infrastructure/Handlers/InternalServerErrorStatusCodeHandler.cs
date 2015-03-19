namespace YorkshireDigital.Web.Infrastructure.Handlers
{
    using System;
    using System.Configuration;
    using Nancy;
    using Nancy.ErrorHandling;
    using Nancy.Responses.Negotiation;
    using YorkshireDigital.Web.Infrastructure.Helpers;
    using YorkshireDigital.Web.Infrastructure.Models;

    public class InternalServerErrorStatusCodeHandler : IStatusCodeHandler
    {
        private readonly IResponseNegotiator responseNegotiator;

        public InternalServerErrorStatusCodeHandler(IResponseNegotiator responseNegotiator)
        {
            this.responseNegotiator = responseNegotiator;
        }

        public bool HandlesStatusCode(HttpStatusCode statusCode, NancyContext context)
        {
            return statusCode == HttpStatusCode.InternalServerError;
        }

        public void Handle(HttpStatusCode statusCode, NancyContext context)
        {
            var response = new Negotiator(context);

            Error error = null;
            if (context.Items.ContainsKey("OnErrorException"))
            {
                var exception = context.Items["OnErrorException"] as Exception;
                if (exception != null)
                {
                    error = new Error { ErrorMessage = exception.Message, FullException = exception.ToString() };
                    SentryHelper.LogException(exception);
                }
            }
            else
            {
                SentryHelper.LogException(new Exception("An unexpected error occurred."));
            }

            bool displayDetails;
            var result = bool.TryParse(ConfigurationManager.AppSettings["Errors_DisplayDetails"], out displayDetails);

            displayDetails = displayDetails && result;

            response.WithModel(new ErrorPageViewModel
            {
                Title = "Sorry, something went wrong",
                Summary = error == null ? "An unexpected error occurred." : error.ErrorMessage,
                Details = error == null || !displayDetails ? null :  error.FullException
            }).WithStatusCode(statusCode)
              .WithView("Error");

            var errorresponse = responseNegotiator.NegotiateResponse(response, context);

            errorresponse.PreExecute(context);

            context.Response = errorresponse;

        }
    }
}