using NHibernate;
using System;
using System.Configuration;
using System.IO;
using System.Linq;
using YorkshireDigital.Data.NHibernate;

namespace YorkshireDigital.Data.Tests.IntegrationTests
{
    public class IntegrationSessionFactoryProvidor
    {
        string _databaseName;

        static IntegrationSessionFactoryProvidor _instance;
        public static IntegrationSessionFactoryProvidor Instance
        {
            get { return _instance ?? (_instance = new IntegrationSessionFactoryProvidor()); }
        }

        ISessionFactory _sessionFactory;

        IntegrationSessionFactoryProvidor() { }

        public void Initialize()
        {
            _sessionFactory = CreateSessionFactory(CreateDatabase());
        }

        ISessionFactory CreateSessionFactory(string connectionString)
        {
            return NHibernateSessionFactoryProvider.BuildSessionFactory(connectionString);
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

        string CreateDatabase()
        {
            string randomName = Path.GetRandomFileName();
            _databaseName = "YorkshireDigital_Database_" + randomName.Substring(0, randomName.IndexOf('.'));

            using (var connection = new System.Data.SqlClient.SqlConnection(string.Format(ConfigurationManager.ConnectionStrings["DatabaseFormat"].ConnectionString, "master")))
            {
                connection.Open();
                using (var cmd = connection.CreateCommand())
                {
                    cmd.CommandType = System.Data.CommandType.Text;
                    cmd.CommandText = "CREATE DATABASE " + _databaseName;
                    cmd.ExecuteNonQuery();
                }
            }

            string connectionString = string.Format(ConfigurationManager.ConnectionStrings["DatabaseFormat"].ConnectionString, _databaseName);
            CreateDatabaseSchema(connectionString);

            return connectionString;
        }

        static void CreateDatabaseSchema(string connectionString)
        {
            Uri assemblyUri = new Uri(System.Reflection.Assembly.GetExecutingAssembly().CodeBase);
            string pathToCreateScript = Path.Combine(Path.GetDirectoryName(assemblyUri.LocalPath), "YorkshireDigital.Database_Create.sql");

            string script = File.ReadAllText(pathToCreateScript);

            int indexOfUse = script.IndexOf("EXECUTE sp_fulltext_database 'enable';", StringComparison.Ordinal);
            script = script.Remove(0, script.IndexOf("GO\r\n", indexOfUse, StringComparison.Ordinal) + 2);

            int indexOfDeclare = script.IndexOf("DECLARE @VarDecimalSupported AS BIT;", StringComparison.Ordinal);
            script = script.Remove(indexOfDeclare, (script.IndexOf("GO\r\n", indexOfDeclare, StringComparison.Ordinal) + 2) - indexOfDeclare);

            int indexOfAlter = script.IndexOf("ALTER DATABASE [$(DatabaseName)]", StringComparison.Ordinal);
            script = script.Remove(indexOfAlter, (script.IndexOf("GO\r\n", indexOfAlter, StringComparison.Ordinal) + 2) - indexOfAlter);


            using (var connection = new System.Data.SqlClient.SqlConnection(connectionString))
            {
                connection.Open();
                using (var cmd = connection.CreateCommand())
                {
                    foreach (var statement in script.Split(new string[] { "GO" }, StringSplitOptions.None).Where(x => !x.Trim().StartsWith("PRINT", StringComparison.Ordinal)))
                    {
                        cmd.CommandType = System.Data.CommandType.Text;
                        cmd.CommandText = statement;
                        cmd.ExecuteNonQuery();
                    }
                }
            }
        }

        void DeleteDatabase()
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
