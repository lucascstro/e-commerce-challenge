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
        public bool IsAvailable { get; private set; }

        public Product(string name, string description, bool isAvailable)
        {
            SetName(name);
            SetDescription(description);
            IsAvailable = isAvailable;
        }

        public void UpdateName(string name) => SetName(name);
        
        public void UpdateDescription(string description) => SetDescription(description);

        public bool MarkAsUnavailable()
        {
            if(IsAvailable == false)
                throw new InvalidOperationException("O produto já está indisponível.");

            IsAvailable = false;
            return IsAvailable;
        }

        public bool MarkAsAvailable()
        {
            if(IsAvailable == true)
                throw new InvalidOperationException("O produto já está disponível.");

            IsAvailable = true;
            return IsAvailable;
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