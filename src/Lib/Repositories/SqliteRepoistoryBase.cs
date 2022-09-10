using Lib.Models;
using Microsoft.Data.Sqlite;

namespace Lib.Repositories
{
    public abstract class SqliteRepositoryBase
    {
        private readonly DbConfig dbConfig;
        
        public SqliteRepositoryBase(DbConfig dbConfig)
        {
            this.dbConfig = dbConfig;
        }
        public virtual SqliteConnection CreateOpenConnection()
        {
            var connection = new SqliteConnection("Data Source=" + dbConfig.DataPath);
            
            connection.Open();
            return connection;
        }
    }
}
