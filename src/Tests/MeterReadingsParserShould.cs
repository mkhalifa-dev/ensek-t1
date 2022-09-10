using Lib.Domain;
using System.Text;

namespace Tests
{
    public class MeterReadingsParserShould
    {
        Stream csvStream;

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
        }

        [TestCase(2344, ExpectedResult = true)]
        [TestCase(2350, ExpectedResult = false)]
        public bool Mark_NonUnique_Readings(int id)
        {
            var sut = new MeterReadingsParser();
            var result = sut.GetItems(csvStream);

            return result.Where(x=> x.Item?.AccountId == id).Any(x=> x.IsNotUnique);
        }
    }
}