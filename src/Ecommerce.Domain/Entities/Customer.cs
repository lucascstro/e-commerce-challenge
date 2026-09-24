namespace Ecommerce.Domain.Entities
{
    public class Customer
    {
        const int MaxNameLength = 100;
        const int MinNameLength = 3;

        public Guid CustomerId { get; private set; } = Guid.NewGuid();
        public string Name { get; private set; }

        public Customer(Guid customerId, string name)
        {
            CustomerId = customerId;
            Name = name;
        }
        
        public void UpdateName(string name)
        {
            if(name.Length > MaxNameLength)
                throw new ArgumentException($"O nome não pode ter mais de {MaxNameLength} caracteres.", nameof(name));
                
            if(name.Length < MinNameLength)
                throw new ArgumentException($"O nome não pode ter menos de {MinNameLength} caracteres.", nameof(name));

            if(string.IsNullOrWhiteSpace(name))
                throw new ArgumentException("O nome não pode ser vazio ou espaços em branco.", nameof(name));
                        
            Name = name;
        }
    }
}