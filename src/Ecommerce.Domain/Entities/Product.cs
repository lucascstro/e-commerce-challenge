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

        public Product(Guid productId, string name, string description, bool isAvailable)
        {
            ProductId = productId;
            Name = name;
            Description = description;
            IsAvailable = isAvailable;
        }

        public void UpdateName(string name)
        {
            if(name.Length > MaxNameLength)
                throw new ArgumentException($"O nome não pode ter mais de {MaxNameLength} caracteres.", nameof(name));
            if(name.Length < MinNameLength)
                throw new ArgumentException($"O nome não pode ter menos de {MinNameLength} caracteres.", nameof(name));

            if(string.IsNullOrWhiteSpace(name))
                throw new ArgumentException("O nome não pode ser vazio ou espaço em branco.", nameof(name));
                        
            Name = name;
        }
        public void UpdateDescription(string description)
        {   
            if(string.Compare(description, Description) == 0)
                throw new ArgumentException("A descrição não pode ser igual a anterior.", nameof(description));

            if(description.Length > MaxDescriptionLength)
                throw new ArgumentException($"A descrição não pode ter mais de {MaxDescriptionLength} caracteres.", nameof(description));
                        
            Description = description;
        }

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
    }
}