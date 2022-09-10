using Lib.Interfaces;
using Lib.Models;
using Microsoft.VisualBasic.FileIO;

namespace Lib.Domain
{

    public class MeterReadingsParser: ICsvParser<MeterReading>
    {
        public ParsedItemCollection<MeterReading> GetItems(Stream stream)
        {
            var uniqueEntries = new HashSet<string>();
            var results = new ParsedItemCollection<MeterReading>();
            var fieldParser = new TextFieldParser(stream);
            fieldParser.HasFieldsEnclosedInQuotes = false;
            fieldParser.SetDelimiters(",");
            fieldParser.TrimWhiteSpace = true;
            
            //discard header row
            fieldParser.ReadLine();

            string[] fields;
            while (!fieldParser.EndOfData)
            {
                fields = fieldParser.ReadFields();
                var accountId = fields[0];
                var readingDate = fields[1];
                var readingValue = fields[2];

                var uniqueKey = string.Concat(accountId, ",", readingValue);
                var line = string.Join(",", fields);
                var result = new ParsedItem<MeterReading>()
                {
                    RawLine = line
                };

                try
                {
                    result.Item = new MeterReading
                    {
                        AccountId = int.Parse(accountId),
                        MeterReadingDateTime = DateTime.Parse(readingDate),
                        MeterReadValue = readingValue
                    };

                    if (uniqueEntries.Contains(uniqueKey))
                    {
                        result.IsNotUnique = true;
                        throw new Exception("Duplicate entry!");
                    }
                }
                catch (Exception ex)
                {
                    result.ErrorMessage = ex.Message;
                }

                results.Add(result);
                uniqueEntries.Add(uniqueKey);
            }

            fieldParser.Close();
            return results;
        }
    }
}
