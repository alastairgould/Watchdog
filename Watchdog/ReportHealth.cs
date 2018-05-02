using System.Fabric;
using System.Fabric.Health;
using Watchdog.Queries;

namespace Watchdog
{
    public class ReportHealth 
    {
        private readonly InstanceIdentifier _instanceIdentifier;

        public ReportHealth(InstanceIdentifier instanceIdentifier)
        {
            _instanceIdentifier = instanceIdentifier;
        }

        public void ReportHealthy()
        {
            var fabricClient = new FabricClient();
            fabricClient.HealthManager.ReportHealth(new StatelessServiceInstanceHealthReport(_instanceIdentifier.PartitionId, _instanceIdentifier.InstanceId, new HealthInformation("Watchdog", "Healthcheck", HealthState.Ok)));
        }

        public void ReportError()
        {
            var fabricClient = new FabricClient();
            fabricClient.HealthManager.ReportHealth(new StatelessServiceInstanceHealthReport(_instanceIdentifier.PartitionId, _instanceIdentifier.InstanceId, new HealthInformation("Watchdog", "Healthcheck", HealthState.Error)));
        }
    }
}
