namespace YorkshireDigital.Api.Infrastructure
{
    using Nancy;
    using Nancy.ModelBinding;
    using Nancy.Validation;
    using NHibernate;
    using YorkshireDigital.Api.Infrastructure.Models;

    public class BaseModule : NancyModule
    {
        internal ISession RequestSession;

        public BaseModule()
        {
            Get["/"] = _ => HttpStatusCode.ImATeapot;
            Post["/"] = _ => HttpStatusCode.ImATeapot;
            Put["/"] = _ => HttpStatusCode.ImATeapot;
            Delete["/"] = _ => HttpStatusCode.ImATeapot;
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