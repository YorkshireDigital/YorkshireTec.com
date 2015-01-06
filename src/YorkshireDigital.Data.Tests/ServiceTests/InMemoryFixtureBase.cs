using ISession = NHibernate.ISession;
using ITransaction = NHibernate.ITransaction;

namespace YorkshireDigital.Data.Tests.ServiceTests
{
    using NUnit.Framework;
    using YorkshireDigital.Data.Tests.Helpers;

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
