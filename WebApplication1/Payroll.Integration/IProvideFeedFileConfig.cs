namespace Payroll.Integration
{
    public interface IProvideFeedFileConfig
    {
        string DirectoryToMonitor { get; }

        string FeedFileName { get; }

        string FeedFileAbsolutePath { get; }
    }
}