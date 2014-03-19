namespace YorkshireTec.ViewModels
{
    using Nancy.Validation;

    public class BaseViewModel
    {
        public PageModel Page { get; set; }

        protected void AddPageErrors(ModelValidationResult result)
        {
            foreach (var kvp in result.Errors)
            {
                foreach (var item in kvp.Value)
                {
                    foreach (var member in item.MemberNames)
                    {
                        Page.Notifications.Add(new NotificationModel { Type = NotificationType.Error, Name = member, Message = item.ErrorMessage });
                    }
                }
            }
        }
    }

    public class BaseViewModel<TModel> : BaseViewModel
    {
        public TModel ViewModel { get; set; }

        public BaseViewModel(TModel model)
        {
            ViewModel = model;
        }
    }

    public class ViewModelBase
    { }
}