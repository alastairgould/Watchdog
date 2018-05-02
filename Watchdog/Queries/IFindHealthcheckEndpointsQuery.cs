using System.Collections.Generic;
using System.Threading.Tasks;

namespace Watchdog.Queries
{
    public interface IFindHealthcheckEndpointsQuery
    {
        Task<IEnumerable<HealthcheckEndpoint>> Execute();
    }
}