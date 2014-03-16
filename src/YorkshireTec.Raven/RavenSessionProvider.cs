namespace YorkshireTec.Raven
{
    using System.Configuration;
    using global::Raven.Client.Document;

    public class RavenSessionProvider
    {
        private static DocumentStore _documentStore;

        public bool SessionInitialized { get; set; }

        public static DocumentStore DocumentStore
        {
            get { return (_documentStore ?? (_documentStore = CreateDocumentStore())); }
        }

        private static DocumentStore CreateDocumentStore()
        {
            var store = new DocumentStore
            {
                Url = ConfigurationManager.AppSettings["Raven_Url"],
                DefaultDatabase = ConfigurationManager.AppSettings["Raven_Database"],
                ApiKey = ConfigurationManager.AppSettings["Raven_ApiKey"],
            };
            store.Initialize();

            return store;
        }
    }
}
