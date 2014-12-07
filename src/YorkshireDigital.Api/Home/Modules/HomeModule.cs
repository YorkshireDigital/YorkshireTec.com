namespace YorkshireDigital.Api.Home.Modules
{
    using NHibernate;
    using YorkshireDigital.Api.Infrastructure;

    public class HomeModule : BaseModule
    {
        public HomeModule(ISessionFactory sessionFactory)
            : base(sessionFactory)
        {
            Get["/"] = _ =>
            {
                return View["Home"];
            };
        }
    }
}