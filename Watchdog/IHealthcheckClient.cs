using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace Watchdog
{
    public interface IHealthcheckClient
    {
        Task<HttpResponseMessage> GetHealthcheck(Uri healthcheck);
    }
}