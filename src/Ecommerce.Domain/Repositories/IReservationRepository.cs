using Ecommerce.Domain.Entities;

namespace Ecommerce.Domain.Repositories
{
    public interface IReservationRepository
    {
        public Task<IEnumerable<Reservation>> GetAllAsync();
        public Task<Reservation> GetByIdAsync(Guid reservationId);
        public Task AddAsync(Reservation reservation);
        public Task UpdateAsync(Reservation reservation);   
        public Task<IEnumerable<Reservation>> GetByCustomerIdAsync(Guid customerId);
        public Task<IEnumerable<Reservation>> GetByProductIdAsync(Guid productId);
        public Task CancelReservationAsync(Guid reservationId);
        public Task DeleteAsync(Guid reservationId);
    }
}