using Lib.Interfaces;
using Lib.Models;

namespace Lib.Domain
{
    public class MeterReadingService : IMeterReadingService
    {
        private readonly IRepositoryWrapper repositoryWrapper;
        private readonly IValidator<ParsedItem<MeterReading>> validator;

        public MeterReadingService(IRepositoryWrapper repositoryWrapper, IValidator<ParsedItem<MeterReading>> validator
            )
        {
            this.repositoryWrapper = repositoryWrapper;
            this.validator = validator;
        }

        public async Task<ProcessResult> ProcessMeterReadings(ParsedItemCollection<MeterReading> parsedItems)
        {
            var result = new ProcessResult();
            foreach (var parsedItem in parsedItems)
            {
                if (!await validator.IsValidAsync(parsedItem))
                {
                    result.Failed++;
                    continue;
                }
                await repositoryWrapper.MeterReadings.AddAsync(parsedItem.Item);
                result.Succeeded++;
            }

            return result;
        }
    }
}
