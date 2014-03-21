namespace YorkshireTec.Tests.RepositoryTests
{
    using global::Raven.Client;
    using global::Raven.Client.Document;
    using global::Raven.Tests.Helpers;
    using NUnit.Framework;
    using YorkshireTec.Raven;

    [TestFixture]
    public class BaseRavenFixture : RavenTestBase
    {
        private DocumentStore documentStore;
        internal IDocumentSession DocumentSession;

        [SetUp]
        public void BaseSetUp()
        {
            documentStore = NewDocumentStore();
            DocumentSession = documentStore.OpenSession();
        }

        [TearDown]
        public void BaseTearDown()
        {
            DocumentSession.Dispose();
        }
    }
}
