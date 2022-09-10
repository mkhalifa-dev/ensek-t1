using Lib.Domain;
using Lib.Interfaces;
using Lib.Models;
using Moq;
using System.Text;

namespace Tests
{
    public class MeterReadingServiceShould
    {
        Stream csvStream;
        MeterReadingService sut;

        [SetUp]
        public void Setup()
        {
            var csv = @"AccountId,MeterReadingDateTime,MeterReadValue
                        2344,22/04/2019 09:24,1002
                        2233,22/04/2019 12:25,323
                        8766,22/04/2019 12:25,3440
                        2344,22/04/2019 12:25,1002
                        2345,22/04/2019 12:25,45522
                        2346,22/04/2019 12:25,999999
                        2347,22/04/2019 12:25,54
                        2348,22/04/2019 12:25,123
                        2349,22/04/2019 12:25,VOID
                        2350,22/04/2019 12:25,5684
                        2351,22/04/2019 12:25,57579
                        2352,22/04/2019 12:25,455";

            csvStream = new MemoryStream(Encoding.UTF8.GetBytes(csv));

            var repoMock = new Mock<IRepositoryWrapper>();
            var accountsRepoMock = new Mock<IAccountsRepository>();
            var meterReadingsRepoMock = new Mock<IMeterReadingsRepository>();

            repoMock.Setup(x => x.Accounts).Returns(accountsRepoMock.Object);
            repoMock.Setup(x => x.MeterReadings).Returns(() => new Mock<IMeterReadingsRepository>().Object);
            accountsRepoMock.Setup(x => x.GetByIdAsync(2345)).ReturnsAsync(new Account
            {
                AccountId = 2345
            });

            var validator = new MeterReadingValidator(accountsRepoMock.Object, meterReadingsRepoMock.Object);
            sut = new MeterReadingService(repoMock.Object, validator);
        }

        [TestCase(1, ExpectedResult = true)]
        [TestCase(2, ExpectedResult = false)]
        public async Task<bool> Return_Succeeded(int succeeded)
        {
            var parser = new MeterReadingsParser();
            var items = parser.GetItems(csvStream);
            var result = await sut.ProcessMeterReadings(items);

            return result.Succeeded == succeeded;
        }


        [TestCase(ExpectedResult = 11)]
        public async Task<int> Return_Failed()
        {
            var parser = new MeterReadingsParser();
            var items = parser.GetItems(csvStream);
            var result = await sut.ProcessMeterReadings(items);

            return result.Failed;
        }

    }
}