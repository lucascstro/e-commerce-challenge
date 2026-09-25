using Ecommerce.Application.Dtos.Request;
using Ecommerce.Application.Dtos.Response;

namespace Ecommerce.Application.Services.Customer
{
    public interface ICustomerService
    {
        public Task<IEnumerable<CustomerResponse>> GetAllCustomersAsync(CancellationToken ct);
        public Task<CustomerResponse> GetCustomerByIdAsync(Guid customerId, CancellationToken ct);
        public Task<CustomerResponse> CreateCustomerAsync(CustomerRequest request, CancellationToken ct);
        public Task<CustomerResponse> UpdateCustomerAsync(Guid customerId, CustomerRequest request, CancellationToken ct);
        public Task DeleteCustomerAsync(Guid customerId, CancellationToken ct);
    }
}
