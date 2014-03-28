namespace YorkshireTec.Infrastructure
{
    using System.Collections.Generic;
    using Nancy;

    public class BaseModule : NancyModule
    {
        protected BaseViewModel<TModel> GetBaseModel<TModel>(TModel model)
        {
            return GetBaseModel(new BaseViewModel<TModel>(model));
        }

        protected BaseViewModel<TModel> GetBaseModel<TModel>(BaseViewModel<TModel> model)
        {
            model.Page = new PageModel
            {
                IsAuthenticated = Context.CurrentUser != null,
                TitleSuffix = "YorkshireTec",
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

        public BaseModule(string modulePath)
            : base(modulePath)
        {
        }
    }
}