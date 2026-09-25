using Ecommerce.Domain.Entities.Enum;

namespace Ecommerce.Application.Dtos.Response
{
    public record AllReservationsByCustomerResponse(
        Guid ReservationId,
        Guid ProductId,
        string Status,
        DateTime CreatedAt
    );

    public record CreateReservationResponse(
        Guid ReservationId,
        Guid ProductId,
        DateTime CreatedAt
    );

    public record UpdateReservationResponse(
        Guid ReservationId,
        StatusReservation Status
    );

    public record ProductResponse(
        Guid ProductId,
        string Name,
        string Description,
        string Status
    );
}