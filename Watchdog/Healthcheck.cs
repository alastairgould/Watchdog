using System;
using Watchdog.Queries;

namespace Watchdog
{
    public class Healthcheck
    {
        private readonly Uri _healthcheckUri;
        private readonly ReportHealth _reportHealth;

        public Healthcheck(HealthcheckEndpoint healthcheckEndpoint)
        {
            _healthcheckUri = healthcheckEndpoint.HealthcheckUri;
            _reportHealth = new ReportHealth(healthcheckEndpoint.InstanceIdentifier);
        }

        public void PerformHealthcheck()
        {
            _reportHealth.Report();
        }
    }
}
