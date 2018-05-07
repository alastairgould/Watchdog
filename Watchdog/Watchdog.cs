using System.Collections.Generic;
using System.Fabric;
using System.Threading;
using System.Threading.Tasks;
using Autofac;
using Autofac.Extras.Quartz;
using Microsoft.ServiceFabric.Services.Communication.Runtime;
using Microsoft.ServiceFabric.Services.Runtime;
using Quartz;
using Watchdog.Queries;

namespace Watchdog
{
    /// <summary>
    /// An instance of this class is created for each service instance by the Service Fabric runtime.
    /// </summary>
    internal sealed class Watchdog : StatelessService
    {
        public Watchdog(StatelessServiceContext context)
            : base(context)
        { }

        /// <summary>
        /// Optional override to create listeners (e.g., TCP, HTTP) for this service replica to handle client or user requests.
        /// </summary>
        /// <returns>A collection of listeners.</returns>
        protected override IEnumerable<ServiceInstanceListener> CreateServiceInstanceListeners()
        {
            return new ServiceInstanceListener[0];
        }

        /// <summary>
        /// This is the main entry point for your service instance.
        /// </summary>
        /// <param name="cancellationToken">Canceled when Service Fabric needs to shut down this service instance.</param>
        protected override async Task RunAsync(CancellationToken cancellationToken)
        {
            var container = ConfigureServices();
            var scheduler = container.Resolve<IScheduler>();

            await ConfigureJob(cancellationToken, scheduler);

            scheduler.Start(cancellationToken).Wait(cancellationToken);
        }

        private static async Task ConfigureJob(CancellationToken cancellationToken, IScheduler scheduler)
        {
            var job = JobBuilder.Create<HealthcheckScheduledJob>().Build();

            var trigger = TriggerBuilder.Create()
                .StartNow()
                .WithSchedule(SimpleScheduleBuilder.RepeatSecondlyForever(10)).Build();

            await scheduler.ScheduleJob(job, trigger, cancellationToken);
        }

        private static IContainer ConfigureServices()
        {
            var builder = new ContainerBuilder();

            builder.RegisterModule(new QuartzAutofacFactoryModule());
            builder.RegisterModule(new QuartzAutofacJobsModule(typeof(HealthcheckScheduledJob).Assembly));

            builder.RegisterType<FindHealthcheckEndpointsQuery>().As<IFindHealthcheckEndpointsQuery>();
            builder.RegisterType<Healthcheck>();
            builder.RegisterType<HealthcheckClient>();
            builder.RegisterType<ReportHealth>();

            var container = builder.Build();
            return container;
        }
    }
}
