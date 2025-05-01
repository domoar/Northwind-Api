using Api;
using Api.Middleware;
using Api.Request.Parameters;
using Api.Request.Validation;
using Api.ServiceExtension;
using Application;
using Domain;
using FluentValidation;
using FluentValidation.AspNetCore;
using Infrastructure;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

builder.Host.UseSerilog();
builder.Services.AddDefaultLogger(builder.Configuration);

builder.Services.AddOpenTelemetryWithExporters(
    builder.Configuration,
    TelemetryExporters.Jaeger | TelemetryExporters.Zipkin);

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

builder.Services.AddProblemDetails();
builder.Services.AddExceptionHandler<GlobalExceptionHandler>();

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddMemoryCache();

builder.Services.AddHttpContextAccessor();

builder.Services
  .AddFluentValidationAutoValidation(fv => {
    fv.DisableDataAnnotationsValidation = true;
  })
  .AddValidatorsFromAssemblyContaining<CustomerRequestValidator>();

builder.Services
  .AddControllers(options => {
    options.SuppressImplicitRequiredAttributeForNonNullableReferenceTypes = true;
  })
  .AddJsonOptions(opts => {
    opts.JsonSerializerOptions.WriteIndented = false;
  });


builder.Services.AddSingleton<Banner>();

var app = builder.Build();

app.UseExceptionHandler();
app.UseStatusCodePages();

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.UseResponseCompression();

if (app.Environment.IsDevelopment() || app.Environment.IsProduction()) {
  app.UseSwagger();
  app.UseSwaggerUI(options => {
    options.SwaggerEndpoint("/swagger/v1/swagger.json", "Northwind Api V1");
    options.DefaultModelRendering(Swashbuckle.AspNetCore.SwaggerUI.ModelRendering.Model);
    options.ConfigObject.AdditionalItems["syntaxHighlight"] = false;
    options.RoutePrefix = "swagger";
  });
}

if (app.Environment.IsDevelopment() || app.Environment.IsStaging()) {
  app.UseDeveloperExceptionPage();
}
else {
  app.UseExceptionHandler();
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
