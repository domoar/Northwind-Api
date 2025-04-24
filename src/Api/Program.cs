using Api;
using Api.middleware;
using Api.serviceExtensions;
using Application;
using Domain;
using Infrastructure;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

builder.Host.UseSerilog();
builder.Services.AddDefaultLogger(builder.Configuration);

builder.Services.AddJaeger(builder.Configuration);
builder.Services.AddZipkin(builder.Configuration);

builder.Services.AddDefaultProblemDetails();
builder.Services.AddDefaultApiVersioning();
builder.Services.AddDefaultResponseCompression();
builder.Services.AddDefaultSwaggerGeneration();

builder.Services.AddSwaggerGen();

builder.Services.AddDomain();

builder.Services.AddInfrastructure(
  builder.Environment,
  builder.Configuration
);

builder.Services.AddApplication();

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddMemoryCache();

builder.Services.AddHttpContextAccessor();

builder
  .Services.AddControllers()
  .AddJsonOptions(options => {
    options.JsonSerializerOptions.WriteIndented = false;
  });

builder.Services.AddSingleton<Banner>();

var app = builder.Build();

app.UseMiddleware<GlobalExceptionHandler>();
app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.UseResponseCompression();

if (app.Environment.IsDevelopment() || app.Environment.IsProduction()) {
  app.UseSwagger();
  app.UseSwaggerUI(options => {
    options.SwaggerEndpoint("/swagger/v1/swagger.json", "Cleanarchitecture Template V1");
    options.DefaultModelRendering(Swashbuckle.AspNetCore.SwaggerUI.ModelRendering.Model);
    options.ConfigObject.AdditionalItems["syntaxHighlight"] = false;
    options.RoutePrefix = "swagger";
  });
}



//app.MapHealthChecks(
//  "_health",
//  new HealthCheckOptions() {
//    ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse
//  });

using (var banner = app.Services.GetRequiredService<Banner>()) {
  banner.LogBanner();
}

await app.RunAsync();
