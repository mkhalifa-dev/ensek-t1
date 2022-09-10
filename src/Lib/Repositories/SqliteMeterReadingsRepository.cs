using Dapper;
using Lib.Interfaces;
using Lib.Models;

namespace Lib.Repositories
{
    public class SqliteMeterReadingsRepository : SqliteRepositoryBase, IMeterReadingsRepository
    {
        public SqliteMeterReadingsRepository(DbConfig dbConfig) : base(dbConfig)
        {
        }

        public async Task<MeterReading> GetPreviousReading(int accountId)
        {
            using (var connection = CreateOpenConnection())
            {
                var sql = @"SELECT AccountId, MeterReadingDateTime, MeterReadValue
                            FROM Meter_Readings
                            WHERE AccountId = @accountId
                            ORDER BY MeterReadingDateTime DESC
                            LIMIT 1";

                return await connection.QueryFirstOrDefaultAsync<MeterReading>(sql, new { accountId });
            }
        }

        public async Task AddAsync(MeterReading meterReading)
        {
            using (var connection = CreateOpenConnection())
            {
                var sql = @"INSERT INTO Meter_Readings(AccountId, MeterReadingDateTime, MeterReadValue)
                            VALUES(@AccountId, @MeterReadingDateTime, @MeterReadValue)";

                var affectedRows = await connection.ExecuteAsync(sql, meterReading);

                if (affectedRows != 1)
                {
                    throw new DataException("Failed to add MeterReading to database");
                }
            }
        }
        public async Task Init()
        {
            using (var connection = CreateOpenConnection())
            {
                await connection.ExecuteAsync("DROP TABLE IF EXISTS Meter_Readings; Create Table Meter_Readings(" +
                    "AccountId int NOT NULL," +
                    "MeterReadingDateTime DATETIME NOT NULL," +
                    "MeterReadValue VARCHAR(5) NOT NULL);");
            }
        }
    }
}
