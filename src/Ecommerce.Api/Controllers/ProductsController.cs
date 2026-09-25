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

        /// <summary>
        /// Lista todos os produtos cadastrados.
        /// </summary>
        /// <returns>A lista completa de produtos.</returns>
        [HttpGet]
        public async Task<IActionResult>GetAll()
        {
            var products = await _productService.GetAllProductsAsync(CancellationToken.None);
            return Ok(products);
        }

        /// <summary>
        /// Obtém um produto pelo seu identificador.
        /// </summary>
        /// <param name="id">Identificador do produto.</param>
        /// <param name="ct">Token de cancelamento da requisição.</param>
        /// <returns>O produto correspondente ao id informado.</returns>
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(Guid id, CancellationToken ct)
        {
            var product = await _productService.GetProductByIdAsync(id, ct);
            return Ok(product);
        }

        /// <summary>
        /// Cadastra um novo produto.
        /// </summary>
        /// <param name="request">Dados do produto a ser criado.</param>
        /// <param name="ct">Token de cancelamento da requisição.</param>
        /// <returns>O produto recém-criado.</returns>
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] ProductRequest request, CancellationToken ct)
        {
            var product = await _productService.CreateProductAsync(request, ct);
            return CreatedAtAction(nameof(GetById), new { id = product.ProductId }, product);
        }

        /// <summary>
        /// Atualiza os dados de um produto existente.
        /// </summary>
        /// <param name="id">Identificador do produto a ser atualizado.</param>
        /// <param name="request">Novos dados do produto.</param>
        /// <param name="ct">Token de cancelamento da requisição.</param>
        /// <returns>O produto atualizado.</returns>
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(Guid id, [FromBody] ProductRequest request, CancellationToken ct)
        {
            var product = await _productService.UpdateProductAsync(id, request, ct);
            return Ok(product);
        }

        /// <summary>
        /// Remove um produto existente.
        /// </summary>
        /// <param name="id">Identificador do produto a ser removido.</param>
        /// <param name="ct">Token de cancelamento da requisição.</param>
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(Guid id, CancellationToken ct)
        {
            await _productService.DeleteProductAsync(id, ct);
            return NoContent();
        }

        /// <summary>
        /// Cria uma reserva para o produto informado em nome de um cliente.
        /// </summary>
        /// <param name="id">Identificador do produto a ser reservado.</param>
        /// <param name="customer_id">Identificador do cliente que está reservando o produto.</param>
        /// <param name="ct">Token de cancelamento da requisição.</param>
        /// <returns>Os dados da reserva criada.</returns>
        [HttpPost("{id}/reserve")]
        public async Task<IActionResult> ReserveProduct(Guid id, [FromQuery] Guid customer_id, CancellationToken ct)
        {
            //TODO verificar se utilizar o controle de usuários é um problema (pois no requisito menciona somente o ID DO PRODUTO na requisição).
            //Gero o id do cliente aleatoriamente e a ideia era pegar o id do cliente logado ao fazer reserva.
            var reservationResponse = await _reservationService.CreateReservationAsync(new CreateReservationRequest(customer_id, id), ct);
            return Ok(reservationResponse);
        }

        /// <summary>
        /// Cancela uma reserva existente.
        /// </summary>
        /// <param name="reservation_id">Identificador da reserva a ser cancelada.</param>
        /// <param name="ct">Token de cancelamento da requisição.</param>
        /// <returns>Os dados da reserva cancelada.</returns>
        [HttpDelete("{reservation_id}/reserve")]
        public async Task<IActionResult> CancelReservation(Guid reservation_id, CancellationToken ct)
        {
            var reservationResponse = await _reservationService.UpdateReservationToStatusCancelledAsync(reservation_id, ct);
            return Ok(reservationResponse);
        }
    }
}