namespace Payroll.Integration
{
    using System.Collections.Generic;

    public interface IFeedFileParser
    {
        IEnumerable<T> GetAllRows<T>(string filename) where T : class, new();
    }
}