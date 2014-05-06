namespace YorkshireTec.Tests.ServiceTests
{
    using NUnit.Framework;
    using YorkshireTec.Tests.Helpers;

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
