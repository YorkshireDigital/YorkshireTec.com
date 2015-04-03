namespace YorkshireDigital.Data.Tests.Helpers
{
    using global::NHibernate;
    using NUnit.Framework;

    public class InMemoryFixtureBase
    {
        private ITransaction transaction;

        [SetUp]
        public void BaseSetup()
        {
            Session = InMemorySessionFactoryProvider.Instance.OpenSession();
            transaction = Session.BeginTransaction();
        }

        [TearDown]
        public void BaseTearDown()
        {
            transaction.Rollback();
            if (Session != null)
                Session.Dispose();
        }

        protected ISession Session { get; private set; }
    }
}
