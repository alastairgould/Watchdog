using System;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using NSubstitute;
using NSubstitute.ExceptionExtensions;
using Watchdog.Queries;
using Xunit;

namespace Watchdog.Tests
{
    public class HealthcheckJobTests
    {
        [Fact]
        public async Task Given_A_Healthcheck_That_Returns_200_When_The_Healthcheck_Is_Performed_Then_The_Service_Is_Reported_As_Healthy()
        {
            var reportHealth = Substitute.For<IReportHealth>();
            var healthcheckJob = CreateHealthcheckJob(reportHealth: reportHealth);

            await healthcheckJob.Execute(null);

            reportHealth.Received().ReportHealthy(Arg.Any<InstanceIdentifier>());
        }

        [Fact]
        public async Task Given_A_Healthcheck_That_Returns_500_When_The_Healthcheck_Is_Performed_Then_The_Service_Is_Reported_As_Unhealthy()
        {
            var reportHealth = Substitute.For<IReportHealth>();
            var healthcheckClient = Substitute.For<IHealthcheckClient>();
            var healthcheckJob = CreateHealthcheckJob(reportHealth: reportHealth, healthcheckClient: healthcheckClient);

            healthcheckClient.GetHealthcheck(Arg.Any<Uri>())
                .Returns(new HttpResponseMessage(HttpStatusCode.InternalServerError));

            await healthcheckJob.Execute(null);

            reportHealth.Received().ReportError(Arg.Any<InstanceIdentifier>());
        }

        [Fact]
        public async Task Given_A_Healthcheck_That_That_Returns_503_When_The_Healthcheck_Is_Performed_Then_The_Service_Is_Reported_As_Unhealthy()
        {
            var reportHealth = Substitute.For<IReportHealth>();
            var healthcheckClient = Substitute.For<IHealthcheckClient>();
            var healthcheckJob = CreateHealthcheckJob(reportHealth: reportHealth, healthcheckClient: healthcheckClient);

            healthcheckClient.GetHealthcheck(Arg.Any<Uri>())
                .Returns(new HttpResponseMessage(HttpStatusCode.ServiceUnavailable));

            await healthcheckJob.Execute(null);

            reportHealth.Received().ReportError(Arg.Any<InstanceIdentifier>());
        }

        [Fact]
        public async Task Given_A_Healthy_Endpoint_When_The_Healthcheck_Is_Performed_Then_The_Correct_Service_Instance_Is_Reported()
        {
            var reportHealth = Substitute.For<IReportHealth>();
            var healthcheckQuery = new FindHealthcheckEndpointsQueryBuilder().Build(1);
            var healthcheckJob = CreateHealthcheckJob(findHealthcheckQuery: healthcheckQuery, reportHealth: reportHealth);

            await healthcheckJob.Execute(null);

            reportHealth.Received().ReportHealthy(healthcheckQuery.HealthcheckEndpoints[0].InstanceIdentifier);
        }

        [Fact]
        public async Task Given_A_Unhealthy_Endpoint_When_The_Healthcheck_Is_Performed_Then_The_Correct_Service_Instance_Is_Reported()
        {
            var reportHealth = Substitute.For<IReportHealth>();
            var healthcheckClient = Substitute.For<IHealthcheckClient>();
            var healthcheckQuery = new FindHealthcheckEndpointsQueryBuilder().Build(1);
            var healthcheckJob = CreateHealthcheckJob(findHealthcheckQuery: healthcheckQuery, reportHealth: reportHealth, healthcheckClient: healthcheckClient);

            healthcheckClient.GetHealthcheck(Arg.Any<Uri>())
                .Returns(new HttpResponseMessage(HttpStatusCode.ServiceUnavailable));

            await healthcheckJob.Execute(null);

            reportHealth.Received().ReportError(healthcheckQuery.HealthcheckEndpoints[0].InstanceIdentifier);
        }

        [Fact]
        public async Task Given_Multiple_Healthy_Healthcheck_Endpoints_When_The_Healthchecks_Are_Performed_Then_All_Endpoints_Are_Reported_Healthy()
        {
            var reportHealth = Substitute.For<IReportHealth>();
            var healthcheckClient = Substitute.For<IHealthcheckClient>();
            var healthcheckQuery = new FindHealthcheckEndpointsQueryBuilder().Build(5);
            var healthcheckJob = CreateHealthcheckJob(findHealthcheckQuery: healthcheckQuery, reportHealth: reportHealth, healthcheckClient: healthcheckClient);

            healthcheckClient.GetHealthcheck(Arg.Any<Uri>())
                .Returns(new HttpResponseMessage(HttpStatusCode.OK));

            await healthcheckJob.Execute(null);

            reportHealth.Received(5).ReportHealthy(Arg.Any<InstanceIdentifier>());
        }

        [Fact]
        public async Task Given_Multiple_Unhealthy_Healthcheck_Endpoints_When_The_Healthchecks_Are_Performed_Then_All_Endpoints_Are_Reported_Unhealthy()
        {
            var reportHealth = Substitute.For<IReportHealth>();
            var healthcheckClient = Substitute.For<IHealthcheckClient>();
            var healthcheckQuery = new FindHealthcheckEndpointsQueryBuilder().Build(5);
            var healthcheckJob = CreateHealthcheckJob(findHealthcheckQuery: healthcheckQuery, reportHealth: reportHealth, healthcheckClient: healthcheckClient);

            healthcheckClient.GetHealthcheck(Arg.Any<Uri>())
                .Returns(new HttpResponseMessage(HttpStatusCode.ServiceUnavailable));

            await healthcheckJob.Execute(null);

            reportHealth.Received(5).ReportError(Arg.Any<InstanceIdentifier>());
        }

        [Fact]
        public async Task Given_Multiple_Healthcheck_Endpoints_With_A_Single_Unhealthy_Endpoint_When_The_Healthchecks_Are_Performed_Then_The_Unhealthy_Endpoint_Is_Reported_Unhealthy()
        {
            var reportHealth = Substitute.For<IReportHealth>();
            var healthcheckClient = Substitute.For<IHealthcheckClient>();
            var healthcheckQuery = new FindHealthcheckEndpointsQueryBuilder().Build(5);
            var healthcheckJob = CreateHealthcheckJob(findHealthcheckQuery: healthcheckQuery, reportHealth: reportHealth, healthcheckClient: healthcheckClient);

            healthcheckClient.GetHealthcheck(Arg.Any<Uri>())
                .Returns(new HttpResponseMessage(HttpStatusCode.OK));

            healthcheckClient.GetHealthcheck(healthcheckQuery.HealthcheckEndpoints[2].HealthcheckUri)
                .Returns(new HttpResponseMessage(HttpStatusCode.ServiceUnavailable));

            await healthcheckJob.Execute(null);

            reportHealth.Received(1).ReportError(healthcheckQuery.HealthcheckEndpoints[2].InstanceIdentifier);
        }

        [Fact]
        public async Task Given_Multiple_Healthcheck_Endpoints_With_A_Single_Unhealthy_Endpoint_When_The_Healthchecks_Are_Performed_Then_The_Healthy_Endpoints_Are_Reported_Healthy()
        {
            var reportHealth = Substitute.For<IReportHealth>();
            var healthcheckClient = Substitute.For<IHealthcheckClient>();
            var healthcheckQuery = new FindHealthcheckEndpointsQueryBuilder().Build(5);
            var healthcheckJob = CreateHealthcheckJob(findHealthcheckQuery: healthcheckQuery, reportHealth: reportHealth, healthcheckClient: healthcheckClient);

            healthcheckClient.GetHealthcheck(Arg.Any<Uri>())
                .Returns(new HttpResponseMessage(HttpStatusCode.OK));

            healthcheckClient.GetHealthcheck(healthcheckQuery.HealthcheckEndpoints[2].HealthcheckUri)
                .Returns(new HttpResponseMessage(HttpStatusCode.ServiceUnavailable));

            await healthcheckJob.Execute(null);

            reportHealth.Received(4).ReportHealthy(Arg.Any<InstanceIdentifier>());
        }

        [Fact]
        public async Task Given_Multiple_Healthcheck_Endpoints_When_The_Healthchecks_Are_Performed_And_One_Throws_An_Exception_Then_The_Other_Healthchecks_Are_Unaffected()
        {
            var reportHealth = Substitute.For<IReportHealth>();
            var healthcheckClient = Substitute.For<IHealthcheckClient>();
            var healthcheckQuery = new FindHealthcheckEndpointsQueryBuilder().Build(5);
            var healthcheckJob = CreateHealthcheckJob(findHealthcheckQuery: healthcheckQuery, reportHealth: reportHealth, healthcheckClient: healthcheckClient);

            healthcheckClient.GetHealthcheck(Arg.Any<Uri>())
                .Returns(new HttpResponseMessage(HttpStatusCode.OK));

            healthcheckClient.GetHealthcheck(healthcheckQuery.HealthcheckEndpoints[2].HealthcheckUri)
                .Throws(new InvalidOperationException());

            await healthcheckJob.Execute(null);

            reportHealth.Received(4).ReportHealthy(Arg.Any<InstanceIdentifier>());
        }

        private HealthcheckScheduledJob CreateHealthcheckJob(IFindHealthcheckEndpointsQuery findHealthcheckQuery = null, IReportHealth reportHealth = null, IHealthcheckClient healthcheckClient = null)
        {
            if (findHealthcheckQuery == null)
            {
                var findHealtcheckQueryBuilder = new FindHealthcheckEndpointsQueryBuilder();
                findHealthcheckQuery = findHealtcheckQueryBuilder.Build();
            }

            if (reportHealth == null)
            {
                reportHealth = Substitute.For<IReportHealth>();
            }

            if (healthcheckClient == null)
            {
                healthcheckClient = Substitute.For<IHealthcheckClient>();

                healthcheckClient.GetHealthcheck(Arg.Any<Uri>())
                    .Returns(new HttpResponseMessage(HttpStatusCode.OK));
            }

            return new HealthcheckScheduledJob(findHealthcheckQuery, reportHealth, healthcheckClient);
        }
    }
}
