namespace Payroll.Integration.Feeds
{
    public interface IScheduleFeedFileMonitoring
    {
        void Start();
        
        void Stop();
    }
}