using Ecommerce.Application.Dtos.Request;
using Ecommerce.Application.Services.Customer;
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
        private readonly ICustomerService _customerService;

        public CustomerController(ILogger<CustomerController> logger, IReservationService reservationService, ICustomerService customerService)
        {
            _logger = logger;
            _reservationService = reservationService;
            _customerService = customerService;
        }

        /// <summary>
        /// Lista todas as reservas de um cliente específico.
        /// </summary>
        /// <param name="id_customer">Identificador do cliente.</param>
        /// <param name="ct">Token de cancelamento da requisição.</param>
        /// <returns>A lista de reservas do cliente informado.</returns>
        [HttpGet("{id_customer}/reservations")]
        public async Task<IActionResult> GetAllReservationsByCustomer(Guid id_customer, CancellationToken ct)
        {
            var reservations = await _reservationService.GetAllReservationsByCustomerAsync(id_customer, ct);
            return Ok(reservations);
        }

        /// <summary>
        /// Lista todos os clientes cadastrados.
        /// </summary>
        /// <param name="ct">Token de cancelamento da requisição.</param>
        /// <returns>A lista completa de clientes.</returns>
        [HttpGet]
        public async Task<IActionResult> GetAll(CancellationToken ct)
        {
            var customers = await _customerService.GetAllCustomersAsync(ct);
            return Ok(customers);
        }

        /// <summary>
        /// Obtém um cliente pelo seu identificador.
        /// </summary>
        /// <param name="id">Identificador do cliente.</param>
        /// <param name="ct">Token de cancelamento da requisição.</param>
        /// <returns>O cliente correspondente ao id informado.</returns>
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(Guid id, CancellationToken ct)
        {
            var customer = await _customerService.GetCustomerByIdAsync(id, ct);
            return Ok(customer);
        }

        /// <summary>
        /// Cadastra um novo cliente.
        /// </summary>
        /// <param name="request">Dados do cliente a ser criado.</param>
        /// <param name="ct">Token de cancelamento da requisição.</param>
        /// <returns>O cliente recém-criado.</returns>
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CustomerRequest request, CancellationToken ct)
        {
            var customer = await _customerService.CreateCustomerAsync(request, ct);
            return CreatedAtAction(nameof(GetById), new { id = customer.CustomerId }, customer);
        }

        /// <summary>
        /// Atualiza os dados de um cliente existente.
        /// </summary>
        /// <param name="id">Identificador do cliente a ser atualizado.</param>
        /// <param name="request">Novos dados do cliente.</param>
        /// <param name="ct">Token de cancelamento da requisição.</param>
        /// <returns>O cliente atualizado.</returns>
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(Guid id, [FromBody] CustomerRequest request, CancellationToken ct)
        {
            var customer = await _customerService.UpdateCustomerAsync(id, request, ct);
            return Ok(customer);
        }

        /// <summary>
        /// Remove um cliente existente.
        /// </summary>
        /// <param name="id">Identificador do cliente a ser removido.</param>
        /// <param name="ct">Token de cancelamento da requisição.</param>
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(Guid id, CancellationToken ct)
        {
            await _customerService.DeleteCustomerAsync(id, ct);
            return NoContent();
        }
    }
}