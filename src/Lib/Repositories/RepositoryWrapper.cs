using Lib.Interfaces;

namespace Lib.Repositories
{
    public class RepositoryWrapper : IRepositoryWrapper
    {
        public RepositoryWrapper(IAccountsRepository accounts, IMeterReadingsRepository meterReadings)
        {
            Accounts = accounts;
            MeterReadings = meterReadings;
        }

        public IAccountsRepository Accounts { get; set; }
        public IMeterReadingsRepository MeterReadings { get; set; }

        public void Save()
        {
            // Commit transaction
        }
    }
}
