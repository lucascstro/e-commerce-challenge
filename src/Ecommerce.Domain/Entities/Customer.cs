using Ecommerce.Domain.Exceptions;

namespace Ecommerce.Domain.Entities
{
    public class Customer
    {
        const int MaxNameLength = 100;
        const int MinNameLength = 3;

        public Guid CustomerId { get; private set; } = Guid.NewGuid();
        public string Name { get; private set; }

        public Customer(string name) => SetName(name);
        
        public void UpdateName(string name) => SetName(name);

        private void SetName(string name)
        {
            if(string.IsNullOrWhiteSpace(name))
                throw new DomainException("O nome não pode ser vazio ou espaços em branco.");

            if(name.Length > MaxNameLength)
                throw new DomainException($"O nome não pode ter mais de {MaxNameLength} caracteres.");
                
            if(name.Length < MinNameLength)
                throw new DomainException($"O nome não pode ter menos de {MinNameLength} caracteres.");
                        
            Name = name;
        }
    }
}