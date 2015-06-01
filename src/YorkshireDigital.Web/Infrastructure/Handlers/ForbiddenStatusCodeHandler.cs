namespace YorkshireDigital.Web.Infrastructure.Handlers
{
    using Nancy;
    using Nancy.ErrorHandling;
    using Nancy.Responses.Negotiation;
    using YorkshireDigital.Web.Infrastructure.Models;

    public class ForbiddenStatusCodeHandler : IStatusCodeHandler
    {
        private readonly IResponseNegotiator responseNegotiator;

        public ForbiddenStatusCodeHandler(IResponseNegotiator responseNegotiator)
        {
            this.responseNegotiator = responseNegotiator;
        }

        public bool HandlesStatusCode(HttpStatusCode statusCode, NancyContext context)
        {
            return statusCode == HttpStatusCode.Forbidden;
        }

        public void Handle(HttpStatusCode statusCode, NancyContext context)
        {
            var response = new Negotiator(context);

            response.WithModel(new ForbiddenViewModel
            {
                Title = "Permission Denied.",
                Summary = "Sorry, you do not have permission to perform that action. Please contact your administrator."
            }).WithStatusCode(statusCode)
              .WithView("Forbidden");

            var errorresponse = responseNegotiator.NegotiateResponse(response, context);
            errorresponse.PreExecute(context);
            context.Response = errorresponse;
        }
    }
}