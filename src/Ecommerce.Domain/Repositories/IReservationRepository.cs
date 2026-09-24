using Ecommerce.Domain.Entities;

namespace Ecommerce.Domain.Repositories
{
    public interface IReservationRepository
    {
        public Task<IEnumerable<Reservation>> GetAllAsync();
        public Task<Reservation> GetByIdAsync(Guid reservationId, CancellationToken ct);
        public Task<IEnumerable<Reservation>> GetByCustomerIdAsync(Guid customerId, CancellationToken ct);
        public Task<IEnumerable<Reservation>> GetByProductIdAsync(Guid productId, CancellationToken ct);
        public Task AddAsync(Reservation reservation, CancellationToken ct);
        public Task UpdateStatusAsync(Reservation reservation, CancellationToken ct);
        public Task DeleteAsync(Guid reservationId, CancellationToken ct);
    }
}