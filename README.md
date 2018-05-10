# Watchdog
Watchdog is an application which runs on service fabric. It is designed to call healthchecks of services running on the cluster and then report the result of the healtchecks to the service fabric health store.

Currently watchdog only supports guest executables and stateless services.

## How to use

To allow Watchdog to start reporting the service healthcheck status, the service must first register a healthcheck with the Watchdog. To do so you must configure the healthcheck inside your servicemanifest file.

