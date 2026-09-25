using Ecommerce.Domain.Entities.Enum;
using Ecommerce.Domain.Exceptions;

namespace Ecommerce.Domain.Entities
{
    public class Reservation
    {
        public Guid ReservationId { get; private set; } = Guid.NewGuid();
        public Guid CustomerId { get; private set; }  
        public Guid ProductId { get; private set; }
        public DateTime CreatedAt { get; private set; } = DateTime.Now;
        public DateTime ExpiresAt { get; private set; } = DateTime.Now.AddMinutes(1);
        public StatusReservation Status { get; private set; } = StatusReservation.Active;

        public Reservation(Guid customerId, Guid productId, Guid reservationId = default)
        {
            if(customerId == Guid.Empty)
                throw new DomainException("O ID do cliente não pode ser vazio.");

            if(productId == Guid.Empty)
                throw new DomainException("O ID do produto não pode ser vazio.");

            if (reservationId != Guid.Empty)
                ReservationId = reservationId;

            CustomerId = customerId;
            ProductId = productId;
        }

        public void UpdateStatus(StatusReservation status)
        {
            if(status == StatusReservation.Expired && Status == StatusReservation.Expired)
                throw new InvalidOperationException("A reserva já está expirada.");
            
            if(status == StatusReservation.Active && Status == StatusReservation.Active)
                throw new InvalidOperationException("A reserva já está ativa.");

            if(status == StatusReservation.Cancelled && Status == StatusReservation.Cancelled)
                throw new InvalidOperationException("A reserva já está cancelada.");

            if(status == StatusReservation.Active && (Status == StatusReservation.Expired || Status == StatusReservation.Cancelled))
                throw new InvalidOperationException("Não é possível reativar uma reserva expirada ou cancelada.");

            if(status == StatusReservation.Expired && Status == StatusReservation.Cancelled)
                throw new InvalidOperationException("Não é possível expirar uma reserva cancelada.");

            if(status == StatusReservation.Cancelled && Status == StatusReservation.Expired)
                throw new InvalidOperationException("Não é possível cancelar uma reserva expirada.");

            Status = status;
        }
    }
}