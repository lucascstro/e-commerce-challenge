using Ecommerce.Application.Services.Reservation;
using Microsoft.AspNetCore.Mvc;

namespace Ecommerce.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ReservationController : ControllerBase
    {
        private readonly ILogger<ReservationController> _logger;
        private readonly IReservationService _reservationService;

        public ReservationController(ILogger<ReservationController> logger, IReservationService reservationService)
        {
            _logger = logger;
            _reservationService = reservationService;
        }

        /// <summary>
        /// Lista todas as reservas cadastradas.
        /// </summary>
        /// <param name="ct">Token de cancelamento da requisição.</param>
        /// <returns>A lista completa de reservas.</returns>
        [HttpGet]
        public async Task<IActionResult> GetAllReservations(CancellationToken ct)
        {
            var reservations = await _reservationService.GetAllReservations(ct);
            return Ok(reservations);
        }
    }
}