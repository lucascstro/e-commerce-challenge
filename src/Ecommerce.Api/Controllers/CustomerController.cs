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

        [HttpGet("{id_customer}/reservations")]
        public async Task<IActionResult> GetAllReservationsByCustomer(Guid id_customer, CancellationToken ct)
        {
            var reservations = await _reservationService.GetAllReservationsByCustomerAsync(id_customer, ct);
            return Ok(reservations);
        }

        [HttpGet]
        public async Task<IActionResult> GetAll(CancellationToken ct)
        {
            var customers = await _customerService.GetAllCustomersAsync(ct);
            return Ok(customers);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(Guid id, CancellationToken ct)
        {
            var customer = await _customerService.GetCustomerByIdAsync(id, ct);
            return Ok(customer);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CustomerRequest request, CancellationToken ct)
        {
            var customer = await _customerService.CreateCustomerAsync(request, ct);
            return CreatedAtAction(nameof(GetById), new { id = customer.CustomerId }, customer);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(Guid id, [FromBody] CustomerRequest request, CancellationToken ct)
        {
            var customer = await _customerService.UpdateCustomerAsync(id, request, ct);
            return Ok(customer);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(Guid id, CancellationToken ct)
        {
            await _customerService.DeleteCustomerAsync(id, ct);
            return NoContent();
        }
    }
}