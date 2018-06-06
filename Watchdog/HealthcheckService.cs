using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;
using Watchdog.Queries;

namespace Watchdog
{
    public class HealthcheckService : BackgroundService
    {
        private readonly IFindHealthcheckEndpointsQuery _healthcheckEndpointsQuery;
        private readonly IReportHealth _reportHealth;
        private readonly IHealthcheckClient _healthcheckClient;

        public HealthcheckService(IFindHealthcheckEndpointsQuery healthcheckEndpointsQuery, IReportHealth reportHealth, IHealthcheckClient healthcheckClient)
        {
            _healthcheckEndpointsQuery = healthcheckEndpointsQuery;
            _reportHealth = reportHealth;
            _healthcheckClient = healthcheckClient;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                var healthCheckEndpoints = await _healthcheckEndpointsQuery.Execute();

                Parallel.ForEach(healthCheckEndpoints, async (healthcheckEndpoint) => 
                    await new Healthcheck(healthcheckEndpoint, _reportHealth, _healthcheckClient).PerformHealthcheckAsync());

                await Task.Delay(TimeSpan.FromSeconds(10), stoppingToken);
            }
        }
    }
}
