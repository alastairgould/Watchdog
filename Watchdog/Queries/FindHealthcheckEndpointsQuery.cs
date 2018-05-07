using System.Linq;
using System.Collections.Generic;
using System.Fabric;
using System.Fabric.Query;
using System.IO;
using System.Threading.Tasks;
using System.Xml.Serialization;
using Newtonsoft.Json.Linq;

namespace Watchdog.Queries
{
    public class FindHealthcheckEndpointsQuery : IFindHealthcheckEndpointsQuery
    {
        private readonly FabricClient _fabricClient;

        public FindHealthcheckEndpointsQuery()
        {
            _fabricClient = new FabricClient();
        }

        public async Task<IEnumerable<HealthcheckEndpoint>> Execute()
        {
            var healthcheckEndpoints = new List<HealthcheckEndpoint>();
            var services = await HealthCheckServices();

            foreach(var service in services)
            {
                var partition = (await _fabricClient.QueryManager.GetPartitionListAsync(service.Item1.ServiceName)).Single();
                var replicas = await _fabricClient.QueryManager.GetReplicaListAsync(partition.PartitionInformation.Id);
                var healthCheckConfiguration = ParseHealthCheckConfiguration(service.Item2);

                foreach (var replica in replicas)
                {
                    string instanceEndpoint = EndpointAddress(replica);
                    healthcheckEndpoints.Add(new HealthcheckEndpoint(new System.Uri($"{instanceEndpoint}{healthCheckConfiguration.Healthcheck}"), new InstanceIdentifier(partition.PartitionInformation.Id, replica.Id)));
                }
            }
         
            return healthcheckEndpoints;
        }

        private static HealthcheckConfiguration ParseHealthCheckConfiguration(ServiceType service)
        {
            XmlSerializer serializer = new XmlSerializer(typeof(HealthcheckConfiguration));
            var stringReader = new StringReader(service.ServiceTypeDescription.Extensions["Watchdog"]);
            var healthCheckConfiguration = (HealthcheckConfiguration) serializer.Deserialize(stringReader);
            return healthCheckConfiguration;
        }

        private static string EndpointAddress(Replica replica)
        {
            JObject endpointObject = JObject.Parse(replica.ReplicaAddress);
            var instanceEndpoint = endpointObject["Endpoints"].First.First.ToString();
            return instanceEndpoint;
        }

        private async Task<IEnumerable<System.Tuple<Service, ServiceType>>> HealthCheckServices()
        {
            var apps = await _fabricClient.QueryManager.GetApplicationListAsync();
            var services = new List<System.Tuple<Service, ServiceType>>();

            foreach(var app in apps)
            {
                var appServices = await _fabricClient.QueryManager.GetServiceListAsync(app.ApplicationName);
                
                foreach(var appService in appServices)
                {
                    var serviceType = (await _fabricClient.QueryManager.GetServiceTypeListAsync(app.ApplicationTypeName, app.ApplicationTypeVersion, appService.ServiceTypeName))
                        .Single();

                    if(serviceType.ServiceTypeDescription.Extensions.ContainsKey("Watchdog"))
                    {
                        services.Add(new System.Tuple<Service, ServiceType>(appService, serviceType));
                    }
                }
            }

            return services;
        }
    }
}
