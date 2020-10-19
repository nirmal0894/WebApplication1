namespace Payroll.Integration.Feeds
{
    using System.IO;
    using Payroll.Integration.Configuration;
    using global::Quartz;

    [DisallowConcurrentExecution]
    public class FeedFileProcessingJob : IInterruptableJob 
    {
        private readonly IEnvironmentFeedConfiguration config;
        private readonly IProcessFeedFiles feedFileProcessor;

        public FeedFileProcessingJob(
            IEnvironmentFeedConfiguration config,
            IProcessFeedFiles feedFileProcessor)
        {
            this.config = config;
            this.feedFileProcessor = feedFileProcessor;
        }

        public void Execute(IJobExecutionContext context)
        {
            if (File.Exists(config.FeedFileAbsolutePath) && FileIsNotBeingWrittenTo())
            {
                feedFileProcessor.Process();
            }
        }

        public void Interrupt()
        {
            feedFileProcessor.Stop();
        }

        private bool FileIsNotBeingWrittenTo()
        {
            FileStream stream = null;

            try
            {
                stream = File.Open(config.FeedFileAbsolutePath, FileMode.Open, FileAccess.ReadWrite, FileShare.None);
            }
            catch (IOException)
            {
                return false;
            }
            finally
            {
                if (stream != null)
                {
                    stream.Close();
                }
            }

            return true;
        }
    }
}