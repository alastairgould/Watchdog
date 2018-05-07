using System;

namespace Watchdog.Queries
{
    public class HealthcheckEndpoint
    {
        public Uri HealthcheckUri { get; }
        public InstanceIdentifier InstanceIdentifier { get; }

        public HealthcheckEndpoint(Uri healthCheckUri, InstanceIdentifier instanceIdentifier)
        {
            HealthcheckUri = healthCheckUri;
            InstanceIdentifier = instanceIdentifier;
        }
    }
}
