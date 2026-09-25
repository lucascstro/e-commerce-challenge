using Ecommerce.Application.Dtos.Request;
using Ecommerce.Application.Dtos.Response;
using Ecommerce.Domain.Entities.Enum;
using Ecommerce.Domain.Repositories;

namespace Ecommerce.Application.Services.Customer
{
    public class CustomerService : ICustomerService
    {
        private readonly ICustomerRepository _customerRepository;
        private readonly IReservationRepository _reservationRepository;
        private readonly IUnitOfWork _unitOfWork;

        public CustomerService(ICustomerRepository customerRepository, IReservationRepository reservationRepository, IUnitOfWork unitOfWork)
        {
            _customerRepository = customerRepository;
            _reservationRepository = reservationRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task<IEnumerable<CustomerResponse>> GetAllCustomersAsync(CancellationToken ct)
        {
            var customers = await _customerRepository.GetAllAsync();
            return customers.Select(c => new CustomerResponse(c.CustomerId, c.Name));
        }

        public async Task<CustomerResponse> GetCustomerByIdAsync(Guid customerId, CancellationToken ct)
        {
            if (customerId == Guid.Empty)
                throw new ArgumentException("O ID do cliente não pode ser vazio", nameof(customerId));

            var customer = await _customerRepository.GetByIdAsync(customerId);
            if (customer == null)
                throw new InvalidOperationException("Cliente não encontrado.");

            return new CustomerResponse(customer.CustomerId, customer.Name);
        }

        public async Task<CustomerResponse> CreateCustomerAsync(CustomerRequest request, CancellationToken ct)
        {
            var customer = new Domain.Entities.Customer(request.Name);

            await _customerRepository.AddAsync(customer);
            await _unitOfWork.SaveChangesAsync(ct);

            return new CustomerResponse(customer.CustomerId, customer.Name);
        }

        public async Task<CustomerResponse> UpdateCustomerAsync(Guid customerId, CustomerRequest request, CancellationToken ct)
        {
            if (customerId == Guid.Empty)
                throw new ArgumentException("O ID do cliente não pode ser vazio", nameof(customerId));

            var customer = await _customerRepository.GetByIdAsync(customerId);
            if (customer == null)
                throw new InvalidOperationException("Cliente não encontrado.");

            await EnsureNoActiveReservationAsync(customerId, ct);

            customer.UpdateName(request.Name);

            await _customerRepository.UpdateAsync(customer);
            await _unitOfWork.SaveChangesAsync(ct);

            return new CustomerResponse(customer.CustomerId, customer.Name);
        }

        public async Task DeleteCustomerAsync(Guid customerId, CancellationToken ct)
        {
            if (customerId == Guid.Empty)
                throw new ArgumentException("O ID do cliente não pode ser vazio", nameof(customerId));

            var customer = await _customerRepository.GetByIdAsync(customerId);
            if (customer == null)
                throw new InvalidOperationException("Cliente não encontrado.");

            await EnsureNoActiveReservationAsync(customerId, ct);

            await _customerRepository.DeleteAsync(customerId);
            await _unitOfWork.SaveChangesAsync(ct);
        }

        private async Task EnsureNoActiveReservationAsync(Guid customerId, CancellationToken ct)
        {
            var reservations = await _reservationRepository.GetByCustomerIdAsync(customerId, ct);
            if (reservations.Any(r => r.Status == StatusReservation.Active))
                throw new InvalidOperationException("Não é possível alterar ou excluir um cliente com reserva ativa.");
        }
    }
}
