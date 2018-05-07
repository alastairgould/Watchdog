using System;
using System.Net;
using Watchdog.Queries;

namespace Watchdog
{
    public class Healthcheck
    {
        private readonly IHealthcheckClient _healthcheckClient;
        private readonly ReportHealth _reportHealth;

        public Healthcheck(HealthcheckEndpoint healthcheckEndpoint, Func<Uri, IHealthcheckClient> createHealthcheckClient, Func<InstanceIdentifier, ReportHealth> createReportHealth)
        {
            _healthcheckClient = createHealthcheckClient(healthcheckEndpoint.HealthcheckUri);
            _reportHealth = createReportHealth(healthcheckEndpoint.InstanceIdentifier);
        }

        public void PerformHealthcheck()
        {
            var healthcheckResponse = _healthcheckClient.GetHealthcheckAsync().GetAwaiter().GetResult();

            if (healthcheckResponse.StatusCode == HttpStatusCode.OK)
            {
                _reportHealth.ReportHealthy();
            }
            else
            {
                _reportHealth.ReportError();
            }
        }
    }
}
