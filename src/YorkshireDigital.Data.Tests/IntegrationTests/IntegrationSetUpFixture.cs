using NUnit.Framework;

namespace YorkshireDigital.Data.Tests.IntegrationTests
{
    [SetUpFixture]
    public class IntegrationSetUpFixture
    {
        [SetUp]
        public void Setup()
        {
            IntegrationSessionFactoryProvidor.Instance.Initialize();
        }

        [TearDown]
        public void TestTeardown()
        {
            IntegrationSessionFactoryProvidor.Instance.Dispose();
        }
    }
}
