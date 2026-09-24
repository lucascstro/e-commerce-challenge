using Ecommerce.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Ecommerce.Infra.Database
{
    public static class Seed
    {
        public static async Task SeedAsync(IServiceProvider serviceProvider, CancellationToken ct = default)
        {
            using var scope = serviceProvider.CreateScope();
            var ctx = scope.ServiceProvider.GetRequiredService<AppDbContext>();
            
            if(await ctx.Reservations.AnyAsync(ct)) return;

            var customers = new List<Customer>
            {
                new("Marina Silva"),
                new("Cabo Daciolo"),
                new("Padre Kelmon"),
            };

            var products = new List<Product>
            {
                new("Carro", "Descrição do carro", true),
                new("Moto", "Descrição da moto", true),
                new("Produto 3", "Descrição do produto 3", true),
            };

            var reservations = new List<Reservation>
            {
                new(customers[0].CustomerId, products[2].ProductId),
                new(customers[1].CustomerId, products[1].ProductId),
                new(customers[2].CustomerId, products[0].ProductId),
            };

            customers.ForEach(c => Console.WriteLine($"Customer: {c.CustomerId} - {c.Name}"));
            ctx.Customers.AddRange(customers);
            ctx.Products.AddRange(products);
            ctx.Reservations.AddRange(reservations);

            await ctx.SaveChangesAsync(ct);
        }
    }
}