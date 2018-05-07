using Watchdog.Queries;

namespace Watchdog
{
    public interface IReportHealth
    {
        void ReportHealthy(InstanceIdentifier instanceIdentifier);
        void ReportError(InstanceIdentifier instanceIdentifier);
    }
}