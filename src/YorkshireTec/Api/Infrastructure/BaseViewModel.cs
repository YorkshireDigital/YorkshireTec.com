namespace YorkshireTec.Api.Infrastructure
{
    using YorkshireTec.Api.Infrastructure.Models;

    public class BaseViewModel
    {
        public PageModel Page { get; set; }
        public FeaturesModel Features { get; set; }

        public BaseViewModel()
        {
            Page = new PageModel();
            Features = new FeaturesModel();
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