using Ecommerce.Domain.Entities;
using Ecommerce.Domain.Entities.Enum;

namespace Ecommerce.Domain.Repositories
{
    public interface IReservationRepository
    {
        public Task<IEnumerable<Reservation>> GetAllAsync(CancellationToken ct);
        public Task<Reservation> GetByIdAsync(Guid reservationId, CancellationToken ct);
        public Task<IEnumerable<Reservation>> GetByCustomerIdAsync(Guid customerId, CancellationToken ct);
        public Task<IEnumerable<Reservation>> GetByProductIdAsync(Guid productId, CancellationToken ct);
        public Task<IEnumerable<Reservation>> GetByStatusAsync(StatusReservation status, CancellationToken ct);
        public Task AddAsync(Reservation reservation, CancellationToken ct);
        public Task UpdateStatusAsync(Reservation reservation, CancellationToken ct);
        public Task DeleteAsync(Guid reservationId, CancellationToken ct);
    }
}