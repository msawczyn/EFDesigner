# Basic HTTP APIs Microservice on DotNet Core Server

---
[Book on Learning Microservices](https://learn.microsoft.com/en-us/dotnet/architecture/microservices/multi-container-microservice-net-applications/data-driven-crud-microservice)

Free course Create and deploy a cloud-native [ASP.NET Core microservice on MS Learn](https://docs.microsoft.com/en-us/learn/modules/microservices-aspnet-core/)

![Request/Response](https://learn.microsoft.com/en-us/dotnet/architecture/microservices/architect-microservice-container-applications/media/communication-in-microservice-architecture/request-response-comms-live-queries-updates.png)

![Architecture](https://github.com/dotnet-architecture/eShopOnContainers/raw/dev/img/eShopOnContainers-architecture.png)

[Example Programs](https://github.com/dotnet-architecture/eShopOnContainers?WT.mc_id=dotnet-35129-website)

At the simplest end, we now have basic HTTP APIs:

You make a request to a URI, and it responds with data, hopefully in the format you requested (JSON, XML, etc.).

This includes APIs that strictly conform with the ReST architectural style,but also simple 

**“CRUD-over-HTTP”** APIs that just use 
 - GET, PUT, POST and DELETE requests to retrieve, store and manage data.

These APIs can apply security using any of the available HTTP authentication options,
and can be made secure simply by applying SSL/TLS to the connection.

HTTP APIs created with .NET Core are self documented using Swagger.

This includes the ability to read the API metadata from a known endpoint and generate client library code.