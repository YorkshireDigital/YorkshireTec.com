namespace YorkshireDigital.Web.Tests
{
    using Nancy;
    using Nancy.Responses;
    using Nancy.ViewEngines;

    public class ApiViewFactory : IViewFactory
    {
        public Response RenderView(string viewName, dynamic model, ViewLocationContext viewLocationContext)
        {
            viewLocationContext.Context.Items["__Nancy_Testing_ViewModel"] = model;
            viewLocationContext.Context.Items["__Nancy_Testing_ViewName"] = viewName;
            return new HtmlResponse();
        }
    }
}
