using Ecommerce.Domain.Entities;

namespace Ecommerce.Domain.Repositories
{
    public interface ICustomerRepository
    {
        public Task<Customer> GetByIdAsync(Guid customerId);
        public Task<IEnumerable<Customer>> GetAllAsync();
        public Task AddAsync(Customer customer);
        public Task UpdateAsync(Customer customer); 
        public Task DeleteAsync(Guid customerId);
    }
}