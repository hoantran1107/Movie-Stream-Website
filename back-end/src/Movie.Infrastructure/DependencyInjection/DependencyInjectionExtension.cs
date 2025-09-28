using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Movie.Application.Abstractions;
using Movie.Domain.Entities;
using Movie.Infrastructure.Data;
using Movie.Infrastructure.Repositories;

namespace Movie.Infrastructure.DependencyInjection;

public static class DependencyInjectionExtension
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services,
        IConfiguration configuration)
    {
        services.AddDbContext<AppDbContext>(options =>
            options.UseNpgsql(configuration.GetConnectionString("DefaultConnection")));
        services.AddScoped<IUnitOfWork, UnitOfWork<AppDbContext>>();
        services.AddScoped<IGenericRepository<MovieItem>, GenericRepository<MovieItem>>();
        return services;
    }
}