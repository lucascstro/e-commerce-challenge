using Ecommerce.Application.Dtos.Response;
using Ecommerce.Domain.Entities.Enum;

namespace Ecommerce.Application.Services.Reservation
{
    public interface IReservationService
    {
        public Task<IEnumerable<AllReservationsByCustomerResponse>> GetAllReservationsByCustomerAsync(Guid customerId, CancellationToken ct);
        public Task<CreateReservationResponse> CreateReservationAsync(Guid customerId, Guid productId, CancellationToken ct);
        public Task<UpdateReservationResponse> UpdateReservationStatusAsync(Guid reservationId, Status status, CancellationToken ct);
    }
}