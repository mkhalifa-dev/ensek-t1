using Lib.Interfaces;
using Lib.Models;
using System.Text.RegularExpressions;

namespace Lib.Domain
{
    public class MeterReadingValidator: IValidator<ParsedItem<MeterReading>>
    {
        static Regex Only5DigitsRegex = new Regex(@"^\d{5}$", RegexOptions.Compiled);
        private readonly IAccountsRepository _accountsRepository;
        private readonly IMeterReadingsRepository _meterReadingsRepository;

        public MeterReadingValidator(IAccountsRepository accountsRepository, IMeterReadingsRepository meterReadingsRepository)
        {
            _accountsRepository = accountsRepository;
            _meterReadingsRepository = meterReadingsRepository;
        }

        public async Task<bool> IsValidAsync(ParsedItem<MeterReading> model)
        {
            if (model.ErrorMessage != null) return false;
            if (model.IsNotUnique) return false;

            var item = model.Item;
            if (!ValueIs5digits(item.MeterReadValue)) return false;
            if (!await AccountIdIsValid(item.AccountId)) return false;
            if (!await IsNotOlderThanPrevious(item.AccountId, item.MeterReadingDateTime)) return false;

            return true;
        }

        /// <summary>
        /// Reading values should be in the format NNNNN
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        private bool ValueIs5digits(string value)
        {
            return Only5DigitsRegex.IsMatch(value);
        }

        /// <summary>
        /// A meter reading must be associated with an Account ID to be deemed valid
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        private async Task<bool> AccountIdIsValid(int id)
        {
            var account = await _accountsRepository.GetByIdAsync(id);
            return account != null;
        }

        /// <summary>
        /// When an account has an existing read, ensure the new read isn’t older than the existing read
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        private async Task<bool> IsNotOlderThanPrevious(int accountId, DateTime readDateTime)
        {
            var reading = await _meterReadingsRepository.GetPreviousReading(accountId);
            return reading == null || readDateTime > reading.MeterReadingDateTime;
        }
    }
}
