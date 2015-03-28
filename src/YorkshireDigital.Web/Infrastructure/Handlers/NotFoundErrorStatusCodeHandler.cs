namespace YorkshireDigital.Web.Infrastructure.Handlers
{
    using Nancy;
    using Nancy.ErrorHandling;
    using Nancy.Responses.Negotiation;
    using Nancy.ViewEngines;
    using YorkshireDigital.Web.Infrastructure.Models;

    public class NotFoundErrorStatusCodeHandler : DefaultViewRenderer, IStatusCodeHandler
    {
        private readonly IResponseNegotiator responseNegotiator;

        public NotFoundErrorStatusCodeHandler(IViewFactory viewFactory, IResponseNegotiator responseNegotiator)
            : base(viewFactory)
        {
            this.responseNegotiator = responseNegotiator;
        }

        public bool HandlesStatusCode(HttpStatusCode statusCode, NancyContext context)
        {
            return statusCode == HttpStatusCode.NotFound;
        }

        public void Handle(HttpStatusCode statusCode, NancyContext context)
        {
            var response = RenderView(context, "PageNotFound", new PageNotFoundViewModel
            {
                Title = "Page Not Found!",
                Summary = "We could not find the page you are looking for :("
            });

            response.StatusCode = statusCode;
            context.Response = response;
        }
    }
}