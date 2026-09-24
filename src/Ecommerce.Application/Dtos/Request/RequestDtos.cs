using Ecommerce.Domain.Entities.Enum;

namespace Ecommerce.Application.Dtos.Request
{
    //criação de reserva do produto.
    //cancelamento manual de reserva do produto.
    //obter todas as reservas de um cliente.
    public record CreateReservationRequest(Guid CustomerId, Guid ProductId);

    public record UpdateReservationRequest(Guid ReservationId, Status Status);

    public record GetAllReservationsByCustomerRequest(Guid CustomerId);
}