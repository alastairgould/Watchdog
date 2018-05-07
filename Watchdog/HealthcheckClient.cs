using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace Watchdog
{
    public class HealthcheckClient : IHealthcheckClient
    {
        private readonly HttpClient _httpClient;

        public HealthcheckClient()
        {
            _httpClient = new HttpClient();
        }

        public async Task<HttpResponseMessage> GetHealthcheck(Uri healthcheckUri)
        {
            return await _httpClient.GetAsync(healthcheckUri);
        }
    }
}
