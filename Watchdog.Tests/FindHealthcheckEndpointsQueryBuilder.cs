using System;
using System.Collections.Generic;
using System.Linq;
using Watchdog.Queries;

namespace Watchdog.Tests
{
    public class FindHealthcheckEndpointsQueryBuilder
    {
        public Guid PartitionId = new Guid("99566b1d-76b6-4083-b83c-d63c129a6a99");

        public FindHealthcheckEndpointsQueryStub Build(int quantity = 1)
        {
            var endpoints = new List<HealthcheckEndpoint>();

            foreach (var number in Enumerable.Range(0, quantity))
            {
                var endpointUrl = new Uri($"http://192.168.0.{number}:5000/healthcheck");
                var endpoint = new HealthcheckEndpoint(endpointUrl, new InstanceIdentifier(PartitionId, number));
                endpoints.Add(endpoint);
            }

            return new FindHealthcheckEndpointsQueryStub(endpoints);
        }
    }
}
