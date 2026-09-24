using Ecommerce.Application.Services.Reservation;
using Microsoft.Extensions.DependencyInjection;

namespace Ecommerce.Application
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddApplication(this IServiceCollection services){
            services.AddScoped<IReservationService, ReservationService>();
            
            return services;
        }

    }
}