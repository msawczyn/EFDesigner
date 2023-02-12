// WebApi And Client

// Basic HTTP APIs Microservice on Server
// --------------------------------------
// At the simplest end, we now have basic HTTP APIs:
// you make a request to a URI, and it responds with data, hopefully in the format you requested (JSON, XML, etc.).
// This includes APIs that strictly conform with the ReST architectural style,
// but also simple “CRUD-over-HTTP” APIs that just use GET, PUT, POST and DELETE requests to retrieve, store and manage data.
// These APIs can apply security using any of the available HTTP authentication options,
// and can be made secure simply by applying SSL/TLS to the connection.
// For basic SOAP-over-HTTP or SOAP-over-TCP request/response WCF applications, an HTTP API is a good potential alternative.

// HTTP APIs created with .NET Core 2.x can be documented using Swagger,
// which includes the ability to read the API metadata from a known endpoint and generate client library code.

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
