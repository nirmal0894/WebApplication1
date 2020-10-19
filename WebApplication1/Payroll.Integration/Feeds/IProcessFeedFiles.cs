namespace Payroll.Integration.Feeds
{
    public interface IProcessFeedFiles
    {
        void Process();

        void Stop();
    }
}