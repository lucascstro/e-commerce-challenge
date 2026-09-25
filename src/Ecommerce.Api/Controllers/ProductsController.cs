using Ecommerce.Application.Dtos.Request;
using Ecommerce.Application.Services.Product;
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
        private readonly IProductService _productService;

        public ProductsController(ILogger<ProductsController> logger, IReservationService reservationService, IProductService productService)
        {
            _logger = logger;
            _reservationService = reservationService;
            _productService = productService;
        }

        [HttpGet]
        public async Task<IActionResult>GetAll()
        {
            var products = await _productService.GetAllProductsAsync(CancellationToken.None);
            return Ok(products);
        }

        [HttpPost("{id}/reserve")]
        public async Task<IActionResult> ReserveProduct(Guid id, [FromQuery] Guid customer_id, CancellationToken ct)
        {
            //TODO verificar se utilizar o controle de usuários é um problema (pois no requisito menciona somente o ID DO PRODUTO na requisição). 
            //Gero o id do cliente aleatoriamente e a ideia era pegar o id do cliente logado ao fazer reserva.
            var reservationResponse = await _reservationService.CreateReservationAsync(new CreateReservationRequest(customer_id, id), ct);
            return Ok(reservationResponse);
        }

        [HttpDelete("{reservation_id}/reserve")]
        public async Task<IActionResult> CancelReservation(Guid reservation_id, CancellationToken ct)
        {
            var reservationResponse = await _reservationService.UpdateReservationToStatusCancelledAsync(reservation_id, ct);
            return Ok(reservationResponse);
        }
    }
}