namespace YorkshireDigital.Web.Home.Modules
{
    using NHibernate;
    using YorkshireDigital.Web.Infrastructure;

    public class HomeModule : BaseModule
    {
        public HomeModule(ISessionFactory sessionFactory)
            : base(sessionFactory)
        {
            Get["/"] = _ =>
            {
                ViewBag.Title = "YorkshireDigital";
                return View["Home"];
            };
        }
    }
}