using Lib.Models;

namespace Lib.Interfaces
{
    public interface IMeterReadingService
    {
        Task<ProcessResult> ProcessMeterReadings(ParsedItemCollection<MeterReading> parsedItems);
    }
}