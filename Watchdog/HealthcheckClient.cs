using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace Watchdog
{
    public class HealthcheckClient : IHealthcheckClient
    {
        private readonly Uri _healthcheckUri;
        private readonly HttpClient _httpClient;

        public HealthcheckClient(Uri healthcheckUri)
        {
            _healthcheckUri = healthcheckUri;
            _httpClient = new HttpClient();
        }

        public async Task<HttpResponseMessage> GetHealthcheckAsync()
        {
            return await _httpClient.GetAsync(_healthcheckUri);
        }
    }
}
