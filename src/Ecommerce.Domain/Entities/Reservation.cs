using Ecommerce.Domain.Entities.Enum;
using Ecommerce.Domain.Exceptions;

namespace Ecommerce.Domain.Entities
{
    public class Reservation
    {
        public Guid ReservationId { get; private set; } = Guid.NewGuid();
        public Guid CustomerId { get; private set; }  
        public Guid ProductId { get; private set; }
        public DateTime CreatedAt { get; private set; } = DateTime.UtcNow;
        public DateTime ExpiresAt { get; private set; } = DateTime.UtcNow.AddHours(72);
        public Status Status { get; private set; } = Status.Active;

        public Reservation(Guid customerId, Guid productId)
        {
            if(customerId == Guid.Empty)
                throw new DomainException("O ID do cliente não pode ser vazio.");

            if(productId == Guid.Empty)
                throw new DomainException("O ID do produto não pode ser vazio.");

            CustomerId = customerId;
            ProductId = productId;
        }

        public void UpdateStatus(Status status)
        {
            if(status == Status.Expired && Status == Status.Expired)
                throw new InvalidOperationException("A reserva já está expirada.");
            
            if(status == Status.Active && Status == Status.Active)
                throw new InvalidOperationException("A reserva já está ativa.");

            Status = Status.Expired;
            ExpiresAt = DateTime.UtcNow;
        }
    }
}