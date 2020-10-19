namespace Payroll.Integration.Feeds
{
    using Payroll.Integration.Configuration;
    using global::Quartz;
    using global::Quartz.Impl;
    using global::Quartz.Impl.Triggers;

    public class FeedFileJobScheduler : IScheduleFeedFileMonitoring
    {
        private readonly IScheduler scheduler;
        private readonly IEnvironmentFeedConfiguration feedConfiguration;

        public FeedFileJobScheduler(
            IScheduler scheduler,
            IEnvironmentFeedConfiguration feedConfiguration)
        {
            this.scheduler = scheduler;
            this.feedConfiguration = feedConfiguration;
        }

        public void Start()
        {
            var fileMonitoredWithoutSuffix = feedConfiguration.FeedFileName.Substring(0, feedConfiguration.FeedFileName.LastIndexOf("."));
            var jobName = string.Format("{0}_FeedFileMonitoringJob", fileMonitoredWithoutSuffix);
            var calendarName = string.Format("{0}_FeedFileEveryNSeconds", fileMonitoredWithoutSuffix);

            scheduler.ScheduleJob(
             new JobDetailImpl(
                 jobName, 
                 typeof(FeedFileProcessingJob)),
             new CalendarIntervalTriggerImpl(
                 calendarName, 
                 IntervalUnit.Second, 
                 feedConfiguration.IntervalInSecondsToPollDirectory));

            scheduler.Start();
        }

        public void Stop()
        {
            scheduler.Shutdown();
        }
    }
}