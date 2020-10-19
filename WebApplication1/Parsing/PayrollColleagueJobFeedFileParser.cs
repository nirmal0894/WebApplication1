namespace PayrollToPositionService.Integration.Parsing
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Text;
    using global::Payroll.Integration.Configuration;
    using GPGService;
    using GPGService.Contracts;
    using Tesco.Logging;

    public class PayrollColleagueJobFeedFileParser : IParseFeedFilesFromPayroll
    {
        private readonly IEnvironmentFeedConfiguration config;
        private readonly ILoggingFrameworkAdapter loggingFrameworkAdapter;
        private readonly IGpgService gpgService;
        private readonly IDecryptionConfiguration decryptionConfiguration;

        public PayrollColleagueJobFeedFileParser(
            IEnvironmentFeedConfiguration config, 
            ILoggingFrameworkAdapter loggingFrameworkAdapter,
            IGpgService gpgService,
            IDecryptionConfiguration decryptionConfiguration)
        {
            this.config = config;
            this.loggingFrameworkAdapter = loggingFrameworkAdapter;
            this.gpgService = gpgService;
            this.decryptionConfiguration = decryptionConfiguration;
        }

        public IEnumerable<PayrollColleagueJob> GetJobRecords()
        {
            var allRows = new List<string>();

            try
            {
                var text = File.ReadAllText(config.FeedFileAbsolutePath);

                byte[] byteArray = Encoding.UTF8.GetBytes(text);

                var result = gpgService.DecryptAndReturnStream(this.decryptionConfiguration.GpgHomePath, this.decryptionConfiguration.BinaryPath, this.decryptionConfiguration.Passphrase, byteArray);

                using (var streamReader = new StreamReader(result))
                {
                    string line = String.Empty;
                    while ((line = streamReader.ReadLine()) != null)
                    {
                        allRows.Add(line);
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }

            var header = "EMP_NUM~JOB_CD_STR_DT~JOB_CD~JOB_DESC~POS_STR_DT~POS_NUM~POS_DESC~STR_DT~LEAVING_DT~BRN_TRN_DT~BRN_NUM~BRN_NAME~LEAVING_RSN_CD~LEAVING_RSN_DESC~LATEST_STR_DT~REINSTM_CODE~REINSTM_DESC~BASIC_HRS_STR_DT~BASIC_HRS~CONT_PAY_STR_DT~CONT_PAY~WORK_LEVEL~CONTRACT_TYPE~DEPT~SECTION~PEN_SENIOR_IND";
            var separator = '~';
            var headerColumns = header.Split(separator);

            foreach (var row in allRows.Skip(1).Where(row => !string.IsNullOrWhiteSpace(row)))
            {
                var rowColumns = row.Split(separator);
                var workLevelIndex = Array.IndexOf(headerColumns, "WORK_LEVEL");
                var isSeniorIndex = Array.IndexOf(headerColumns, "PEN_SENIOR_IND");
                var leavingDateIndex = Array.IndexOf(headerColumns, "LEAVING_DT");
                var employeeNumberIndex = Array.IndexOf(headerColumns, "EMP_NUM");
                var branchNumber = Array.IndexOf(headerColumns, "BRN_NUM");

                yield return new PayrollColleagueJob
                              {
                                  WorkLevel = rowColumns[workLevelIndex],
                                  IsSenior = rowColumns[isSeniorIndex],
                                  LeavingDate = rowColumns[leavingDateIndex],
                                  EmployeeNumber = rowColumns[employeeNumberIndex],
                                  BranchNumber = rowColumns[branchNumber].Trim()
                              };
            }
        }
    }
}