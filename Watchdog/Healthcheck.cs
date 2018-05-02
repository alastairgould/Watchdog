using System;
using System.Net;
using System.Net.Http;
using Watchdog.Queries;

namespace Watchdog
{
    public class Healthcheck
    {
        private readonly Uri _healthcheckUri;
        private readonly ReportHealth _reportHealth;

        public Healthcheck(HealthcheckEndpoint healthcheckEndpoint, Func<InstanceIdentifier, ReportHealth> createReportHealth)
        {
            _healthcheckUri = healthcheckEndpoint.HealthcheckUri;
            _reportHealth = createReportHealth(healthcheckEndpoint.InstanceIdentifier);
        }

        public void PerformHealthcheck()
        {
            var httpClient = new HttpClient();
            var result = httpClient.GetAsync(_healthcheckUri).GetAwaiter().GetResult();

            if (result.StatusCode == HttpStatusCode.OK)
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
