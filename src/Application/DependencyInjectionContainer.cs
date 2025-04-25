using Application.service;
using Application.Service;
using Microsoft.Extensions.DependencyInjection;

namespace Application;
public static class DependencyInjectionContainer {
  public static IServiceCollection AddApplication(this IServiceCollection services) {
    services.AddScoped<ProductService>();
    services.AddScoped<EmployeeService>();
    services.AddScoped<CustomerService>();
    return services;
  }
}
