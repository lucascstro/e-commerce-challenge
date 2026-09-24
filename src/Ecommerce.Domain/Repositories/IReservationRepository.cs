using Ecommerce.Domain.Entities;

namespace Ecommerce.Domain.Repositories
{
    public interface IReservationRepository
    {
        public Task<IEnumerable<Reservation>> GetAllAsync();
        public Task<Reservation> GetByIdAsync(Guid reservationId, CancellationToken ct = default);
        public Task AddAsync(Reservation reservation, CancellationToken ct = default);
        public Task UpdateAsync(Reservation reservation, CancellationToken ct = default);   
        public Task<IEnumerable<Reservation>> GetByCustomerIdAsync(Guid customerId, CancellationToken ct = default);
        public Task<IEnumerable<Reservation>> GetByProductIdAsync(Guid productId, CancellationToken ct = default);
        public Task CancelReservationAsync(Guid reservationId, CancellationToken ct = default);
        public Task DeleteAsync(Guid reservationId, CancellationToken ct = default);
    }
}