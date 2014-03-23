﻿namespace YorkshireTec.Modules
{
    using System.Collections.Generic;
    using Nancy;
    using YorkshireTec.Infrastructure;
    using YorkshireTec.ViewModels;

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