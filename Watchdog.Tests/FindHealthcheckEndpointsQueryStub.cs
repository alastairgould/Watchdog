using System.Collections.Generic;
using System.Threading.Tasks;
using Watchdog.Queries;

namespace Watchdog.Tests
{
    public class FindHealthcheckEndpointsQueryStub : IFindHealthcheckEndpointsQuery
    {
        public List<HealthcheckEndpoint> HealthcheckEndpoints { get; }

        public FindHealthcheckEndpointsQueryStub(List<HealthcheckEndpoint> healthcheckEndpoints)
        {
            HealthcheckEndpoints = healthcheckEndpoints;
        }

        public Task<IEnumerable<HealthcheckEndpoint>> Execute()
        {
            return Task.FromResult((IEnumerable<HealthcheckEndpoint>)HealthcheckEndpoints);
        }
    }
}
