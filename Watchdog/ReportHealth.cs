using System;
using System.Fabric;
using System.Fabric.Health;
using Watchdog.Queries;

namespace Watchdog
{
    public class ReportHealth : IReportHealth
    {
        private readonly FabricClient _fabricClient;

        public ReportHealth()
        {
            _fabricClient = new FabricClient();
        }

        public void ReportHealthy(InstanceIdentifier instanceIdentifier)
        {
            SendHealthReport(instanceIdentifier, HealthState.Ok);
        }

        public void ReportError(InstanceIdentifier instanceIdentifier)
        {
            SendHealthReport(instanceIdentifier, HealthState.Error);
        }

        private void SendHealthReport(InstanceIdentifier instanceIdentifier, HealthState state)
        {
            var healthInformation = new HealthInformation("Watchdog", "Healthcheck", state)
            {
                TimeToLive = TimeSpan.FromSeconds(30)
            };

            var healthReport = new StatelessServiceInstanceHealthReport(instanceIdentifier.PartitionId,
                instanceIdentifier.InstanceId, healthInformation);

            _fabricClient.HealthManager.ReportHealth(healthReport);
        }
    }
}
