using Ecommerce.Application.Services.Reservation;
using Microsoft.AspNetCore.Mvc;

namespace Ecommerce.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProductsController : ControllerBase
    {
        private readonly ILogger<ProductsController> _logger;
        private readonly IReservationService _reservationService;

        public ProductsController(ILogger<ProductsController> logger, IReservationService reservationService)
        {
            _logger = logger;
            _reservationService = reservationService;
        }

        [HttpPost("{product_id}/{customer_id}/reserve")]
        public async Task<IActionResult> ReserveProduct(Guid id_product, Guid id_customer, CancellationToken ct)
        {
            //TODO verificar se utilizar o controle de usuários é um problema (pois no requisito menciona somente o ID DO PRODUTO na requisição). 
            //Gero o id do cliente aleatoriamente e a ideia era pegar o id do cliente logado ao fazer reserva.
            
            var reservationResponse = await _reservationService.CreateReservationAsync(id_customer, id_product, ct);
            return Ok(reservationResponse);
        }
    }
}