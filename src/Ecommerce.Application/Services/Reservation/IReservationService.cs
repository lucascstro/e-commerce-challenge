using Ecommerce.Application.Dtos.Request;
using Ecommerce.Application.Dtos.Response;

namespace Ecommerce.Application.Services.Reservation
{
    public interface IReservationService
    {
        public Task<IEnumerable<AllReservationsByCustomerResponse>> GetAllReservationsByCustomerAsync(Guid customerId, CancellationToken ct);
        public Task<CreateReservationResponse> CreateReservationAsync(CreateReservationRequest request, CancellationToken ct);
        public Task<UpdateReservationResponse> UpdateReservationToStatusExpiredAsync(Guid reservationId, CancellationToken ct);
        public Task<UpdateReservationResponse> UpdateReservationToStatusCancelledAsync(Guid reservationId, CancellationToken ct);
        
    }
}