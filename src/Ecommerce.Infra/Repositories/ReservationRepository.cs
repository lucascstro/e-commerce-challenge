using Ecommerce.Domain.Entities;
using Ecommerce.Domain.Repositories;
using Ecommerce.Infra.Database;
using Microsoft.EntityFrameworkCore;

namespace Ecommerce.Infra.Repositories
{
    public class ReservationRepository : IReservationRepository
    {
        private readonly AppDbContext _context;

        public ReservationRepository(AppDbContext context)
        {
            _context = context;
        }
        public async Task<IEnumerable<Reservation>> GetAllAsync(CancellationToken ct)
            => await _context.Reservations.AsNoTracking().ToListAsync();

        public async Task<IEnumerable<Reservation>> GetByCustomerIdAsync(Guid customerId, CancellationToken ct)
        {
            var reservations = await _context.Reservations
                 .Where(r => r.CustomerId == customerId)
                 .ToListAsync(ct);

            return reservations;
        }

        public async Task<Reservation> GetByIdAsync(Guid reservationId, CancellationToken ct = default)
            => await _context.Reservations.AsNoTracking().FirstOrDefaultAsync(r => r.ReservationId == reservationId, ct);
        
        public async Task<IEnumerable<Reservation>> GetByProductIdAsync(Guid productId, CancellationToken ct = default)
            => await _context.Reservations.AsNoTracking().Where(r => r.ProductId == productId).ToListAsync(ct);
        
        public async Task AddAsync(Reservation reservation, CancellationToken ct = default)
        {
            await _context.Reservations.AddAsync(reservation);
        }

        public async Task DeleteAsync(Guid reservationId, CancellationToken ct = default)
        {
            var reservation = _context.Reservations.FirstOrDefault(r => r.ReservationId == reservationId);
            _context.Reservations.Remove(reservation);
        }

        public async Task UpdateStatusAsync(Reservation reservation, CancellationToken ct = default)
        {
            _context.Reservations.Update(reservation);
        }
    }
}