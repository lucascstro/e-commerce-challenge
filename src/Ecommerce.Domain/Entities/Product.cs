using Ecommerce.Domain.Entities.Enum;
using Ecommerce.Domain.Exceptions;

namespace Ecommerce.Domain.Entities
{
    public class Product
    {
        const int MaxNameLength = 100;
        const int MinNameLength = 3;
        const int MaxDescriptionLength = 200;
        public Guid ProductId { get; private set; } = Guid.NewGuid();
        public string Name { get; private set; }
        public string? Description { get; private set; }
        public StatusProduct Status { get; private set; }
        public byte[] RowVersion { get; private set; } = Array.Empty<byte>(); 

        public Product(string name, string description, StatusProduct status)
        {
            SetName(name);
            SetDescription(description);
            Status = status;
        }

        public void UpdateName(string name) => SetName(name);
        
        public void UpdateDescription(string description) => SetDescription(description);

        public void MarkAsUnavailable()
        {
            if(Status == StatusProduct.Unavailable)
                throw new InvalidOperationException("O produto já está indisponível.");

            Status = StatusProduct.Unavailable;
        }

        public void MarkAsAvailable()
        {
            if(Status == StatusProduct.Available)
                throw new InvalidOperationException("O produto já está disponível.");

            Status = StatusProduct.Available;
        }

        public void MarkAsReserved()
        {
            if(Status == StatusProduct.Unavailable)
                throw new InvalidOperationException("O produto está indisponível.");

            if(Status == StatusProduct.Reserved)
                throw new InvalidOperationException("O produto já está reservado.");

            Status = StatusProduct.Reserved;
        }

        private void SetName(string name)
        {
            if(string.IsNullOrWhiteSpace(name))
                throw new DomainException("O nome não pode ser vazio ou espaço em branco.");
            
            name = name.Trim();
            if(name.Length > MaxNameLength)
                throw new DomainException($"O nome não pode ter mais de {MaxNameLength} caracteres.");

            if(name.Length < MinNameLength)
                throw new DomainException($"O nome não pode ter menos de {MinNameLength} caracteres.");
                        
            Name = name;
        }

        private void SetDescription(string description)
        {
            description = description.Trim();
            
            if(string.Compare(description, Description) == 0)
                throw new DomainException("A descrição não pode ser igual a anterior.");

            if(description.Length > MaxDescriptionLength)
                throw new DomainException($"A descrição não pode ter mais de {MaxDescriptionLength} caracteres.");
                        
            Description = description;
        }
    }
}