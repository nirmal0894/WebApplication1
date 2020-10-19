namespace PayrollToPositionService.Integration.Parsing
{
    using System.Collections.Generic;
    using global::Payroll.Integration.Configuration;
    using Tesco.Logging;

    public class LoggingPayrollColleagueJobFeedFileParser : IParseFeedFilesFromPayroll
    {
        private readonly IEnvironmentFeedConfiguration config;
        private readonly IParseFeedFilesFromPayroll decoratedParser;
        private readonly ILoggingFrameworkAdapter logger;

        public LoggingPayrollColleagueJobFeedFileParser(
            IEnvironmentFeedConfiguration config,
            IParseFeedFilesFromPayroll decoratedParser,
            ILoggingFrameworkAdapter logger)
        {
            this.config = config;
            this.decoratedParser = decoratedParser;
            this.logger = logger;
        }

        public IEnumerable<PayrollColleagueJob> GetJobRecords()
        {
            logger.LogDebug("About to get position rows from file. Configuration:", config);
            var rows = decoratedParser.GetJobRecords();
            logger.LogDebug("Retrieved rows from feed file");
            return rows;
        }
    }
}
