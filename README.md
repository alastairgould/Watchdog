# Watchdog
Watchdog is an application which runs on service fabric. It is designed to call http healthchecks of services running on the cluster and then report the result of the healtchecks to the service fabric health store.

Currently watchdog only supports guest executables and stateless services.

## How to use

To allow Watchdog to start reporting the service healthcheck status, the service must first register a healthcheck with the Watchdog. To do so you must configure the healthcheck inside your servicemanifest.xml file

```xml
  <ServiceTypes>
    <StatelessServiceType ServiceTypeName="HealthcheckType">
      <Extensions>
        <Extension Name="Watchdog">
          <Watchdog xmlns="http://schemas.microsoft.com/2015/03/fabact-no-schema">
            !-- The relative url for the healthcheck -->
            <Healthcheck>/Healthcheck</Healthcheck>
          </Watchdog>
        </Extension>
      </Extensions>
    </StatelessServiceType>
  </ServiceTypes>
```

## How it works 

Watchdog queries service fabric and looks for all services which have a Watchdog configuration. For each service it finds, it identifies all service instances running for that service and hits the healthcheck endpoint. It then reports the result of the healthcheck for the service instance to the service fabric health store.

Currently Watchdog is designed to be run as single instance application to avoid either multiple Watchdogs hitting the same 
endpoints at similar times, or the added complexity of cordination between multiple processes. However there are plans to extend 
the application to run using multiple instances by either making each instance responsible for every service instances on its own 
node or by cordination between Watchdogs.
