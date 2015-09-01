using NHibernate;
using NUnit.Framework;

namespace YorkshireDigital.Data.Tests.IntegrationTests
{
    public class IntegrationFixtureBase
    {
        ITransaction _transaction;
        [SetUp]
        public void BaseSetup()
        {
            Session = IntegrationSessionFactoryProvidor.Instance.OpenSession();
            _transaction = Session.BeginTransaction();
        }

        [TearDown]
        public void BaseTearDown()
        {
            _transaction.Rollback();
            if (Session != null)
                Session.Dispose();
        }

        protected ISession Session { get; private set; }
    }
}
