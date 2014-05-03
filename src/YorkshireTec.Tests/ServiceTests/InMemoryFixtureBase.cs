namespace YorkshireTec.Tests.ServiceTests
{
    using NHibernate;
    using NUnit.Framework;
    using YorkshireTec.Tests.Helpers;

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
