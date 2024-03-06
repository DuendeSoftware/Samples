var builder = DistributedApplication.CreateBuilder(args);

var apiService = builder.AddProject<Projects.Aspire_ApiService>("apiservice");

builder.AddProject<Projects.Aspire_Web>("webfrontend")
    .WithReference(apiService);

builder.AddProject<Projects.IdentityServer>("identityserver");

builder.Build().Run();
