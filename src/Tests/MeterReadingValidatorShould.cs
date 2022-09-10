using Lib.Domain;
using Lib.Interfaces;
using Lib.Models;
using Moq;

namespace Tests
{
    public class MeterReadingValidatorShould
    {
        IValidator<ParsedItem<MeterReading>> sut;

        [SetUp]
        public void Setup()
        {
            var accountsRepo = new Mock<IAccountsRepository>();
            var meterReadingsRepoMock = new Mock<IMeterReadingsRepository>();

            accountsRepo.Setup(x => x.GetByIdAsync(It.Is<int>(x => x == 1))).ReturnsAsync(
            new Account
            {
                AccountId = 1
            });

            meterReadingsRepoMock.Setup(x => x.GetPreviousReading(1)).ReturnsAsync(new MeterReading
            {
                AccountId = 1,
                MeterReadingDateTime = DateTime.Now.Subtract(TimeSpan.FromDays(30)),
                MeterReadValue = "1"
            });

            sut = new MeterReadingValidator(accountsRepo.Object, meterReadingsRepoMock.Object);
        }

        // Positive cases
        [TestCase("12345", ExpectedResult = true)]
        [TestCase("00001", ExpectedResult = true)]

        // Negative cases
        [TestCase("", ExpectedResult = false)]
        [TestCase("VOID", ExpectedResult = false)]
        [TestCase("123456", ExpectedResult = false)]
        [TestCase("ABCDE", ExpectedResult = false)]
        [TestCase("ABCDE12345", ExpectedResult = false)]
        public async Task<bool> Validate_When_Reading_Values_Is_In_Format_NNNNN(string value)
        {
            var sampleReading = new MeterReading
            {
                AccountId = 1,
                MeterReadingDateTime = DateTime.UtcNow,
                MeterReadValue = value
            };

            var result = await sut.IsValidAsync(new ParsedItem<MeterReading>
            {
                Item = sampleReading
            });

            return result;
        }

        [TestCase(1,ExpectedResult = true)]
        [TestCase(2,ExpectedResult = false)]
        [TestCase(3,ExpectedResult = false)]
        public async Task<bool> Validate_When_Reading_Is_Associated_To_A_Valid_AccountId_1(int accountId)
        {
            var sampleReading = new MeterReading
            {
                AccountId = accountId,
                MeterReadingDateTime = DateTime.UtcNow,
                MeterReadValue = "12345"
            };

            var result = await sut.IsValidAsync(new ParsedItem<MeterReading>
            {
                Item = sampleReading
            });
            return result;
        }

        [TestCase(true, ExpectedResult = false)]
        [TestCase(false, ExpectedResult = true)]
        public async Task<bool> Validate_When_Reading_Is_Not_Unique(bool notUnique)
        {
            var sampleReading = new MeterReading
            {
                AccountId = 1,
                MeterReadingDateTime = DateTime.UtcNow,
                MeterReadValue = "12345"
            };

            var result = await sut.IsValidAsync(new ParsedItem<MeterReading>
            {
                IsNotUnique = notUnique,
                Item = sampleReading
            });
            return result;
        }

        [TestCase(null, ExpectedResult = true)]
        [TestCase("", ExpectedResult = false)]
        [TestCase(" ", ExpectedResult = false)]
        [TestCase("123", ExpectedResult = false)]
        public async Task<bool> Validate_When_Reading_Failed_To_Parse(string error)
        {
            var sampleReading = new MeterReading
            {
                AccountId = 1,
                MeterReadingDateTime = DateTime.UtcNow,
                MeterReadValue = "12345"
            };

            var result = await sut.IsValidAsync(new ParsedItem<MeterReading>
            {
                ErrorMessage = error,
                Item = sampleReading
            });
            return result;
        }

        [TestCase(10, ExpectedResult = true)]
        [TestCase(32, ExpectedResult = false)]
        public async Task<bool> Validate_When_Reading_Date_Is_Older_Than_Previous(int age)
        {
            var sampleReading = new MeterReading
            {
                AccountId = 1,
                MeterReadingDateTime = DateTime.Now.Subtract(TimeSpan.FromDays(age)),
                MeterReadValue = "12345"
            };

            var result = await sut.IsValidAsync(new ParsedItem<MeterReading>
            {
                Item = sampleReading
            });
            return result;
        }
    }
}