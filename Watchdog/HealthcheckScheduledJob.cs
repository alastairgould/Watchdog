using System;
using System.Threading.Tasks;
using Quartz;
using Watchdog.Queries;

namespace Watchdog
{
    public class HealthcheckScheduledJob : IJob
    {
        private readonly IFindHealthcheckEndpointsQuery _healthcheckEndpointsQuery;
        private readonly Func<HealthcheckEndpoint, Healthcheck> _createHealthcheck;

        public HealthcheckScheduledJob(IFindHealthcheckEndpointsQuery findHealthcheckEndpointsQuery,
            Func<HealthcheckEndpoint, Healthcheck> createHealthcheck)
        {
            _healthcheckEndpointsQuery = findHealthcheckEndpointsQuery;
            _createHealthcheck = createHealthcheck;
        }

        public async Task Execute(IJobExecutionContext context)
        {
            var healthCheckEndpoints = await _healthcheckEndpointsQuery.Execute();
            Parallel.ForEach(healthCheckEndpoints, (healthcheckEndpoint) => _createHealthcheck(healthcheckEndpoint).PerformHealthcheck());
        }
    }
}
