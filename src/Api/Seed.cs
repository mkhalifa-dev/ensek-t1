using Lib.Models;
using Lib.Repositories;

namespace Api
{
    public static class Seed
    {
        public static async Task AddAcounts(DbConfig dbconfig)
        {
            var lines = File.ReadAllLines("../../data/Test_Accounts.csv");
            var items = lines.Skip(1).Select(x => new ParsedItem<Account>
            {
                Item = new Account()
                {
                    AccountId = int.Parse(x.Split(',')[0]),
                    FirstName = x.Split(',')[1],
                    LastName = x.Split(',')[2]
                }
            });
            var accountsRepo = new SqliteAccountsRepository(dbconfig);
            var meterReadingsRepo = new SqliteMeterReadingsRepository(dbconfig);
            var parsed = new ParsedItemCollection<Account>(items);
        
            await accountsRepo.Init(parsed);
            await meterReadingsRepo.Init();
        }
    }
}
