namespace Payroll.Integration.Configuration
{
    using Newtonsoft.Json;

    public class FeedFileConfig
    {
        [JsonProperty(Required = Required.Always)]
        public string DirectoryToMonitor { get; set; }

        [JsonProperty(Required = Required.Always)]
        public string FileName { get; set; }

        [JsonProperty(Required = Required.Always)]
        public int IntervalInSecondsToPollDirectory { get; set; }
    }
}
