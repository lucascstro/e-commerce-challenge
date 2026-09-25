using Ecommerce.Domain.Repositories;
using Ecommerce.Infra.BackgroundServices;
using Ecommerce.Infra.Database;
using Ecommerce.Infra.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Ecommerce.Infra
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfra(this IServiceCollection services)
        {
            services.AddDbContext<Database.AppDbContext>(options =>
                options.UseInMemoryDatabase("EcommerceDb"));
            services.AddScoped<IReservationRepository, ReservationRepository>();
            services.AddScoped<IProductRepository, ProductRepository>();
            services.AddScoped<IUnitOfWork, UnitOfWork>();
            services.AddHostedService<ReservationExpirationValidationBackgroundService>();

            return services;
        }
    }
}