using System;
using System.Configuration;
using System.IO;
using FA.MashUp.Utils;
using Microsoft.SqlServer.Dac;
using NHibernate;

namespace FA.MashUp.Tests.Helpers
{
    public class IntegrationSessionFactoryProvidor
    {
        private string _databaseName;

        private static IntegrationSessionFactoryProvidor _instance;
        public static IntegrationSessionFactoryProvidor Instance
        {
            get { return _instance ?? (_instance = new IntegrationSessionFactoryProvidor()); }
        }

        private ISessionFactory _sessionFactory;

        private IntegrationSessionFactoryProvidor() { }

        public void Initialize()
        {
            _sessionFactory = CreateSessionFactory(CreateDatabase());
        }

        private ISessionFactory CreateSessionFactory(string connectionString)
        {
            return SessionFactoryProvider.CreateSessionFactory(connectionString);
        }

        public ISession OpenSession()
        {
            return _sessionFactory.OpenSession();
        }

        public void Dispose()
        {
            if (_sessionFactory != null)
                _sessionFactory.Dispose();

            _sessionFactory = null;
            
            DeleteDatabase();
        }

        private string CreateDatabase()
        {
            Uri assemblyUri = new Uri(System.Reflection.Assembly.GetExecutingAssembly().CodeBase);
            string pathToDacpac = Path.Combine(Path.GetDirectoryName(assemblyUri.LocalPath), "FA.MashUp.Database.dacpac");

            string randomName = Path.GetRandomFileName();
            _databaseName = "FAMashUp_UnitTests_" + randomName.Substring(0, randomName.IndexOf('.'));
            string connectionString = string.Format(ConfigurationManager.ConnectionStrings["DatabaseFormat"].ConnectionString, _databaseName);
            DacServices services = new DacServices(connectionString);

            DacPackage dacpac = DacPackage.Load(pathToDacpac);

            DacDeployOptions deployOptions = new DacDeployOptions
            {
                AllowIncompatiblePlatform = true
            };

            services.Deploy(dacpac, _databaseName, true, deployOptions);

            return connectionString;
        }

        private void DeleteDatabase()
        {
            using (var connection = new System.Data.SqlClient.SqlConnection(ConfigurationManager.ConnectionStrings["DatabaseMaster"].ConnectionString))
            {
                connection.Open();
                using (var cmd = connection.CreateCommand())
                {
                    cmd.CommandType = System.Data.CommandType.Text;

                    cmd.CommandText = "ALTER DATABASE " + _databaseName + " SET SINGLE_USER WITH ROLLBACK IMMEDIATE";
                    cmd.ExecuteNonQuery();

                    cmd.CommandText = "DROP DATABASE " + _databaseName;
                    cmd.ExecuteNonQuery();
                }
            }
        }
    }
}
