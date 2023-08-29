using AnimalStays.Application.Common.Interfaces.Authentication;
using AnimalStays.Application.Common.Interfaces.Persistence;
using AnimalStays.Application.Common.Interfaces.Services;
using AnimalStays.Infrastructure.Authentication;
using AnimalStays.Infrastructure.Persistence;
using AnimalStays.Infrastructure.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace AnimalStays.Infrastructure;

public static class DependencyInjection
{
  public static IServiceCollection AddInfrastructure(
      this IServiceCollection services,
      ConfigurationManager configuration)
  {
    services.Configure<JwtSettings>(configuration.GetSection(JwtSettings.SectionName));
    services.AddSingleton<IJwtTokenGenerator, JwtTokenGenerator>();
    services.AddSingleton<IDateTimeProvider, DateTimeProvider>();
    services.AddScoped<IUserRepository, UserRepository>();
    return services;
  }
}