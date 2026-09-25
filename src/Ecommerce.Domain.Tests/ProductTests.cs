using Ecommerce.Domain.Entities;
using Ecommerce.Domain.Entities.Enum;
using Xunit;

namespace Ecommerce.Domain.Tests
{
    public class ProductTests
    {
        [Fact]
        public void Product_Should_Have_A_Valid_Id()
        {
            var name = "Produto Teste";
            var description = "Descrição do Produto Teste";
            var isAvailable = true;
            var product = new Product(name, description, StatusProduct.Available);
            Assert.IsType<Guid>(product.ProductId);
        }

        [Fact]
        public void Product_Should_Have_A_Valid_Name()
        {
            var name = "Produto Teste";
            var description = "Descrição do Produto Teste";
            var isAvailable = true;
            var product = new Product(name, description, StatusProduct.Available);
            Assert.Equal(name, product.Name);
        }

        [Fact]
        public void Product_Should_Have_A_Valid_Description()
        {
            var name = "Produto Teste";
            var description = "Descrição do Produto Teste";
            var isAvailable = true;
            var product = new Product(name, description, StatusProduct.Available);
            Assert.Equal(description, product.Description);
        }

        [Fact]
        public void Product_Should_Have_A_Valid_IsAvailable()
        {
            var name = "Produto Teste";
            var description = "Descrição do Produto Teste";
            var isAvailable = true;
            var product = new Product(name, description, StatusProduct.Available);
            Assert.Equal(isAvailable, product.Status == StatusProduct.Available);
        }

        [Fact]
        public void Product_Should_Be_not_Available_When_Marked_As_Unavailable()
        {
            var name = "Produto Teste";
            var description = "Descrição do Produto Teste";
            var isAvailable = true;
            var product = new Product(name, description, StatusProduct.Available);
            product.MarkAsUnavailable();
            Assert.Equal(StatusProduct.Unavailable, product.Status);
        }
        
        [Fact]
        public void Product_Should_Be_Available_When_Marked_As_Available()
        {
            var name = "Produto Teste";
            var description = "Descrição do Produto Teste";
            var isAvailable = false;
            var product = new Product(name, description, StatusProduct.Unavailable);
            product.MarkAsAvailable();
            Assert.Equal(StatusProduct.Available, product.Status);
        }
    }
}