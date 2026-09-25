using Ecommerce.Domain.Entities;
using Ecommerce.Domain.Entities.Enum;
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
                new("Carro", "Sedan 4 portas, motor 1.6, câmbio automático", StatusProduct.Reserved),
                new("Moto", "Motocicleta 160cc, ideal para uso urbano", StatusProduct.Reserved),
                new("Bicicleta", "Bicicleta aro 29 com 21 marchas", StatusProduct.Reserved),
                new("Patinete Elétrico", "Patinete com autonomia de 25 km e velocidade máxima de 25 km/h", StatusProduct.Available),
                new("Skate", "Skate completo com shape de maple canadense", StatusProduct.Available),
                new("Capacete", "Capacete fechado com viseira antirrisco", StatusProduct.Available),
                new("Luva de Proteção", "Luva de couro com proteção nos dedos", StatusProduct.Available),
                new("Cadeado", "Cadeado em U de aço temperado", StatusProduct.Available),
                new("Bomba de Ar", "Bomba de ar portátil com manômetro", StatusProduct.Unavailable),
                new("Kit de Ferramentas", "Kit com chaves e reparos para bicicleta e moto", StatusProduct.Unavailable)
            };

            var reservations = new List<Reservation>
            {
                new(customers[0].CustomerId, products[2].ProductId),
                new(customers[1].CustomerId, products[1].ProductId),
                new(customers[2].CustomerId, products[0].ProductId),
            };

            customers.ForEach(c => Console.WriteLine($"Customer: {c.CustomerId} - {c.Name}"));
            reservations.ForEach(r => Console.WriteLine($"Reservation: {r.ReservationId} - Customer: {r.CustomerId} - Product: {r.ProductId}"));
            products.ForEach(p => Console.WriteLine($"Product: {p.ProductId} - {p.Name} - {p.Description} - Available: {p.Status}"));
            ctx.Customers.AddRange(customers);
            ctx.Products.AddRange(products);
            ctx.Reservations.AddRange(reservations);

            await ctx.SaveChangesAsync(ct);
        }
    }
}