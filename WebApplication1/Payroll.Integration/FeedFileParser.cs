namespace Payroll.Integration
{
    using System.Collections.Generic;
    using LINQtoCSV;

    public class FeedFileParser : IFeedFileParser
    {
        public IEnumerable<T> GetAllRows<T>(string feedFileName) where T : class, new()
        {
            CsvFileDescription inputFileDescription = new CsvFileDescription
            {
                SeparatorChar = '~',
                IgnoreTrailingSeparatorChar = true,
                FirstLineHasColumnNames = true
            };

            var cc = new CsvContext();

            return cc.Read<T>(feedFileName, inputFileDescription);
        }
    }
}
