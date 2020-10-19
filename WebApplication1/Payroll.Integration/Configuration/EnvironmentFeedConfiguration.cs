namespace Payroll.Integration.Configuration
{
    using System;
    using System.IO;
    using Infrastructure.Common.Configuration;

    public class EnvironmentFeedConfiguration<TApplication> : IEnvironmentFeedConfiguration where TApplication : ColleagueApplication
    {
        private static readonly Lazy<FeedFileConfig> Config;

        static EnvironmentFeedConfiguration()
        {
            Config = new Lazy<FeedFileConfig>(
                () => new EnvironmentConfigProvider<TApplication>().GetConfigSection<FeedFileConfig>());
        }

        public string DirectoryToMonitor
        {
            get { return Config.Value.DirectoryToMonitor; }
        }

        public string FeedFileName
        {
            get { return Config.Value.FileName; }
        }

        public string FeedFileAbsolutePath
        {
            get { return Path.Combine(DirectoryToMonitor, FeedFileName); }
        }

        public int IntervalInSecondsToPollDirectory
        {
            get { return Config.Value.IntervalInSecondsToPollDirectory; }
        }
    }
}