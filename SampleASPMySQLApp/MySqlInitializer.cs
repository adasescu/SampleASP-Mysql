using SampleASPMySQLApp.Models;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Web.Configuration;

namespace SampleASPMySQLApp
{
    public class MySqlInitializer : IDatabaseInitializer<ApplicationDbContext>
    {
        public void InitializeDatabase(ApplicationDbContext context)
        {
            if (!context.Database.Exists())
            {
                // if database did not exist before - create it
                context.Database.Create();
            }
            else
            {
                string connString = WebConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;
                string db = "Database=";
                string dbName = connString.Substring(connString.LastIndexOf(db) + db.Length, 33);

                // query to check if MigrationHistory table is present in the database
                var migrationHistoryTableExists = ((IObjectContextAdapter)context).ObjectContext.ExecuteStoreQuery<int>(
                string.Format(
                  "SELECT COUNT(*) FROM information_schema.tables WHERE table_schema = '{0}' AND table_name = '__MigrationHistory'",
                  dbName));

                // if MigrationHistory table is not there (which is the case first time we run) - create it
                if (migrationHistoryTableExists.FirstOrDefault() == 0)
                {
                    context.Database.Delete();
                    context.Database.Create();
                }
            }
        }
    }
}