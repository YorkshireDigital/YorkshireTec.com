namespace YorkshireTec.Home.Modules
{
    using Nancy;
    using YorkshireTec.Home.ViewModels;
    using YorkshireTec.Infrastructure;

    public class HomeModule : BaseModule
    {
        public HomeModule()
        {
            Get["/"] = _ =>
            {
                var viewModel = new IndexViewModel();
                var model = GetBaseModel(viewModel);

                model.Page.Title = "Home";
                return Negotiate.WithModel(model).WithView("Index");
            };
        }
    }
}