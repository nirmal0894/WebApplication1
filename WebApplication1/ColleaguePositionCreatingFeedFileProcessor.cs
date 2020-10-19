namespace PayrollToPositionService.Integration.Position
{
    using Payroll.Integration.Feeds;

    public class ColleaguePositionCreatingFeedFileProcessor : IProcessFeedFiles
    {
        private readonly ICreatePositions positionCreator;

        public ColleaguePositionCreatingFeedFileProcessor(
            ICreatePositions positionCreator)
        {
            this.positionCreator = positionCreator;
        }

        public void Process()
        {
            positionCreator.ProcessPayrollFeedFile();
        }

        public void Stop()
        {
            positionCreator.StopProcessing();
        }
    }
}