using Ecommerce.Application.Services.Customer;
using Ecommerce.Application.Services.Product;
using Ecommerce.Application.Services.Reservation;
using Microsoft.Extensions.DependencyInjection;

namespace Ecommerce.Application
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddApplication(this IServiceCollection services){
            services.AddScoped<IReservationService, ReservationService>();
            services.AddScoped<IProductService, ProductService>();
            services.AddScoped<ICustomerService, CustomerService>();

            return services;
        }

    }
}