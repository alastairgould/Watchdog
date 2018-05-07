using System.Threading.Tasks;
using Quartz;
using Watchdog.Queries;

namespace Watchdog
{
    public class HealthcheckScheduledJob : IJob
    {
        private readonly IFindHealthcheckEndpointsQuery _healthcheckEndpointsQuery;
        private readonly IReportHealth _reportHealth;
        private readonly IHealthcheckClient _healthcheckClient;

        public HealthcheckScheduledJob(IFindHealthcheckEndpointsQuery findHealthcheckEndpointsQuery,
            IReportHealth reportHealth,
            IHealthcheckClient healthcheckClient)
        {
            _healthcheckEndpointsQuery = findHealthcheckEndpointsQuery;
            _reportHealth = reportHealth;
            _healthcheckClient = healthcheckClient;
        }

        public async Task Execute(IJobExecutionContext context)
        {
            var healthCheckEndpoints = await _healthcheckEndpointsQuery.Execute();
            Parallel.ForEach(healthCheckEndpoints, async (healthcheckEndpoint) => await new Healthcheck(healthcheckEndpoint, _reportHealth, _healthcheckClient).PerformHealthcheckAsync());
        }
    }
}
