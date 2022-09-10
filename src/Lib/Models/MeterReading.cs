namespace Lib.Models
{
    public class MeterReading : IEquatable<MeterReading>
    {
        public int AccountId { get; set; }
        public DateTime MeterReadingDateTime { get; set; }
        public string MeterReadValue { get; set; }

        public bool Equals(MeterReading other)
        {
            return AccountId == other.AccountId && MeterReadValue == other.MeterReadValue;
        }
    }
}
