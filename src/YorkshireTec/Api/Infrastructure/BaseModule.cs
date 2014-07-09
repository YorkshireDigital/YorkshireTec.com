namespace YorkshireTec.Api.Infrastructure
{
    using System.Collections.Generic;
    using Nancy;
    using NHibernate;
    using YorkshireTec.Api.Infrastructure.Models;

    public class BaseModule : NancyModule
    {
        internal ISession RequestSession;

        protected BaseViewModel<TModel> GetBaseModel<TModel>(TModel model)
        {
            return GetBaseModel(new BaseViewModel<TModel>(model));
        }

        protected BaseViewModel<TModel> GetBaseModel<TModel>(BaseViewModel<TModel> model)
        {
            model.Page = new PageModel
            {
                IsAuthenticated = Context.CurrentUser != null,
                TitleSuffix = "YorkshireDigital",
                FriendlyName = Context.CurrentUser != null ? ((UserIdentity)Context.CurrentUser).FriendlyName : "",
                CurrentUser = Context.CurrentUser != null ? ((UserIdentity)Context.CurrentUser).UserId : "",
                Email = Context.CurrentUser != null ? ((UserIdentity)Context.CurrentUser).Email : "",
                Notifications = new List<NotificationModel>()
            };

            return model;
        }
        
        public BaseModule()
        {
        }

        public BaseModule(ISessionFactory sessionFactory, string modulePath)
            : base(string.Format("/api/{0}", modulePath))
        {
            RequestSession = sessionFactory.GetCurrentSession();
        }
    }
}