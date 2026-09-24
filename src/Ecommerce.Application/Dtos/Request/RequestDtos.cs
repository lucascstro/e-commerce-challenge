using Ecommerce.Domain.Entities.Enum;

namespace Ecommerce.Application.Dtos.Request
{
    public record CreateReservationRequest(Guid CustomerId, Guid ProductId);
    public record UpdateReservationRequest(Guid ReservationId, Status Status);
    public record GetAllReservationsByCustomerRequest(Guid CustomerId);
}