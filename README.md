# WCF session with NetTcp

This sample demonstrates how to host a sessionfull WCF service with an NetTcp endpoint along with other WCF services over BasicHttp.

The sample intentionally doesn't use minimal API.

## project structure and startup

The project SessionNetTcpServer holds the server running the services.

For each service there is a dedicated client (console application) connecting to the service.

In order to start the sample select multiple start up projects and start the server with all clients.

## services

### CalculatorService
The CalculatorService is taken from the other samples and is provided via a BasicHttpEndpoint.
It is stateless and does not require sessions. The main purpose is to ensure the BasicHttpEndpoint is still working
when a NetTcp configuration is added to the server.

It is called by the client SessionNetTcpCalculatorConsoleClient.

### SpecialService
The SpecialService is a service requiring a session per client. To enable a WCF session it is provided over NetTcp.
The service can increment and decrement a value which is hold in the session server side.

It is called by the client SessionNetTcpSpecialConsoleClient.

### CallbackService
The CallbackService is a service requiring a session per client. To enable a WCF sessionit is provided over NetTcp.
The service allows to subscribe for updates which are delivered via the callback contract.

Not yet implemented.
