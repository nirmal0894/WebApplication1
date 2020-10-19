namespace Payroll.Integration.Configuration
{
    public interface IEnvironmentFeedConfiguration
    {
        string DirectoryToMonitor { get; }

        string FeedFileName { get; }
        
        string FeedFileAbsolutePath { get; }

        int IntervalInSecondsToPollDirectory { get; }
    }
}