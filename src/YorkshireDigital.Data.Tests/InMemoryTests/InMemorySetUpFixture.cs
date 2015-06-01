namespace YorkshireDigital.Data.Tests.InMemoryTests
{
    using NUnit.Framework;

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
