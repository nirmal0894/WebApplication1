namespace PayrollToPositionService.Integration.Parsing
{
    using System.Collections.Generic;

    public interface IParseFeedFilesFromPayroll
    {
        IEnumerable<PayrollColleagueJob> GetJobRecords();
    }
}