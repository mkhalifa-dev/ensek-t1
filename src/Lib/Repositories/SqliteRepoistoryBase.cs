using Lib.Models;
using Microsoft.Data.Sqlite;
using System.Data.SqlClient;

namespace Lib.Repositories
{
    public abstract class SqliteRepoistoryBase
    {
        private readonly DbConfig dbConfig;
        
        public SqliteRepoistoryBase(DbConfig dbConfig)
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
