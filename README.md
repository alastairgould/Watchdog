# Watchdog
Watchdog is an application which runs on service fabric. It is designed to call http healthchecks of services running on the cluster and then report the result of the healtchecks to the service fabric health store.

Currently watchdog only supports guest executables and stateless services.

## How to use

To allow Watchdog to start reporting the service healthcheck status, the service must first register a healthcheck with the Watchdog. To do so you must configure the healthcheck inside your servicemanifest.xml file

```xml
<?xml version="1.0" encoding="utf-8"?>
<ServiceManifest Name="HealthcheckPkg"
                 Version="1.0.0"
                 xmlns="http://schemas.microsoft.com/2011/01/fabric"
                 xmlns:xsd="http://www.w3.org/2001/XMLSchema"
                 xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance">
  <ServiceTypes>
    <!-- This is the name of your ServiceType. 
         This name must match the string used in RegisterServiceType call in Program.cs. -->
    <StatelessServiceType ServiceTypeName="HealthcheckType">
      <Extensions>
        <Extension Name="Watchdog">
          <Watchdog xmlns="http://schemas.microsoft.com/2015/03/fabact-no-schema">
            <Healthcheck>/Healthcheck</Healthcheck>
          </Watchdog>
        </Extension>
      </Extensions>
    </StatelessServiceType>
  </ServiceTypes>

  <!-- Code package is your service executable. -->
  <CodePackage Name="Code" Version="1.0.0">
    <EntryPoint>
      <ExeHost>
        <Program>Healthcheck.exe</Program>
        <WorkingFolder>CodePackage</WorkingFolder>
      </ExeHost>
    </EntryPoint>
  </CodePackage>

  <!-- Config package is the contents of the Config directoy under PackageRoot that contains an 
       independently-updateable and versioned set of custom configuration settings for your service. -->
  <ConfigPackage Name="Config" Version="1.0.0" />

  <Resources>
    <Endpoints>
      <!-- This endpoint is used by the communication listener to obtain the port on which to 
           listen. Please note that if your service is partitioned, this port is shared with 
           replicas of different partitions that are placed in your code. -->
      <Endpoint Protocol="http" Name="ServiceEndpoint" Type="Input" />
    </Endpoints>
  </Resources>
</ServiceManifest>
```

## How it works 

Watchdog queries service fabric and looks for all services which have a Watchdog configuration. For each service it finds, it identifies all service instances running for that service and hits the healthcheck endpoint. It then reports the result of the healthcheck for the service instance to the service fabric health store.

Currently Watchdog is designed to be run as single instance application to avoid either multiple Watchdogs hitting the same 
endpoints at similar times, or the added complexity of cordination between multiple processes. However there are plans to extend 
the application to run using multiple instances by either making each instance responsible for every service instances on its own 
node or by cordination between Watchdogs.
