using System;
using System.Collections.Generic;
using NSubstitute;
using Watchdog.Queries;
using Xunit;

namespace Watchdog.Tests
{
    public class UnitTest1
    {
        [Fact]
        public void Given_A_Healthy_Service_When_Healtcheck_Is_Performed_Then_The_Service_Is_Reported_As_Healthy()
        {
            /*
            var partitionId = Guid.Parse("ddd9b77e-502b-4aca-b888-09f6ecc65f80");
            var instanceId = 5;


            Func<HealthcheckEndpoint, Healthcheck> createHealthcheck = (endpoint) => new Healthcheck(endpoint, null, null);
                 

            var findHealthcheckQuery = Substitute.For<IFindHealthcheckEndpointsQuery>();
            findHealthcheckQuery.Execute().Returns(new List<HealthcheckEndpoint>()
            {
                new HealthcheckEndpoint(new Uri("http://192.168.0.1:5000:5000/healthcheck"), new InstanceIdentifier(partitionId, instanceId))
            });

            var healthcheckSceduledJob = new HealthcheckScheduledJob(findHealthcheckQuery, createHealthcheck);
            */

        }
    }
}
