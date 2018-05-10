# Watchdog
Watchdog is an application which runs on service fabric. It is designed to call http healthchecks of services running on the cluster and then report the result of the healtchecks to the service fabric health store.

Currently watchdog only supports guest executables and stateless services.

## How to use

To allow Watchdog to start reporting the service healthcheck status, the service must first register a healthcheck with the Watchdog. To do so you must configure the healthcheck inside your servicemanifest.xml file

```xml
<Test>
  <Test>
  </Test>
</Test>
```

## How it works 

Watchdog queries service fabric and looks for all services which have a Watchdog configuration. For each service it finds, it identifies all service instances running for that service and hits the healthcheck endpoint. It then reports the result of the healthcheck to the service fabric health store.
