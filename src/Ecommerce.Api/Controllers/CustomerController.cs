using Ecommerce.Application.Services.Reservation;
using Microsoft.AspNetCore.Mvc;

namespace Ecommerce.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CustomerController : ControllerBase
    {
        private readonly ILogger<CustomerController> _logger;
        private readonly IReservationService _reservationService;

        public CustomerController(ILogger<CustomerController> logger, IReservationService reservationService)
        {
            _logger = logger;
            _reservationService = reservationService;
        }

        [HttpGet("{id_customer}/reservations")]
        public async Task<IActionResult> GetAllReservationsByCustomer(Guid id_customer, CancellationToken ct)
        {
            var reservations = await _reservationService.GetAllReservationsByCustomerAsync(id_customer, ct);
            return Ok(reservations);
        }
    }
}