namespace YorkshireTec.MailingList.Modules
{
    using Nancy;
    using YorkshireTec.Infrastructure;
    using YorkshireTec.MailingList.ViewModels;
    
    public class ConfirmationModule : BaseModule
    {
        public ConfirmationModule()
            : base("MailingList/Confirmation")
        {
            Get["/"] = _ =>
            {
                var viewModel = new IndexViewModel();
                var model = GetBaseModel(viewModel);

                model.Page.Title = "Bringing Yorkshires digital community together";
                return Negotiate.WithModel(model).WithView("Confirmation");
            };
        }
    }
}