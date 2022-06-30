// Copyright (c) Duende Software. All rights reserved.
// See LICENSE in the project root for license information.

using BackendApiHost;

var builder = WebApplication.CreateBuilder(args);

var app = builder.ConfigureServices().ConfigurePipeline();

app.Run();