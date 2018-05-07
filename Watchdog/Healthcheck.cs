using System.Net;
using System.Threading.Tasks;
using Watchdog.Queries;

namespace Watchdog
{
    public class Healthcheck
    {
        private readonly HealthcheckEndpoint _healthcheckEndpoint;
        private readonly IHealthcheckClient _healthcheckClient;
        private readonly IReportHealth _reportHealth;

        public Healthcheck(HealthcheckEndpoint healthcheckEndpoint, 
            IReportHealth reportHealth,
            IHealthcheckClient healthcheckClient)
        {
            _healthcheckEndpoint = healthcheckEndpoint;
            _healthcheckClient = healthcheckClient;
            _reportHealth = reportHealth;
        }

        public async Task PerformHealthcheckAsync()
        {
            var healthcheckResponse = await _healthcheckClient.GetHealthcheck(_healthcheckEndpoint.HealthcheckUri);

            if (healthcheckResponse.StatusCode == HttpStatusCode.OK)
            {
                _reportHealth.ReportHealthy(_healthcheckEndpoint.InstanceIdentifier);
            }
            else
            {
                _reportHealth.ReportError(_healthcheckEndpoint.InstanceIdentifier);
            }
        }
    }
}
