namespace YorkshireDigital.Data.Tests.ServiceTests
{
    using NUnit.Framework;
    using YorkshireDigital.Data.Tests.Helpers;

    [SetUpFixture]
    public class InMemorySetUpFixture
    {
        [SetUp]
        public void Setup()
        {
            InMemorySessionFactoryProvider.Instance.Initialize();
        }

        [TearDown]
        public void TestTeardown()
        {
            InMemorySessionFactoryProvider.Instance.Dispose();
        }
    }
}
