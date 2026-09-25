using Ecommerce.Application.Services.Product;
using Ecommerce.Application.Services.Reservation;
using Ecommerce.Domain.Entities.Enum;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace Ecommerce.Infra.BackgroundServices
{
    public class ReservationExpirationValidationBackgroundService : BackgroundService
    {
        private readonly IServiceScopeFactory _serviceProvider;
        private readonly ILogger<ReservationExpirationValidationBackgroundService> _logger;

        public ReservationExpirationValidationBackgroundService(IServiceScopeFactory serviceProvider, ILogger<ReservationExpirationValidationBackgroundService> logger)
        {
            _serviceProvider = serviceProvider;
            _logger = logger;
        }
        
        protected override async Task ExecuteAsync(CancellationToken ct)
        {
            while (!ct.IsCancellationRequested)
            {
                _logger.LogInformation("Iniciando verificação de expiração de reservas...");

                using (var serviceScope = _serviceProvider.CreateScope())
                {
                    var serviceReservation = serviceScope.ServiceProvider.GetRequiredService<IReservationService>();
                    var serviceProduct = serviceScope.ServiceProvider.GetRequiredService<IProductService>();

                    var reservations = await serviceReservation.GetReservationsByStatusAsync(StatusReservation.Active, ct);

                    foreach (var reservation in reservations)
                    {
                        _logger.LogInformation($"Dados inicial da Reserva:  Id:{reservation.ReservationId} - CreatedAt: {reservation.CreatedAt} - ExpiresAt: {reservation.ExpiresAt} - Status: {reservation.Status}");

                        if (reservation.ExpiresAt <= DateTime.Now)
                        {
                            var updated = await serviceReservation.UpdateReservationToStatusExpiredAsync(reservation.ReservationId, ct);
                            _logger.LogInformation($"Reserva de Id {updated.ReservationId} expirada e atualizada para status '{updated.Status}'.");

                            _logger.LogInformation($"Checkando se produto da reserva de Id {reservation.ReservationId} foi atualizado para 'Available'...");
                            var product = await serviceProduct.GetProductByIdAsync(reservation.ProductId, ct);

                            if (product.Status == StatusProduct.Available.ToString())
                                _logger.LogInformation($"Produto de Id {reservation.ProductId} atualizado para 'Available' com sucesso.");
                            else
                                _logger.LogWarning($"Produto de Id {reservation.ProductId} não foi atualizado para 'Available'.");
                        }
                    }
                }

                _logger.LogInformation("Verificação de expiração de reservas finalizada.");
                await Task.Delay(TimeSpan.FromSeconds(30), ct);
            }
        }
    }
}