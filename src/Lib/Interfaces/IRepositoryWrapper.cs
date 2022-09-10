namespace Lib.Interfaces
{
    public interface IRepositoryWrapper
    {
        IAccountsRepository Accounts { get; set; }
        IMeterReadingsRepository MeterReadings { get; set; }

        void Save();
    }
}