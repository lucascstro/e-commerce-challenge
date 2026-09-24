using Ecommerce.Domain.Entities;
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
            var product = new Product(name, description, isAvailable);
            Assert.IsType<Guid>(product.ProductId);
        }

        [Fact]
        public void Product_Should_Have_A_Valid_Name()
        {
            var name = "Produto Teste";
            var description = "Descrição do Produto Teste";
            var isAvailable = true;
            var product = new Product(name, description, isAvailable);
            Assert.Equal(name, product.Name);
        }

        [Fact]
        public void Product_Should_Have_A_Valid_Description()
        {
            var name = "Produto Teste";
            var description = "Descrição do Produto Teste";
            var isAvailable = true;
            var product = new Product(name, description, isAvailable);
            Assert.Equal(description, product.Description);
        }

        [Fact]
        public void Product_Should_Have_A_Valid_IsAvailable()
        {
            var name = "Produto Teste";
            var description = "Descrição do Produto Teste";
            var isAvailable = true;
            var product = new Product(name, description, isAvailable);
            Assert.Equal(isAvailable, product.IsAvailable);
        }

        [Fact]
        public void Product_Should_Be_not_Available_When_Marked_As_Unavailable()
        {
            var name = "Produto Teste";
            var description = "Descrição do Produto Teste";
            var isAvailable = true;
            var product = new Product(name, description, isAvailable);
            product.MarkAsUnavailable();
            Assert.False(product.IsAvailable);
        }
        
        [Fact]
        public void Product_Should_Be_Available_When_Marked_As_Available()
        {
            var name = "Produto Teste";
            var description = "Descrição do Produto Teste";
            var isAvailable = false;
            var product = new Product(name, description, isAvailable);
            product.MarkAsAvailable();
            Assert.True(product.IsAvailable);
        }
    }
}