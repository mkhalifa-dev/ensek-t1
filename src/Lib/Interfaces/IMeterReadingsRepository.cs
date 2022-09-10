using Lib.Models;

namespace Lib.Interfaces
{
    public interface IMeterReadingsRepository
    {
        Task<MeterReading> GetPreviousReading(int accountId);
        Task AddAsync(MeterReading meterReading);
    }
}
