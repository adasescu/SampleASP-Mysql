using SampleASPMySQLApp.Models;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using MySql.Data.MySqlClient;
using System.Web.Configuration;

namespace SampleASPMySQLApp
{
    public class MySqlInitializer : IDatabaseInitializer<ApplicationDbContext>
    {
        public void InitializeDatabase(ApplicationDbContext context)
        {
            string db = new MySqlConnectionStringBuilder(WebConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString).Database;

            // query to check if MigrationHistory table is present in the database
            var migrationHistoryTableExists = ((IObjectContextAdapter)context).ObjectContext.ExecuteStoreQuery<int>(
                string.Format(
                "SELECT COUNT(*) FROM information_schema.tables WHERE table_schema = '{0}' AND table_name = '__MigrationHistory'",
                db));

            bool dbMigrated = migrationHistoryTableExists.FirstOrDefault() != 0;

            if (!dbMigrated)
            {
                // if MigrationHistory table is not there (which is the case first time we run) - create it
                context.Database.Delete();
                context.Database.Create();
            }
        }
    }
}