# Watchdog
Watchdog is an application designed to monitor the health of services running on a service fabric cluster 
using HTTP healthchecks. The result of these healthchecks are then reported to the service fabric health store.

It's designed to be simple and easy to use. 

Currently watchdog only supports guest executables and stateless services.

## How to use

To allow Watchdog to start reporting the healthcheck of your services, the service must first register a healthcheck with the Watchdog. To do so you must configure the healthcheck inside your services servicemanifest.xml file as an extension:

An example of how to do this:

```xml
  <ServiceTypes>
    <StatelessServiceType ServiceTypeName="ServiceTypeName">
      <Extensions>
        <Extension Name="Watchdog">
          <Watchdog xmlns="http://schemas.microsoft.com/2015/03/fabact-no-schema">
            !-- The relative url of your healthcheck -->
            <Healthcheck>/api/healthcheck</Healthcheck>
          </Watchdog>
        </Extension>
      </Extensions>
    </StatelessServiceType>
  </ServiceTypes>
```

## How it works 

Watchdog queries service fabric and looks for all services which have a Watchdog configuration. For each service it finds, it identifies all service instances running for that service and hits the healthcheck endpoint. It then reports the result of the healthcheck to the service fabric health store.

Currently Watchdog is designed to be run as single instance application to avoid either multiple Watchdogs hitting the same 
endpoints at similar times, or the added complexity of coordination between multiple processes. There are plans to extend 
the application to run  multiple instances by either making each instance of the Watchdog responsible for reporting health of service instances on its own node or by coordination between Watchdog processes(Reliable collections, actor model, etc).
