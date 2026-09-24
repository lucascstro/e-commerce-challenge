namespace Ecommerce.Application.Dtos.Response
{
    public class ResponseDtos
    {
        public record AllReservationsByCustomerResponse(
            Guid ReservationId,
            string ProductName,
            DateTime CreatedAt
        );

        public record CreateReservationResponse(
            Guid ReservationId,
            string ProductName,
            DateTime CreatedAt
        );

        public record UpdateReservationResponse(
            Guid ReservationId,
            string Status
        );
    }
}