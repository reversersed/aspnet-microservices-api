using Extensions;
using Ocelot.DependencyInjection;
using Ocelot.Middleware;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddCors(options =>
    options.AddDefaultPolicy(b =>
        b.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod()));

builder.Services.AddJwtAuthentication();

builder.Configuration.SetBasePath(builder.Environment.ContentRootPath)
    .AddOcelot("routes/", builder.Environment, MergeOcelotJson.ToFile)
    .AddEnvironmentVariables();

builder.Services.AddOcelot();

var app = builder.Build();

app.Logger.LogInformation("[GatewayApi] Starting API as {environment}", builder.Environment.EnvironmentName);

app.UseAuthentication();
app.UseAuthorization();

app.UseCors();

app.UseOcelot().Wait();

app.Run();
