using Ecommerce.Domain.Entities.Enum;

namespace Ecommerce.Application.Dtos.Request
{
    public record CreateReservationRequest(Guid CustomerId, Guid ProductId);
    public record ProductRequest(string Name, string Description, StatusProduct Status);
}