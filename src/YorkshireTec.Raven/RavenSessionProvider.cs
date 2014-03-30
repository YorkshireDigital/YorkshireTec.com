namespace YorkshireTec.Raven
{
    using System.Configuration;
    using global::Raven.Client.Document;
    using global::Raven.Client.Embedded;
    using global::Raven.Database.Server;

    public class RavenSessionProvider
    {
        private static DocumentStore _documentStore;

        public bool SessionInitialized { get; set; }

        public static DocumentStore DocumentStore
        {
            get { return (_documentStore ?? (_documentStore = CreateDocumentStore())); }
        }

        public static DocumentStore EmbeddableDocumentStore
        {
            get { return (_documentStore ?? (_documentStore = CreateDocumentStore(true))); }
        }

        private static DocumentStore CreateDocumentStore(bool embeddable = false)
        {
            DocumentStore store;
            if (embeddable)
            {
                store = CreateEmbeddableDocumentStore();
            }
            else
            {
                store = new DocumentStore
                {
                    Url = ConfigurationManager.AppSettings["Raven_Url"],
                    DefaultDatabase = ConfigurationManager.AppSettings["Raven_Database"],
                    ApiKey = ConfigurationManager.AppSettings["Raven_ApiKey"],
                };
            }
            store.Initialize();

            return store;
        }

        private static DocumentStore CreateEmbeddableDocumentStore()
        {
            NonAdminHttp.EnsureCanListenToWhenInNonAdminContext(8082);
            return new EmbeddableDocumentStore
            {
                DataDirectory = "App_Data",
                UseEmbeddedHttpServer = true,
                Configuration = { Port = 8082 }
            };
        }
    }
}
