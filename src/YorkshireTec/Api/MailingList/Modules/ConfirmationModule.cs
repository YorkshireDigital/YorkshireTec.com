namespace YorkshireTec.Api.MailingList.Modules
{
    using Nancy;
    using NHibernate;
    using YorkshireTec.Api.Infrastructure;
    using YorkshireTec.Api.MailingList.ViewModels;

    public class ConfirmationModule : BaseModule
    {
        public ConfirmationModule(ISessionFactory sessionFactory)
            : base(sessionFactory, "MailingList/Confirmation")
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