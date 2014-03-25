namespace YorkshireTec.ViewModels
{
    public class BaseViewModel
    {
        public PageModel Page { get; set; }
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