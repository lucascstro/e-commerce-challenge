using Ecommerce.Application.Dtos.Request;
using Ecommerce.Application.Dtos.Response;
using Ecommerce.Domain.Entities.Enum;
using Ecommerce.Domain.Repositories;

namespace Ecommerce.Application.Services.Reservation
{
    public class ReservationService : IReservationService
    {
        private readonly IReservationRepository _reservationRepository;
        private readonly IProductRepository _productRepository;
        private readonly IUnitOfWork _unitOfWork;

        public ReservationService(IReservationRepository reservationRepository, IProductRepository productRepository, IUnitOfWork unitOfWork)
        {
            _reservationRepository = reservationRepository;
            _productRepository = productRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task<IEnumerable<ReservationsResponse>> GetAllReservations(CancellationToken ct)
        {
            var reservations = await _reservationRepository.GetAllAsync(ct);
            return reservations.Select(r => new ReservationsResponse
            (
                r.ReservationId,
                r.ProductId,
                r.Status.ToString(),
                r.CreatedAt,
                r.ExpiresAt
            ));
        }

        public async Task<IEnumerable<ReservationsResponse>> GetAllReservationsByCustomerAsync(Guid customerId, CancellationToken ct)
        {
            if (customerId == Guid.Empty)
                throw new ArgumentException("O ID do cliente não pode ser vazio", nameof(customerId));

            var reservations = await _reservationRepository.GetByCustomerIdAsync(customerId, ct);
            return reservations.Select(r => new ReservationsResponse
            (
                r.ReservationId,
                r.ProductId,
                r.Status.ToString(),
                r.CreatedAt,
                r.ExpiresAt
            ));
        }

        public async Task<CreateReservationResponse> CreateReservationAsync(CreateReservationRequest request, CancellationToken ct)
        {
            if (request.CustomerId == Guid.Empty)
                throw new ArgumentException("O ID do cliente não pode ser vazio", nameof(request.CustomerId));
            if (request.ProductId == Guid.Empty)
                throw new ArgumentException("O ID do produto não pode ser vazio", nameof(request.ProductId));

            var reservation = new Domain.Entities.Reservation(request.CustomerId, request.ProductId);
            var product = await _productRepository.GetByIdAsync(request.ProductId);
            product.MarkAsUnavailable();
            
            await _reservationRepository.AddAsync(reservation, ct);
            await _productRepository.UpdateAsync(product);
            await _unitOfWork.SaveChangesAsync(ct);
            
            return new CreateReservationResponse(
                reservation.ReservationId,
                reservation.ProductId,
                reservation.CreatedAt
            );
        }

        public async Task<UpdateReservationResponse> UpdateReservationToStatusExpiredAsync(Guid reservationId, CancellationToken ct)
            => await UpdateReservationStatusAsync(reservationId, StatusReservation.Expired, ct);

        public async Task<UpdateReservationResponse> UpdateReservationToStatusCancelledAsync(Guid reservationId, CancellationToken ct)
            => await UpdateReservationStatusAsync(reservationId, StatusReservation.Cancelled, ct);

        private async Task<UpdateReservationResponse> UpdateReservationStatusAsync(Guid reservationId, StatusReservation status, CancellationToken ct)
        {
            if (reservationId == Guid.Empty)
                throw new ArgumentException("O ID da reserva não pode ser vazio", nameof(reservationId));

            var reservation = await _reservationRepository.GetByIdAsync(reservationId, ct);
            if(reservation == null)
                throw new ArgumentException("Reserva não encontrada", nameof(reservationId));

            var product = await _productRepository.GetByIdAsync(reservation.ProductId);

            reservation.UpdateStatus(status);
            product.MarkAsAvailable();
            await _productRepository.UpdateAsync(product);
            await _reservationRepository.UpdateStatusAsync(reservation, ct);
            await _unitOfWork.SaveChangesAsync(ct);

            return new UpdateReservationResponse(
                reservation.ReservationId,
                reservation.Status
            );
        }
    }
}
