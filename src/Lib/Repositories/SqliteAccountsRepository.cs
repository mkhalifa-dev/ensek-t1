using Dapper;
using Lib.Interfaces;
using Lib.Models;

namespace Lib.Repositories
{
    public class SqliteAccountsRepository : SqliteRepositoryBase, IAccountsRepository
    {
        public SqliteAccountsRepository(DbConfig dbConfig) : base(dbConfig)
        {
        }

        public async Task<Account> GetByIdAsync(int id)
        {
            using(var connection = CreateOpenConnection())
            {
                var sql = @"SELECT AccountId, FirstName, LastName
                            FROM Accounts
                            WHERE AccountId = @id";

                return await connection.QueryFirstOrDefaultAsync<Account>(sql, new {id});
            }
        }


        public async Task Init(ParsedItemCollection<Account> accounts)
        {
            using (var connection = CreateOpenConnection())
            {
                connection.Execute("DROP TABLE IF EXISTS Accounts; Create Table Accounts (" +
                    "AccountId int NOT NULL," +
                    "FirstName VARCHAR(100) NOT NULL," +
                    "LastName VARCHAR(1000) NOT NULL);");

                var sql = @"INSERT INTO Accounts(AccountId, FirstName, LastName)
                            VAlUES(@AccountId, @FirstName, @LastName)";

                foreach (var account in accounts)
                {
                    await connection.ExecuteAsync(sql, account.Item);
                }
            }
        }
    }
}
